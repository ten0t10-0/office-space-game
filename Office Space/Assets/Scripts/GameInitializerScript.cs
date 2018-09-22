using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerScript : MonoBehaviour
{
    private void Start()
    {
        GameMaster.Instance.Skybox_Current = new Material(GameMaster.Instance.SkyboxDayMaterial);
        RenderSettings.skybox = GameMaster.Instance.Skybox_Current;
        DynamicGI.UpdateEnvironment();

        int saveSlot = -1;

        GameMaster gm = GameMaster.Instance;

        if (gm.IsMainMenu)
            saveSlot = gm.SaveSlotCurrent;
        else
            saveSlot = gm.SaveSlotDefault;

        gm.InitializeGame(saveSlot);
    }

    private void OnDestroy()
    {
        Destroy(GameMaster.Instance.Skybox_Current);
    }
}
