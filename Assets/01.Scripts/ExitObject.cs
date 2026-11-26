using UnityEngine;
using UnityEngine.WSA;
using Tile = UnityEngine.Tilemaps.Tile;

public class ExitObject : CellObject
{
    public Tile _exitTile;
    
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManger.Instance.BoardManager.SetCellTile(cell, _exitTile);
    }

    public override void PlayerEntered()
    {
        GameManger.Instance.NewLevel();
    }
}
