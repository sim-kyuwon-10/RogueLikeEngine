using System;
using UnityEngine;

public class TurnManager
{
    private int _turnCount;
    public event Action OnTick;

    public TurnManager()
    {
        _turnCount = 1;
    }

    public void Tick()
    {
        OnTick?.Invoke();
        _turnCount++;
        Debug.Log($"CurrentTurn: {_turnCount}");
    }
}
