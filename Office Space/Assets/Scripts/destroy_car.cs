using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_car : MonoBehaviour {

	public GameObject other;
	traffic spawner;

	// Use this for initialization
	public void destroy_event () 
	{
		
		Destroy (other);
		spawner.SpawnCar ();
		
	}

	void Awake()
	{
		spawner = FindObjectOfType<traffic> ();
	}


}
