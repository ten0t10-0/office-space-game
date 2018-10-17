using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtnSounds : MonoBehaviour 
{
	MenuSound sm;

	void Awake ()
	{
		sm = gameObject.GetComponent<MenuSound> ();
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
	public void Back()
	{
		sm.Play ("Back");
	}
	public void MenuNext()
	{
		sm.Play ("NextM");
	}
}
