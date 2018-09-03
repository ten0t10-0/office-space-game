using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievmentsUI : MonoBehaviour 
{

	public GameObject achievment;
	public TextMeshProUGUI text;
	public Animator acheiv;
	Queue aQ = new Queue();
	bool playing = false;

//	void Update()
//	{
//		if (aQ.Count > 0 && playing == false )
//			StartCoroutine(DisplayAll());
//			
//	}
//
//
	public void addAcheivment(string text)
	{
		aQ.Enqueue (text);


	}
//
//	public void displayAchievment (string description)
//	{
//		playing = true;
//		text.SetText (description);
//		achievment.SetActive (true);
//		acheiv.SetBool ("achO", true);
//
//		//StartCoroutine(showAchievment());
//		FindObjectOfType<SoundManager>().Play("Achievement");
//		aQ.Dequeue ();
//		acheiv.SetBool ("achO", false);
//	}
////	IEnumerator showAchievment()
////	{
////		yield return new WaitForSeconds(5);
////
////		achievment.SetActive (false);
////
////	}
//
//	IEnumerator DisplayAll()
//	{
//		playing = true; 
//		WaitForSeconds wait = new WaitForSeconds( 5f ) ;
//		foreach (string i in aQ) 
//		{
//			playing = true;
//			displayAchievment (aQ.);
//			Debug.Log ("Scooop"+aQ.ToString());
//			yield return wait;
//		}
//		playing = false;
//	}

}
