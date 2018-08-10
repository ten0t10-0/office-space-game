using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour 
{

	[SerializeField]
	private AudioClip[] clips;

	AudioSource audioSource;

	// Use this for initialization
	private void Awake () 
	{
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	private void Step()
	{
		AudioClip clip = GetRandomClip ();
		audioSource.PlayOneShot(clip);	
	}

	private AudioClip GetRandomClip()
	{
		return clips [UnityEngine.Random.Range (0, clips.Length)];
	}
}
