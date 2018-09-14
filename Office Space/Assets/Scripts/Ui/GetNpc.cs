using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetNpc : MonoBehaviour {

	public GameObject cube1,cube2,cube3;
	public int line;
	public CharacterCustomizationScript Cus1,Cus2,Cus3;
	public TextMeshPro time1, time2, time3;
	int timer1,timer2,timer3;
	bool cusWait1 = false, cusWait2 = false, cusWait3 = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC")
		{
			Enter (other, line);
			Debug.Log("Blooooooooooooooooooooooooooooooop");
		}
	}
	void Update()
	{
//		if (cusWait1 == true)
//		{
//			timer1 -= Time.deltaTime; 
//		}
//
//		if (cusWait2 == true) 
//		{
//			timer2 -= Time.deltaTime; 
//		}
//
//		if (cusWait3 == true) 
//		{
//			timer3 -= Time.deltaTime; 
//		}
//
//
//		if (time1 < 0) 
//		{
//			
//		}
	}

	void Enter(Collider col, int i)
	{
		switch (i) 
		{
		case 1:
			{
				Cus1 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube1.SetActive (true);
				cusWait1 = true;
				Debug.Log ("Customer Counter " + i);
				break;
			}
		case 2:
			{
				Cus2 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube2.SetActive (true);
				cusWait2 = true;
				Debug.Log ("Customer Counter " + i);
				break;
			}
		case 3:
			{
				Cus3 = col.gameObject.GetComponent<CharacterCustomizationScript> ();
				cube3.SetActive (true);
				cusWait3 = true;
				Debug.Log ("Customer Counter " + i);
				break;
			}
		}
	}
}
