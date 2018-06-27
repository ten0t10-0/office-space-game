using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMounts : MonoBehaviour 
{
	public Transform currentMount;

	public float speed = 0.1f;

	float totalDistance;

	void Start () 
	{
		GameMaster.Instance.UIMode = false;
	}
		
	void Update ()
	{

		totalDistance = Vector3.Distance (Camera.main.transform.position, currentMount.position);

		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, currentMount.position, speed);
		Camera.main.transform.rotation = Quaternion.Slerp (transform.rotation, currentMount.rotation, speed);

	}
}
