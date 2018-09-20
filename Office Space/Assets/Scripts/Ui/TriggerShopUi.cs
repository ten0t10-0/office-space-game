using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShopUi : MonoBehaviour 
{
	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;
	ServeCustomer cus;
	public int line;

	void Awake()
	{
		cus = FindObjectOfType<ServeCustomer> ();
	}

	void Update ()
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				
				OpenPanel.SetActive(false);
				cus.StartCustomerUi (1);
				Debug.Log ("Woooooooooooo1");
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
			Debug.Log ("Enter 1");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
		}
	}
	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}
}
