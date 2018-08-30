using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHover : MonoBehaviour {

	HoverCarControl hover;
	public GameObject mount;
	Vector3 pos,rot;
	bool onBoard = false;
	public Collider trigger;
	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

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
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E) && onBoard == false) {

				GameMaster.Instance.PlayerControl = false;
				hover.enabled = true;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().isKinematic = true;
				GameMaster.Instance.CurrentPlayerObject.transform.parent = mount.transform;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MovePosition (mount.transform.position); 
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MoveRotation (mount.transform.rotation);
				onBoard = true;
			}

		}
					else if (Input.GetKeyDown (KeyCode.E) && onBoard == true) 
					{
						hover.enabled = false;
						GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().isKinematic = false;
						GameMaster.Instance.CurrentPlayerObject.transform.parent = null;
						GameMaster.Instance.PlayerControl = true;
						//trigger.enabled = false;
						onBoard = false;
					}
	

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
		}
	}
	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}

}
