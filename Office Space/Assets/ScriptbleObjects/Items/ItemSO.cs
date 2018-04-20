using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "T", menuName = "Item")]
public class ItemSO : ScriptableObject
{
	public string Name; 
	public ItemCategory Category; 
	public ItemQuality Quality;  //Low-end, Medium-end, High-end
	public float UnitCost;
	public float UnitSpace; 

	public Sprite pic; 
}
