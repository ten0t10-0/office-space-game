using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Calender : MonoBehaviour {


	[SerializeField] //items
	private GameObject dayContainer;
	public Transform Content;

	int month;
	int day ;
	int year;


	//int daysInMonth;


	void Start () 
	{
	   //month = GameMaster.Instance.GameDateTime.Month;
		month = 9;
		day = GameMaster.Instance.GameDateTime.Day;
		year = GameMaster.Instance.GameDateTime.Year;


		calculateMonth ();


	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void calculateMonth ()
	{

//		DayOfWeek dayName = GameMaster.Instance.GameDateTime.DayOfWeek;
//		GameObject newBlank;
//
//
//		switch (dayName) {
//		case DayOfWeek.Tuesday:
//			
//				newBlank = Instantiate (dayContainer, Content);
//				newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//				break;
//
//		case DayOfWeek.Wednesday:
//			
//				for (int i = 0; i >= 1; i++) {
//					newBlank = Instantiate (dayContainer, Content);
//					newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//
//				}
//				break;
//
//		case DayOfWeek.Thursday:
//			
//				for (int i = 0; i >= 2; i++) {
//					newBlank = Instantiate (dayContainer, Content);
//					newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//
//				}
//				break;
//
//		case DayOfWeek.Friday:
//			
//				for (int i = 0; i >= 3; i++) {
//					newBlank = Instantiate (dayContainer, Content);
//					newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//
//				}
//				break;
//
//		case DayOfWeek.Saturday:
//			
//				for (int i = 0; i >= 4; i++) {
//					newBlank = Instantiate (dayContainer, Content);
//					newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//
//				}
//				break;
//
//		case DayOfWeek.Sunday:
//			
//				for (int i = 0; i >= 5; i++) {
//					newBlank = Instantiate (dayContainer, Content);
//					newBlank.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
//
//				}
//				break;
//			}

		


		for (var date = new DateTime (year, month, 1); date.Month == month; date = date.AddDays (1))
		{
			
			GameObject newItem = Instantiate(dayContainer, Content);

			newItem.transform.Find("day").GetComponent<TMP_Text>().text = date.Day.ToString();

			//if (date.Day == day) 

				//var panel = newItem.transform.Find ("Colour");

			
		


		}


	}
}
