using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }
    
    private Tilemap _tilemap;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Tile walltiles;
    [SerializeField] private int width, height;
    [SerializeField] private PlayerController player;

    private CellData[,] _boardData;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        CreateBoard();
    }

    public void CreateBoard()
    { 
        _boardData = new CellData[width, height];
        
        for (int i = 0; i < height; i++)
        {
            Tile tile;
            
            for (int j = 0; j < width; j++)
            {
                _boardData[i, j] = new CellData();
                
                if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                {
                    tile = walltiles;
                    
                    _boardData[i, j].Passable = false;
                }
                else
                {
                    int randomIndex = Random.Range(0, tiles.Length);
                    tile = tiles[randomIndex];
                    
                    _boardData[i, j].Passable = true;
                }
                _tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }

    public Vector3 GetCellPosition(Vector2Int cellIndex)
    {
        return _tilemap.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellPos)
    {
        if (cellPos.x < 0 || cellPos.x >= width || cellPos.y < 0 || cellPos.y >= height)
        {
            return null;
        }
        return _boardData[cellPos.x, cellPos.y];
    }
}
