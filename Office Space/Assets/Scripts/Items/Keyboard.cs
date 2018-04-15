﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Keyboard",menuName="Keyboard")]
public class Keyboard : ScriptableObject
{
	public string Name;
	public ItemCategory Category; 
	public ItemQuality Quality; 
	public float UnitCost; 
	public float UnitSpace;

	public Sprite pic; 


}
