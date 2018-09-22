using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpawnManager : MonoBehaviour 
{
	Spawn_NPC npc1;
	spawn_NPCv2 npc2;
	spawn_the3rd npc3;


	void Awake()
	{
		npc1 = FindObjectOfType<Spawn_NPC> ();
		npc2 = FindObjectOfType<spawn_NPCv2> ();
		npc3 = FindObjectOfType<spawn_the3rd> ();
	}
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SpawnAI1()
	{
		npc3.SpawnAgent ();
	}
	public void SpawnAI2()
	{
		npc2.SpawnAgent ();
	}
	public void SpawnAI3()
	{
		npc1.SpawnAgent ();
	}
		
}
