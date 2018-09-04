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

				SetMount ();
				OpenPanel.SetActive (false);
				GameMaster.Instance.ModeSetUI();
				GameMaster.Instance.CameraLock = true;
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MovePosition (cube.transform.position); 
				GameMaster.Instance.CurrentPlayerObject.GetComponent<Rigidbody> ().MoveRotation (cube.transform.rotation);

				hud.SetActive (false);
				custom.SetActive (true);

			}
		}
	}



	public void SetMount()
	{
		FindObjectOfType<CamMounts> ().setMount(bodyMount);
	}

	public void Exit()
	{
		GameMaster.Instance.CameraLock = false;
		GameMaster.Instance.ModeSetPlay();
		hud.SetActive (true);
		custom.SetActive (false);
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
