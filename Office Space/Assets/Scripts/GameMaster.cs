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
    public static float DiscountPrice(float price, float discount)
    {
        return (price * (1f - discount));
    }

    public static bool Roll(float chance)
    {
        if (chance < 1f && chance > 0f)
            return UnityEngine.Random.Range(0f, 1f) <= chance;
        else if (chance >= 1f)
            return true;
        else
            return false;
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
    [HideInInspector]
    public NPCManager NPCManager;
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
    public float initPlayerMarkup = 0.15f;
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
    public int SaveCountMax = 10;
    public int SaveSlotDefault = 0;
    private int SaveSlotCurrent;
    #endregion

    #region <Bools>
    public bool UIMode = false;
    public bool BuildMode = false;
    public bool OfflineMode = false;
    public bool TEMPSaveGame = true;
    public bool SleepMode = false; //* + Check save data

    [HideInInspector]
    public bool CameraLock = false;

    [HideInInspector]
    public bool DayEnd = false; //Day at end - No more events until next day (order generation, random events (?), etc)
    #endregion

    #region <Difficulty>
    public int initDifficulty = 0; //0 = First
    public List<DifficultySO> DifficultySettings;
    [HideInInspector]
    public int Difficulty { get; private set; }
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
        NPCManager = GetComponent<NPCManager>();

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
        #endregion

        #region <Validate Init Date & Time>
        if (initGameDateYear == 0)
            initGameDateYear = DateTime.Today.Year;
        if (initGameDateMonth == 0)
            initGameDateMonth = DateTime.Today.Month;
        if (initGameDateDay == 0)
            initGameDateDay = DateTime.Today.Day;
        #endregion

        currentMessage = MSG_GEN_NA;

        SaveSlotCurrent = SaveSlotDefault;
    }

    private void Start() //***
    {
        bool saveSlotFound = false;

        for (int i = 0; i < SaveCountMax; i++)
        {
            if (File.Exists(GetSaveFilePath(i)))
            {
                saveSlotFound = true;
                SaveSlotCurrent = i;
                i = SaveCountMax; //break
            }
        }

        if (!saveSlotFound)
        {
            NewGame();
            //NewGameTEST();
        }
        else
        {
            LoadGame(SaveSlotCurrent);

            //Set up area
            CustomizationManager.Office.SetUpOffice(Player.OfficeCustomizationData);
        }

        #region
        //Spawn player
        SpawnPlayer();

        //Set Camera target
        Camera.main.GetComponent<CameraController>().SetTarget(CurrentPlayerObject.GetComponent<Rigidbody>().transform);
        #endregion

        //**TEST**
        //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().UnsetClothing(ClothingSlot.Upper);
        //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().UnsetClothing(ClothingSlot.Lower);
        //CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().SetClothing(4);
    }

    public void InitializeGame()
    {
        //UIMode = false;
        BuildMode = false;
        //OfflineMode = false;
        //TutorialMode = false;

        ModeSetPlay();

        NPCManager.DestroyAllNPCs();

        tPlayerPlayTime = tGameTime = Time.time;
    }

    public void NewGame()
    {
        string message;

        //Initialize game
        InitializeGame();

        //Initialize Date & Time
        GameDateTime = new DateTime(initGameDateYear, initGameDateMonth, initGameDateDay, 0, 0, 0);
        GameDateTime = GameDateTime.AddHours(initGameTimeHour);
        GameDateTime = GameDateTime.AddMinutes(initGameTimeMinutes);

        //Clear notifications
        Notifications = new NotificationList();

        //Set Difficulty
        Difficulty = initDifficulty;

        //Set events
        chanceNextOrder = GetDifficultySetting().OrderGenerationRate;

        //Initialize Player
        Player = new Player(initPlayerName, initBusinessName, initPlayerLevel, initPlayerExperience, initPlayerMoney, initPlayerMarkup, initPlayerInventorySpace, initPlayerShopSpace);

        //Generate Suppliers
        SupplierManager.GenerateSuppliers(initNumberOfSuppliers, out message);

        //TEST: Generating supplier items *
        SupplierManager.PopulateSupplierInventories();

        //  ^ Adding office object:
        int iObject;
        CustomizationManager.Office.InitializeOfficeObject(0, out iObject);
        GameObject newObject2 = CustomizationManager.Office.CurrentObjects[iObject];
        newObject2.transform.position = new Vector3(0f, 0f, 7.63f);
        newObject2.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        GameObject.Find("monitor").GetComponent<OfficeObjectScript>().SetParent(iObject);
    }

    private void NewGameTEST()
    {
        InitializeGame();

        #region <Initialize Date & Time>
        GameDateTime = new DateTime(initGameDateYear, initGameDateMonth, initGameDateDay, 0, 0, 0);

        GameDateTime = GameDateTime.AddHours(initGameTimeHour);
        GameDateTime = GameDateTime.AddMinutes(initGameTimeMinutes);
        #endregion

        Notifications = new NotificationList();

        //out Messages (GUI/Debug purposes)
        string resultGeneric;

        if (!File.Exists(GetSaveFilePath(SaveSlotDefault)))
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
            Player = new Player(initPlayerName, initBusinessName, initPlayerLevel, initPlayerExperience, initPlayerMoney, initPlayerMarkup, initPlayerInventorySpace, initPlayerShopSpace);

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
            GameObject newObject2 = CustomizationManager.Office.CurrentObjects[iObject];
            newObject2.transform.position = new Vector3(0f, 0f, 7.63f);
            newObject2.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CustomizationManager.Office.InitializeOfficeObject(1, out iObject);
            CustomizationManager.Office.CurrentObjects[iObject].transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CustomizationManager.Office.InitializeOfficeObject(2, out iObject);
            CustomizationManager.Office.CurrentObjects[iObject].transform.position = new Vector3(1.71f, 2.089318f, 7.24f);
            CustomizationManager.Office.CurrentObjects[iObject].GetComponent<OfficeObjectScript>().SetParent(0);

            CustomizationManager.Office.InitializeOfficeObject(2, out iObject);

            CustomizationManager.Office.InitializeOfficeObject(2, out iObject);
            CustomizationManager.Office.CurrentObjects[iObject].transform.position = new Vector3(2f, 0f, 0f);

            //TEST: Save Game
            SaveGame(SaveSlotDefault);
        }
        else
        {
            //TEMP: Delete save game
            //DeleteSave();

            //TEST: Load Game
            LoadGame(SaveSlotDefault);

            //TEST: Spawn (EXISTING) player
            SpawnPlayer();

            //TEST: Set Office
            CustomizationManager.Office.SetUpOffice(Player.OfficeCustomizationData);
        }

        #region **DEBUG LOGS**
        CreateDebugLogs();
        #endregion

        Camera.main.GetComponent<CameraController>().SetTarget(CurrentPlayerObject.GetComponent<Rigidbody>().transform);
    }

    /// <summary>
    /// Instantiates a Character object as the player, and binds clothing to the player according to the customization data in the Player class.
    /// </summary>
    private void SpawnPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            CurrentPlayerObject = Instantiate(GenericCharacterObject, Vector3.up, Quaternion.Euler(Vector3.zero));
            CurrentPlayerObject.name = "Character (PLAYER)";

            CurrentPlayerObject.AddComponent<PlayerController>();
            CurrentPlayerObject.tag = "Player";

            CurrentPlayerObject.GetComponent<PlayerController>().Initialize();

            CustomizationManager.Character.SetPlayer(CurrentPlayerObject, Player.CharacterCustomizationData);
        }
    }

    private void Update()
    {
        currentTime = Time.time;

        if (!SleepMode)
        {
            float timeAdvance = (tGameTime + 60 / GameTimeSpeed) + Time.deltaTime; // (Time.deltaTime added so that if the game is lagging bad, the in game time will adjust)

            //Increase in-game time by 1 minute if 60 seconds (divided by Game Time speed) have passed:
            if (currentTime >= timeAdvance) 
            {
                AdvanceInGameTime(1);

                if (!DayEnd)
                {
                    #region <Generate order>
                    if (GetDifficultySetting().GenerateOrders)
                    {
                        if (OrderManager.GetOpenOrders().Count < GetDifficultySetting().MaxSimultaneousOpenOrders)
                        {
                            //Debug.Log(chanceNextOrder.ToString());

                            if (Roll(chanceNextOrder))
                            {
                                if (Roll(Player.Business.CustomerTolerance))
                                {
                                    //***
                                    OrderManager.GenerateOrder();

                                    Debug.Log("*ORDER GENERATED*");

                                    //Debug.Log("ORDER:");
                                    //foreach (OrderItem item in OrderManager.Orders[OrderManager.Orders.Count - 1].Items)
                                    //    Debug.Log(string.Format("{0} x {1}", item.Quantity.ToString(), item.Name));
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
                    #region <Next day & checks>
                    if (OrderManager.GetOpenOrders().Count == 0) //Once all orders are closed...
                    {
                        NextDay(); //*
                    }
                    #endregion
                }

                #region <Close overdue orders> ***
                for (int i = 0; i < OrderManager.Orders.Count; i++)
                {
                    if (OrderManager.Orders[i].Open)
                    {
                        if (GameDateTime >= OrderManager.Orders[i].DateDue)
                            CancelOrder(i);
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
            SaveGame(SaveSlotCurrent);

        //TEST: Lock & Unlock cursor / Sleep mode toggle
        if (Input.GetKeyDown(KeyCode.C) && !Input.GetKey(KeyCode.RightShift))
        {
            //if (Cursor.lockState == CursorLockMode.None)
            //    Cursor.lockState = CursorLockMode.Locked;
            //else
            //    Cursor.lockState = CursorLockMode.None;

            SleepMode = !SleepMode;
        }

        //TEST: Player customization stuff
        if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CharacterCustomizationScript player = CurrentPlayerObject.GetComponent<CharacterCustomizationScript>();

                //player.UpdateBodyColor(new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

                //player.UpdateClothingColor(ClothingSlot.Upper, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
                //player.UpdateClothingColor(ClothingSlot.Lower, new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));

                //if (player.IsClothingSlotUsed(ClothingSlot.Costume))
                //    player.UnsetClothing(ClothingSlot.Costume);
                //else if (player.IsClothingSlotUsed(ClothingSlot.Upper) && player.IsClothingSlotUsed(ClothingSlot.Lower))
                //{
                //    player.UnsetClothing(ClothingSlot.Upper);
                //    player.UnsetClothing(ClothingSlot.Lower);
                //}
                //else
                //{
                //    player.SetClothing(3);
                //}

                ClothingSlot slot = (ClothingSlot)UnityEngine.Random.Range(0, 3);

                switch (slot)
                {
                    case ClothingSlot.Costume:
                        player.SetClothing(3); break;
                    case ClothingSlot.Upper:
                        player.SetClothing(5); break;
                    case ClothingSlot.Lower:
                        player.SetClothing(4); break;
                    default:
                        break;
                }

                Debug.Log("*" + slot.ToString());
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                //CharacterCustomizationScript player = CurrentPlayerObject.GetComponent<CharacterCustomizationScript>();

                //player.UnsetClothing(ClothingSlot.Costume);

                CharacterCustomizationScript player = CurrentPlayerObject.GetComponent<CharacterCustomizationScript>();

                player.RandomizeAppearance();
                player.GetCustomizationData();
            }
        }
        #endregion
    }

    #region <"Mode" change methods>
    /// <summary>
    /// UIMode true, Cursor unlocked.
    /// </summary>
    public void ModeSetUI()
    {
        UIMode = true;

        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// UIMode false, Cursor hidden.
    /// </summary>
    public void ModeSetPlay()
    {
        UIMode = false;

        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

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
                float discount = SupplierManager.Suppliers[iSupplier].DiscountPercentage;

                successful = Player.Business.PurchaseItem(item, discount, performValidation, out result);
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
    public void CompleteOrder(int iOrderToComplete, Dictionary<int, int> itemQuantities, out float payment, out string result)
    {
        result = MSG_ERR_DEFAULT;

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

        OrderManager.CompleteOrder(iOrderToComplete, items, GameDateTime, Player.Business.GetTotalMarkup(), out payment, out score, out penaltyMult);

        Player.Business.Money += payment;
        Player.IncreaseExperience(score);

        Debug.Log("Order score: " + score.ToString());

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

        //TEST: Generating supplier items
        SupplierManager.PopulateSupplierInventories();

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

    public void CheckDifficulty()
    {
        if (Difficulty != -1 && Difficulty < 3)
        {
            if (Player.Level >= DifficultySettings[Difficulty + 1].LevelRequirement)
            {
                Difficulty++;
            }
        }
    }
    #endregion

    #region <SAVING & LOADING METHODS>
    public void SaveGame(int saveSlot)
    {
        if (TEMPSaveGame)
        {
            BinaryFormatter bf = new BinaryFormatter();

            Player.CharacterCustomizationData = CurrentPlayerObject.GetComponent<CharacterCustomizationScript>().GetCustomizationData();
            Player.OfficeCustomizationData = CustomizationManager.Office.GetCustomizationData();

            //Save data to GameData object (saveData):
            GameData gameData = new GameData
            {
                Player = this.Player,

                Suppliers = SupplierManager.Suppliers,

                Orders = OrderManager.Orders,

                Difficulty = this.Difficulty,

                GameDateTime = this.GameDateTime,
                GameTimeSpeed = this.GameTimeSpeed,

                ChanceNextOrder = this.chanceNextOrder,

                SleepMode = this.SleepMode,
                DayEnd = this.DayEnd,

                DayEndCurrent = this.dayEndCurrent,

                Notifications = this.Notifications
            };

            SaveData saveData = new SaveData
            {
                Date = DateTime.Now,
                GameData = gameData
            };

            FileStream file = File.Create(GetSaveFilePath(saveSlot));

            bf.Serialize(file, saveData);

            file.Close();

            //LOG:
            Debug.Log("GAME DATA SAVED TO '" + Application.persistentDataPath + "'!");
        }
    }

    public void LoadGame(int saveSlot)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(GetSaveFilePath(saveSlot), FileMode.Open);

        SaveData loadData = (SaveData)bf.Deserialize(file);

        file.Close();

        GameData gameData = loadData.GameData;

        //Load data from GameData object (loadData):
        Player = gameData.Player;

        SupplierManager.Suppliers = gameData.Suppliers;

        OrderManager.Orders = gameData.Orders;

        Difficulty = gameData.Difficulty;

        GameDateTime = gameData.GameDateTime;
        GameTimeSpeed = gameData.GameTimeSpeed;

        chanceNextOrder = gameData.ChanceNextOrder;

        SleepMode = gameData.SleepMode;
        DayEnd = gameData.DayEnd;

        dayEndCurrent = gameData.DayEndCurrent;

        Notifications = gameData.Notifications;

        //LOG:
        Debug.Log("GAME DATA LOADED!");
    }

    //public void DeleteSave()
    //{
    //    File.Delete(Application.persistentDataPath + saveFileDirString);
    //}

    public string GetSaveFilePath(int saveSlot)
    {
        return Application.persistentDataPath + "/" + SaveFileName + Convert.ToString(saveSlot).PadLeft(3, '0') + SaveFileExtension;
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
