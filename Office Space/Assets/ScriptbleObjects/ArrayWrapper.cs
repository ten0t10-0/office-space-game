using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrayLayout  
{
	[System.Serializable]
	public struct rowData
	{
		public ScriptableObject[] row;
	}
	public rowData[] rows = new rowData[7]; 

}
