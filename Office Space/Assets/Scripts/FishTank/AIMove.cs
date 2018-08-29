using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour 
{
	private FishTank aiManager;

	private bool hasTarget = false;
	private bool isTurning; 

	private Vector3 wayPoint;
	private Vector3 lastwaypoint = new Vector3 (01,0f,0f); 

	private Animator animator; 
	private float speed; 


	void Start ()
	{ 
		
		//aiManager = FindObjectOfType<FishTank> ();

		aiManager = transform.parent.GetComponentInParent<FishTank> (); 
		animator = GetComponent<Animator> (); 
		//SetUpNPC ();
	}

//	void SetUpNPC()
//	{
//		float scale = Random.Range(0f, 4f);
//		transform.localScale += new Vector3(scale * 1.5f, scale, scale); 
//	}

	void Update ()
	{ 
		if (!hasTarget) 
		{
			
			hasTarget = CanFindTarget ();
		} 
		else 
		{
			RotateNPC(wayPoint, speed); 
			transform.position = Vector3.MoveTowards (transform.position, wayPoint, speed * Time.deltaTime); 
		}
			
		if (transform.position == wayPoint)
			hasTarget = false; 
	}

	bool CanFindTarget() 
	{ 
		
		wayPoint = aiManager.RandomWaypoint ();

		if (lastwaypoint == wayPoint) 
		{ 
			wayPoint = aiManager.RandomWaypoint (); 
			return false; 
		} 
		else 
		{ 

			lastwaypoint = wayPoint; 
		
			speed = Random.Range (.5f, 1f);
			animator.speed = speed; 

			return true; 
		}
	} 
	//Rotate the NPC to face new waypoint 
	void RotateNPC (Vector3 waypoint,float currentSpeed) 
	{ 
		float TurnSpeed = currentSpeed * Random.Range(6f, 6f); 

		Vector3 LookAt = waypoint - transform.position; 
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (LookAt), TurnSpeed * Time.deltaTime);
	} 

	

}
