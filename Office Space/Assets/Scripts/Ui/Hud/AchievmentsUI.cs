using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievmentsUI : MonoBehaviour 
{

	public GameObject achievment;
	public TextMeshProUGUI text;

	public void displayAchievment (string description)
	{
		text.SetText (description);
		achievment.SetActive (true);

		StartCoroutine(showAchievment());
		FindObjectOfType<SoundManager>().Play("Achievement");
	}
	IEnumerator showAchievment()
	{
		yield return new WaitForSeconds(3);
		achievment.SetActive (false);
	}

}
