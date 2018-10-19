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

	public TextMeshProUGUI monthName;

	public Sprite image;

	void Start () 
	{
		calculateMonth();
    }

    private void Update()
    {
        calculateMonth();
    }

    public void calculateMonth ()
	{

		ClearCalender ();

        month = GameMaster.Instance.GameDateTime.Month;
        day = GameMaster.Instance.GameDateTime.Day;
        year = GameMaster.Instance.GameDateTime.Year;
        monthName.SetText(GameMaster.Instance.GameDateTime.ToString("MMMM"));

        DateTime dt = new DateTime(year, month, 1);
		GameObject newItem;

		switch (dt.DayOfWeek) 
		{
		case DayOfWeek.Tuesday:
			
			newItem = Instantiate (dayContainer, Content);
			newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";
				break;

		case DayOfWeek.Wednesday:
			
				for (int i = 0; i <= 1; i++) 
				{
				newItem = Instantiate (dayContainer, Content);
				newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";

				}
				break;

		case DayOfWeek.Thursday:
			
				for (int i = 0; i <= 2; i++) 
				{
					newItem = Instantiate (dayContainer, Content);
					newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";

				}
				break;

		case DayOfWeek.Friday:
			
				for (int i = 0; i <= 3; i++) 
				{
					newItem = Instantiate (dayContainer, Content);
					newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";

				}
				break;

		case DayOfWeek.Saturday:
			
				for (int i = 0; i <= 4; i++) 
				{
					newItem = Instantiate (dayContainer, Content);
					newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";

				}
				break;

		case DayOfWeek.Sunday:
			
				for (int i = 0; i <= 5; i++) 
				{
					newItem = Instantiate (dayContainer, Content);
					newItem.transform.Find ("day").GetComponent<TMP_Text> ().text = "";

				}
				break;
		}

		


		for (var date = new DateTime (year, month, 1); date.Month == month; date = date.AddDays (1))
		{
			newItem = Instantiate(dayContainer, Content);

			newItem.transform.Find("day").GetComponent<TMP_Text>().text = date.Day.ToString();

			if (date.Day == GameMaster.Instance.GameDateTime.Day) 
			{
				newItem.transform.Find ("Image").GetComponent<Image> ().sprite = image;;
			}
		}


	}

	public void ClearCalender()
	{
		foreach (Transform child in Content)
		{
			Destroy(child.gameObject);
		}
	}
}
