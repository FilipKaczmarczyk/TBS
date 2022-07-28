using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private LayerMask unitLayerMask;
    
    private Unit _selectedUnit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) 
                return;

            if (_selectedUnit != null)
            {
                _selectedUnit.Move(MouseWorld.GetPosition());
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out Unit unit))
            {
                _selectedUnit = unit;
                return true;
            }
        }

        return false;
    }
}
