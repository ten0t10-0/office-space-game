using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_NPCv2 : MonoBehaviour 
{

	public GameObject nagent;
	public GameObject goalObject;

	public void SpawnAgent() 
	{
		GameObject na = (GameObject)Instantiate (nagent, this.transform.position, Quaternion.identity);
		na.GetComponent<AI_try3> ().Target = gameObject.transform;
		//Invoke ("SpawnAgent", Random.Range (1, 30));

		na.GetComponent<CharacterCustomizationScript> ().RandomizeAppearance ();
	}
}
