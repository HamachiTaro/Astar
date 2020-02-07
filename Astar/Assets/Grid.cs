/// <summary>
/// contain the data of position
/// this class does not draw.
/// </summary>
public class Grid
{
    
    private Node[,] _nodes;
    
    private Node _startNode;
    public Node StartNode => _startNode;
    
    private Node _goalNode;
    public Node GoalNode => _goalNode;
    
    private int _xNum;
    public int XNum => _xNum;
    
    private int _zNum;
    public int ZNum => _zNum;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="numRows"></param>
    /// <param name="numCols"></param>
    public Grid(int xNum, int zNum)
    {
        _xNum = xNum;
        _zNum = zNum;
        _nodes = new Node[xNum, zNum];

        var id = 0;
        for (int x = 0; x < _xNum; x++)
        {
            for (int z = 0; z < _zNum; z++)
            {
                var node = new Node(x, z, id);
                _nodes[x, z] = node;

                id++;
            }
        }
    }

    /// <summary>
    /// return thd specified x,z position node
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Node GetNode(int x, int z)
    {
        return _nodes[x, z];
    }

    /// <summary>
    /// set thd node as goal
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void SetGoalNode(int x, int z)
    {
        _goalNode = _nodes[x, z];
    }

    public void ClearGoal()
    {
        _goalNode = null;
    }

    /// <summary>
    /// set the node as start
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void SetStartNode(int x, int z)
    {
        _startNode = _nodes[x, z];
    }

    public void ClearStart()
    {
        _startNode = null;
    }

    /// <summary>
    /// set the node is walkable or not
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="walkable"></param>
    public void SetWalkable(int x, int z, bool walkable)
    {
        _nodes[x, z].walkable = walkable;
    }

    /// <summary>
    /// set all node as normal
    /// </summary>
    public void AllNormal()
    {
        foreach (var node in _nodes)
        {
            node.walkable = false;
        }

        _startNode = null;
        _goalNode = null;
    }
}