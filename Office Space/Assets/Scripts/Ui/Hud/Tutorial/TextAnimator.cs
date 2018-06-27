using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextAnimator : MonoBehaviour {

	public TextMeshProUGUI textArea;
	public string[] startText;
	public float speed = 0.5f;
	public GameObject textPanel;

	int sIndex = 0;
	int characterIndex = 0;

	public static bool TutorialMode = true;

	// Use this for initialization
	void Start () 
	{
//		if (TutorialMode == true) 
//		{
			StartCoroutine (displayTimer ());
			textPanel.SetActive (true);
//		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			if (characterIndex < startText [sIndex].Length) 
			{
				characterIndex = startText [sIndex].Length;
			}
			else if(sIndex < startText.Length)
			{
				sIndex++;
				characterIndex = 0;
			}
		}
	}

	IEnumerator displayTimer()
	{
		while (1 == 1) 
		{
			yield return new WaitForSeconds(speed);
			if (characterIndex > startText [sIndex].Length) 
			{
				continue;
			}


			textArea.SetText( startText[sIndex].Substring(0,characterIndex));
			characterIndex++;
		}
	}
}
