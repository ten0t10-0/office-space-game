using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {

	private Queue<string> sentences;

	public TextMeshProUGUI name;
	public TextMeshProUGUI text;

	public Animator animator;
	string letters;

	public int count = 0;
	public GameObject tutorialGuy,spawnLocation,next,canvas;
	private GameObject currentGuy;
	public bool done = true;
	[HideInInspector]
	public CharacterAnimationScript charaA;


	void Start () 
	{
		sentences = new Queue<string> ();
		currentGuy = Instantiate (tutorialGuy, spawnLocation.transform);

		currentGuy.GetComponent<CharacterCustomizationScript>().UnsetAllClothing();
        currentGuy.GetComponent<CharacterCustomizationScript>().SetClothing(1);
        currentGuy.GetComponent<CharacterCustomizationScript>().SetAccessoriesByPreset(GameMaster.Instance.CustomizationManager.Character.AccessoryPresets[0]);
        currentGuy.GetComponent<CharacterCustomizationScript>().UpdateBodyColor(GameMaster.Instance.CustomizationManager.Character.SkinColors[3]);

        charaA = currentGuy.GetComponent<CharacterAnimationScript> ();
		canvas.SetActive (false);

	}


	public void StartDialogue(Dialogue dialogue)
	{
		name.SetText (dialogue.name);
		Debug.Log(dialogue.sentences [1]);
		animator.SetBool ("UpO", true);

		sentences.Clear ();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence ();
	}

	public void DisplayNextSentence()
	{
		count++;
	
		if (sentences.Count == 0) 
		{
			EndDialogue ();
			return;
		}
		string sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		next.SetActive (false);
		StartCoroutine(TypeSentence(sentence));

		//text.SetText (sentence);
	}

	void EndDialogue()
	{
		count = 0;
		animator.SetBool ("UpO", false);
	}

	IEnumerator TypeSentence(string sentence)
	{
		text.SetText ("");
		done = false;
		foreach (char letter in sentence.ToCharArray()) 
		{
			letters += letter;
			text.SetText (letters);

			yield return null;
		}
		done = true;
		letters = "";
		next.SetActive (true);
	}

}
