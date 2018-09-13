using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwan_2ndpedestrian : MonoBehaviour 
{
	public GameObject nagent;
	public GameObject goalObject;
	public int minValue;
	public int maxValue;

	// Use this for initialization
	void Start () 
	{

		Invoke ("SpawnAgent", Random.Range(minValue,maxValue));
	}

	// Update is called once per frame
	public void SpawnAgent() 
	{
		GameObject na = (GameObject)Instantiate (nagent, this.transform.position, Quaternion.identity);
		na.GetComponent<pedestrian_the2nd> ().Target = gameObject.transform;
		//Invoke ("SpawnAgent", Random.Range (1, 30));

		na.GetComponent<CharacterCustomizationScript> ().RandomizeAppearance ();
	}
}