using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="T",menuName="Item")]
public class ItemTemp : ScriptableObject
{
	public string Id;
	public string Name;
	public ItemCategory Category; 
	public ItemQuality Quality; 
	public float UnitCost; 
	public float UnitSpace;

	public Sprite pic; 


}
