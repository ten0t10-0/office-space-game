using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupplierAI : Supplier
{
    public float MarkupPercentage { get; set; }

    protected const float markupPercentage_DEFAULT = 0.00f;

    #region <Constructors>
    public SupplierAI(string name, float markupPercent) : base(name)
    {
        Inventory = new Inventory();

        MarkupPercentage = markupPercent;
    }

    //***(TEMP)
    public SupplierAI(string name) : base(name)
    {
        Inventory = new Inventory();

        MarkupPercentage = markupPercentage_DEFAULT;
    }
    #endregion

    //TEMP:
    public override string ToString()
    {
        return base.ToString() + "; Markup Percentage: " + MarkupPercentage.ToString() + "; Condition Percentage: " + ConditionPercentage.ToString();
    }
}
