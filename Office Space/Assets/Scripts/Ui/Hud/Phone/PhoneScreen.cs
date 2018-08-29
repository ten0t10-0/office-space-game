using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScreen : MonoBehaviour {


	public Animator animator;
	public GameObject tab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
