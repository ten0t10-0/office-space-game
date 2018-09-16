using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServeCustomer : MonoBehaviour 
{
	public GameObject OpenPanel = null,line1,line2,line3;
	private bool isInsideTrigger = false;
	public Animator hudO, timerA, timerB, timerC;
	public int line;
	CustomerInteractionUI cusI;
	public TextMeshProUGUI time1,time2,time3;

	[HideInInspector]
	public CharacterCustomizationScript cus1,cus2,cus3;

	//float start1;
	bool cusWait1 = false, cusWait2 = false, cusWait3 = false;
	float start1,start2,start3;

	void Awake()
	{
		cusI = FindObjectOfType<CustomerInteractionUI> ();
	}

	void Update ()
	{

		if (time1.gameObject.activeInHierarchy)
		{
			if (cusWait1 == true) 
			{
				float t = start1 - Time.time;
				string min = ((int)t / 60).ToString ();
				string sec = (t % 60).ToString ("f0");
				if (t < 60) {
					time1.color = new Color32 (249, 137, 0, 255);
				}
				if (t < 30) {
					time1.color = new Color32 (216, 43, 43, 255);
					timerA.SetBool ("TimeO", true);
				}
				if (t <= 0) 
				{
					cusWait1 = false;
					line1.SetActive (false);
					Debug.Log ("end tiiiiiimer");
				}
				time1.SetText (min + ":" + sec);
			}

		}
		if (time2.gameObject.activeInHierarchy)
		{
			if (cusWait2 == true) 
			{
				float t2 = start2 - Time.time;
				string min = ((int)t2 / 60).ToString ();
				string sec = (t2 % 60).ToString ("f0");
				if (t2 < 60) {
					time2.color = new Color32 (249, 137, 0, 255);
				}
				if (t2 < 30) {
					time2.color = new Color32 (216, 43, 43, 255);
					timerB.SetBool ("TimeO", true);
				}
				if (t2 <= 0) 
				{
					cusWait2 = false;
					line2.SetActive (false);
					Debug.Log ("end tiiiiiimer");
				}
				time2.SetText (min + ":" + sec);
			}

		}
		if (time3.gameObject.activeInHierarchy)
		{
			if (cusWait3 == true) 
			{
				float t3 = start3 - Time.time;
				string min = ((int)t3 / 60).ToString ();
				string sec = (t3 % 60).ToString ("f0");
				if (t3 < 60) {
					time3.color = new Color32 (249, 137, 0, 255);
				}
				if (t3 < 30) {
					time3.color = new Color32 (216, 43, 43, 255);
					timerA.SetBool ("TimeO", true);
				}
				if (t3 <= 0) 
				{
					cusWait3 = false;
					line3.SetActive (false);
					Debug.Log ("end tiiiiiimer");
				}
				time3.SetText (min + ":" + sec);
			}

		}


	}




	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}

	public void StartCustomerUi(int i)
	{
		switch (i) 
		{
		case 1:
			{
				Debug.Log ("Wooooop1111111111111111111111111");
				line1.SetActive (false);
				OpenPanel.SetActive (false);
				cusI.startInteraction (cus1);


				break;
			}
		case 2:
			{
				OpenPanel.SetActive (false);
				line2.SetActive (false);
				cusI.startInteraction (cus2);
				break;
			}
		case 3:
			{
				OpenPanel.SetActive (false);
				line3.SetActive (false);
				cusI.startInteraction (cus3);
				break;
			}
		}
	}
	public void StartTimer(int i)
	{
		switch (i) 
		{
		case 1:
			{
				start1 = Random.Range (100f, 250f);
				cusWait1 = true;
				Debug.Log ("start tiiiiiimer");
				break;
			}
		case 2:
			{
				start2 = Random.Range (100f, 250f);
				cusWait2 = true;
				break;
			}
		case 3:
			{	
				start3 = Random.Range (100f, 250f);
				cusWait3 = true;
	
				break;
			}
		}
	}

}
