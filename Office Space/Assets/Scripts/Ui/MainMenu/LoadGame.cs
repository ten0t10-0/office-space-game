using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {

	[SerializeField] //saves
	private GameObject SaveContainer;
	private Transform scrollViewContent;

	int selectedSlot = -1;
	GameObject selectedContainer;

	public GameObject confirmPanel;
	public TextMeshProUGUI loadtext;
	public GameObject loading;

	void Start()
	{
		addSaves ();
	}

	void addSaves()
	{
		ClearScroll ();

		for (int i = 0; i < GameMaster.Instance.SaveCountMax; i++)
		{
			GameObject newItem = Instantiate (SaveContainer, scrollViewContent);
			newItem.transform.Find ("num").GetComponent<TMP_Text> ().text = (i + 1).ToString();

			if (File.Exists (GameMaster.Instance.GetSaveFilePath (i))) 
			{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (GameMaster.Instance.GetSaveFilePath (i), FileMode.Open);
				SaveData loadData = (SaveData)bf.Deserialize (file);
				file.Close ();	

				newItem.transform.Find ("empty").GetComponent<TMP_Text> ().text = "";
				newItem.transform.Find ("date").GetComponent<TMP_Text> ().text = loadData.Date.ToString();
			}
			newItem.GetComponent<Button>().onClick.AddListener(delegate {SaveGame(int.Parse(newItem.transform.Find ("num").GetComponent<TMP_Text>().text),newItem);});
		}


	}
	void SaveGame(int i,GameObject newItem)
	{
		selectedContainer = newItem;

		Debug.Log (i-1);
		selectedSlot = i - 1;
		//GameMaster.Instance.SaveGame (i-1);
	}
	public void Load()
	{
		if (!File.Exists (GameMaster.Instance.GetSaveFilePath (selectedSlot))) 
		{
			Debug.Log ("nosaveGame");
			return;
		}
		if (selectedSlot == -1) 
		{
			Debug.Log ("Please select a savegame");
			return;
		}
		else 
		{
			loading.SetActive (true);
            GameMaster.Instance.SaveSlotCurrent = selectedSlot;

			StartCoroutine(LoadAsynchronously("Main"));
			
			Debug.Log ("i is loaded!!??");
		}

		confirmPanel.SetActive (false);
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

    public void LoadPanel()
	{
		loadtext.SetText("Load Game" +selectedSlot+"?");
	}

	public void ClearScroll()
	{

		if (scrollViewContent == null)
		{
			scrollViewContent = transform.Find("Scroll View/Viewport/Content");
		}

		foreach (Transform child in scrollViewContent)
		{
			Destroy(child.gameObject);
		}
	}
}
