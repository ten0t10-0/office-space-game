using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DestroyAllNPCs()
    {
        for (int i = 0; i < CurrentNPCs.Length; i++)
        {
            if (CurrentNPCs[i] != null)
                Destroy(CurrentNPCs[i]);
        }
    }

    public void CurrentNPCs_ResetAt(int index)
    {
        CurrentNPCs[index] = null;
    }

    /// <summary>
    /// Maximum number of NPCs already present in game?
    /// </summary>
    public bool IsMaxNPCs
    {
        get { return GetNextID() == -1; }
    }

    public int GetNextID()
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
