using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expBar : MonoBehaviour 
{
	public float fill;
	public Image content;

	void Update () 
	{
		FillBar();
	}

	public void FillBar()
	{
		content.fillAmount = CalculateBar();
	}

	public float CalculateBar()
	{
		float value = GameMaster.Instance.Player.Business.WarehouseInventory.TotalSpaceUsed();
		float iMin = 0;
		float iMax = GameMaster.Instance.Player.Business.WarehouseInventory.MaximumSpace;

		return (value - iMin) / (iMax - iMin);

	}

}
