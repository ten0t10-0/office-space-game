using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievmentsUI : MonoBehaviour 
{

	public GameObject achievment;
	public TextMeshProUGUI text;
	public Animator acheiv;
	List<string> ach = new List<string>();
	bool playing = false;
	bool running;
	WaitForSeconds wait = new WaitForSeconds(5);
	int c =1,i=0;

	void Update()
	{
		if (ach.Count > 0 && playing == false) 
		{
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
		acheiv.SetBool ("achO", true);

		FindObjectOfType<SoundManager>().Play("Achievement");

	}

	public void DisplayAll()
	{

		if (running == true) 
		{
			playing = true; 

			displayAchievment (ach [c]);

			c++;

			if (c > i-1) 
			{
				ach.Clear ();
				playing = false;
				running = false;
			}
		}
	}

}
