using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud2 : MonoBehaviour
{

	public Animator hudO;
	public float fill;
	public Image content;
	public GameObject hudCanvas;
	public TextMeshProUGUI level;
	public GameObject load;

	PauseMenu pause;

	void Awake()
	{
		pause = FindObjectOfType<PauseMenu> ();
	}

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
		float iMin = GameMaster.Instance.Player.GetLevelExperience(GameMaster.Instance.Player.Level);
        float iMax = GameMaster.Instance.Player.GetLevelExperience(GameMaster.Instance.Player.Level + 1);

		return (value - iMin) / (iMax - iMin);

	}

	public void Quit()
	{
		pause.Resume ();
		load.SetActive (true);

        GameMaster.Instance.ModeSetUI();

		StartCoroutine(LoadAsynchronously("MainMenu"));
	}

	private IEnumerator LoadAsynchronously(string sceneName)
	{
		AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

		while (!op.isDone)
		{
			float progress = Mathf.Clamp01(op.progress / .9f);

			Debug.Log(progress);

			yield return null;
		}
	}

}
