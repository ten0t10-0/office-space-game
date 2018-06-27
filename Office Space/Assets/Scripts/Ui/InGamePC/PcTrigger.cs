using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTrigger : MonoBehaviour {

	//public Canvas ComputerCanvas;
	public GameObject hud;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	public GameObject screen;

	void Update ()
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				OpenPanel.SetActive (false);
				GameMaster.Instance.UIMode = true;
				screen.SetActive (false);
				hud.SetActive (false);
			}
		}
	}

	public void CloseShop()
	{
		//ComputerCanvas.enabled = false;
        GameMaster.Instance.UIMode = false;
        //Time.timeScale = 1;
		hud.SetActive(true);
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
