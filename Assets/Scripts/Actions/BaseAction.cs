using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static EventHandler OnActionStarted;
        public static EventHandler OnActionEnded;
        
        public Unit Unit { get; private set; }
        
        protected bool IsActive;
        
        private Action _onActionComplete;
        
        private const int DefaultActionCost = 1;
        
        [field: SerializeField] public GridVisualType VisualType { get; private set; }

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
            _onActionComplete = onActionComplete;
            
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionEnd()
        {
            IsActive = false;
            _onActionComplete();
            
            OnActionEnded?.Invoke(this, EventArgs.Empty);
        }

        public EnemyAIAction GetBestEnemyAIAction()
        {
            var enemyAIActionList = new List<EnemyAIAction>();
            
            var validActionGridPositionList =  GetValidActionGridPositionList();

            foreach (var gridPosition in validActionGridPositionList)
            {
                var enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort((a, b) => b.ActionValue - a.ActionValue);

                return enemyAIActionList[0];
            }
            else
            {
                // No possible actions
                return null;
            }
        }

        protected abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    }
}
