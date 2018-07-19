using UnityEngine;
using System.Collections;
using System;

public class DayNightController : MonoBehaviour 
{

    public Light sun;

    [Range(0,1)]
	public float currentTimeOfDay = 0;
	float time;

    float sunInitialIntensity;
    void Start() 
	{
        sunInitialIntensity = sun.intensity;
    }

    void Update() 
	{

        UpdateSun();

		time = (Convert.ToSingle(GameMaster.Instance.GameDateTime.Hour)) + (Convert.ToSingle(GameMaster.Instance.GameDateTime.Minute) / 100);
		currentTimeOfDay = time / 24;
	
        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun() 
	{

        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
	
        float intensityMultiplier = 1;

        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) 
		{
            intensityMultiplier = 0;
        }

        else if (currentTimeOfDay <= 0.25f) 
		{

            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }

        else if (currentTimeOfDay >= 0.73f) 
		{
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}