using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonClick : MonoBehaviour 
{
	SoundManager sm;

	void Awake ()
	{
		sm = gameObject.GetComponent<SoundManager> ();
	}


	public void Confirm()
	{
		sm.Play ("Confirm");
	}
	public void Cancel()
	{
		sm.Play ("Cancel");
	}
	public void Click()
	{
		sm.Play ("Click");
	}
	public void Hover()
	{
		sm.Play ("Hover");
	}
	public void MenuNext()
	{
		sm.Play ("NextM");
	}

}
