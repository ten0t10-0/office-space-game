using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerScript : MonoBehaviour
{
    private void Start()
    {
        int saveSlot = -1;

        GameMaster gm = GameMaster.Instance;

        if (gm.IsMainMenu)
            saveSlot = gm.SaveSlotCurrent;
        else
            saveSlot = gm.SaveSlotDefault;

        gm.InitializeGame(saveSlot);
    }
}
