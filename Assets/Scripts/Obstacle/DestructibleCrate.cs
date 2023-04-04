using System;
using Grid;
using UnityEngine;

namespace Obstacle
{
    public class DestructibleCrate : MonoBehaviour
    {
        public static event EventHandler OnAnyDestroyed;
        
        private GridPosition _gridPosition;

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }
        
        public void Damage()
        {
            Destroy(gameObject);
            
            OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}