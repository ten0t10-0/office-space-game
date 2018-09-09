using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievmentsUI : MonoBehaviour 
{

	public GameObject achievment;
	public TextMeshProUGUI text;
	public Animator acheiv;
	//Queue<string> aQ = new Queue<string>();
	List<string> ach = new List<string>();
	bool playing = false;
	bool running;
	WaitForSeconds wait = new WaitForSeconds(5);
	int c =1,i=0;

	void Update()
	{
		if (ach.Count > 0 && playing == false) 
		{
			Debug.Log ("Ir Staaaaaaarted");
			c = 0;
			i = ach.Count;
			running = true;
			DisplayAll();


		}
			
			
	}


	public void addAcheivment(string text)
	{
		ach.Add(text);


	}

	public void setfalse ()
	{
		acheiv.SetBool ("achO", false);
	}


	public void displayAchievment (string description)
	{
		
		playing = true;
		text.SetText (description);
		//achievment.SetActive (true);
		acheiv.SetBool ("achO", true);

		//StartCoroutine(showAchievment());
		FindObjectOfType<SoundManager>().Play("Achievement");
		//acheiv.SetBool ("achO", false);

	}

	public void DisplayAll()
	{
		
		Debug.Log ("Im Callledpppppppppppppppppp");

		if (running == true) 
		{

			playing = true; 

			Debug.Log ("Staaart c = " + c + " i = " + i);
			displayAchievment (ach [c]);

			//StartCoroutine (showAchievment ());
			Debug.Log ("Bloooooooop" + ach [c].ToString ());
			c++;
			Debug.Log ("++ c = " + c + " i = " + i);

			if (c > i-1) 
			{
				Debug.Log("RunwhenPPPPPPPPPPPPPPPPPPPPPPP");
				ach.Clear ();
				playing = false;
				running = false;
			}
		}
	}

}
