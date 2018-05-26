using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class floatText : MonoBehaviour 
{
	public Animator animator;
	private TextMeshProUGUI moneyText;
	static public GameObject mText;

	void OnEnable()
	{
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

		Destroy(gameObject, clipInfo[0].clip.length);

	}

	public void SetText(string text)
	{
		moneyText.SetText (text);
	}
	public static void CreateText(string text)
	{
		GameObject newSupp = Instantiate (mText);
	}


}

