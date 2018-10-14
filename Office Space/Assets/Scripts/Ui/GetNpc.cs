using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetNpc : MonoBehaviour {

	public GameObject interact;
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
				serve.Counter1True ();
				serve.StartTimer (en);
				Debug.Log ("Customer Counter on Enter" + en);
				break;
			}
		case 2:
			{
				serve.cus2 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				serve.AI2 = col.gameObject.GetComponent<AI_try3> ();
				serve.Counter2True ();
				serve.StartTimer (en);
				Debug.Log ("Customer Counter " + en);
				break;
			}
		case 3:
			{
				serve.cus3 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				serve.AI3 = col.gameObject.GetComponent<AI_try>();
				serve.Counter3True ();
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
				serve.Counter1False ();
				break;
			}
		case 2:
			{
				serve.Counter2False ();
				break;
			}
		case 3:
			{
				serve.Counter3False ();
				break;
			}
		}
	}

}
