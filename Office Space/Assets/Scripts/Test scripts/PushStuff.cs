using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStuff : MonoBehaviour 
{
	public float power= 10.0f;
	Rigidbody body;
	Vector3 pushDir;

	void  OnHit (ControllerColliderHit hit)
	{
		body = hit.collider.attachedRigidbody;

		if(body == null || body.isKinematic)
			return;
		if(hit.moveDirection.y <-0.3f)
			return;
		
		pushDir = new Vector3(hit.moveDirection.x,0,hit.moveDirection.x);
		body.velocity = pushDir * power;
	}

}
