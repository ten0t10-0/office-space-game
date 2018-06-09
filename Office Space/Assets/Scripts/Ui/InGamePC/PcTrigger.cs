using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcTrigger : MonoBehaviour {

	//public Canvas ComputerCanvas;
	public GameObject hud;

	void OnTriggerEnter(Collider other)
	{
		//if (other.gameObject.CompareTag("Player"))
            //OpenShop();
			
	}
//	void OpenShop()
//	{
//		ComputerCanvas.enabled = true;
//        GameMaster.Instance.UIMode = true;
//		//Time.timeScale = 0;
//		Cursor.visible = (ComputerCanvas.gameObject.activeInHierarchy);
//	}

	public void CloseShop()
	{
		//ComputerCanvas.enabled = false;
        GameMaster.Instance.UIMode = false;
        //Time.timeScale = 1;
		hud.SetActive(true);
    }


}
