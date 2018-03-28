using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	public void PlayGame()
	{
		//cont load saved , new game another function
	}
	public void QuitGame()
	{
		Debug.Log ("QUIT");
		Application.Quit ();
	}

}
