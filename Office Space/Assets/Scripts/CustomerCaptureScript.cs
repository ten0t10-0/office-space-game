using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerCaptureScript : MonoBehaviour
{
    [HideInInspector]
    public int Count { get; private set; }

    private void Awake()
    {
        Count = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterCustomizationScript>() != null)
            Count++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterCustomizationScript>() != null)
            Count--;
    }
}
