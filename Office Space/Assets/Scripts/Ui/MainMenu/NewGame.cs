using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour 
{
	public InputField CompanyName;
	public InputField UserName;
	public GameObject text,loading;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameMaster.Instance.OfflineMode == false) 
		{
			UserName.interactable = false;
			UserName.text = GameMaster.Instance.CurrentUsername;
		}
        else
        {
            UserName.interactable = true;
        }
	}
	public void startGame()
	{
        Debug.Log("OFFLINE MODE?: " + GameMaster.Instance.OfflineMode.ToString());
		GameMaster.Instance.CurrentBusinessName = CompanyName.text;

		if (GameMaster.Instance.OfflineMode == true)
			GameMaster.Instance.CurrentUsername = UserName.text;

		if (CompanyName.text == "")
			text.SetActive (true);
		else 
		{
			loading.SetActive (true);
			StartCoroutine(LoadAsynchronously("Main"));
		}
	
	}

	private IEnumerator LoadAsynchronously(string sceneName)
	{
		AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

		while (!op.isDone)
		{
			float progress = Mathf.Clamp01(op.progress / .9f);

			Debug.Log(progress);

			yield return null;
		}
	}
}
