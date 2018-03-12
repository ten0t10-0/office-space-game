using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //Add new scripts to the GameMaster object in Prefabs folder.

    public static GameMaster instance = null;

    public float playerMoney = 10000;

    //+ Any data/info we want to have the game save such as furniture locations, current orders/generated customers/suppliers, etc.
    //Can do it all in this script (i think) or in separate scripts.
    //These values must be able to be retrieved from either a save file or from Azure database (player money/hours played, etc)

	private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
