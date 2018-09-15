﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class AI_try4 : MonoBehaviour 
{
	NavMeshAgent nm;
	Rigidbody rb;
	Animator anim;
	public Transform Target;
	public Transform[] WayPoints;
	public int Cur_Waypoint;
	public int tempr_Waypoint;
	public float speed, stop_distance;
	public float PauseTimer;
	[SerializeField]
	private float cur_timer;
	Transform sm;
	public List<Transform> npcWayPoints;
	bool taken1 = false;
	bool taken2 = false;
	bool taken3 = false;


	void Awake()
	{
		//		sm = GameObject.Find ("Waypoints").transform;
		//
		//		for (int i = 0; i < sm.childCount; i++)
		//		{
		//			WayPoints [i] = sm.GetChild (i);
		//		}
		taken1 = false;
		taken2 = false;
		taken3 = false;
	}

	// Use this for initialization
	void Start () 
	{
		sm = GameObject.Find ("way").transform;

		for (int i = 0; i < sm.childCount; i++)
		{
			WayPoints [i] = sm.GetChild (i);

		}



		nm = GetComponent<NavMeshAgent> ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();

		rb.freezeRotation = true;

		Target = WayPoints [Cur_Waypoint];
		cur_timer = PauseTimer;

	}

	// Update is called once per frame
	void Update () 
	{ 
		nm.acceleration = speed;
		nm.stoppingDistance = stop_distance;


		float distance = Vector3.Distance (transform.position, Target.position);


		//Move to waypoint

		if (distance > stop_distance && WayPoints.Length > 0) 
		{
			anim.SetBool ("IsWalking", true);
			anim.SetBool ("IsIdling", false);

			//Find Waypoint
			Target = WayPoints [Cur_Waypoint];

		}

		else if (distance <= stop_distance && WayPoints.Length > 0) 

		{
			if (cur_timer > 0) 
			{
				cur_timer -= 0.01f;
				anim.SetBool ("IsWalking", false);
				anim.SetBool ("IsIdling", true);
			}
			if (cur_timer <= 0) 
			{
				Cur_Waypoint++;
				if (Cur_Waypoint >= WayPoints.Length) 
				{
					Cur_Waypoint = 0;
				}
				Target = WayPoints [Cur_Waypoint];


				cur_timer = Random.Range(0.1f,0.3f);
			}
		}
		nm.SetDestination (Target.position);

		//		if (Cur_Waypoint == 4) 
		//		{
		//			cur_timer = 3;
		//		}
		//
		//		if (Cur_Waypoint == 5) 
		//		{
		//			cur_timer = 3;
		//		}
		//
		//		if (Cur_Waypoint == 7) 
		//		{
		//			cur_timer = 3;
		//		}
		//
		//		if (Cur_Waypoint == 10) 
		//		{
		//			cur_timer = 3;
		//		}
	}
}
