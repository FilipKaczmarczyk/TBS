using System;
using PathfindingSystem;
using Units;
using UnityEngine;

namespace Grid
{
    public class LevelGrid : MonoBehaviour
    {
        public event EventHandler OnAnyUnitMove;
        public static LevelGrid Instance { get; private set; }
    
        [SerializeField] private Transform gridDebugObjectPrefab;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        private GridSystem<GridObject> _gridSystem;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one LevelGrid " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        
            _gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
            // _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        }

        private void Start()
        { 
            Pathfinding.Instance.Setup(width, height, cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }
    
        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            var gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }

        public void UnitMovedGridPosition(GridPosition fromGridPosition, GridPosition toGridPosition, Unit unit)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
        
            OnAnyUnitMove?.Invoke(this, EventArgs.Empty);
        }

        public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
        public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    
        public bool CheckValidGridPosition(GridPosition gridPosition) => _gridSystem.CheckValidGridPosition(gridPosition);
        public int GetWidth() => _gridSystem.GetWidth();
        public int GetHeight() => _gridSystem.GetHeight();
    
        public bool CheckIsUnitAtPosition(GridPosition gridPosition)
        {
            var gridObject = _gridSystem.GetGridObject(gridPosition);
        
            return gridObject.CheckIsOccupied();
        }
    
        public Unit GetUnitAtPosition(GridPosition gridPosition)
        {
            var gridObject = _gridSystem.GetGridObject(gridPosition);
        
            return gridObject.GetUnit();
        }
    }
}
