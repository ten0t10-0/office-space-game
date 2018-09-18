using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetNpc : MonoBehaviour {

	public GameObject cube1,cube2,cube3,interact;
	public Collider col1, col2, col3;
	public int line;

	ServeCustomer serve;

	void Awake()
	{
		serve = FindObjectOfType<ServeCustomer> ();

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			Enter (other, line);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "NPC")
		{
			Exit (other,line);
			if (interact.activeInHierarchy)
				interact.SetActive (false);
		}
	}

	void Enter(Collider col, int i)
	{
		switch (i) 
		{
		case 1:
			{
				serve.cus1 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube1.SetActive (true);
				col1.enabled = true;
				serve.StartTimer (i);
				Debug.Log ("Customer Counter on Enter" + i);
				break;
			}
		case 2:
			{
				serve.cus2 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube2.SetActive (true);
				serve.StartTimer (i);
				col2.enabled = true;
				Debug.Log ("Customer Counter " + i);
				break;
			}
		case 3:
			{
				serve.cus3 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube3.SetActive (true);
				col3.enabled = true;
				serve.StartTimer (i);
				Debug.Log ("Customer Counter " + i);
				break;
			}
		}
	}
	void Exit(Collider col, int i)
	{
		switch (i) 
		{
		case 1:
			{
				cube1.SetActive (false);
				col1.enabled = false;
				break;
			}
		case 2:
			{
				col2.enabled = false;
				cube2.SetActive (false);
				break;
			}
		case 3:
			{
				col3.enabled = false;
				cube3.SetActive (false);
				break;
			}
		}
	}

}
