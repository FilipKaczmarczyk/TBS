using System;
using Actions;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction _selectedAction;
    private Unit _selectedUnit;
    private bool _isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        _selectedUnit = FindObjectOfType<Unit>();
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        if (_isBusy) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (TryHandleUnitSelection()) return;
            
        HandleSelectionAction();
    }

    private void HandleSelectionAction()
    {
        var gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (_selectedAction.CheckIsValidGridPosition(gridPosition))
        {
            SetBusy();
            _selectedAction.TakeAction(gridPosition, ClearBusy);
        }
    }
    
    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }
    
    private bool TryHandleUnitSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out Unit unit))
            {
                if (unit == _selectedUnit) return false; 
                
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        
        SetSelectedAction(unit.GetMoveAction());
        
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
    
    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}