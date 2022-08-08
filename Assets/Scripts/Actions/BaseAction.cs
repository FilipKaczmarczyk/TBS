using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit Unit;
        protected bool IsActive;
        protected Action ONActionComplete;
        
        private const int DefaultActionCost = 1;

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }

        public abstract Sprite GetActionImage();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool CheckIsValidGridPosition(GridPosition gridPosition)
        {
            var validGridPositions = GetValidActionGridPositionList();

            return validGridPositions.Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidActionGridPositionList();

        public virtual int GetActionPointsCost()
        {
            return DefaultActionCost;
        }

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            ONActionComplete = onActionComplete;
        }

        protected void ActionEnd()
        {
            IsActive = false;
            ONActionComplete();
        }
    }
}
