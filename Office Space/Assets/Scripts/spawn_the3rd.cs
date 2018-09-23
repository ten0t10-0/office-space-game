using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_the3rd : MonoBehaviour {

	public GameObject nagent;
	public GameObject goalObject;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	public void SpawnAgent() 
	{
		GameObject na = (GameObject)Instantiate (nagent, this.transform.position, Quaternion.identity);
		na.GetComponent<AI_try4> ().Target = gameObject.transform;

		//Invoke ("SpawnAgent", Random.Range (1, 30));

		na.GetComponent<CharacterCustomizationScript> ().RandomizeAppearance ();
	}
}
