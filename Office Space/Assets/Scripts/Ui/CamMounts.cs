using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMounts : MonoBehaviour 
{
	public Transform currentMount;

	public float speed = 0.1f;
	//public float duration = 3.0f;

	float startTime, totalDistance;
	float journeyFraction;

	void Start () 
	{
		GameMaster.Instance.UIMode = false;

	}

	// Update is called once per frame
	void Update () 
	{

		totalDistance = Vector3.Distance(Camera.main.transform.position, currentMount.position);

		//float currentDuration = (Time.time - startTime) * speed;
		//journeyFraction = currentDuration / totalDistance;

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, currentMount.position, speed);
		Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speed);

		if (Input.GetKeyDown (KeyCode.V)) 
		{

			//Ui.transform.position = Vector3.Lerp(Camera.main.transform.position, currentMount.position, speedFactor);
			//Ui.transform.position = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedFactor);
			GameMaster.Instance.UIMode = true;
		}
	}


}
