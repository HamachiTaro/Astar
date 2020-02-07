using UniRx;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public enum NodeType
    {
        Start,
        Goal,
        UnWalkable,
    }

    public enum State
    {
        None,
        GenerateGrid,
        EditGrid,
        FindPath,
    }

    private ReactiveProperty<State> _stateAsObservable;

    private CompositeDisposable _stateDisposables;

    private Grid _grid;
    
    /// <summary>
    /// UI全体を管理するクラス
    /// </summary>
    [SerializeField] private UIController _uiController;

    [SerializeField] private GridController _gridController;

    void Start()
    {
        _stateDisposables = new CompositeDisposable();

        _stateAsObservable = new ReactiveProperty<State>();
        _stateAsObservable
            .Subscribe(state => {
                _stateDisposables.Clear();

                switch (state)
                {
                    case State.GenerateGrid:
                        OnGenerateGrid();
                        break;
                    
                    case State.EditGrid:
                        OnEdit();
                        break;
                    
                    case State.FindPath:
                        OnFindPath();
                        break;
                }
            })
            .AddTo(gameObject);

        _stateAsObservable.Value = State.GenerateGrid;
    }
    
    private void OnGenerateGrid()
    {
        _grid = _gridController.Generate(10, 10);

        _stateAsObservable.Value = State.EditGrid;
    }

    private void OnEdit()
    {
        _gridController.EditNodeType = NodeType.Start;
        _gridController.EditGrid();
        
        _uiController.OnNodeTypeAsObservable
            .Subscribe(nodeType => _gridController.EditNodeType = nodeType)
            .AddTo(_stateDisposables);
        
        _uiController.ClickSearchAsObservable
            .Subscribe(_ => {
                _gridController.EndEditGrid();
                _stateAsObservable.Value = State.FindPath;
            })
            .AddTo(_stateDisposables);

        _uiController.ClickClearAsObservable
            .Subscribe(_ => _gridController.Clear())
            .AddTo(_stateDisposables);
    }

    private void OnFindPath()
    {
        var aStar = new AStar();
        aStar.FindPath(_grid);
        var path = aStar.Paths;

        Debug.Log("---");
        Debug.Log(path.Count);
        foreach (var p in path)
        {
            Debug.Log(p);
        }
        
        
        _gridController.SetAsPath(path);
    }
    
}