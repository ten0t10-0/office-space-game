using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebtDayA : MonoBehaviour 
{
	public GameObject debtday;

	public void DisableDebtDay()
	{
		debtday.SetActive (false);
	}
}
