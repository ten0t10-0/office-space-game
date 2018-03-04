using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizer : MonoBehaviour
{
    //*Clothing object variables (surely a better way to do this)
    public GameObject body;
    public GameObject bodyOnez01;
    public GameObject clothOnez01;
    //private GameObject clothPantsLong, clothPantsShort, clothShirtSleeveLong, etc...
    //*

    bool test;

    private void Start()
    {
        test = false;
        ClothingTest(test);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TestTag"))
        {
            if (!test)
                test = true;
            else
                test = false;

            ClothingTest(test);
        }
    }

    private void ClothingTest(bool b)
    {
        if (b)
        {
            bodyOnez01.SetActive(false);
        }
        else
        {
            bodyOnez01.SetActive(true);
        }
    }
}
