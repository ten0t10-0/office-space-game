using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class AI_try : MonoBehaviour 
{
	NavMeshAgent nm;
	Rigidbody rb;
	Animator anim;
	public Transform Target;
	public Transform[] WayPoints;
	public int Cur_Waypoint;
	public float speed, stop_distance;
	public float PauseTimer;
	[SerializeField]
	private float cur_timer;

	void Awake()
	{
		Transform sm = GameObject.Find ("Waypoints").transform;

		for (int i = 0; i < sm.childCount; i++)
		{
			WayPoints [i] = sm.GetChild (i);
		}
	}

	// Use this for initialization
	void Start () 
	{
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
				Target = WayPoints[Cur_Waypoint];
				cur_timer = PauseTimer;
			}
		}

		nm.SetDestination (Target.position);
	}
}
