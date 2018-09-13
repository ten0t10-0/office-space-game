using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy2nd_pedestrian : MonoBehaviour 
{
	public GameObject other;
	public GameObject Waypoint;
	spwan_2ndpedestrian spawn;
	// Update is called once per frame
	void OnTriggerEnter(Collider Waypoint)
	{
		if (Waypoint.tag == "destructor1") 
		{
			Destroy (other);
			spawn.SpawnAgent ();
		}
	}
	void Awake()
	{
		spawn = FindObjectOfType<spwan_2ndpedestrian> ();
	}
}

