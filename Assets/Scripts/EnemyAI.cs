using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForTurn,
        TakingTurn,
        Busy
    }

    private State _state;
    
    private float _timer;

    private void Awake()
    {
        _state = State.WaitingForTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (_state)
        {
            case State.WaitingForTurn:
                
                break;
            
            case State.TakingTurn:
                TakeTurn();
                
                break;
            
            case State.Busy:
                
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _state = State.TakingTurn;
    }
    
    private void TakeTurn()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0f)
        {
            if (TryTakeEnemyAIAction(SetStateTakingTurn))
            {
                _state = State.Busy;
            }
            else
            {
                TurnSystem.Instance.NextTurn();
            }
        }
    }
    
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (var enemy in UnitManager.Instance.EnemyUnitList)
        {
            if (TryTakeEnemyAIAction(enemy, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }
    
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        var spinAction = enemyUnit.GetSpinAction();

        var gridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.CheckIsValidGridPosition(gridPosition)) return false;
        
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;
            
        spinAction.TakeAction(gridPosition, onEnemyAIActionComplete);

        return true;
    }

    private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs empty)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            _state = State.TakingTurn;
            _timer = 2.0f;
        }
    }
}
