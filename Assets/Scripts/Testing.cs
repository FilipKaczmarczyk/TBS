using Grid;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual gridSystemVisual;
 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            var startGridPosition = new GridPosition(0, 0);

            var gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (var i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]), Color.white, 10f);
            }
        }
    }
}
