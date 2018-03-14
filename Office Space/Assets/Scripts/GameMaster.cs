using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemQuality { None, Low, Medium, High }
public enum ItemCategory { None, Furniture, Electronics, Appliances, Clothing, Media } //Add...

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance = null;

    #region Variables
    //SCRIPTS: (Add new scripts to the GameMaster object in Prefabs folder)
    private SupplierManager supplierManager;
    private CustomerManager customerManager;

    //PLAYER:
    public Player Player;
    public string initialPlayerName = "New Player";
    public string initialBusinessName = "My Business";
    public float initialPlayerMoney = 10000;
    public float initialPlayerInventorySpace = 100;

    //GAME:
    public int Difficulty = 0; //0 = Tutorial

    public int GameTimeHour, GameTimeMinutes;
    public float GameTimeSpeed = 60; //60: +/-1 second (in reality) is equal to 1 minute in the game.

    [HideInInspector]
    public List<Supplier> Suppliers;

    //TIMERS/LAPSES:
    private float tPlayerPlayTime;
    private float tGameTime;
    private float currentTime;
    #endregion

    private void Awake()
    {
        #region <Setups>
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion

        supplierManager = GetComponent<SupplierManager>();
        customerManager = GetComponent<CustomerManager>();

        #region <TEST NEW GAME METHOD>
        NewGameTEST();
        #endregion
    }

    private void NewGameTEST()
    {
        Player = new Player(initialPlayerName, initialPlayerMoney, initialBusinessName, initialPlayerInventorySpace);

        Suppliers = supplierManager.GenerateSuppliers(5);

        #region **DEBUG LOGS**
        CreateDebugLogs();
        #endregion

        tPlayerPlayTime = tGameTime = Time.time;

    }

    private void Update()
    {
        currentTime = Time.time;

        if (currentTime >= tPlayerPlayTime + 1)
        {
            tPlayerPlayTime = currentTime;
            Player.PlayTime += 1;

            #region **DEBUG PLAY TIME**
            Debug.Log("Play time (s): " + Player.PlayTime.ToString());
            #endregion
        }

        if (currentTime >= (tGameTime + 60/GameTimeSpeed) + Time.deltaTime) //Multiplied by time.deltaTime so that if the game is lagging bad, the in game time will adjust.
        {
            tGameTime = currentTime;
            AdvanceInGameTime();
            //<Adjust time in GUI with GameTimeString() string method>
            #region **DEBUG GAME TIME**
            //Debug.Log("Game Time: " + GameTimeString());
            #endregion
        }
    }

    #region <GAME TIME METHODS>
    private void AdvanceInGameTime()
    {
        GameTimeMinutes++;

        if (GameTimeMinutes >= 60)
        {
            GameTimeMinutes = 0;

            GameTimeHour++;

            if (GameTimeHour >= 24)
                GameTimeHour = 0;
        }
    }

    public string GameTimeString()
    {
        string gameTimeString = "";

        gameTimeString += GameTimeHour.ToString().PadLeft(2, '0');
        gameTimeString += ":";
        gameTimeString += GameTimeMinutes.ToString().PadLeft(2, '0');

        return gameTimeString;
    }
    #endregion

    private void CreateDebugLogs()
    {
        string lineL = "----------------------------------------------------------------";
        string lineS = "=======================";

        Debug.Log("SUPPLIERS:");
        Debug.Log(lineS);
        foreach (Supplier s in Suppliers)
        {
            Debug.Log("*Supplier:");
            Debug.Log(s.ToString());
            Debug.Log("*<OK!>");
            Debug.Log("*Inventory:");
            Debug.Log(s.Inventory.ToString());
            Debug.Log("*<OK!>");
            Debug.Log("*Inventory Items:");
            if (s.Inventory.InventoryItems.Count != 0)
            {
                foreach (InventoryItem item in s.Inventory.InventoryItems)
                    Debug.Log(item.ToString());
            }
            else
            {
                Debug.Log("0");
            }
            Debug.Log("*<OK!>");
            Debug.Log("*<DONE!>");
            Debug.Log(lineL);
        }
        Debug.Log("*<START!>");
    }
}
