using System;
using Actions;
using GameRules;
using Grid;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawn;
        public static event EventHandler OnAnyUnitDead;
    
        [SerializeField] private bool isEnemy;
    
        private GridPosition _gridPosition;
        private HealthSystem _healthSystem;
        private BaseAction[] _baseActions;
        private int _actionPoints = 2;

        private const int MAX_ACTION_POINTS = 2;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _baseActions = GetComponents<BaseAction>();
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
            TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
            _healthSystem.OnDead += HealthSystem_OnDead;
        
            OnAnyUnitSpawn?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
            if (newGridPosition != _gridPosition)
            {
                var oldGridPosition = _gridPosition;
                _gridPosition = newGridPosition;
            
                LevelGrid.Instance.UnitMovedGridPosition(oldGridPosition, _gridPosition, this);
            }
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (var baseAction in _baseActions)
            {
                if (baseAction is T baseActionType)
                {
                    return baseActionType;
                }
            }

            return null;
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

        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
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
        
            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        }

        public float GetHealthNormalized()
        {
            return _healthSystem.GetHealthNormalized();
        }
    }
}
