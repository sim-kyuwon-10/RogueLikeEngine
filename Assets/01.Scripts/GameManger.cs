using System;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance { get; private set; }
    
    [field:SerializeField]public BoardManager BoardManager { get; private set; }
    [field:SerializeField]public PlayerController PlayerController { get; private set; }
    [field:SerializeField]public TurnManager TurnManager { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        TurnManager = new TurnManager();
        
        BoardManager.CreateBoard();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }
}
