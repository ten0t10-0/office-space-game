using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamGuide : MonoBehaviour {

	public Transform currentMount;

	public float speedFactor= 0.1f;
	public float zoomFactor= 1.0f;
	public Vector3 lastPosition;

	void  Start ()
	{
		lastPosition = transform.position;
	}

	void  Update ()
	{
		transform.position = Vector3.Lerp(transform.position, currentMount.position, speedFactor);
		transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedFactor);




	}

	public void  setMount ( Transform newMount  )
	{
		currentMount = newMount;
	}﻿
}
