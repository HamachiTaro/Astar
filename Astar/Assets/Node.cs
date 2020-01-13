using UnityEngine;

public class Node
{
    public Vector2 pos;

    public int x;
    public int z;
    
    /// スタート地点までのコストとゴールまでのコストの合計
    public float f;
    /// スタート地点からノードまでのコスト
    public float g;
    /// ノードからゴールまでのコスト
    public float h;
    public bool walkable;
    public Node parent;

    public int id;
    
    public Node(int x, int z, int id)
    {
        this.x = x;
        this.z = z;
        walkable = true;

        this.id = id;
    }

    public string DebugString()
    {
        return $"ID : {id}, x: {x}, z :{z}, f:{f}, g:{g}, h:{h}";
    }
}
