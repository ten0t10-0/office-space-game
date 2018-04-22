using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[SerializeField]
public enum ItemQuality { Low, Medium, High }
[SerializeField]
public enum ItemCategory { None, Electronics, Furniture } //Add...

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance = null;

    #region [Fields]

    #region <SCRIPTS>
    //(Add new scripts to the GameMaster object in Prefabs folder)
    [HideInInspector]
    public SupplierManager SupplierManager;
    [HideInInspector]
    public CustomerManager CustomerManager;
    [HideInInspector]
    public OrderManager OrderManager;
    [HideInInspector]
    public ItemManager ItemManager;
    #endregion

    #region <PLAYER CLASS & INFO>
    public Player Player;
    public string initPlayerName = "New Player";
    public string initBusinessName = "My Business";
    public float initPlayerMoney = 10000;
    public float initPlayerInventorySpace = 100;
    public int initPlayerLevel = 1;
    public int initPlayerExperience = 0;
    #endregion

    #region <GAME>
    public string SaveFileName = "Game";
    public string SaveFileExtension = ".gd";
    private string saveFileDirString;

    public bool UIMode = false;
    public bool OfflineMode = false;

    public int Difficulty = 0; //0 = Tutorial

    public DateTime GameDateTime;

    public int initGameDateYear;
    [Range(0, 12)]
    public int initGameDateMonth;
    [Range(0, 31)]
    public int initGameDateDay;

    [Range(0, 23)]
    public int initGameTimeHour;
    [Range(0, 59)]
    public int initGameTimeMinutes;

    public float GameTimeSpeed = 60; //60: +/-1 second (in reality) is equal to 1 minute in the game.
    #endregion

    #region <SUPPLIER MANAGER INFO>
    public int initNumberOfSuppliers = 5;
    #endregion

    #region <TIMERS/LAPSES>
    private float tPlayerPlayTime;
    private float tGameTime;

    private float currentTime;
    #endregion

    #region <MESSAGES>
    public const string MSG_ERR_DEFAULT = "Uh oh.";

    public const string MSG_GEN_NA = "N/A";
    #endregion

    #endregion

    #region [Classes]
    //User details: username and password + encryption/encoding
    //(For security, maybe only use this class as a private variable within a block/struct so that all info held gets disposed once the code in the block/struct ends.)
    private class UserDetails
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDetails(string username, string password)
        {
            Username = username;
            Password = password;
        }

        private string Encrypt(string s)
        {
            return s;
        } //*
        private string Decrypt(string s)
        {
            return s;
        } //*
    }

    #endregion

    private void Awake()
    {
        #region <SP Setup>
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion

        SupplierManager = GetComponent<SupplierManager>();
        CustomerManager = GetComponent<CustomerManager>();
        OrderManager = GetComponent<OrderManager>();
        ItemManager = GetComponent<ItemManager>();

        #region <Validate Game Save File Name & Extension>
        string tempSaveFileName, tempSaveFileExtension;

        //NAME:
        tempSaveFileName = "";

        for (int i = 0; i < SaveFileName.Length; i++)
        {
            if (Char.IsLetterOrDigit(SaveFileName[i]))
                tempSaveFileName += SaveFileName[i];
        }

        //EXTENSION:
        tempSaveFileExtension = ".";

        if (SaveFileExtension[0] != '.')
            SaveFileExtension = '.' + SaveFileExtension;

        for (int i = 1; i < SaveFileExtension.Length; i++)
        {
            if (Char.IsLetter(SaveFileExtension[i]))
                tempSaveFileExtension += Char.ToLower(SaveFileExtension[i]);
        }

        //FINALLY:
        SaveFileName = tempSaveFileName;
        SaveFileExtension = tempSaveFileExtension;

        saveFileDirString = "/" + SaveFileName + SaveFileExtension;

        //*TEMP:
        Debug.Log(saveFileDirString + "*");
        #endregion

        #region <Initialize Date & Time>
        if (initGameDateYear == 0)
            initGameDateYear = DateTime.Today.Year;
        if (initGameDateMonth == 0)
            initGameDateMonth = DateTime.Today.Month;
        if (initGameDateDay == 0)
            initGameDateDay = DateTime.Today.Day;

        //if invalid init date entered, default to today's date:
        try
        { GameDateTime = new DateTime(initGameDateYear, initGameDateMonth, initGameDateDay, 0, 0, 0); }
        catch
        { GameDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0); }

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

        if (!File.Exists(Application.persistentDataPath + saveFileDirString))
        {
            //Player Initializer
            Player = new Player(initPlayerName, initPlayerMoney, initBusinessName, initPlayerInventorySpace);

            //Supplier generator
            SupplierManager.GenerateSuppliers(initNumberOfSuppliers, out generateSuppliersResult);

            //TEST: Adding items
            SupplierManager.Suppliers[0].Inventory.AddItem(new InventoryItem(new ItemID(1, 0, 0), 10, 1f), out genericMessage);
            SupplierManager.Suppliers[0].Inventory.AddItem(new InventoryItem(new ItemID(2, 0, 2), 5, 1f), out genericMessage);

            //TEST: Adding orders
            OrderManager.OrdersOpen.Add(new Order(CustomerManager.GenerateCustomer(), SupplierManager.Suppliers[0].Inventory.Items, GameDateTime.AddHours(-1), GameDateTime.AddHours(2)));

            //TEST: Save Game
            SaveGame();

            #region **DEBUG LOGS**
            Debug.Log("SUPPLIER GENERATOR RESULT: " + generateSuppliersResult);
            Debug.Log(OrderManager.OrdersOpen[0].DateReceived.ToString());
            CreateDebugLogs();
            #endregion
        }
        else
        {
            //TEST: Load Game
            LoadGame();

            #region ***DEBUG LOGS***
            Debug.Log(OrderManager.OrdersOpen[0].DateReceived.ToString());
            CreateDebugLogs();
            #endregion
        }

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

        //Increase in-game time by 1 minute if 60 seconds (divided by Game Time speed) have passed:
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

    #region <SUPPLIER/PLAYER SALES METHODS>
    // (WIP) *
    #endregion

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
        foreach (InventoryItem item in Player.Business.Inventory.Items)
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

    public string TimeString12(DateTime dateTime)
    {
        return dateTime.ToShortTimeString();
    }
    public string TimeString24(DateTime dateTime)
    {
        return dateTime.ToString("HH:mm");
    }
    #endregion

    #region <SAVING/LOADING>
    private void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        GameData saveData = new GameData(Player, SupplierManager.Suppliers, OrderManager.OrdersOpen, OrderManager.OrdersFilled, OrderManager.OrdersFailed);

        FileStream file = File.Create(Application.persistentDataPath + saveFileDirString);

        bf.Serialize(file, saveData);

        file.Close();

        //LOG:
        Debug.Log("GAME DATA SAVED TO '" + Application.persistentDataPath + "'!");
    }

    private void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + saveFileDirString, FileMode.Open);

        GameData loadData = (GameData)bf.Deserialize(file);

        file.Close();

        //Load data from GameData object:
        Player = loadData.Player;

        SupplierManager.Suppliers = loadData.Suppliers;

        OrderManager.OrdersOpen = loadData.OrdersOpen;
        OrderManager.OrdersFilled = loadData.OrdersFilled;
        OrderManager.OrdersFailed = loadData.OrdersFailed;

        //LOG:
        Debug.Log("GAME DATA LOADED!");
    }
    #endregion

    #region**DEBUG LOGS METHOD**
    private void CreateDebugLogs()
    {
        string lineL = "----------------------------------------------------------------";
        string lineS = "=======================";

        Debug.Log("SUPPLIERS:");
        Debug.Log(lineS);
        foreach (SupplierAI s in SupplierManager.Suppliers)
        {
            Debug.Log("*Supplier:");
            Debug.Log(s.ToString());
            Debug.Log("*<OK!>");
            Debug.Log("*Inventory:");
            Debug.Log(s.Inventory.ToString());
            Debug.Log("*<OK!>");
            Debug.Log("*Inventory Items:");
            if (s.Inventory.Items.Count != 0)
            {
                foreach (InventoryItem item in s.Inventory.Items)
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
