using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType { Test, Customer }

public class NPCManager : MonoBehaviour
{
    public int MaximumNPCs = 10;
    public Vector3 DefaultPosition;
    public Vector3 DefaultRotation;

    [HideInInspector]
    public GameObject[] CurrentNPCs;

    private void Awake()
    {
        CurrentNPCs = new GameObject[MaximumNPCs];
    }

    public List<NPCCustomer> GetCustomerNPCData()
    {
        List<NPCCustomer> customerList = new List<NPCCustomer>();

        //*

        return customerList;
    }

    public void SpawnNPC(NPCType npcType)
    {
        int npcId = GetNextID();

        if (npcId != -1)
        {
            switch (npcType)
            {
                case NPCType.Customer:
                    {
                        CurrentNPCs[npcId] = Instantiate(GameMaster.Instance.GenericCharacterObject, DefaultPosition, Quaternion.Euler(DefaultRotation));

                        //*

                        break;
                    }
            }
        }
    }

    public void DestroyNPC(int npcId)
    {
        Destroy(CurrentNPCs[npcId]);
        CurrentNPCs[npcId] = null;
    }

    public void DestroyAllNPCs()
    {
        for (int i = 0; i < CurrentNPCs.Length; i++)
        {
            Destroy(CurrentNPCs[i]);
            CurrentNPCs[i] = null;
        }
    }

    private int GetNextID()
    {
        int id = -1;

        for (int i = 0; i < CurrentNPCs.Length; i++)
        {
            if (CurrentNPCs[i] == null)
            {
                id = i;
                i = CurrentNPCs.Length; //break
            }
        }

        return id;
    }
}
