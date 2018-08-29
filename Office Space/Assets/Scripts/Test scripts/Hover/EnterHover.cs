using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHover : MonoBehaviour {

	HoverCarControl hover;
	public GameObject mount;
	Vector3 pos,rot;

	// Use this for initialization
	void Start () 
	{
		hover = GetComponent<HoverCarControl> ();
		pos = mount.transform.position;
		rot = mount.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameMaster.Instance.PlayerControl = false;
			hover.enabled = true;
			GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().isKinematic = true;
			GameMaster.Instance.CurrentPlayerObject.transform.parent = mount.transform;
			GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().transform.position = pos;
			GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().transform.Rotate(rot);
		
		}
	}
}
