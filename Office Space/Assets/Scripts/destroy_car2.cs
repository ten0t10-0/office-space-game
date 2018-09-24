using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_car2 : MonoBehaviour {

	public GameObject other;
	traffic2 spawner;

	// Use this for initialization
	public void destroy_event2 () 
	{

		Destroy (other);
		spawner.SpawnCar2 ();

	}

	void Awake()
	{
		spawner = FindObjectOfType<traffic2> ();
	}


}
