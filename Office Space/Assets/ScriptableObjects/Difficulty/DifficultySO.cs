using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty Setting", menuName = "Difficulty Setting")]
public class DifficultySO : ScriptableObject
{
    public string Description;
    public int LevelRequirement;
    public bool GenerateOrders;
    public int MaxSimultaneousOpenOrders;
    public int MaxOrderItems;
    public int MaxOrderItemQuantity;
    public bool GenerateOrderDueDate;
    public float OrderTimeShippingTimeMultiplier;
    public float OrderTimeAddedPerItem;
    //public float OrderDelay;
    public float OrderGenerationRate;
    public bool IncludeCustomerTolerance;
    public float CustomerToleranceIncrement;
    public float CustomerToleranceDecrement;
}
