using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    
    [SerializeField] private LayerMask unitLayerMask;
    
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

    private void Update()
    {
        if (_isBusy) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) 
                return;

            if (_selectedUnit != null)
            {
                var gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (_selectedUnit.GetMoveAction().CheckIsValidPositionToMove(gridPosition))
                {
                    SetBusy();
                    _selectedUnit.GetMoveAction().Move(gridPosition, ClearBusy);
                }
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (TryHandleUnitSelection()) 
                return;

            if (_selectedUnit != null)
            {
                SetBusy();
                _selectedUnit.GetSpinAction().Spin(ClearBusy);
            }
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
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
