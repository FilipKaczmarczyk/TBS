using System;
using System.Collections.Generic;
using DG.Tweening;
using Grid;
using UnityEngine;

namespace Actions
{
    public class ShootAction : BaseAction
    {
        public event EventHandler<OnShootEventArgs> OnShoot;

        public class OnShootEventArgs : EventArgs
        {
            public Unit TargetUnit;
            public Unit ShootingUnit;
        }
        
        private enum State
        {
            Aiming,
            Shooting,
            CoolOff,
        }
        
        [SerializeField] private Sprite shootSprite;

        private const int ShootActionCost = 2;

        [field: SerializeField] public int MAXShootDistance { get; private set; } = 7;
    
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShootBullet;
        private bool _isAiming;

        private void Update()
        {
            if (!IsActive)
                return;

            _stateTimer -= Time.deltaTime;

            switch (_state)
            {
                case State.Aiming:
                    if (!_isAiming)
                    {
                        _isAiming = true;
                        transform.DOLookAt(_targetUnit.transform.position, 0.5f);
                    }
                    
                    break;
                
                case State.Shooting:
                    if (_canShootBullet)
                    {
                        Shoot();
                        _canShootBullet = false;
                    }
                    
                    break;
                
                case State.CoolOff:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_stateTimer <= 0f)
            {
                NextState();
            }
        }

        private void Shoot()
        {
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                TargetUnit = _targetUnit,
                ShootingUnit = Unit
            });
                
            _targetUnit.Damage(40);
        }

        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    _state = State.Shooting;
                    var shootingStateTime = .1f;
                    _stateTimer = shootingStateTime; 
                    
                    break;
                
                case State.Shooting:
                    _state = State.CoolOff;
                    var coolOffStateTime = .1f;
                    _stateTimer = coolOffStateTime;
                    
                    break;
                
                case State.CoolOff:
                    ActionEnd();
                    
                    break;
            }
        }

        public override Sprite GetActionImage()
        {
            return shootSprite;
        }

        public override void TakeAction(GridPosition gridPosition, Action onShootComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtPosition(gridPosition);
            
            _state = State.Aiming;
            
            var aimingStateTime = .5f;
            _isAiming = false;
            _stateTimer = aimingStateTime;

            _canShootBullet = true;
            
            ActionStart(onShootComplete);
        }
        
        public override int GetActionPointsCost()
        {
            return ShootActionCost;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var unitGridPosition = Unit.GetGridPosition();
            
            return GetValidActionGridPositionList(unitGridPosition);
        }
        
        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            var validGridPositions = new List<GridPosition>();

            for (var x = -MAXShootDistance; x <= MAXShootDistance; x++)
            {
                for (var z = -MAXShootDistance; z <= MAXShootDistance; z++)
                {
                    var offsetGridPosition = new GridPosition(x, z);
                    var testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.CheckValidGridPosition(testGridPosition))
                        continue;

                    var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    
                    if (testDistance > MAXShootDistance) 
                        continue;

                    if (!LevelGrid.Instance.CheckIsUnitAtPosition(testGridPosition))
                        continue;

                    var targetUnit = LevelGrid.Instance.GetUnitAtPosition(testGridPosition);
                    
                    if (targetUnit.IsEnemy() == Unit.IsEnemy()) 
                        continue; 
                        
                    validGridPositions.Add(testGridPosition);
                }
            }
        
            return validGridPositions;
        }

        public Unit GetTargetUnit()
        {
            return _targetUnit;
        }

        protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            var targetUnit = LevelGrid.Instance.GetUnitAtPosition(gridPosition);
            targetUnit.GetHealthNormalized();

            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
            };
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}
