using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMounts : MonoBehaviour 
{
	public Transform currentMount;

	public float speed = 0.1f;

	void Start () 
	{
		GameMaster.Instance.UIMode = false;
	}
		
	void Update ()
	{

        if (GameMaster.Instance.CameraLock)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, currentMount.position, speed);
            Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speed);
        }
	}

	public void  setMount ( Transform newMount  )
	{
		currentMount = newMount;
	}﻿
}
