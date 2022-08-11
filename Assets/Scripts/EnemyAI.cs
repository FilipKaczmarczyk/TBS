using System;
using Actions;
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
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        
        foreach (var baseAction in enemyUnit.GetBaseActions())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                var testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                
                if (testEnemyAIAction != null && testEnemyAIAction.ActionValue > bestEnemyAIAction.ActionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.GridPosition, onEnemyAIActionComplete);
            
            return true;
        }
        else
        {
            return false;
        }
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
