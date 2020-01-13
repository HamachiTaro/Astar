using UnityEngine;

public class ToggleType : MonoBehaviour
{
    [SerializeField] private AppController.NodeType _nodeType;

    public AppController.NodeType NodeType => _nodeType;
}
