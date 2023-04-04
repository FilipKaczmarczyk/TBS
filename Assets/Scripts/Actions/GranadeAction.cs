using System;
using System.Collections.Generic;
using Effects;
using Enemies;
using Grid;
using UnityEngine;

namespace Actions
{
    public class GranadeAction : BaseAction
    {
        [field: SerializeField] public int MAXThrowDistance { get; private set; } = 7;
        
        [SerializeField] private Sprite granadeSprite;
        [SerializeField] private Transform grenadeProjectile;
        
        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
        }

        public override Sprite GetActionImage()
        {
            return granadeSprite;
        }
        
        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var validGridPositions = new List<GridPosition>();

            var unitGridPosition = Unit.GetGridPosition();
            
            for (var x = -MAXThrowDistance; x <= MAXThrowDistance; x++)
            {
                for (var z = -MAXThrowDistance; z <= MAXThrowDistance; z++)
                {
                    var offsetGridPosition = new GridPosition(x, z);
                    var testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.CheckValidGridPosition(testGridPosition))
                        continue;

                    var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    
                    if (testDistance > MAXThrowDistance) 
                        continue;
                    
                    validGridPositions.Add(testGridPosition);
                }
            }
         
            return validGridPositions;
        }
        
        protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 0
            };
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            var grenade = Instantiate(grenadeProjectile, Unit.GetWorldPosition(), Quaternion.identity);
            grenade.GetComponent<GrenadeProjectile>().Setup(gridPosition, OnGranadeBehaviourComplete);
            
            ActionStart(onActionComplete);
        }

        private void OnGranadeBehaviourComplete()
        {
            ActionEnd();
        }
    }
}
