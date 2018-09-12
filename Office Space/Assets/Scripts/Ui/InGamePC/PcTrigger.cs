using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTrigger : MonoBehaviour {

	public GameObject hud;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	public GameObject screen;
	public Transform pcMount;
	public Animator hudO;


	void Update ()
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				hudO.SetBool ("UIO", true);
				SetMount ();
				OpenPanel.SetActive (false);
                GameMaster.Instance.ModeSetUI();
                GameMaster.Instance.CameraLock = true;
				screen.SetActive (false);
				hud.SetActive (false);
				FindObjectOfType<SoundManager>().Play("Pc");
			}
		}
	}

	public void SetMount()
	{
		FindObjectOfType<CamMounts> ().setMount(pcMount);
	}

	public void CloseShop()
	{
        GameMaster.Instance.ModeSetPlay();
        GameMaster.Instance.CameraLock = false;
		hud.SetActive(true);
		hudO.SetBool ("UIO", false);
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
