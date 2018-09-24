using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traffic : MonoBehaviour 
{
	public Animator anim;
	public GameObject[] prefabs = new GameObject[5];
	public Transform location;
	// Use this for initialization
	void Start () 
	{
		Invoke ("SpawnCar", 5);
	}
	
	// Update is called once per frame
	public void SpawnCar () 
	{
		GameObject rPrefab;
		rPrefab = Instantiate(prefabs[Random.Range(0, prefabs.Length - 1)], location.position, location.rotation);
	}
}
