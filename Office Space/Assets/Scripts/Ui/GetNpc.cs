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

	void Enter(Collider col, int en)
	{
		switch (en) 
		{
		case 1:
			{
				serve.cus1 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				serve.AI1 = col.gameObject.GetComponent<AI_try4> ();
				cube1.SetActive (true);
				col1.enabled = true;
				serve.StartTimer (en);
				Debug.Log ("Customer Counter on Enter" + en);
				break;
			}
		case 2:
			{
				serve.cus2 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				serve.AI2 = col.gameObject.GetComponent<AI_try3> ();
				cube2.SetActive (true);
				serve.StartTimer (en);
				col2.enabled = true;
				Debug.Log ("Customer Counter " + en);
				break;
			}
		case 3:
			{
				serve.cus3 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				serve.AI3 = col.gameObject.GetComponent<AI_try>();
				cube3.SetActive (true);
				col3.enabled = true;
				serve.StartTimer (en);
				Debug.Log ("Customer Counter " + en);
				break;
			}
		}
	}
	void Exit(Collider col, int ex)
	{
		switch (ex) 
		{
		case 1:
			{
				cube1.SetActive (false);
				col1.enabled = false;
				break;
			}
		case 2:
			{
				cube2.SetActive (false);
				col2.enabled = false;
				break;
			}
		case 3:
			{
				cube3.SetActive (false);
				col3.enabled = false;
				break;
			}
		}
	}

}
