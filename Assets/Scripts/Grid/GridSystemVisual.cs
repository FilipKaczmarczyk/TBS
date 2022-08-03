using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform gridSystemVisualSinglePrefab;

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
        }

        private void Update()
        {
            UpdateGridVisual();
        }

        public void HideAllGridPositions()
        {
            for (var x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            {
                for (var z = 0; z < LevelGrid.Instance.GetHeight(); z++)
                {
                    _gridSystemVisualSingles[x, z].Hide();
                }
            }
        }

        public void ShowGridPositionList(List<GridPosition> gridPositionList)
        {
            foreach (var gridPosition in gridPositionList)
            {
                _gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
            }
        }

        private void UpdateGridVisual()
        {
            HideAllGridPositions();

            if (UnitActionSystem.Instance.GetSelectedUnit() == null)
                return;
            
            var baseAction = UnitActionSystem.Instance.GetSelectedAction();
            
            ShowGridPositionList(baseAction.GetValidActionGridPositionList());
        }
    }
}
