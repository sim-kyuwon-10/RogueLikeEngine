using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance { get; private set; }
    
    [field:SerializeField]public BoardManager BoardManager { get; private set; }
    [field:SerializeField]public PlayerController PlayerController { get; private set; }
    [field:SerializeField]public TurnManager TurnManager { get; private set; }

    public int FoodCount { get; private set; }

    public Action<int> OnFoodAmountChanged;
    public Action<int> OnLvChanged;
    public Action<bool> OnGameOver;
    
    public GameObject target;

    private int level = 1;
    private int foodDefault = 10;
    private bool gameOver = true;

    public int Level => level;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappened;
    }

    private void Start()
    {
        StartNewLeveL();
    }

    private void Update()
    {
        if (PlayerController.IsDead)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Replay();
            }
        }
    }

    public void Boom()
    {
        Destroy(target);
    }

    private void Replay()
    {
        gameOver = false;
        
        StartNewLeveL();
    }

    private void OnTurnHappened()
    {
        ChangeFood(-1);
        OnFoodAmountChanged?.Invoke(FoodCount);
    }

    public void ChangeFood(int amount)
    {
        FoodCount += amount;

        if (FoodCount <= 0)
        {
            FoodCount = 0;
            OnGameOver?.Invoke(true);
        }
        
        OnFoodAmountChanged?.Invoke(FoodCount);
    }
    
    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.CreateBoard();
        level++;
        
        FoodCount = foodDefault;
        OnFoodAmountChanged?.Invoke(FoodCount);
        OnLvChanged?.Invoke(level);
        
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
    }

    public void StartNewLeveL()
    {
        level = 1;

        FoodCount = foodDefault;
        
        BoardManager.Clean();
        BoardManager.CreateBoard();
        
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
        
        OnFoodAmountChanged?.Invoke(FoodCount);
        OnLvChanged?.Invoke(level);
        
        OnGameOver?.Invoke(false);
    }
}
