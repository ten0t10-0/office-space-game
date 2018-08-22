using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour 
{
	//ref for other scripts
	public static bool isPaused = false;

	public GameObject pauseMenu; 

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				Resume ();
			} 
			else
			{
				Pause ();
			}
		}
	}

	public void Resume()
	{
		pauseMenu.SetActive (false);
		Time.timeScale = 1f;
		isPaused = false;
       // GameMaster.Instance.ModeSetPlay();
		gameObject.GetComponent<GraphicRaycaster> ().enabled = false;

	}
      void Pause()
	{
		pauseMenu.SetActive (true);
		Time.timeScale = 0f;
		isPaused = true;
        //GameMaster.Instance.ModeSetUI();
		gameObject.GetComponent<GraphicRaycaster> ().enabled= true;
	}
}
