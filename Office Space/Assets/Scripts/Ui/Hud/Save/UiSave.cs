﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System;


public class UiSave : MonoBehaviour 
{
	[SerializeField] //saves
	private GameObject SaveContainer;
	private Transform scrollViewContent;

	int selectedSlot = -1;
	GameObject selectedContainer;

	public GameObject confirmPanel;
	public TextMeshProUGUI loadtext;
	public TextMeshProUGUI savetext;
	public TextMeshProUGUI text;
	public InputField savename;

	string message = "";

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
			newItem.transform.Find ("Button/Slot").GetComponent<TMP_Text> ().text = (i + 1).ToString();

			if (File.Exists (GameMaster.Instance.GetSaveFilePath (i))) 
			{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (GameMaster.Instance.GetSaveFilePath (i), FileMode.Open);
				SaveData loadData = (SaveData)bf.Deserialize (file);
				file.Close ();	

				newItem.transform.Find ("Button/Empty").GetComponent<TMP_Text> ().text = "";
				newItem.transform.Find ("Button/Date").GetComponent<TMP_Text> ().text = loadData.Date.ToString();
			}
			newItem.transform.Find ("Button").GetComponent<Button>().onClick.AddListener(delegate {SaveGame(int.Parse(newItem.transform.Find ("Button/Slot").GetComponent<TMP_Text>().text),newItem);});
		}
				
				
	}
	void SaveGame(int i,GameObject newItem)
	{
		selectedContainer = newItem;
	
		Debug.Log (i-1);
		selectedSlot = i - 1;
		//GameMaster.Instance.SaveGame (i-1);
	}
	public void Save()
	{
		Debug.Log ("Runnnnn PPPPP");
		if (selectedSlot == -1) 
		{
			DisplayMessage ("Please select a savegame");
			Debug.Log ("Please select a savegame");
			return;
		}
		else
			GameMaster.Instance.SaveGame (selectedSlot,savename.ToString());

		DisplayMessage ("Game Saved");

		selectedContainer.transform.Find ("Button/Empty").GetComponent<TMP_Text> ().text = "";
		selectedContainer.transform.Find ("Button/Date").GetComponent<TMP_Text> ().text = DateTime.Now.ToString();
		selectedContainer.transform.Find ("Button/Name").GetComponent<TMP_Text> ().text = savename.text;

		confirmPanel.SetActive (false);
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
			DisplayMessage ("Please select a savegame");
			Debug.Log ("Please select a savegame");
			return;
		}
		else 
		{
			GameMaster.Instance.LoadGame(selectedSlot);
			Debug.Log ("i is loaded!!??");
		}
			
		confirmPanel.SetActive (false);
	}
	public void SavePanel()
	{
		savetext.SetText("Save Game using slot " +(selectedSlot+1) +"?");
	}
	public void LoadPanel()
	{
		loadtext.SetText("Load Game " +(selectedSlot + 1 )+"?");
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
	public void DisplayMessage(string m)
	{
		text.SetText (m);
		text.gameObject.SetActive (true); 
		StartCoroutine (messages ());
	}
	IEnumerator messages()
	{
		yield return new WaitForSeconds(4);
		text.gameObject.SetActive (false);
	}
}
