using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _cellPos;
    public bool _isDead = false;

    public bool IsDead => _isDead;
    public Vector2Int Cellpos => _cellPos;

    private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Start()
    {
        GameManger.Instance.OnGameOver += Dead;
    }

    private void Dead(bool value)
    {
        _isDead = value;
    }

    private void OnDestroy()
    {
        GameManger.Instance.OnGameOver -= Dead;
    }

    public void Spawn(BoardManager boardManager, Vector2Int cellPos)
    {
        _boardManager = boardManager;
        _cellPos = cellPos;

        transform.position = boardManager.GetCellPosition(cellPos);
    }

    private void Update()
    {
        if (_isDead)
        return;
        
        CheckMove();
    }
    
    
    public void CheckMove()
    {
        Vector2Int nextCell = _cellPos;
        
        bool hasMoved = false;
        
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            nextCell.y += 1;
            hasMoved = true;
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            nextCell.y -= 1;
            hasMoved = true;
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            nextCell.x -= 1;
            hasMoved = true;
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            nextCell.x += 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData = _boardManager.GetCellData(nextCell);

            if (cellData != null && cellData.Passable)
            {
                if (cellData.containObject == null)
                {
                    transform.position = _boardManager.GetCellPosition(nextCell);
                    _cellPos = nextCell;
                }
                else if (cellData.containObject.PlayerWantedEnter())
                {
                    transform.position = _boardManager.GetCellPosition(nextCell);
                    _cellPos = nextCell;
                    
                    cellData.containObject.PlayerEntered();
                }
                else
                {
                    _playerAnimation.Attack();
                }
                GameManger.Instance.TurnManager.Tick();
            }
        }


    }
}
