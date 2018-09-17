using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializerScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
		GameMaster.Instance.InitializeGame (GameMaster.Instance.SaveSlotCurrent);
	}
}
