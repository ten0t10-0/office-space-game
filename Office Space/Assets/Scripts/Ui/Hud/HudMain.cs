using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudMain : MonoBehaviour {


	public TextMeshProUGUI money;
	public TextMeshProUGUI time;

	void Start () 
	{
		money.SetText("$ "+(GameMaster.Instance.Player.Business.Money).ToString());


	}
	

	void Update () 
	{
		time.SetText (GameMaster.Instance.GameTimeString12 ());
	}
}
