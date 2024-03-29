using System;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using Grid;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        [SerializeField] private Sprite spinSprite;
        
        private const int SpinActionCost = 2;
        
        public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
        {
            transform.DOLocalRotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).
                SetRelative(true).SetEase(Ease.Linear).OnComplete(ActionEnd);
            
            ActionStart(onSpinComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            var unitGridPosition = Unit.GetGridPosition();

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }

        public override Sprite GetActionImage()
        {
            return spinSprite;
        }

        public override int GetActionPointsCost()
        {
            return SpinActionCost;
        }

        protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 0,
            };
        }
    }
}
