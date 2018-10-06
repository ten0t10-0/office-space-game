using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAnimationDisable : MonoBehaviour 
{
	public GameObject UiObject;

	public void DisableUIObject()
	{
		UiObject.SetActive (false);
	}
}
