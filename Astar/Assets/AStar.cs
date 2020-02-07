using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    
    /// 保留リスト
    private List<Node> _openList;
    
    /// 確定リスト
    private List<Node> _closedList;
    
    private Grid _grid;
    
    private Node _goalNode;
    
    private Node _startNode;

    /// 最終的な経路
    private List<Node> _paths;
    public List<Node> Paths => _paths;

    /// 上下左右に移動する際のコスト
    private float straightCost = 1f;

    /// 斜めに移動する際のコスト
    private float diagCost = Mathf.Sqrt(2);
    
    public void FindPath(Grid grid)
    {
        _grid = grid;
        _openList = new List<Node>();
        _closedList = new List<Node>();

        _startNode = _grid.StartNode;
        Debug.Log(_startNode.DebugString()); 
        _goalNode = _grid.GoalNode;
        Debug.Log(_goalNode.DebugString()); 

        _startNode.g = 0;
        _startNode.h = EstimationCostToGoal(_startNode);
        _startNode.f = _startNode.g + _startNode.h;
        
        SearchPath();
    }

    bool SearchPath()
    {
        // 現在ノード、スタートノードから開始
        var node = _startNode;

        var count = 0;
        
        while (node != _goalNode)
        {
            // 周辺ノードの開始、終了座標を決定
            var startX = Mathf.Max(0, node.x - 1);
            var endX = Mathf.Min(_grid.XNum - 1, node.x + 1);
            
            var startZ = Mathf.Max(0, node.z - 1);
            var endZ = Mathf.Min(_grid.ZNum - 1, node.z + 1);
            
            // 周辺ノードを走査する
            for (int x = startX; x <= endX; x++)
            {
                Debug.Log("---- x loop ---");
                Debug.Log($"x : {x}");
                for (int z = startZ; z <= endZ; z++)
                {
                    Debug.Log("---- z loop ---");
                    Debug.Log($"z : {z}");
                    
                    var test = _grid.GetNode(x, z);
                    Debug.Log("--");
                    Debug.Log(test.DebugString());
                    
                    // 現在ノードについては調べない。侵入不可の場合も調べない。
                    if (test == node || !test.walkable)
                    {
                        continue;
                    }

                    var cost = (test.x == node.x || test.z == node.z) ? straightCost : diagCost;
                    
                    var g = node.g + cost;
                    var h = EstimationCostToGoal(test);
                    var f = g + h;

                    // すでに保留リストか確定リストに入っている
                    if (IsOpen(test) || IsClosed(test))
                    {
                        Debug.Log("already in the list");
                        if (test.f > f)
                        {
                            Debug.Log("update costs");
                            test.f = f;
                            test.g = g;
                            test.h = h;
                            test.parent = node;
                        }
                    }
                    else
                    {
                        Debug.Log("not in the list. add to _openList");
                        test.f = f;
                        test.g = g;
                        test.h = h;
                        test.parent = node;
                        _openList.Add(test);
                    }
                }
            }
            
            Debug.Log("add to _closedList list : " + node.DebugString());

            if (node != _startNode)
            {
                Debug.Log(node.parent.DebugString());
            }
            else
            {
                Debug.Log("start node does not have parent.");
            }
            _closedList.Add(node);
            
            if (_openList.Count == 0)
            {
                // Debug.Log("no path found");
                return false;
            }
            
            // get the cost which has the minimum f.
            _openList = _openList.OrderBy(x => x.f).ToList();
            node = _openList.First();
            _openList.RemoveAt(0);
            // node = _openList.OrderBy(x => x.f).First();
            
            // Debug.Log("******************");
            //
            // foreach (var hoge in _openList)
            // {
            //     Debug.Log(hoge.DebugString());
            // }
            // Debug.Log("************");
            // Debug.Log("新しいnode : " + node.DebugString());
            
            // count++;
            // Debug.Log($"count is {count}");
        }

        BuildPath();
        return true;
    }

    private void BuildPath()
    {
        _paths = new List<Node>();
        var node = _goalNode;
        Debug.Log("******************");
        Debug.Log(node.DebugString());
        _paths.Add(node);
        
        while (node != _startNode)
        {
            node = node.parent;
            _paths.Insert(0, node);
        }
    }

    /// <summary>
    /// if node is included in open list
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    bool IsOpen(Node node)
    {
        return _openList.Any(n => n == node);
    }

    /// <summary>
    /// if node is included in closed list
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    bool IsClosed(Node node)
    {
        return _closedList.Any(n => n == node);
    }
    
    /// <summary>
    /// get estimation cost from received node to goal
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    float EstimationCostToGoal(Node node)
    {
        var dx = node.x - _goalNode.x;
        var dz = node.z - _goalNode.z;
        var cost = Mathf.Sqrt(dx * dx + dz * dz);
        return cost;
    }
}
