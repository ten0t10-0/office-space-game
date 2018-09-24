using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScores : MonoBehaviour 
{
	[SerializeField] 
	private GameObject SaveContainer;
	private Transform scrollViewContent;

	List<DBPlayer> dbplay = new List<DBPlayer>();

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
		ClearScroll ();
		dbplay = GameMaster.Instance.DBManager.GetHighScores ();
		int i = 1;

		foreach (DBPlayer player in dbplay) 
		{
			GameObject newItem = Instantiate (SaveContainer, scrollViewContent);
			newItem.transform.Find ("Num").GetComponent<TMP_Text> ().text = i.ToString ();
			newItem.transform.Find ("Name").GetComponent<TMP_Text> ().text = player.Username;
			newItem.transform.Find ("Score").GetComponent<TMP_Text> ().text = player.Experience.ToString();

			i++;
		}
	}
}
