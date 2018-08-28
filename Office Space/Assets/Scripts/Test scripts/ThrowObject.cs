using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

	 Transform player;
	public Transform playerCam;
	public float throwForce = 100;
	bool hasPlayer = false;
	bool beingCarried = false;
	private bool touched = false;

	void Start()
	{
	}

	void Update()
	{
		player = GameMaster.Instance.CurrentPlayerObject.transform;
		float dist = Vector3.Distance(gameObject.transform.position, player.position);
		if (dist <= 1.9f)
		{
			hasPlayer = true;
		}
		else
		{
			hasPlayer = false;
		}
		if (hasPlayer && Input.GetKey(KeyCode.Q))
		{
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<Collider> ().enabled = false;
			transform.parent = GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ().Transform_HandR;
			transform.position = GameMaster.Instance.CurrentPlayerObject.GetComponent<CharacterCustomizationScript> ().Transform_HandR.position;
			beingCarried = true;

		}
		if (beingCarried)
		{
			if (touched)
			{
				GetComponent<Rigidbody>().isKinematic = false;
				transform.parent = null;
				beingCarried = false;
				touched = false;
			}
			if (Input.GetMouseButtonDown(0))
			{
				GetComponent<Rigidbody>().isKinematic = false;
				transform.parent = null;
				beingCarried = false;
				GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
				GetComponent<Collider> ().enabled = true;
			}
			else if (Input.GetMouseButtonDown(1))
			{
				GetComponent<Rigidbody>().isKinematic = false;
				transform.parent = null;
				beingCarried = false;
			}
		}
	}
	void OnTriggerEnter()
	{
		if (beingCarried)
		{
			touched = true;
		}
	}
}
