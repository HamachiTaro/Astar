using UnityEngine;

public class NodeView : MonoBehaviour
{
    private Material mat;
    
    public int ID { get; set; }
    
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    public void SetAsStart()
    {
        mat.SetColor("_Color", Color.red);
    }

    public void SetAsGoal()
    {
        mat.SetColor("_Color", Color.blue);
    }

    public void SetAsUnWalkable()
    {
        mat.SetColor("_Color", Color.black);
    }

    public void SetPath()
    {
        mat.SetColor("_Color", Color.cyan);
    }

    public void SetAsNormal()
    {
        mat.SetColor("_Color", Color.white);
    }
}
