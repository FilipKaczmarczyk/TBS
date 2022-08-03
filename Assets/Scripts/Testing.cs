using Grid;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual gridSystemVisual;
 
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            var gridPositionList = unit.GetMoveAction().GetValidActionGridPositionList();
            
            gridSystemVisual.HideAllGridPositions();
            
            gridSystemVisual.ShowGridPositionList(gridPositionList);
        }*/
    }
}
