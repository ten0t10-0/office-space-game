using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public string initPlayerName = "New Player";
    public string initBusinessName = "My Business";
    public float initPlayerMoney = 10000;
    public float initPlayerInventorySpace = 100;

    //GAME:
    public int Difficulty = 0; //0 = Tutorial

    public DateTime GameDateTime;

    public int initGameDateYear, initGameDateMonth, initGameDateDay;

    public int initGameTimeHour, initGameTimeMinutes;
    public float GameTimeSpeed = 60; //60: +/-1 second (in reality) is equal to 1 minute in the game.

    [HideInInspector]
    public List<Supplier> Suppliers;
    public int initNumberOfSuppliers = 5;

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

        #region <Initialize Date & Time>
        if (initGameDateYear == 0)
            initGameDateYear = DateTime.Today.Year;
        if (initGameDateMonth == 0)
            initGameDateMonth = DateTime.Today.Month;
        if (initGameDateDay == 0)
            initGameDateDay = DateTime.Today.Day;

        GameDateTime = new DateTime(initGameDateYear, initGameDateMonth, initGameDateDay, 0, 0, 0);

        GameDateTime = GameDateTime.AddHours(initGameTimeHour);
        GameDateTime = GameDateTime.AddMinutes(initGameTimeMinutes);
        #endregion

        //<TEST NEW GAME METHOD>
        NewGameTEST();
    }

    private void NewGameTEST()
    {
        //out Messages (GUI/Debug purposes)
        string genericMessage;
        string generateSuppliersResult;

        //Player Initializer
        Player = new Player(initPlayerName, initPlayerMoney, initBusinessName, initPlayerInventorySpace);

        //Supplier generator (passed from SupplierManager instance)
        Suppliers = supplierManager.GenerateSuppliers(initNumberOfSuppliers, out generateSuppliersResult);

        //TEST: Adding items
        Suppliers[0].Inventory.AddItem(new Item("Table", ItemCategory.Furniture, ItemQuality.Medium, 700, 2), 10, 1, out genericMessage);
        Suppliers[0].Inventory.AddItem(new Item("Designer Table", ItemCategory.Furniture, ItemQuality.High, 3000, 2), 8, 0.9f, out genericMessage);

        #region **DEBUG LOGS**
        Debug.Log("SUPPLIER GENERATOR RESULT: " + generateSuppliersResult);
        CreateDebugLogs();
        #endregion

        tPlayerPlayTime = tGameTime = Time.time;
    }

    private void Update()
    {
        currentTime = Time.time;

        //Increase Player's play time by 1 second if 1 second has passed:
        if (currentTime >= tPlayerPlayTime + 1)
        {
            tPlayerPlayTime = currentTime;
            Player.PlayTime += 1;

            #region **DEBUG PLAY TIME**
            //Debug.Log("Play time (s): " + Player.PlayTime.ToString());
            #endregion
        }

        //Increase in-game time by 1 minute if 60 seconds (divided by Game Time speed) has passed:
        if (currentTime >= (tGameTime + 60/GameTimeSpeed) + Time.deltaTime) // (Time.deltaTime added so that if the game is lagging bad, the in game time will adjust)
        {
            tGameTime = currentTime;
            AdvanceInGameTime(1);
            
            //<Adjust time in GUI with GameTimeString() string method>

            #region **DEBUG GAME TIME**
            //Debug.Log("Game Time: " + GameTimeString12());
            #endregion
        }
    }

    #region <GAME TIME METHODS>
    private void AdvanceInGameTime(int minutesToAdd)
    {
        int previousGameTimeHour = GameDateTime.Hour;

        GameDateTime = GameDateTime.AddMinutes(minutesToAdd);

        if (GameDateTime.Hour == 0 && previousGameTimeHour == 23)
            NextDay();
    }

    private void NextDay() //**Called ONCE from AdvanceGameTime() method, when the clock is set back to 00:00
    {
        //PLAYER INVENTORY ITEMS
        foreach (InventoryItem item in Player.Business.Inventory.InventoryItems)
            item.Age += 1;

        #region **DEBUG NEXT DAY**
        //Debug.Log("NEXT DAY");
        #endregion
    }

    public string GameTimeString12()
    {
        return GameDateTime.ToShortTimeString();
    }
    public string GameTimeString24()
    {
        return GameDateTime.ToString("HH:mm");
    }
    #endregion

    #region**DEBUG LOGS METHOD**
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
    #endregion
}
