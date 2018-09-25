using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderNotetificationClose : MonoBehaviour 
{
	public GameObject order;

	public void setOff()
	{
		order.SetActive (false);
	}

}
