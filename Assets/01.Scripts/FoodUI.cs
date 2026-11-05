using System;
using TMPro;
using UnityEngine;

public class FoodUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _foodText;

    private void Start()
    {
        GameManger.Instance.OnFoodAmountChanged += ChangeFoodText;
        _foodText.text = $"Food: {GameManger.Instance._foodCount}";
    }

    private void ChangeFoodText(int food)
    {
        _foodText.text = $"Food: {food}";
    }

    private void OnDestroy()
    {
        GameManger.Instance.OnFoodAmountChanged -= ChangeFoodText;
    }
}
