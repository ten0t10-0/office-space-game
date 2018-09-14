using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeCustomer : MonoBehaviour 
{
	public GameObject hud;

	public GameObject OpenPanel = null,lineC;
	private bool isInsideTrigger = false;
	public Animator hudO;
	public int line;
	CustomerInteractionUI cusI;

	void Awake()
	{
		cusI = FindObjectOfType<CustomerInteractionUI> ();
	}

	void Update ()
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				hudO.SetBool ("UIO", true);
				lineC.SetActive (false);
				hud.SetActive (false);

			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
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

	public void StartCustomerUi(int i)
	{
		switch (i) {
		case 1:
			{

				break;
			}
		case 2:
			{
				break;
			}
		}
	}

}
