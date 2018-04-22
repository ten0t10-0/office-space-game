using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public float MarkupPercentage { get; set; }
    public float ConditionPercentage { get; set; } //could also act as the supplier's "rating"

    protected const float markupPercentage_DEFAULT = 0.00f;
    private const float conditionPercentage_DEFAULT = 1.00f; //*^

    #region <Constructors>
    public SupplierAI(string name, float markupPercent, float conditionPercent) : base(name)
    {
        Inventory = new Inventory();

        MarkupPercentage = markupPercent;
        ConditionPercentage = conditionPercent;
    }

    //***(TEMP)
    public SupplierAI(string name) : base(name)
    {
        Inventory = new Inventory();

        MarkupPercentage = markupPercentage_DEFAULT;
        ConditionPercentage = conditionPercentage_DEFAULT;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Markup Percentage: " + MarkupPercentage.ToString() + "; Condition Percentage: " + ConditionPercentage.ToString();
    }
}
