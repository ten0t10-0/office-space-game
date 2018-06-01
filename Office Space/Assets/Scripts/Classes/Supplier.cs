using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Supplier
{
    public string Name { get; set; }

    protected Supplier(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return "Name: " + Name;
    }
}
