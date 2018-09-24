using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour 

{
	public AudioMixer mix;
	Resolution[] res;
	public TMP_Dropdown dropdown;

	void Start()
	{
        res = Screen.resolutions;
		dropdown.ClearOptions ();

		List<string> options = new List<string> ();
		int currentRes = 0;

		for (int i = 0; i < res.Length; i++) 
		{
			string option = res [i].width + " x " + res [i].height;
			options.Add (option);

			if (res [i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height) 
			{
				currentRes = i;
			}
		}
		dropdown.AddOptions (options);
		dropdown.value = currentRes;
		dropdown.RefreshShownValue ();
	}

	public void setVolume(float volume)
	{
		mix.SetFloat ("Volume",volume);
	}
	public void SetQuality(int index)
	{
		QualitySettings.SetQualityLevel (index);
	}
	public void SetFull(bool isFull)
	{
        Screen.fullScreen = !Screen.fullScreen;
	}
	public void Setres(int resIndex)
	{
		Resolution resolution = res [resIndex];

		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
	}
}
