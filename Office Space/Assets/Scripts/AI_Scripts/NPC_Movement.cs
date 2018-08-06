using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour 
{
	public enum MovementStates
	{
		Idle,
		Walking,
		Running
	}

	public MovementStates MovementType;

	float speed = 2.0f;
	float rotateSpeed = 2.0f;

	public GameObject Waypoint;
	GameObject WP;
	bool wayP = false;

	float min_distance = -5.0f;
	float max_distance = 5.0f;

	Animator anim;
	private Rigidbody playerRigidbody; 
	private NavMeshAgent m_NavmeshAgent;

	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody>();
	}

	void Start()
	{
		StartCoroutine (ChooseAction ());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (MovementType == MovementStates.Walking) 
		{
			anim.SetBool("IsWalking", true);
			createWayPoint ();
		}

		if (MovementType == MovementStates.Idle) 
		{
			anim.SetBool("IsWalking", false);
		}

		if (wayP) 
		{
			move ();
		}
	}

	void createWayPoint()
	{
		if (!wayP) 
		{
			float distance_x = transform.position.x + Random.Range (min_distance, max_distance);
			float distance_z = transform.position.z + Random.Range (min_distance, max_distance);

			WP = Instantiate (Waypoint, new Vector3 (distance_x, 0, distance_z), Quaternion.identity) as GameObject;

			wayP = true;
		}
	}

	private IEnumerator ChooseAction()
	{
		while (true) 
		{
			yield return new WaitForSeconds (1.0f);
			if (!wayP) 
			{
				int num = Random.Range (0, 2);
				if (num == 0) {
					MovementType = MovementStates.Idle;
				} 
				else if (num == 1) 
				{
					MovementType = MovementStates.Walking;
				} 
				else if (num == 2) 
				{
					MovementType = MovementStates.Running;
				}
			}
		}
	}
	void move()
	{
		transform.position = Vector3.MoveTowards (transform.position, WP.transform.position, speed * Time.deltaTime);

		var rotation = Quaternion.LookRotation (WP.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotateSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider Waypoint)
	{
		if (Waypoint.tag == "waypoint") 
		{
			Destroy (WP);
			wayP = false;
			MovementType = MovementStates.Idle;
		}
	}
}
