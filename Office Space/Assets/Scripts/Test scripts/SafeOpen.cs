using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeOpen : MonoBehaviour 
{

	public Animator safe;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;
	bool open = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				
				if (open == true) {
					safe.SetBool ("SafeO", true);
					open = false;
				}
					else {
					safe.SetBool ("SafeO", false);
					open = true;
				}
			}
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
		}
	}

	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}
}
