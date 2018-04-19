using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Supplier
{
    public string Name { get; set; }
    public Inventory Inventory { get; set; }

    protected Supplier(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return "Name: " + Name;
    }
}
