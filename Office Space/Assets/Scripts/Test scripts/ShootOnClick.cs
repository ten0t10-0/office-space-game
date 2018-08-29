using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnClick : MonoBehaviour {


	public GameObject hitMaterial;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo))
			{
				var rig = hitInfo.collider.GetComponent<Rigidbody>();
				if(rig != null)
				{
					//rig.GetComponent<MeshRenderer>().material = hitMaterial;
					rig.AddForceAtPosition(ray.direction * 50f, hitInfo.point, ForceMode.VelocityChange);
				}
				GameObject impact = Instantiate (hitMaterial, hitInfo.point, Quaternion.LookRotation (hitInfo.normal));
				Destroy (impact, 1f);

			}
		}

	}
}
