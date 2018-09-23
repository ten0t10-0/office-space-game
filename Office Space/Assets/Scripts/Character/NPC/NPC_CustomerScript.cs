using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_CustomerScript : MonoBehaviour
{
    [HideInInspector]
    public int NPC_ID = -1;

    private void Awake()
    {
        NPC_ID = GameMaster.Instance.NPCManager.GetNextID();
        GameMaster.Instance.NPCManager.CurrentNPCs[NPC_ID] = this.gameObject;

        Debug.Log("MY ID IS " + NPC_ID.ToString());
    }

    private void OnDestroy()
    {
        GameMaster.Instance.NPCManager.CurrentNPCs_ResetAt(NPC_ID);
        
        Debug.Log("ID DEAD IS " + NPC_ID.ToString());
    }
}
