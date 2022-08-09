using System;
using Actions;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;
    
    private GridPosition _gridPosition;
    private HealthSystem _healthSystem;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActions;
    private int _actionPoints = 2;

    private const int MAX_ACTION_POINTS = 2;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActions = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
        TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Update()
    {
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(_gridPosition, newGridPosition, this);
            _gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }
    
    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActions()
    {
        return _baseActions;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }

        return false;
    }

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return _actionPoints >= baseAction.GetActionPointsCost();
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints() => _actionPoints;

    private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs empty)
    {
        if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn()) ||
            !isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {
            _actionPoints = MAX_ACTION_POINTS;
        
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public bool IsEnemy() => isEnemy;

    public void Damage(int damageAmount)
    {
        _healthSystem.TakeDamage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
    }
}
