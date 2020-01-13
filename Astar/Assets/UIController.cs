using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIController : MonoBehaviour
{
    private Subject<AppController.NodeType> subject;

    public IObservable<AppController.NodeType> OnNodeTypeAsObservable => subject;

    [SerializeField] private List<Toggle> _toggleButtons;

    [SerializeField] private Button _searchButton;

    [SerializeField] private Button _clearButton;
    
    public IObservable<Unit> ClickSearchAsObservable => _searchButton.OnClickAsObservable();
    
    public IObservable<Unit> ClickClearAsObservable => _clearButton.OnClickAsObservable();
    
    private void Awake()
    {
        subject = new Subject<AppController.NodeType>();
    }

    void Start()
    {
        // トグルのOnValueChangedAsObservable()をまとめる。
        var toggleEvents = _toggleButtons
            .Select(btn => btn
                // トグルの変更を監視して、    
                .OnValueChangedAsObservable()
                // トグルがONになったら、
                .Where(x => x)
                // トグルボタン自身をストリームにする
                .Select( _ => btn));

        Observable.Merge(toggleEvents)
            .Subscribe(btn => {
                var type = btn.GetComponent<ToggleType>().NodeType;
                subject.OnNext(type);
            })
            .AddTo(gameObject);
    }
}
