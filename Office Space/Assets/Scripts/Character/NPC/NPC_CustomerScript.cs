﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_CustomerScript : MonoBehaviour
{
    [HideInInspector]
    public int NPC_ID = -1;

    public Customer CustomerInfo;
    public Item ItemHeld;

    public bool IsHoldingItem
    {
        get { return ItemHeld != null; }
    }

    public void Initialize(int id)
    {
        NPC_ID = id;
        CustomerInfo = GameMaster.Instance.CustomerManager.GenerateCustomer();
        ItemHeld = null;
    }

    private void OnDestroy()
    {
        GameMaster.Instance.NPCManager.CurrentNPCs_ResetAt(NPC_ID);
    }
}