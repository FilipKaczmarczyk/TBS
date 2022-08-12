using Grid;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform pathfindingGridDebugObjectPrefab;
    
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        _gridSystem = new GridSystem<PathNode>(10, 10, 2f, (g, position) => new PathNode(position));
        
        _gridSystem.CreateDebugObjects(pathfindingGridDebugObjectPrefab);
    }
}
