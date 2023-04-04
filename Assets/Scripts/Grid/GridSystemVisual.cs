using System;
using System.Collections.Generic;
using Actions;
using Units;
using UnityEngine;

namespace Grid
{
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        Orange,
        Green,
        SoftRed
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType type;
        public Material material;
    }
    
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform gridSystemVisualSinglePrefab;
        [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

        private GridSystemVisualSingle[,] _gridSystemVisualSingles;

        private void Start()
        {
            _gridSystemVisualSingles =
                new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
            
            for (var x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (var z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    var gridPosition = new GridPosition(x, z);
                    
                    var gridSystemVisualSingle = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition),
                        Quaternion.identity);

                    _gridSystemVisualSingles[x, z] = gridSystemVisualSingle.GetComponent<GridSystemVisualSingle>();
                }
            }

            UpdateGridVisual();
            
            UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
            LevelGrid.Instance.OnAnyUnitMove += LevelGrid_OnAnyUnitMove;
        }

        private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }
        
        private void LevelGrid_OnAnyUnitMove(object sender, EventArgs e)
        {
            UpdateGridVisual();
        }

        private void HideAllGridPositions()
        {
            for (var x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (var z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    _gridSystemVisualSingles[x, z].Hide();
                }
            }
        }
        
        private void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
        {
            foreach (var gridPosition in gridPositionList)
            {
                _gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
            }
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            var gridPositionsList = new List<GridPosition>();
            
            for (var x = -range; x <= range; x++)
            {
                for (var z = -range; z <= range; z++)
                {
                    var offsetGridPosition = new GridPosition(x, z);
                    var testGridPosition = gridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.CheckValidGridPosition(testGridPosition))
                        continue;

                    var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    
                    if (testDistance > range) 
                        continue;
                    
                    gridPositionsList.Add(testGridPosition);
                }
            }
            
            ShowGridPositionList(gridPositionsList, gridVisualType);
        }

        private void UpdateGridVisual()
        {
            HideAllGridPositions();

            if (UnitActionSystem.Instance.GetSelectedUnit() == null)
                return;
            
            var selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            
            if (selectedAction is ShootAction shootAction)
            {
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.MAXShootDistance,
                    GridVisualType.SoftRed);
            }
            
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), selectedAction.VisualType);
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (var gridVisualTypeMaterial in gridVisualTypeMaterials)
            {
                if (gridVisualTypeMaterial.type == gridVisualType)
                {
                    return gridVisualTypeMaterial.material;
                }
            }

            return null;
        }
    }
}
