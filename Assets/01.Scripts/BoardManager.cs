using System;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject containObject;
    }
    
    private Tilemap _tilemap;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Tile walltiles;
    [SerializeField] private int width, height;
    [SerializeField] private PlayerController player;

    private CellData[,] _boardData;

    public GameObject[] _foodPrefab;
    public GameObject _obstaclePrefab;

    private List<Vector2Int> _emptyCellList = new();
    
    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
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
                    
                    _emptyCellList.Add(new Vector2Int(i, j));
                }
                _tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }

        _emptyCellList.Remove(new Vector2Int(1,1));
        CreateObstacle();
        CreateFood();
    }

    public void CreateFood()
    {
        int foodcount = Random.Range(1, 6);
        
        for (int i = 0; i < foodcount; i++)
        {
            int randomFood = Random.Range(0, _foodPrefab.Length);
            int randomIndex = Random.Range(0, _emptyCellList.Count);

            var food = Instantiate(_foodPrefab[randomFood]);
            food.transform.position = GetCellPosition(_emptyCellList[randomIndex]);
            _boardData[_emptyCellList[randomIndex].x, _emptyCellList[randomIndex].y].containObject = food.GetComponent<CellObject>();
            _emptyCellList.RemoveAt(randomIndex);
        }
    }

    public void CreateObstacle()
    {
        int ObstacleCnt = Random.Range(6, 10);
        
        for (int i = 0; i < ObstacleCnt; i++)
        {
            int randomIndex = Random.Range(0, _emptyCellList.Count);
            var obstacle = Instantiate(_obstaclePrefab);

            Vector2Int cellPos = _emptyCellList[randomIndex];
            obstacle.GetComponent<CellObject>().Init(cellPos);
            obstacle.transform.position = GetCellPosition(cellPos);
            _boardData[cellPos.x, cellPos.y].containObject = obstacle.GetComponent<CellObject>();
            _emptyCellList.RemoveAt(randomIndex);
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

    public void SetCellTile(Vector2Int cellPos, Tile tile)
    {
        _tilemap.SetTile((Vector3Int)cellPos, tile);
    }
}
