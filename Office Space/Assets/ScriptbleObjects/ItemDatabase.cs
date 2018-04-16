using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ItemDatabase",menuName="Database")]
public class ItemDatabase : ScriptableObject
{

	public List <ItemTemp> itemDictionary = new List<ItemTemp> ();
}
