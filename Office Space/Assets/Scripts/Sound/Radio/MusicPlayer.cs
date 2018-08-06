using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MusicPlayer : MonoBehaviour {

	string directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments).Replace("\\", "/");

	private List<string> Songs = new List<string>();

	void Awake () 
	{
		directoryPath += "/My Games/OfficeSpaceMusic/";

		if(!Directory.Exists(directoryPath))
		{    
			//if it doesn't, create it
			Directory.CreateDirectory(directoryPath);

		}
	}


	void Start () 
	{

	}


	void Update () 
	{
		
	}
}
