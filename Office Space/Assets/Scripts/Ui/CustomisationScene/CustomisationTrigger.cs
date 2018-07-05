using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationTrigger : MonoBehaviour {

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;
	public GameObject cube,hud,custom;
	public Transform bodyMount;
	Vector3 pos,rot;
	CamMounts cam;

	void Update ()
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				OpenPanel.SetActive (false);

				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().transform.position = pos;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().transform.Rotate(rot);
				SetMount ();
				GameMaster.Instance.UIMode = true;

				hud.SetActive (false);
				custom.SetActive (true);

			}
		}
	}



	public void SetMount()
	{
		FindObjectOfType<CamMounts> ().setMount(bodyMount);
	}


	void Start () 
	{
		pos = cube.transform.position;
		rot = cube.transform.eulerAngles;
	}


	void OnTriggerEnter(Collider other)
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
