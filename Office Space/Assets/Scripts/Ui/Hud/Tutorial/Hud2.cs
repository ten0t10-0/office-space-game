using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud2 : MonoBehaviour
{

	public Animator hudO;
	public float fill;
	public Image content;
	public GameObject hudCanvas;
	public TextMeshProUGUI level;

	void Update()
	{

		FillBar();
		level.SetText (GameMaster.Instance.Player.Level.ToString ());
	}


	public void FillBar()
	{
		content.fillAmount = CalculateBar();
	}

	public float CalculateBar()
	{
		float value = GameMaster.Instance.Player.Experience;
		float iMin = 0;
		float iMax = GameMaster.Instance.Player.GetLevelExperience(GameMaster.Instance.Player.Level);

		return (value - iMin) / (iMax - iMin);

	}

}
