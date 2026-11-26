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

    public Tilemap _tilemap;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Tile walltiles;
    [SerializeField] private int width, height;
    [SerializeField] private PlayerController player;

    private CellData[,] _boardData;

    public CellObject[] _foodPrefab;
    public CellObject _obstaclePrefab;
    public CellObject _exitPrefab;
    public CellObject _enemyPrefab;

    private List<Vector2Int> _emptyCellList;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    public void CreateBoard()
    {
        _boardData = new CellData[width, height];

        Tile tile;

        _emptyCellList = new List<Vector2Int>();
        
        for (int i = 0; i < height; i++)
        {

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

        _emptyCellList.Remove(new Vector2Int(1, 1));
        CreateExit();
        CreateEnemy();
        
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
            
            Vector2Int cellPos = _emptyCellList[randomIndex];
            AddObject(food, cellPos);
            _emptyCellList.RemoveAt(randomIndex);
            // food.transform.position = GetCellPosition(_emptyCellList[randomIndex]);
            // _boardData[_emptyCellList[randomIndex].x, _emptyCellList[randomIndex].y].containObject =
            //     food.GetComponent<CellObject>();
            // _emptyCellList.RemoveAt(randomIndex);
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
            AddObject(obstacle, cellPos);
            _emptyCellList.RemoveAt(randomIndex);
        }
    }

    public void CreateExit()
    {
        var exit = Instantiate(_exitPrefab);

        Vector2Int cellPos = new Vector2Int(width - 2, height - 2);
        AddObject(exit, cellPos);
        _emptyCellList.Remove(new Vector2Int(width - 2, height - 2));
    }
    
    public void CreateEnemy()
    {
        var enemy = Instantiate(_enemyPrefab);
    
        Vector2Int cellPos = new Vector2Int(width - 2, height - 3);
        AddObject(enemy, cellPos);
        _emptyCellList.Remove(cellPos);
    }

    public void AddObject(CellObject obj, Vector2Int cellPos)
    {
        obj.Init(cellPos);
        obj.transform.position = GetCellPosition(cellPos);
        _boardData[cellPos.x, cellPos.y].containObject = obj;
    }

    public Tile GetCellTile(Vector2Int cellPos)
    {
        return _tilemap.GetTile<Tile>((Vector3Int)cellPos);
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

    public void Clean()
    {
        if(_boardData == null)
            return;
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var data = _boardData[i, j];

                if (data.containObject != null)
                {
                    Destroy(data.containObject.gameObject);
                }
                
                SetCellTile(new Vector2Int(i, j), null);
            }
        }
    }
}
