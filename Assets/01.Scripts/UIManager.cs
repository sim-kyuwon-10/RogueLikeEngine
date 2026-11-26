using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _curLv;
    
    [SerializeField] private GameObject _gameOver;

    private void Start()
    {
        GameManger.Instance.OnFoodAmountChanged += ChangeFoodText;
        GameManger.Instance.OnGameOver += GameOver;

        GameManger.Instance.OnLvChanged += Level;
        
        
        _foodText.text = $"Food: {GameManger.Instance.FoodCount}";
        _curLv.text = $"LV.{GameManger.Instance.Level}";
        
        _gameOver.SetActive(false);
    }

    private void Level(int lv)
    {
        _curLv.text = $"LV.{lv}";
    }

    private void GameOver(bool value)
    {
        if (value)
            _gameOverText.text = $"YOU SURVIVED {GameManger.Instance.Level}LEVELS";
        
        _gameOver.SetActive(value);
    }

    private void ChangeFoodText(int food)
    {
        _foodText.text = $"Food: {food}";
    }

    private void OnDestroy()
    {
        GameManger.Instance.OnFoodAmountChanged -= ChangeFoodText;
        GameManger.Instance.OnGameOver -= GameOver;
        GameManger.Instance.OnLvChanged -= Level;
    }
}
