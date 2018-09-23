using System.Collections;
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
	bool endOfDay = false;

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
				float t = start1 - Time.time;
				string min = ((int)t / 60).ToString ();
				string sec = (t % 60).ToString ("f0");
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
					AI1.exit4 = true;
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
				float t2 = start2 - Time.time;
				string min = ((int)t2 / 60).ToString ();
				string sec = (t2 % 60).ToString ("f0");
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
					AI2.exit3 = true;
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
				float t3 = start3 - Time.time;
				string min = ((int)t3 / 60).ToString ();
				string sec = (t3 % 60).ToString ("f0");
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
					AI3.exit = true;
					SpawnAfterServed(3);
					line3.SetActive (false);
					Debug.Log ("end tiiiiiimer");
				}
				time3.SetText (min + ":" + sec);
			}

		}
		if (GameMaster.Instance.DayEnd == true) 
		{
			endOfDay = true;
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
	public void AiExit(int ae)
	{
		Debug.Log ("Runblopps>"+ae.ToString ());
		switch (ae) 
		{
		case 1:
			{
				AI1.exit4 = true;
				break;
			}
		case 2:
			{
				AI2.exit3 = true;
				break;
			}
		case 3:
			{	
				AI3.exit = true;
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
		if (endOfDay == false)
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

}
