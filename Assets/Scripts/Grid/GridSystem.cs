using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Grid
{
    public class GridSystem<TGridObject>
    {
        private int _width;
        private int _height;
        private float _cellSize;
    
        private TGridObject[,] _gridObjects;
    
        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _gridObjects = new TGridObject[_width,_height];

            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    _gridObjects[x, z] = createGridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition(
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
        }

        public void CreateDebugObjects(Transform debugPrefab)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var z = 0; z < _height; z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    
                    var debugTransform = Object.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    var gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }
        
        public TGridObject GetGridObject(GridPosition gridPosition)
        {
            return _gridObjects[gridPosition.x, gridPosition.z];
        }

        public bool CheckValidGridPosition(GridPosition gridPosition)
        {
            return gridPosition.x >= 0 && 
                   gridPosition.x < _width && 
                   gridPosition.z >= 0 && 
                   gridPosition.z < _height;
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }
    }
}
