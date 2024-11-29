using UnityEngine;

public struct MoveAction
{
    public Vector2Int newPos;
    public bool destroy;
    public bool fusedInto;
    public MoveAction(Vector2Int newPos, bool destroy)
    {
        this.newPos = newPos;
        this.destroy = destroy;
        this.fusedInto = false;
    }
    public void FuseInto()
    {
        fusedInto = true;
    }
}
