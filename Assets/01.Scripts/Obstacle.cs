using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacle : CellObject
{
    public Tile tile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManger.Instance.BoardManager.SetCellTile(cell, tile);
    }
}
