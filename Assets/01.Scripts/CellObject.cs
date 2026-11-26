using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int _cell;
    
    public virtual void Init(Vector2Int cell)
    {
        _cell = cell;
    }

    public virtual bool PlayerWantedEnter()
    {
        return true;
    }
    
    public virtual void PlayerEntered()
    {
        
    }
}
