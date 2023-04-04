using System;
using Actions;
using GameRules;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Units
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }
        public event EventHandler OnSelectedUnitChange;
        public event EventHandler OnSelectedActionChange;
        public event EventHandler<bool> OnBusyChange;
        public event EventHandler OnActionStarted;
    
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
            SelectUnitAtStart();
            SetSelectedUnit(_selectedUnit);
        }

        private void Update()
        {
            if (_isBusy) return;

            if (!TurnSystem.Instance.IsPlayerTurn()) return;
        
            if (EventSystem.current.IsPointerOverGameObject()) return;
        
            if (!Input.GetMouseButtonDown(0)) return;
        
            if (TryHandleUnitSelection()) return;
            
            HandleSelectionAction();
        }

        private void SelectUnitAtStart()
        {
            var units = FindObjectsOfType<Unit>();

            foreach (var unit in units)
            {
                if(unit.IsEnemy()) 
                    continue;
            
                _selectedUnit = unit;
            }
        }

        private void HandleSelectionAction()
        {
            var gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.CheckIsValidGridPosition(gridPosition)) return;
        
            if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;
            
            SetBusy();
            _selectedAction.TakeAction(gridPosition, ClearBusy);
        
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    
        private void SetBusy()
        {
            _isBusy = true;
        
            OnBusyChange?.Invoke(this, _isBusy);
        }

        private void ClearBusy()
        {
            _isBusy = false;
        
            OnBusyChange?.Invoke(this, _isBusy);
        }
    
        private bool TryHandleUnitSelection()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out Unit unit))
                {
                    if (unit == _selectedUnit) return false; // Unit is already selected

                    if (unit.IsEnemy()) return false; // Click on enemy
                
                    SetSelectedUnit(unit);
                    return true;
                }
            }

            return false;
        }

        private void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
        
            SetSelectedAction(unit.GetAction<MoveAction>());
        
            OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelectedAction(BaseAction action)
        {
            _selectedAction = action;
        
            OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
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
}
