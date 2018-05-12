using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

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
		GameMaster.Instance.UIMode = false;
	}
      void Pause()
	{
		pauseMenu.SetActive (true);
		Time.timeScale = 0f;
		isPaused = true;
		GameMaster.Instance.UIMode = true;
	}
}
