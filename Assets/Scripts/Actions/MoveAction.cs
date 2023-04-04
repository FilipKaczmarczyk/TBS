using System;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using Grid;
using PathfindingSystem;
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
        
        private List<Vector3> _positionList;
        private int _currentPositionIndex;
        
        private void Update()
        {
            if(!IsActive) return;

            var targetPosition = _positionList[_currentPositionIndex];
            
            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
            {
                _currentPositionIndex ++;
                
                if (_currentPositionIndex >= _positionList.Count)
                {
                    transform.position = targetPosition;
                
                    OnStopMoving?.Invoke(this, EventArgs.Empty);

                    ActionEnd();
                }
            }
            else
            {
                var moveDirection = (targetPosition - transform.position).normalized;
                
                transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            }
        }

        public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
        {
            var pathGridPositions = Pathfinding.Instance.FindPath(Unit.GetGridPosition(), targetPosition, out int pathLength);
            
            _currentPositionIndex = 0;
            _positionList = new List<Vector3>();

            foreach (var pathGridPosition in pathGridPositions)
            {
                _positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
            }

            transform.DOLookAt(LevelGrid.Instance.GetWorldPosition(targetPosition), rotateTime);
            
            OnStartMoving?.Invoke(this, EventArgs.Empty);
            
            ActionStart(onMoveComplete);
        }
        
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var validGridPositions = new List<GridPosition>();

            var unitGridPosition = Unit.GetGridPosition();

            const int pathfindingDistanceMultiplier = 10;
            
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
                    
                    if (!Pathfinding.Instance.IsWalkableGridPosition(validGridPosition))
                        continue;
                    
                    if (!Pathfinding.Instance.HasPath(unitGridPosition, validGridPosition))
                        continue;

                    if (Pathfinding.Instance.GetPathLength(unitGridPosition, validGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
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

        protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            var targetCountAtPosition = Unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = targetCountAtPosition * 10
            };
        }
    }
}
