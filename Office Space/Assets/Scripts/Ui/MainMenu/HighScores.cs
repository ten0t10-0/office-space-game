using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScores : MonoBehaviour 
{
	[SerializeField] 
	private GameObject SaveContainer;
	private Transform scrollViewContent;

	public void ClearScroll()
	{

		if (scrollViewContent == null)
		{
			scrollViewContent = transform.Find("Scroll View/Viewport/Content");
		}

		foreach (Transform child in scrollViewContent)
		{
			Destroy(child.gameObject);
		}
	}

	public void AddScores()
	{
		//foreach
//		GameObject newItem = Instantiate (SaveContainer, scrollViewContent);
//		newItem.transform.Find ("Num").GetComponent<TMP_Text> ().text = (i + 1).ToString();
//		newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = (i + 1).ToString();
//		newItem.transform.Find ("Score").GetComponent<TMP_Text> ().text = (i + 1).ToString();
	}
}
