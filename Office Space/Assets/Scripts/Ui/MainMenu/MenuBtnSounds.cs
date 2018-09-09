using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtnSounds : MonoBehaviour {

	SoundManager sm;

	void Awake ()
	{
		sm = gameObject.GetComponent<SoundManager> ();
	}
	void Start()
	{
		sm.Play ("MenuMusic");
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
