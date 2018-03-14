using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{

    public string Name { get; set; }

    public Customer(string name)
    {
        Name = name;
        //...
    }
}
