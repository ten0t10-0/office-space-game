using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> Items;
}
