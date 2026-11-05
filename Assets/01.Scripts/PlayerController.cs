using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _cellPos;

    public void Spawn(BoardManager boardManager, Vector2Int cellPos)
    {
        _boardManager = boardManager;
        _cellPos = cellPos;

        transform.position = boardManager.GetCellPosition(cellPos);
    }

    private void Update()
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
                GameManger.Instance.TurnManager.Tick();
                transform.position = _boardManager.GetCellPosition(nextCell);
                _cellPos = nextCell;
                
                if(cellData.containObject != null)
                    cellData.containObject.PlayerEntered();
            }
        }
    }
}
