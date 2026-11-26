using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Obstacle : CellObject
{
    public Tile ObstacleTile;
    private Tile _originTile;
    public Tile broeknTile;

    private int maxHealth = 3;
    private int curHealth;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        curHealth = maxHealth;

        _originTile = GameManger.Instance.BoardManager.GetCellTile(_cell);
        GameManger.Instance.BoardManager.SetCellTile(cell, ObstacleTile);
    }

    public override bool PlayerWantedEnter()
    {
        curHealth--;
        if (curHealth == 2)
        {
            GameManger.Instance.BoardManager.SetCellTile(_cell, broeknTile);
        }
        if (curHealth <= 0)
        {
            GameManger.Instance.BoardManager.SetCellTile(_cell, _originTile);
            
            Destroy(gameObject);
        }
        return false;
    }
}
