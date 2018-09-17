using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBPlayer
{
    public string Username { get; set; }
    public int? Experience { get; set; }
    public float? Money { get; set; }

    public override string ToString()
    {
        return string.Format("Username: {0}; Experience: {1}; Money: {2}", Username, Experience.ToString(), Money.ToString());
    }
}
