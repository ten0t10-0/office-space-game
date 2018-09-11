using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNpcDetails : MonoBehaviour 
{

	public CharacterCustomizationScript Cus;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			Cus = other.gameObject.GetComponent<CharacterCustomizationScript> ();
			Debug.Log("Blooooooooooooooooooooooooooooooop");
		}
	}
}
