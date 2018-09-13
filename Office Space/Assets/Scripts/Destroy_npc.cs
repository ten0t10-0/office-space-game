using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_npc : MonoBehaviour 
{
	public GameObject other;
	public GameObject Waypoint;
	spawn_pedestrian spawn;
	// Update is called once per frame
	void OnTriggerEnter(Collider Waypoint)
	{
		if (Waypoint.tag == "destructor") 
		{
			Destroy (other);
			spawn.SpawnAgent ();
		}
	}
	void Awake()
	{
		spawn = FindObjectOfType<spawn_pedestrian> ();
	}
}
