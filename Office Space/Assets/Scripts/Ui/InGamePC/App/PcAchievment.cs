using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PcAchievment : MonoBehaviour
{
	[SerializeField] 
	private GameObject Container;

	private Transform scrollView;

	int i =0;

	void Start()
	{
		
	}

	public void AddAchievment()
	{
		ClearScroll ();

		foreach (AchievementSO item in GameMaster.Instance.AchievementManager.Achievements) 
		{

			GameObject newItem = Instantiate (Container, scrollView);
			newItem.transform.Find("Name").GetComponent<TMP_Text> ().text = item.des.ToString();
			newItem.transform.Find("Des").GetComponent<TMP_Text> ().text = item.Name;
			if (i == unlocked (i))
				newItem.transform.Find ("Image").GetComponent<Image> ().gameObject.SetActive(false);
			i++;

		}
	}
	int unlocked(int index)
	{
		foreach (int i in GameMaster.Instance.Player.UnlockedAchievements) 
		{
			if (i == index)
				return i;
			
		}
		return -1;
	}

	public void ClearScroll()
	{
		if (scrollView == null)
		{
			scrollView = transform.Find("Scroll View/Viewport/Content");
		}
		foreach (Transform child in scrollView)
		{
			Destroy(child.gameObject);
		}
	}
}
