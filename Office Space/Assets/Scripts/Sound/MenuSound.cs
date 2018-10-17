using UnityEngine;
using UnityEngine.Audio;
using System;

public class MenuSound : MonoBehaviour 
{

	public SoundClass[] sounds;

	public AudioMixerGroup Master;

	void Awake () 
	{
		foreach (SoundClass s in sounds) 
		{
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.outputAudioMixerGroup = Master;
			s.source.clip = s.clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.loop = s.loop;
		}

	}
	void Start()
	{
		Play ("Theme");
	}

	public void Play(string name)
	{
		SoundClass s = Array.Find (sounds, sound => sound.name == name);

		s.source.Play ();
	}
}
