using System;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyObject : CellObject
{
    private BoardManager _board;
    
    private void Start()
    {
        _board = GameManger.Instance.BoardManager;
        GameManger.Instance.TurnManager.OnTick += TurnHappend;
    }

    private void OnDestroy()
    {
        GameManger.Instance.TurnManager.OnTick -= TurnHappend;
    }

    private void TurnHappend()
    {
        var playerCell = GameManger.Instance.PlayerController.Cellpos;

        int xDist = playerCell.x - _cell.x;
        int yDist = playerCell.y - _cell.y;

        int absXDist = math.abs(xDist);
        int absYDist = math.abs(yDist);

        if ((absXDist == 0 && absYDist == 1) || (absXDist == 1 && absYDist == 0))
        {
            var currentCell = _board.GetCellData(_cell);
            currentCell.containObject = null;

            transform.position = _board.GetCellPosition(playerCell);
            
            GameManger.Instance.Boom();
            GameManger.Instance.OnGameOver(true);
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveX(xDist))
                {
                    TryMoveY(yDist);
                }
            }
            else
            {
                if (!TryMoveY(yDist))
                {
                    TryMoveX(xDist);
                }
            }
        }
    }

    private bool TryMoveX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(_cell + Vector2Int.right);
        }
        
        return MoveTo(_cell + Vector2Int.left);
    }
    
    private bool TryMoveY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(_cell + Vector2Int.up);
        }
        
        return MoveTo(_cell + Vector2Int.down);
    }

    private bool MoveTo(Vector2Int cellPos)
    {
        var data = _board.GetCellData(cellPos);

        if (data == null || !data.Passable || data.containObject != null)
        {
            return false;
        }

        var currentCell = _board.GetCellData(_cell);
        currentCell.containObject = null;

        data.containObject = this;
        _cell = cellPos;
        transform.position = _board.GetCellPosition(_cell);

        return true;
    }
    
    
}
