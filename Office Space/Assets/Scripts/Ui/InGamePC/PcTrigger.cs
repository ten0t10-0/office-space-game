using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTrigger : MonoBehaviour {

	public Canvas ComputerCanvas;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"));
		OpenShop();
			
	}
	void OpenShop()
	{
		ComputerCanvas.enabled = true;
		Time.timeScale = 0;
		Cursor.visible = (ComputerCanvas.gameObject.activeInHierarchy);
	}

	void CloseShop()
	{
		ComputerCanvas.enabled = false;
		Time.timeScale = 1;
	}


}
