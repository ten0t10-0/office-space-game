using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTank : MonoBehaviour {


	public List<Transform> fishWaypoints = new List<Transform> ();

	public FishAIClass[] AiObject = new FishAIClass[3];


	public float spawntimer  { get { return SpawnTimer; } }
	public Vector3 spawnArea {get{ return SpawnArea; } } 

	[SerializeField]
	private float SpawnTimer; 

	[SerializeField] 
	private Vector3 SpawnArea = new Vector3 (3f, 2f ,5f); 

	// Use this for initialization
	void Start () 
	{
		GetWayPoint ();
		CreateAIGroup ();
		InvokeRepeating("SpawnNPC", 0.5f, spawntimer); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateAIGroup()
	{
		GameObject AiGroupSpawn;
		for (int i = 0; i < 2; i++) 
		{
			AiGroupSpawn = new GameObject (AiObject [i].AIGroupName);
			AiGroupSpawn.transform.parent = this.gameObject.transform;

		}
	}
	void GetWayPoint()
	{
		Transform[] waypointList = transform.GetComponentsInChildren<Transform> ();

			for (int i = 0; i < waypointList.Length; i++ )
			{
				if (waypointList[i].tag=="FishWayPoint")
				{
					fishWaypoints.Add(waypointList[i]);
				}
			}
	}
	void SpawnNPC ()
	{ 

		for (int i = 0; i < 2; i++) 
		{ 
			GameObject tempGroup = GameObject.Find(AiObject[i].AIGroupName); 
			if(tempGroup.GetComponentInChildren<Transform>().childCount < AiObject[i].MaxAi) 
			{ 
				for (int y = 0; y < Random.Range(0,AiObject[i].SpawnAmount); y++) 
				{ 
					Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
					GameObject tempSpawn; 
					tempSpawn = Instantiate(AiObject[i].ObjectPrefab, RandomPosition(), randomRotation); 
					tempSpawn.transform.parent = tempGroup.transform; 
					tempSpawn.AddComponent<AIMove>(); 
				}
			}
		}

	}
	//public method for Random Position within the Spawn Area public 
	public Vector3 RandomPosition() 
	{ 
		Vector3 randomPosition = new Vector3
		( 
			Random.Range(-spawnArea.x, spawnArea.x),
			Random.Range(-spawnArea.y, spawnArea.y), 
			Random.Range(-spawnArea.z, spawnArea.z)
		); 
		randomPosition = transform.TransformPoint(randomPosition * .5f);
		return randomPosition; 
	} 

	 public Vector3 RandomWaypoint() 
	{ 
		int randomWP = Random.Range(0, (fishWaypoints.Count - 1)); 
		Vector3 randomWaypoint = fishWaypoints[randomWP].transform.position;
		return randomWaypoint; 
	} 

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawCube (transform.position, spawnArea);
	}
}
