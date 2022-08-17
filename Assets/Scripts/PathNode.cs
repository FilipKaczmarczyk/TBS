using Grid;

public class PathNode
{
    private GridPosition _gridPosition;
    private int _gCost;
    private int _hCost;
    private int _fCost;
    private PathNode _cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }
    
    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }
    
    public int GetHCost()
    {
        return _hCost;
    }
    
    public int GetFCost()
    {
        return _fCost;
    }

    public void SetGCost(int gCost)
    {
        _gCost = gCost;
    }
    
    public void SetHCost(int hCost)
    {
        _hCost = hCost;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNode = null;
    }

    public void SetCameFormPathNode(PathNode pathNode)
    {
        _cameFromPathNode = pathNode;
    }
    
    public PathNode GetCameFormPathNode()
    {
        return _cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}
