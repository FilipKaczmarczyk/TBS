using Actions;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActions;
    private int _actionPoints = 2;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActions = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }
}
