using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supplier
{
    public string Name { get; set; }
    public Inventory Inventory { get; set; }
    public float MarkupPercentage { get; set; }
    public float BuyPriceMultiplier { get; set; }
    public float ConditionPercentage { get; set; } //could also act as the supplier's "rating"

    private const float markupPercentage_DEFAULT = 0.00f;
    private const float buyPriceMultiplier_DEFAULT = 1.00f;
    private const float conditionPercentage_DEFAULT = 1.00f;

    #region <Constructors>
    //Player Supplier/Business
    public Supplier(string name, float maximumInventorySpace)
    {
        Name = name;
        Inventory = new Inventory(maximumInventorySpace);

        MarkupPercentage = markupPercentage_DEFAULT;
        BuyPriceMultiplier = buyPriceMultiplier_DEFAULT;
        ConditionPercentage = conditionPercentage_DEFAULT;
    }

    //AI Supplier
    public Supplier(string name, float markupPercent, float buyPriceMult, float conditionPercent)
    {
        Name = name;
        Inventory = new Inventory();

        MarkupPercentage = markupPercent;
        BuyPriceMultiplier = buyPriceMult;
        ConditionPercentage = conditionPercent;
    }

    //***(TEMP) AI Supplier
    public Supplier(string name)
    {
        Name = name;
        Inventory = new Inventory();

        MarkupPercentage = markupPercentage_DEFAULT;
        BuyPriceMultiplier = buyPriceMultiplier_DEFAULT;
        ConditionPercentage = conditionPercentage_DEFAULT;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return "Name: " + Name + "; Markup Percentage: " + MarkupPercentage.ToString() + "; Buy Price Multiplier: " + BuyPriceMultiplier.ToString() + "; Condition Percentage: " + ConditionPercentage.ToString();
    }
}
