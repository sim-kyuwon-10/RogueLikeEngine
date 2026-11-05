using System;
using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int value;
    
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManger.Instance.ChangeFood(value);
    }
}
