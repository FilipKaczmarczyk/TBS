using System.Collections.Generic;
using Grid;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    [SerializeField] private Transform pathfindingGridDebugObjectPrefab;
    [SerializeField] private LayerMask obstacleLayerMask;
    
    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Pathfinding " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
    
        Instance = this;
        
        _gridSystem = new GridSystem<PathNode>(10, 10, 2f, (g, position) => new PathNode(position));
        _gridSystem.CreateDebugObjects(pathfindingGridDebugObjectPrefab);
    }

    public void Setup(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        
        _gridSystem = new GridSystem<PathNode>(width, height, cellSize, (g, position) => new PathNode(position));
        // _gridSystem.CreateDebugObjects(pathfindingGridDebugObjectPrefab);

        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                var gridPosition = new GridPosition(x, z);
                var worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                var raycastOffsetDistance = 5f;
                
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                    raycastOffsetDistance * 2, obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        var openList = new List<PathNode>();
        var closeList = new List<PathNode>();

        var startNode = _gridSystem.GetGridObject(startGridPosition);
        var endNode = _gridSystem.GetGridObject(endGridPosition);
        
        openList.Add(startNode);

        for (var x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (var z = 0; z < _gridSystem.GetHeight(); z++)
            {
                var gridPosition = new GridPosition(x, z);
                var pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            var currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                // REACHED FINAL NODE
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                var tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFormPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        
        // NO PATH FOUND

        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        var gridPositionDistance = gridPositionA - gridPositionB; // DISTANCE BETWEEN TWO POSITIONS
        
        var xDistance = Mathf.Abs(gridPositionDistance.x); // ABSOLUTE VALUE OF X DISTANCE
        var zDistance = Mathf.Abs(gridPositionDistance.z); // ABSOLUTE VALUE OF Z DISTANCE
        
        // FOR EACH PAIR OF DISTANCES X AND Y WILL BE DIAGONALLY
        
        var remainingStraightDistance = Mathf.Abs(xDistance - zDistance); // REMAINING DISTANCE WILL BE STRAIGHT

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + remainingStraightDistance * MOVE_STRAIGHT_COST;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        var lowestFCostPathNode = pathNodeList[0];

        for (var i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }
    
    private List<PathNode> GetNeighbourList(PathNode pathNode)
    {
        var neighbourList = new List<PathNode>();

        var gridPosition = pathNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z)); // LEFT

            if (gridPosition.z - 1>= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); // LEFT DOWN
            }

            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); // LEFT UP
            }
        }

        if (gridPosition.x + 1 < _gridSystem.GetWidth())
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z)); // RIGHT

            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); // RIGHT DOWN
            }

            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); // RIGHT UP
            }
        }

        if (gridPosition.z - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1)); // DOWN
        }

        if (gridPosition.z + 1 < _gridSystem.GetHeight())
        {
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1)); // UP
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        var pathNodeList = new List<PathNode>();
        
        pathNodeList.Add(endNode);

        var currentNode = endNode;

        while (currentNode.GetCameFormPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFormPathNode());
            currentNode = currentNode.GetCameFormPathNode();
        }

        pathNodeList.Reverse();

        var gridPositionList = new List<GridPosition>();

        foreach (var pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
}
