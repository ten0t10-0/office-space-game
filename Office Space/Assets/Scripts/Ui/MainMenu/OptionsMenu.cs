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
	public TextMeshProUGUI percentage;

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
	public void UpdateVolume(float value)
	{
		float min = -80f;
		float max = 0f; 
		float scaledValue;
		scaledValue = (value - min) / (max - min);
		if (scaledValue < .3)
			percentage.color = Color.red;
		if (scaledValue > .3)
			percentage.color = Color.yellow;
		if (scaledValue > .7)
			percentage.color = Color.green;
		
		percentage.SetText ((scaledValue * 100).ToString("f0") + "%");
	}
}
