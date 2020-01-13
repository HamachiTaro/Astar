using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

/// <summary>
/// マス目の管理。
/// 見た目もデータも扱う。
/// </summary>
public class GridController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject nodePrefab;

    private List<NodeView> _nodeViews;

    private Grid _grid;

    private SingleAssignmentDisposable _editDisposable;
    
    private AppController.NodeType _nodeType;
    public AppController.NodeType EditNodeType {
        get => _nodeType;
        set => _nodeType = value;
    }


    private int _startNodeId = -1;
    
    private int _goalNodeId = -1;
    
    public Grid Generate(int xNum, int zNum)
    {
        _nodeViews = new List<NodeView>();

        var count = 0;
        for (var x = 0; x < xNum; x++)
        {
            for (var z = 0; z < zNum; z++)
            {
                var go = Instantiate(nodePrefab, transform).GetComponent<NodeView>();
                go.transform.position = new Vector3(x, 0, z);
                go.ID = count;
                _nodeViews.Add(go);

                count++;
            }
        }
        
        // データ作成
        _grid = new Grid(xNum, zNum);

        return _grid;
    }

    public void EditGrid()
    {
        _editDisposable = new SingleAssignmentDisposable();
        _editDisposable.Disposable = Observable.EveryUpdate()
            .Subscribe(_ => Editing());
    }

    public void EndEditGrid()
    {
        _editDisposable.Dispose();
    }

    public void SetAsPath(IReadOnlyList<Node> nodes)
    {
        foreach (var node in nodes)
        {
            var id = node.id;
            var nodeView = _nodeViews.First(x => x.ID == id);
            nodeView.SetPath();
        }
    }
    
    
    
    

    /// <summary>
    /// editing the grid
    /// </summary>
    private void Editing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var distance = 100f;
            var duration = 3f;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);

            var hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, distance))
            {
                var hitObject = hit.collider.gameObject;

                if (hitObject.GetComponent<NodeView>())
                {
                    var nodeView = hitObject.GetComponent<NodeView>();
                    
                    // TODO よくない
                    var x = (int)nodeView.transform.position.x;
                    var z = (int)nodeView.transform.position.z;
                    var nodeViewId = nodeView.ID;
                    
                    switch (_nodeType)
                    {
                        case AppController.NodeType.Start:

                            // 
                            if (_startNodeId == -1)
                            {
                                nodeView.SetAsStart();
                                _grid.SetStartNode(x, z);
                                _startNodeId = nodeViewId;
                            }
                            // clear as normal, if the node is the "start".
                            else if(_startNodeId == nodeViewId)
                            {
                                Debug.Log("this node is start. clear");
                                nodeView.SetAsNormal();
                                _grid.ClearStart();
                                _startNodeId = -1;
                            }
                            else
                            {
                                // clear the last start
                                _nodeViews.First(nv => nv.ID == _startNodeId).SetAsNormal();
                                _grid.ClearStart();
                                
                                // new start node
                                _grid.SetStartNode(x, z);
                                nodeView.SetAsStart();
                                _startNodeId = nodeViewId;
                            }
                            break;
                        
                        case AppController.NodeType.Goal:

                            if (_goalNodeId == -1)
                            {
                                nodeView.SetAsGoal();
                                _grid.SetGoalNode(x, z);
                                _goalNodeId = nodeViewId;
                            }
                            // clear as normal, if the node is the goal.
                            else if (_goalNodeId == nodeViewId)
                            {
                                Debug.Log("this node is goal. clear");
                                nodeView.SetAsNormal();
                                _grid.ClearGoal();
                                _goalNodeId = -1;
                            }
                            else
                            {
                                // clear the last start
                                _nodeViews.First(nv => nv.ID == _goalNodeId).SetAsNormal();
                                _grid.ClearGoal();
                                
                                // new start node
                                _grid.SetGoalNode(x, z);
                                nodeView.SetAsGoal();
                                _goalNodeId = nodeViewId;
                            }
                            
                            break;
                        
                        case AppController.NodeType.UnWalkable:

                            // if ()
                            // {
                            //     
                            // }
                            nodeView.SetAsUnWalkable();
                            _grid.SetWalkable(x, z, false);
                            
                            break;
                    }
                }
            }
        }
    }
    
    
}