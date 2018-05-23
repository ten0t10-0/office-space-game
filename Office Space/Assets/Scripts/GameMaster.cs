using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance = null;

    public void Log(string message)
    {
        //currentMessage = message;
        //Debug.Log(currentMessage);
    }

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
    [HideInInspector]
    public CustomizationManager CustomizationManager;
    #endregion

    #region <PLAYER/NPC>
    public GameObject GenericCharacterObject;
    [HideInInspector]
    public GameObject CurrentPlayerObject;
    [HideInInspector]
    public Player Player;
    public string initPlayerName = "New Player";
    public string initBusinessName = "My Business";
    public float initPlayerMoney = 10000;
    public float initPlayerInventorySpace = 100;
    public float initPlayerShopSpace = 10;
    public int initPlayerLevel = 1;
    public int initPlayerExperience = 0;
    #endregion

    #region <GAME>

    #region <Game Data file info>
    public string SaveFileName = "Game";
    public string SaveFileExtension = ".gd";
    private string saveFileDirString;
    #endregion

    #region <Bools>
    public bool UIMode = false;
    public bool OfflineMode = false;
    public bool TEMPSaveGame = true;
    #endregion

    #region <Difficulty>
    public int Difficulty = 0; //0 = Tutorial
    #endregion

    #region <Date & Time>
    [Range(0, 11)]
    public int DayStartHour = 8;
    [Range(12, 23)]
    public int DayEndHour = 20;

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
    private string currentMessage;

    public const string MSG_ERR_DEFAULT = "Uh oh.";
    public const string MSG_GEN_NA = "N/A";
    #endregion

    #endregion

    #region [Classes]
    //User details: username and password + encryption/encoding
    //(For security, maybe only use this class as a private variable within a block so that all info held gets disposed once the code in the block ends.)
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
        #region <Pattern Setup>
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
        CustomizationManager = GetComponent<CustomizationManager>();

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

        currentMessage = MSG_GEN_NA;
    }

    private void Start()
    {
        //***<TEST NEW GAME METHOD>***
        NewGameTEST();
    }

    private void NewGameTEST()
    {
        //out Messages (GUI/Debug purposes)
        string resultGeneric;

        if (!File.Exists(Application.persistentDataPath + saveFileDirString))
        {
            //Player Initializer
            Player = new Player(initPlayerName, initBusinessName, initPlayerMoney, initPlayerInventorySpace, initPlayerShopSpace);

            //Supplier generator
            SupplierManager.GenerateSuppliers(initNumberOfSuppliers, out resultGeneric);
            Debug.Log("*SUPPLIER GENERATOR RESULT: " + resultGeneric);

            //TEST: Adding supplier items
            //SupplierManager.Suppliers[0].Inventory.AddItem(new Item("CPU high"), out resultGeneric);
            //SupplierManager.Suppliers[0].Inventory.AddItem(new Item("GPU medium"), out resultGeneric);
            //SupplierManager.Suppliers[1].Inventory.AddItem(new Item("GPU high"), out resultGeneric);
            //SupplierManager.Suppliers[1].Inventory.AddItem(new Item("gpu low"), out resultGeneric);
            //SupplierManager.Suppliers[2].Inventory.AddItem(new Item("cpu low"), out resultGeneric);

            //TEST: Generating supplier items
            SupplierManager.PopulateSupplierInventories();

            //TEST: Checking for unique items for each supplier
            for (int iSupplier = 0; iSupplier < SupplierManager.Suppliers.Count; iSupplier++)
            {
                int[] supplierUniqueItems = SupplierManager.GetSupplierUniqueItems(iSupplier);
                if (supplierUniqueItems.Length != 0)
                {
                    Debug.Log(string.Format("*Supplier {0} unique items:", iSupplier.ToString()));

                    foreach (int itemId in supplierUniqueItems)
                    {
                        Debug.Log(string.Format("- '{0}'", ItemManager.Database.Items[itemId].Name));
                    }
                }
                else
                {
                    Debug.Log(string.Format("*Supplier {0} has no unique items.", iSupplier.ToString()));
                }
            }
            Debug.Log("*<OK!>");

            //TEST: Adding player items
            Debug.Log(string.Format("Player Inventory space: {0}/{1}", Player.Business.WarehouseInventory.TotalSpaceUsed(), Player.Business.WarehouseInventory.MaximumSpace));
            Player.Business.WarehouseInventory.AddItem(new OrderItem(SupplierManager.Suppliers[0].Inventory.Items[0].ItemID, 5), true, out resultGeneric);
            Debug.Log(resultGeneric);
            Debug.Log(string.Format("Player Inventory space: {0}/{1}", Player.Business.WarehouseInventory.TotalSpaceUsed(), Player.Business.WarehouseInventory.MaximumSpace));
            Player.Business.WarehouseInventory.AddItem(new OrderItem("nothing", 20), true, out resultGeneric);
            Debug.Log(resultGeneric);
            Debug.Log(string.Format("Player Inventory space: {0}/{1}", Player.Business.WarehouseInventory.TotalSpaceUsed(), Player.Business.WarehouseInventory.MaximumSpace));
            Player.Business.WarehouseInventory.AddItem(new OrderItem("nothing", 20), true, out resultGeneric); //Merges with current "Totally Nothing" list item instead of creating new list item
            Debug.Log(resultGeneric);
            Debug.Log(string.Format("Player Inventory space: {0}/{1}", Player.Business.WarehouseInventory.TotalSpaceUsed(), Player.Business.WarehouseInventory.MaximumSpace));

            //TEST: PLAYER *purchasing* items from AI SUPPLIER
            //SaleSupplierToPlayer(0, 1, 1000, true, out resultGeneric); //(Testing; too expensive)
            //Debug.Log("*PURCHASE RESULT: " + resultGeneric);
            //Debug.Log("*Remaining Player money: " + Player.Business.Money);
            //SaleSupplierToPlayer(1, 1, 2, true, out resultGeneric); //Supplier 1; Item 1 (low-end GPU); x2
            //Debug.Log("*PURCHASE RESULT: " + resultGeneric);
            //Debug.Log("*Remaining Player money: " + Player.Business.Money.ToString());

            //TEST: Sending items to shop inventory
            //Player.Business.MoveItemsToShop(0, 500, true, out resultGeneric); //quantity too high
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(0, 3, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(1, 10, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(2, 2, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric); //quantity to be removed = item total quantity; item removed completely from warehouse inventory

            //TEST: Putting items up on special
            //Player.Business.ShopInventory.SetItemsOnSpecial(1, 0.5f, out resultGeneric);
            //Debug.Log("*ITEMS ON SPECIAL RESULT: " + resultGeneric);

            //TEST: Taking items off special
            //Player.Business.ShopInventory.UnsetItemsOnSpecial(0, out resultGeneric);
            //Debug.Log("*ITEMS OFF SPECIAL RESULT: " + resultGeneric);

            //TEST Sending items back to warehouse
            //Player.Business.MoveItemsToWarehouse(0, 5, true, out resultGeneric); //quantity too high
            //Debug.Log("*MOVE ITEMS TO WAREHOUSE RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToWarehouse(0, 3, true, out resultGeneric); //quantity to be removed = item total quantity; item removed completely from shop inventory
            //Debug.Log("*MOVE ITEMS TO WAREHOUSE RESULT: " + resultGeneric);

            //TEST: Adding orders ***
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (Item item in SupplierManager.Suppliers[0].Inventory.Items)
            {
                int qty = UnityEngine.Random.Range(5, 21);
                orderItems.Add(new OrderItem(item, qty));
            }
            OrderManager.OrdersOpen.Add(new Order(CustomerManager.GenerateCustomer(), orderItems, GameDateTime, GameDateTime.AddHours(2)));
            Debug.Log("*ORDER 1: " + OrderManager.OrdersOpen[0].ToString());
            Debug.Log("*ORDER 1 ITEMS:");
            foreach (OrderItem orderItem in OrderManager.OrdersOpen[0].Items)
                Debug.Log(orderItem.ToString());

            //TEST: Spawn (NEW) player
            SpawnPlayer();

            //TEST: Set up Office
            CustomizationManager.Office.SetUpOffice(Player.OfficeCustomizationData);

            //(TEST): Change wall colors
            //CustomizationManager.Office.MaterialWallsCurrent.color = new Color(0.7f, .2f, .9f);

            //  ^ Adding office object:
            //GameObject newObject1 = CustomizationManager.Office.InitializeOfficeObject(0);
            //newObject1.transform.position = new Vector3(-4.33f, 0f, 10.39f);
            //newObject1.transform.rotation = Quaternion.Euler(-90f, 90f, 0f);

            //  ^ Adding office object:
            GameObject newObject2 = CustomizationManager.Office.InitializeOfficeObject(0);
            newObject2.transform.position = new Vector3(0, 0f, -7.63f);
            newObject2.transform.rotation = Quaternion.Euler(-90f, 90f, 0);

            //TEST: Save Game
            SaveGame();
        }
        else
        {
            //TEMP: Delete save game
            //DeleteSave();

            //TEST: Load Game
            LoadGame();

            //TEST: Spawn (EXISTING) player
            SpawnPlayer();

            //TEST: Set Office
            CustomizationManager.Office.SetUpOffice(Player.OfficeCustomizationData);
        }

        #region **DEBUG LOGS**
        CreateDebugLogs();
        #endregion

        Camera.main.GetComponent<CameraController>().SetTarget(CurrentPlayerObject.GetComponent<Rigidbody>().transform);

        tPlayerPlayTime = tGameTime = Time.time;
    }

    /// <summary>
    /// Instantiates a Character object as the player, and binds clothing to the player according to the customization data in the Player class.
    /// </summary>
    private void SpawnPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            CurrentPlayerObject = Instantiate(GenericCharacterObject, Vector3.up, Quaternion.Euler(Vector3.zero));

            CurrentPlayerObject.AddComponent<PlayerController>();
            CurrentPlayerObject.tag = "Player";

            //TEST: Body color change (Before setting player char - changing color in customization data in player class)
            //Player.CharacterCustomizationData.UpdateBodyColorInfo(new Color(0f, 0f, 0f));

            CustomizationManager.Character.SetPlayer(CurrentPlayerObject, Player.CharacterCustomizationData);

            //TEST A: (OK) Body color change (After setting player char - changing color in customization data held by player object customization script component - REQUIRES ReloadCharacterAppearance() method to be called!)
            //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().CustomizationData.UpdateBodyColorInfo(new Color(0f, 0f, 0f));
            //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().ReloadCharacterAppearance();

            //TEST B: (-Nope-) Body color change (After setting player char - directly changing color of MaterialBody field in player object customization script component - *Does NOT update customization data - will not be saved*)
            //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().MaterialBody.color = new Color(0f, 0f, 0f);

            //TEST C: (*Preferred!*) Body color change (After setting player char - Using custom method UpdateBodyColor() )
            //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().UpdateBodyColor(new Color(0f, 0f, 0f));
        }
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

    #region <SALES METHOD(S)>
    // SalePlayerToOnlinePlayer
    // SaleOnlinePlayerToPlayer
    // ^ Both would have to work hand-in-hand

    /// <summary>
    /// Executes a sale where the player purchases items from a supplier. (VALIDATION REQUIRED: Player money and Player inventory space)
    /// </summary>
    /// <param name="iSupplier">The Supplier (index).</param>
    /// <param name="iSupplierItem">The Supplier's Item (index).</param>
    /// <param name="quantity">The quantity to be purchased.</param>
    /// <param name="performValidation">Set to false only if Player money AND Player inventory space has already been validated.</param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool SaleSupplierToPlayer(int iSupplier, int iSupplierItem, int quantity, bool performValidation, out string result)
    {
        bool successful = false;
        result = MSG_ERR_DEFAULT;

        if (iSupplier < SupplierManager.Suppliers.Count)
        {
            if (iSupplierItem < SupplierManager.Suppliers[iSupplier].Inventory.Items.Count)
            {
                OrderItem item = new OrderItem(SupplierManager.Suppliers[iSupplier].Inventory.Items[iSupplierItem], quantity);
                float markup = SupplierManager.Suppliers[iSupplier].MarkupPercentage;

                successful = Player.Business.PurchaseItem(item, markup, performValidation, out result);
            }
            else
                Debug.Log("*INVALID SUPPLIER ITEM INDEX.");
        }
        else
            Debug.Log("*INVALID SUPPLIER INDEX.");

        return successful;
    }
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

    //public string TimeString12(DateTime dateTime)
    //{
    //    return dateTime.ToShortTimeString();
    //}
    //public string TimeString24(DateTime dateTime)
    //{
    //    return dateTime.ToString("HH:mm");
    //}
    #endregion

    #region <SAVING/LOADING>
    private void SaveGame()
    {
        if (TEMPSaveGame)
        {
            BinaryFormatter bf = new BinaryFormatter();

            Player.CharacterCustomizationData = CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().CustomizationData;
            Player.OfficeCustomizationData = CustomizationManager.Office.GetCustomizationData();

            //Save data to GameData object (saveData):
            GameData saveData = new GameData
            {
                Player = this.Player,

                Suppliers = SupplierManager.Suppliers,

                OrdersOpen = OrderManager.OrdersOpen,
                OrdersFilled = OrderManager.OrdersFilled,
                OrdersFailed = OrderManager.OrdersFailed,

                GameDateTime = this.GameDateTime,
                GameTimeSpeed = this.GameTimeSpeed
            };

            FileStream file = File.Create(Application.persistentDataPath + saveFileDirString);

            bf.Serialize(file, saveData);

            file.Close();

            //LOG:
            Debug.Log("GAME DATA SAVED TO '" + Application.persistentDataPath + "'!");
        }
    }

    private void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + saveFileDirString, FileMode.Open);

        GameData loadData = (GameData)bf.Deserialize(file);

        file.Close();

        //Load data from GameData object (loadData):
        Player = loadData.Player;

        SupplierManager.Suppliers = loadData.Suppliers;

        OrderManager.OrdersOpen = loadData.OrdersOpen;
        OrderManager.OrdersFilled = loadData.OrdersFilled;
        OrderManager.OrdersFailed = loadData.OrdersFailed;

        GameDateTime = loadData.GameDateTime;
        GameTimeSpeed = loadData.GameTimeSpeed;

        //LOG:
        Debug.Log("GAME DATA LOADED!");
    }

    private void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + saveFileDirString);
    }
    #endregion

    #region**DEBUG LOGS METHOD**
    private void CreateDebugLogs()
    {
        string lineL = "----------------------------------------------------------------";
        string lineS = "=======================";

        Debug.Log("PLAYER:");
        Debug.Log(lineS);
        Debug.Log("*Player:");
        Debug.Log(Player.ToString());
        Debug.Log("*<OK!>");
        Debug.Log("*Business:");
        Debug.Log(Player.Business.ToString());
        Debug.Log("*<OK!>");
        Debug.Log("*Inventory:");
        Debug.Log(Player.Business.WarehouseInventory.ToString());
        Debug.Log("*<OK!>");
        Debug.Log("*Inventory Items:");
        if (Player.Business.WarehouseInventory.Items.Count != 0)
        {
            foreach (OrderItem item in Player.Business.WarehouseInventory.Items)
                Debug.Log(item.ToString());
        }
        else
        {
            Debug.Log("0");
        }
        Debug.Log("*<OK!>");
        Debug.Log("*SHOP Inventory:");
        Debug.Log(Player.Business.ShopInventory.ToString());
        Debug.Log("*<OK!>");
        Debug.Log("*SHOP Inventory Items:");
        if (Player.Business.ShopInventory.Items.Count != 0)
        {
            foreach (OrderItem item in Player.Business.ShopInventory.Items)
                Debug.Log(item.ToString());
        }
        else
        {
            Debug.Log("0");
        }
        Debug.Log("*<OK!>");
        Debug.Log("*SHOP Special Items:");
        if (Player.Business.ShopInventory.ItemsOnSpecial.Count != 0)
        {
            foreach (SpecialItem item in Player.Business.ShopInventory.ItemsOnSpecial)
                Debug.Log(item.ToString());
        }
        else
        {
            Debug.Log("0");
        }
        Debug.Log(lineL);

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
                foreach (Item item in s.Inventory.Items)
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
