using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Supplier
{
    public string Name { get; set; }
    public int Level { get; set; }

    /// <summary>
    /// New Supplier - Level 1 (Player).
    /// </summary>
    /// <param name="name"></param>
    protected Supplier(string name)
    {
        Name = name;
        Level = 1;
    }

    /// <summary>
    /// New Supplier - Custom level (AI).
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    protected Supplier(string name, int level)
    {
        Name = name;
        Level = level;
    }

    public float GetMarkup()
    {
        return (float)Level / 10;
    }

    public override string ToString()
    {
        return "Name: " + Name + "; Level: " + Level.ToString();
    }
}
