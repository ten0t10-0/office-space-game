using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameMaster;
    //public GameObject soundManager maybe

    private void Awake()
    {
        if (GameMaster.Instance == null)
            Instantiate(gameMaster);
    }
}
