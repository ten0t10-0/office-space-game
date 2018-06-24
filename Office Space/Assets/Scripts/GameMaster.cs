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

    #region [Static Methods]
    public static float MarkupPrice(float price, float markup)
    {
        return (price * (1f + markup));
    }

    public static bool Roll(float chance)
    {
        if (chance < 1f)
            return UnityEngine.Random.Range(0f, 1f) <= chance;
        else
            return true;
    }
    #endregion

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
    [HideInInspector]
    public GUIManager GUIManager;
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
    public int PlayerExperienceBase = 100;
    #endregion

    #region <GAME>

    #region <Game Data file info>
    public string SaveFileName = "Game";
    public string SaveFileExtension = ".gd";
    private string saveFileDirString;
    #endregion

    #region <Bools>
    public bool UIMode = false;
    public bool BuildMode = false;
    public bool OfflineMode = false;
    public bool TEMPSaveGame = true;
    public bool TutorialMode = false; //* + Check save data

    [HideInInspector]
    public bool DayEnd = false; //Day at end - No more events until next day (order generation, random events (?), etc)
    #endregion

    #region <Difficulty>
    public int initDifficulty = 0; //0 = First
    public List<DifficultySO> DifficultySettings;
    [HideInInspector]
    public int Difficulty;
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

    private int dayEndCurrent;
    #endregion

    #region <Misc>
    public string CurrencySymbol = "$";
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

    #region <EVENT TIMERS>
    private float chanceNextOrder;
    #endregion

    #region <MESSAGES/NOTIFCATIONS>
    public int MaxStoredNotifications = 50;
    [HideInInspector]
    public NotificationList Notifications;

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
        GUIManager = GetComponent<GUIManager>();

        #region <Manager-specific initializations>
        CustomizationManager.Office.Initialize();
        #endregion

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

        Notifications = new NotificationList();
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
            ////TEST: Adding Notifications
            //for (int c = 1; c <= 3; c++)
            //{
            //    Notifications.Add("TEST Notification " + c.ToString() + ".");
            //}

            ////TEST: Displaying Notifications
            //List<Notification> notifications = Notifications.GetAll();

            //for (int i = notifications.Count - 1; i >= 0; i--)
            //{
            //    Debug.Log(notifications[i].Text);
            //}

            //TEST: Set difficulty
            Difficulty = initDifficulty;

            //TEST: Set events
            chanceNextOrder = GetDifficultySetting().OrderGenerationRate;

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

            ////TEST: PLAYER *purchasing* items from AI SUPPLIER
            //SaleSupplierToPlayer(0, 1, 1000, true, out resultGeneric); //(Testing; too expensive)
            //Debug.Log("*PURCHASE RESULT: " + resultGeneric);
            //Debug.Log("*Remaining Player money: " + Player.Business.Money);
            //SaleSupplierToPlayer(1, 1, 2, true, out resultGeneric); //Supplier 1; Item 1 (low-end GPU); x2
            //Debug.Log("*PURCHASE RESULT: " + resultGeneric);
            //Debug.Log("*Remaining Player money: " + Player.Business.Money.ToString());

            ////TEST: Sending items to shop inventory
            //Player.Business.MoveItemsToShop(0, 500, true, out resultGeneric); //quantity too high
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(0, 3, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(1, 10, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToShop(2, 2, true, out resultGeneric);
            //Debug.Log("*MOVE ITEMS TO SHOP RESULT: " + resultGeneric); //quantity to be removed = item total quantity; item removed completely from warehouse inventory

            ////TEST: Putting items up on special
            //Player.Business.ShopInventory.SetItemsOnSpecial(1, 0.5f, out resultGeneric);
            //Debug.Log("*ITEMS ON SPECIAL RESULT: " + resultGeneric);

            ////TEST: Taking items off special
            //Player.Business.ShopInventory.UnsetItemsOnSpecial(0, out resultGeneric);
            //Debug.Log("*ITEMS OFF SPECIAL RESULT: " + resultGeneric);

            ////TEST: Sending items back to warehouse
            //Player.Business.MoveItemsToWarehouse(0, 5, true, out resultGeneric); //quantity too high
            //Debug.Log("*MOVE ITEMS TO WAREHOUSE RESULT: " + resultGeneric);
            //Player.Business.MoveItemsToWarehouse(0, 3, true, out resultGeneric); //quantity to be removed = item total quantity; item removed completely from shop inventory
            //Debug.Log("*MOVE ITEMS TO WAREHOUSE RESULT: " + resultGeneric);

            ////TEST: Adding orders ***
            //List<OrderItem> orderItems = new List<OrderItem>();
            ////foreach (Item item in SupplierManager.Suppliers[0].Inventory.Items)
            ////{
            ////    int qty = UnityEngine.Random.Range(5, 21);
            ////    orderItems.Add(new OrderItem(item, qty));
            ////}
            //foreach (OrderItem item in Player.Business.WarehouseInventory.Items)
            //{
            //    int qty = 1;
            //    orderItems.Add(new OrderItem(item.ItemID, qty));
            //}
            //OrderManager.Orders.Add(new Order(CustomerManager.GenerateCustomer(), orderItems, GameDateTime, GameDateTime.AddHours(1.5)));
            //Debug.Log("*ORDER 1: " + OrderManager.Orders[0].ToString());
            //Debug.Log("*ORDER 1 ITEMS:");
            //foreach (OrderItem orderItem in OrderManager.Orders[0].Items)
            //    Debug.Log(orderItem.ToString());

            ////*TEST: Generating order
            //OrderManager.GenerateOrder();
            //Debug.Log("*ORDER 1: " + OrderManager.Orders[0].ToString());
            //Debug.Log("*ORDER 1 ITEMS:");
            //foreach (OrderItem orderItem in OrderManager.Orders[0].Items)
            //    Debug.Log(orderItem.ToString());

            ////TEST: Completing order
            //Dictionary<int, int> itemQuantities = new Dictionary<int, int>();
            //foreach (OrderItem item in Player.Business.WarehouseInventory.Items)
            //{
            //    int qty = UnityEngine.Random.Range(1, 3);
            //    itemQuantities.Add(item.ItemID, qty);
            //}
            //CompleteOrder(0, itemQuantities, out resultGeneric);
            //Debug.Log(resultGeneric);
            //Debug.Log(OrderManager.Orders[0].ToString());

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
            int iObject;
            CustomizationManager.Office.InitializeOfficeObject(0, out iObject);
            GameObject newObject2 = CustomizationManager.Office.GetOfficeObject(iObject);
            newObject2.transform.position = new Vector3(0f, 0f, 7.63f);
            newObject2.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CustomizationManager.Office.InitializeOfficeObject(1, out iObject);
            CustomizationManager.Office.GetOfficeObject(iObject).transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CustomizationManager.Office.InitializeOfficeObject(2, out iObject);
            CustomizationManager.Office.GetOfficeObject(iObject).transform.position = new Vector3(1.71f, 2.089318f, 7.24f);

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

        if (!TutorialMode)
        {
            //Increase in-game time by 1 minute if 60 seconds (divided by Game Time speed) have passed:
            if (currentTime >= (tGameTime + 60 / GameTimeSpeed) + Time.deltaTime) // (Time.deltaTime added so that if the game is lagging bad, the in game time will adjust)
            {
                AdvanceInGameTime(1);

                if (!DayEnd)
                {
                    #region <Generate order>
                    if (GetDifficultySetting().GenerateOrders)
                    {
                        if (OrderManager.GetOpenOrders().Count < GetDifficultySetting().MaxSimultaneousOpenOrders)
                        {
                            Debug.Log(chanceNextOrder.ToString());
                            if (Roll(chanceNextOrder))
                            {
                                if (Roll(Player.Business.CustomerTolerance))
                                {
                                    //***
                                    OrderManager.GenerateOrder();

                                    Debug.Log("*ORDER GENERATED!");

                                    Debug.Log("ORDER:");
                                    foreach (OrderItem item in OrderManager.Orders[OrderManager.Orders.Count - 1].Items)
                                        Debug.Log(string.Format("{0} x {1}", item.Quantity.ToString(), item.Name));
                                }
                                else
                                {
                                    //TEMP:
                                    Debug.Log("*TOLERANCE*");
                                }

                                chanceNextOrder = GetDifficultySetting().OrderGenerationRate;
                            }
                            else
                            {
                                chanceNextOrder += GetDifficultySetting().OrderGenerationRate; //keep increasing chance, otherwise player could potentially wait forever :p
                            }
                        }
                    }
                    #endregion

                    #region <Random events, etc>
                    //*
                    #endregion
                }
                else
                {
                    //Checks before next day starts: *
                    if (OrderManager.GetOpenOrders().Count == 0) //Once all orders are closed...
                    {
                        NextDay(); //*
                    }
                }

                #region <Close overdue orders> ***
                for (int i = 0; i < OrderManager.Orders.Count; i++)
                {
                    if (OrderManager.Orders[i].Open)
                    {
                        if (GameDateTime >= OrderManager.Orders[i].DateDue)
                            CancelOrder(i); //* Maybe rather penalize player score if orders are late?
                        else
                            Debug.Log("*Time remaining: " + OrderManager.Orders[i].GetTimeRemaining().ToString()); //***TEMP!
                    }
                }
                #endregion

                tGameTime = currentTime;
            }
        }

        //*<Check for new notifications> (Pop ups) ?

        #region <Player play time>
        //Increase Player's play time by 1 second if 1 second has passed:
        if (currentTime >= tPlayerPlayTime + 1)
        {
            Player.PlayTime += 1;
            tPlayerPlayTime = currentTime;

            #region **DEBUG PLAY TIME**
            //Debug.Log("Play time (s): " + Player.PlayTime.ToString());
            #endregion
        }
        #endregion

        #region <INPUTS>
        //TEST: Saving during gameplay
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.S))
            SaveGame();

        //TEST: Lock & Unlock cursor
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
        #endregion
    }

    #region <SALES METHOD(S)>
    // SalePlayerToOnlinePlayer
    // SaleOnlinePlayerToPlayer
    // ^ Both would have to work hand-in-hand?

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
                float markup = SupplierManager.Suppliers[iSupplier].MarkupPercent;

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

    #region <ORDER METHODS>
    /// <summary>
    /// Complete the specified order with the specified dictionary of items. (Quantities must be validated before this method is called)
    /// </summary>
    /// <param name="iOrderToComplete">The index of the order to complete.</param>
    /// <param name="itemQuantities">Dictionary containing ItemID's as the key, and the quantity as the value.</param>
    public void CompleteOrder(int iOrderToComplete, Dictionary<int, int> itemQuantities, out string result)
    {
        result = MSG_ERR_DEFAULT;

        float payment;
        int score;
        float penaltyMult;

        List<OrderItem> items = new List<OrderItem>();

        result = "";
        string tempResult;

        foreach (int itemID in itemQuantities.Keys)
        {
            items.Add(new OrderItem(itemID, itemQuantities[itemID]));

            for (int iPlayerItem = 0; iPlayerItem < Player.Business.WarehouseInventory.Items.Count; iPlayerItem++)
            {
                if (Player.Business.WarehouseInventory.Items[iPlayerItem].ItemID == itemID)
                {
                    if (itemQuantities[itemID] < Player.Business.WarehouseInventory.Items[iPlayerItem].Quantity)
                    {
                        Player.Business.WarehouseInventory.Items[iPlayerItem].ReduceQuantity(itemQuantities[itemID], false, out tempResult);
                    }
                    else
                    {
                        Player.Business.WarehouseInventory.RemoveItem(iPlayerItem, out tempResult);
                    }

                    result += tempResult + "; ";
                }
            }
        }

        OrderManager.CompleteOrder(iOrderToComplete, items, GameDateTime, out payment, out score, out penaltyMult);

        Player.Business.Money += payment;
        Player.IncreaseExperience(score);

        if (GetDifficultySetting().IncludeCustomerTolerance)
            Player.Business.CustomerTolerance = Mathf.Clamp(Player.Business.CustomerTolerance + (GetDifficultySetting().CustomerToleranceIncrement * penaltyMult), 0f, 1f);
    }

    /// <summary>
    /// Cancels (Fails) an order. Decreases Player Business reputation (Customer Tolerance).
    /// </summary>
    /// <param name="iOrderToCancel"></param>
    public void CancelOrder(int iOrderToCancel)
    {
        OrderManager.CloseOrder(iOrderToCancel);

        if (GetDifficultySetting().IncludeCustomerTolerance)
            Player.Business.CustomerTolerance = Mathf.Clamp(Player.Business.CustomerTolerance - GetDifficultySetting().CustomerToleranceDecrement, 0f, 1f);
    }
    #endregion

    #region <CUSTOMIZATION METHODS>
    //public void PurchaseOfficeItem(int iOfficeItem)
    //{
    //    int iObject;

    //    OfficeItemSO officeItem = CustomizationManager.Office.Items[iOfficeItem];

    //    Player.Business.Money -= officeItem.Price;

    //    CustomizationManager.Office.InitializeOfficeObject(iOfficeItem, out iObject);
    //    CustomizationManager.Office.SelectObject(iObject);
    //}
    #endregion

    #region <GAME TIME METHODS>
    private void AdvanceInGameTime(int minutesToAdd)
    {
        GameDateTime = GameDateTime.AddMinutes(minutesToAdd);

        if (!DayEnd && GameDateTime.Hour >= DayEndHour)
        {
            DayEnd = true;
            dayEndCurrent = GameDateTime.Day;
        }
    }

    private void NewDay()
    {
        if (GameDateTime.Day == dayEndCurrent)
            GameDateTime = GameDateTime.AddDays(1);

        GameDateTime = new DateTime(GameDateTime.Year, GameDateTime.Month, GameDateTime.Day, DayStartHour, 0, 0);

        chanceNextOrder = GetDifficultySetting().OrderGenerationRate;

        DayEnd = false;
    }

    private void NextDay() //**
    {
        #region **DEBUG NEXT DAY**
        Debug.Log("NEXT DAY");
        #endregion

        //*** TEMP:
        NewDay();
    }

    #region <Time Display format methods>
    public string GameTimeString12()
    {
        return TimeString12(GameDateTime);
    }
    public string GameTimeString24()
    {
        return TimeString24(GameDateTime);
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

    #endregion

    #region <MISC METHODS>
    public DifficultySO GetDifficultySetting()
    {
        return DifficultySettings[Difficulty];
    }
    #endregion

    #region <SAVING & LOADING METHODS>
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

                Orders = OrderManager.Orders,

                Difficulty = this.Difficulty,

                GameDateTime = this.GameDateTime,
                GameTimeSpeed = this.GameTimeSpeed,

                ChanceNextOrder = this.chanceNextOrder,

                TutorialMode = this.TutorialMode,
                DayEnd = this.DayEnd,

                DayEndCurrent = this.dayEndCurrent,

                Notifications = this.Notifications
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

        OrderManager.Orders = loadData.Orders;

        Difficulty = loadData.Difficulty;

        GameDateTime = loadData.GameDateTime;
        GameTimeSpeed = loadData.GameTimeSpeed;

        chanceNextOrder = loadData.ChanceNextOrder;

        TutorialMode = loadData.TutorialMode;
        DayEnd = loadData.DayEnd;

        dayEndCurrent = loadData.DayEndCurrent;

        Notifications = loadData.Notifications;

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
