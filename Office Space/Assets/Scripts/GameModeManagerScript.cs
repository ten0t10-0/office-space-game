using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Office, Shop, None }

public class GameModeManagerScript : MonoBehaviour
{
    public GameMode InitGameMode = GameMode.None;

    [HideInInspector]
    public GameMode GameMode_Current { get; private set; }

    [HideInInspector]
    public GameModeOffice Office;
    [HideInInspector]
    public GameModeShop Shop;

    private void Awake()
    {
        Office = GetComponent<GameModeOffice>();
        Shop = GetComponent<GameModeShop>();

        ChangeGameMode(InitGameMode);
    }

    public void ChangeGameMode(GameMode gameMode)
    {
        GameMode_Current = gameMode;

        switch (gameMode)
        {
            case GameMode.Office:
                {
                    Shop.enabled = false;
                    Office.enabled = true;

                    break;
                }
            case GameMode.Shop:
                {
                    Office.enabled = false;
                    Shop.enabled = true;

                    break;
                }
            case GameMode.None:
                {
                    Office.enabled = false;
                    Shop.enabled = false;

                    break;
                }
        }
    }
}
