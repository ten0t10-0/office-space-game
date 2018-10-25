﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServeCustomer : MonoBehaviour 
{
	public GameObject OpenPanel = null,line1,line2,line3;
	private bool isInsideTrigger = false;
	public Animator hudO, timerA, timerB, timerC;
	CustomerInteractionUI cusI;
	public TextMeshProUGUI time1,time2,time3;
	public Collider col1,col2,col3;

	AiSpawnManager spawner;

	[HideInInspector]
	public CharacterCustomizationScript cus1,cus2,cus3;

	[HideInInspector]
	public AI_try4 AI1;

	[HideInInspector]
	public AI_try3 AI2;

	[HideInInspector]
	public AI_try AI3;


	//float start1;
	bool cusWait1 = false, cusWait2 = false, cusWait3 = false;
	float start1,start2,start3;
    float prevTime1, prevTime2, prevTime3;

	void Awake()
	{
		cusI = FindObjectOfType<CustomerInteractionUI> ();
		spawner = FindObjectOfType<AiSpawnManager> ();
	}

	void Update ()
	{

		if (time1.gameObject.activeInHierarchy)
		{
			if (cusWait1 == true) 
			{
				float t = start1 - (Time.time - prevTime1) + Time.deltaTime;
				string min = ((int)t / 60).ToString ();
				string sec = (t % 60).ToString ("f0").PadLeft(2, '0');
                if (t >= 60)
                {
                    time1.color = new Color32(45, 246, 52, 255);
                }
				if (t < 60) 
				{
					time1.color = new Color32 (249, 137, 0, 255);
				}
				if (t < 30) 
				{
					time1.color = new Color32 (216, 43, 43, 255);
					timerA.SetBool ("TimeO", true);
				}
				if (t <= 0) 
				{
					cusWait1 = false;
					AiExit (1);
					SpawnAfterServed (1);
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
				float t2 = start2 - (Time.time - prevTime2) + Time.deltaTime;
                string min = ((int)t2 / 60).ToString ();
				string sec = (t2 % 60).ToString ("f0").PadLeft(2, '0');
                if (t2 >= 60)
                {
                    time2.color = new Color32(45, 246, 52, 255);
                }
                if (t2 < 60) 
				{
					time2.color = new Color32 (249, 137, 0, 255);
				}
				if (t2 < 30) 
				{
					time2.color = new Color32 (216, 43, 43, 255);
					timerB.SetBool ("TimeO", true);
				}
				if (t2 <= 0) 
				{
					cusWait2 = false;
					AiExit (2);
					SpawnAfterServed (2);
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
				float t3 = start3 - (Time.time - prevTime3) + Time.deltaTime;
                string min = ((int)t3 / 60).ToString ();
				string sec = (t3 % 60).ToString ("f0").PadLeft(2, '0');
                if (t3 >= 60)
                {
                    time3.color = new Color32(45, 246, 52, 255);
                }
                if (t3 < 60) 
				{
					time3.color = new Color32 (249, 137, 0, 255);
				}
				if (t3 < 30) 
				{
					time3.color = new Color32 (216, 43, 43, 255);
					timerC.SetBool ("TimeO", true);
				}
				if (t3 <= 0) 
				{
					cusWait3 = false;
					AiExit (3);
					SpawnAfterServed(3);
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

	public void StartCustomerUi(int sc)
	{
		Debug.Log ("Starting interaction with " + sc.ToString ());

		switch (sc) 
		{
		case 1:
			{
				int c1 = 1;
				line1.SetActive (false);
				OpenPanel.SetActive (false);
				cusI.startInteraction (cus1,c1);
				col1.enabled = false;

				break;
			}
		case 2:
			{
				int c2 = 2;
				OpenPanel.SetActive (false);
				line2.SetActive (false);
				cusI.startInteraction (cus2,c2);
				col2.enabled = false;
				break;
			}
		case 3:
			{
				int c3 = 3;
				OpenPanel.SetActive (false);
				line3.SetActive (false);
				cusI.startInteraction (cus3,c3);
				col3.enabled = false;
				break;
			}
		}
	}
	public void StartTimer(int st)
	{
		switch (st) 
		{
		case 1:
			{
				start1 = Random.Range (45f, 90f);
                prevTime1 = Time.time;
				cusWait1 = true;
				Debug.Log ("start tiiiiiimer");
				break;
			}
		case 2:
			{
				start2 = Random.Range(45f, 90f);
                    prevTime2 = Time.time;
				cusWait2 = true;
				break;
			}
		case 3:
			{	
				start3 = Random.Range(45f, 90f);
                    prevTime3 = Time.time;
				cusWait3 = true;
	
				break;
			}
		}
	}
	public void AiExit(int ae)
	{
		Debug.Log ("Runblopps>"+ae.ToString ());
		switch (ae) 
		{
		case 1:
			{
				//AI1.exit4 = true;
				AI1.cur_timer = 0;
				break;
			}
		case 2:
			{
				//AI2.exit3 = true;
				AI2.cur_timer = 0;
				break;
			}
		case 3:
			{	
				//AI3.exit = true;
				AI3.cur_timer = 0;
				break;
			}
		}
	}

	void spawnAI2()
	{
		spawner.SpawnAI2();
	}
	void spawnAI3()
	{
		spawner.SpawnAI3();
	}
	void spawnAI1()
	{
		spawner.SpawnAI1();
	}

	void SpawnAfterServed(int i)
	{
		if (GameMaster.Instance.DayEnd == false)
		{
			switch (i) 
			{
			case 1:
				{
					Invoke ("spawnAI1", Random.Range (3f, 6f));
					break;
				}
			case 2:
				{
					Invoke ("spawnAI2", Random.Range (3f, 5f));
					break;
				}
			case 3:
				{
					Invoke ("spawnAI3", Random.Range (3f, 5f));
					break;
				}
			}
		}
	}
	public void setFalse()
	{
		line1.SetActive (false);
		line3.SetActive (false);
		line2.SetActive (false);
	}

	public void Counter1False()
	{
		line1.SetActive (false);
		col1.enabled = false;
	}

	public void Counter2False()
	{
		line2.SetActive (false);
		col2.enabled = false;
	}

	public void Counter3False()
	{
		line3.SetActive (false);
		col3.enabled = false;
	}

	public void Counter1True()
	{
		line1.SetActive (true);
		col1.enabled = true;
	}

	public void Counter2True()
	{
		line2.SetActive (true);
		col2.enabled = true;
	}

	public void Counter3True()
	{
		line3.SetActive (true);
		col3.enabled = true;
	}

}
