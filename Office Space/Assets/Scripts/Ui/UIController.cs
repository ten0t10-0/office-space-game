using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour 
{
	public GameObject buildmode,endDayOffice,endDayShop;
	public Animator buildM,endDayOffA,endDayShopA;

	public Collider ShopDoor, ClosetDoor, PcTrigger;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void BuildModeUIOpen()
	{
		buildmode.SetActive (true);
		buildM.SetBool ("BuildO", true);
		ShopDoor.enabled = false;
		ClosetDoor.enabled = false;
		PcTrigger.enabled = false;
	}
	public void BuildModeUIClose()
	{
		
		buildM.SetBool ("BuildO", false);
		ShopDoor.enabled = true;
		ClosetDoor.enabled = true;
		PcTrigger.enabled = true;
		StartCoroutine (BuildClose ());

	}
	IEnumerator BuildClose()
	{
		yield return new WaitForSeconds(2);
		buildmode.SetActive (false);
	}

	public void EndDayOffice()
	{
		endDayOffice.SetActive (true);
		endDayOffA.SetBool ("EndOfficeO", true);
		//set varibles
	}
	public void EndDayShop()
	{
		endDayOffice.SetActive (true);
		endDayOffA.SetBool ("EndShopO", true);
		//set varibles
	}

}
