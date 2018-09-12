﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneScreen : MonoBehaviour 
{


	public Animator animator;
	public GameObject tab,noti,order;
	public TextMeshProUGUI topbar,notiT,orderT;
	int count = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameMaster.Instance.OrderManager.CountOpen > 0) 
		{
			order.SetActive (true);
			orderT.SetText (GameMaster.Instance.OrderManager.CountOpen.ToString ());
		}
		else
			order.SetActive (false);
		
		if (count > 0) 
		{
			noti.SetActive (true);
			notiT.SetText (count.ToString());
		}
		else
			noti.SetActive (false);
		
	}

	public void OpenTablet()
	{
		animator.SetBool ("PhoneH", true);
		StartCoroutine (TabOpen());
	}

	public void CloseTablet()
	{
		animator.SetBool ("PhoneH", false);
		tab.SetActive (false);
		//animator.SetBool ("PhoneN", false);
	}

	IEnumerator TabOpen()
	{
		yield return new WaitForSeconds(.5f);
		tab.SetActive (true);
		GameMaster.Instance.ModeSetUI ();

	}
	public void BuildMode()
	{
		if (GameMaster.Instance.BuildMode == true)
			GameMaster.Instance.BuildMode = false;
		else
			GameMaster.Instance.BuildMode = true;
	}
	public void home()
	{
		topbar.SetText ("Home");
	}
	public void Note()
	{
		topbar.SetText ("Notifications");
	}
	public void Order()
	{
		topbar.SetText ("Orders");
	}
	public void noteCount()
	{
		count = 0;
		List <Notification> noteList = GameMaster.Instance.Notifications.GetAll ();

		foreach (Notification note in noteList) 
		{
			if (note.Read == false)
			{
				count++;
			}

		}
	}
}
