using System;
using System.Collections.Generic;
using DG.Tweening;
using Grid;
using UnityEngine;

namespace Actions
{
    public class MoveAction : BaseAction
    {
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;
        
        [SerializeField] private Sprite moveSprite;
        
        [SerializeField] private int maxMoveDistance = 4;

        [SerializeField] private float moveSpeed = 4.0f;
        [SerializeField] private float stoppingDistance = 0.05f;
        [SerializeField] private float rotateTime = 0.5f;
        
        private Vector3 _targetPosition;

        protected override void Awake()
        {
            base.Awake();
            _targetPosition = transform.position;
        }
    
        private void Update()
        {
            if(!IsActive) return;
            
            if (Vector3.Distance(transform.position, _targetPosition) < stoppingDistance)
            {
                transform.position = _targetPosition;
                
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionEnd();
            }
            else
            {
                var moveDirection = (_targetPosition - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }

        public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        
            transform.DOLookAt(_targetPosition, rotateTime);
            
            OnStartMoving?.Invoke(this, EventArgs.Empty);
            
            ActionStart(onMoveComplete);
        }
        
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var validGridPositions = new List<GridPosition>();

            var unitGridPosition = Unit.GetGridPosition();
        
            for (var x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (var z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    var offsetGridPosition = new GridPosition(x, z);
                    var validGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.CheckValidGridPosition(validGridPosition))
                        continue;
                
                    if (validGridPosition == unitGridPosition)
                        continue;
                
                    if (LevelGrid.Instance.CheckIsUnitAtPosition(validGridPosition))
                        continue;
                
                    validGridPositions.Add(validGridPosition);
                }
            }
        
            return validGridPositions;
        }
        
        public override Sprite GetActionImage()
        {
            return moveSprite;
        }
    }
}
