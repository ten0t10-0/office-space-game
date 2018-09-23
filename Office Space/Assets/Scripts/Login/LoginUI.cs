using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour 
{
	public GameObject loginP;
	public GameObject registerP;
	public GameObject loadingP;

	public InputField LoginUsernameField;
	public InputField LoginPasswordField;
	public InputField RegisterUsernameField;
	public InputField RegisterPasswordField;
	public InputField RegisterConfirmPasswordField;

	public Text LoginErrorText;
	public Text RegisterErrorText;

	private string playerUsername = "";
	private string playerPassword = "";

	void ResetAllUIElements ()
	{
		//This resets all of the UI elements. It clears all the strings in the input fields and any errors being displayed
		LoginUsernameField.text = "";
		LoginPasswordField.text = "";
		RegisterUsernameField.text = "";
		RegisterPasswordField.text = "";
		RegisterConfirmPasswordField.text = "";
		LoginErrorText.text = "";
		RegisterErrorText.text = "";

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Login_LoginButtonPressed ()
	{
		//Called when player presses button to Login

		//Get the username and password the player entered
		playerUsername = LoginUsernameField.text;
		playerPassword = LoginPasswordField.text;

		//Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
		if (playerUsername.Length > 3)
		{
			if (playerPassword.Length > 5)
			{
				//Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
				loginP.gameObject.SetActive(false);
				loadingP.gameObject.SetActive(true);
				//StartCoroutine(LoginUser());
			}
			else
			{
				//Password too short so it must be wrong
				LoginErrorText.text = "Error: Password Incorrect";
			}
		} else
		{
			//Username too short so it must be wrong
			LoginErrorText.text = "Error: Username Incorrect";
		}
	}

	public void Register_RegisterButtonPressed ()
	{
		//Called when the player presses the button to register

		//Get the username and password and repeated password the player entered
		playerUsername = RegisterUsernameField.text;
		playerPassword = RegisterPasswordField.text;

		string confirmedPassword = RegisterConfirmPasswordField.text;

		//Make sure username and password are long enough
		if (playerUsername.Length > 3)
		{
			if (playerPassword.Length > 5)
			{
				//Check the two passwords entered match
				if (playerPassword == confirmedPassword)
				{
					//Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
					registerP.gameObject.SetActive(false);
					loadingP.gameObject.SetActive(true);
					//StartCoroutine(RegisterUser());
				}
				else
				{
					//Passwords don't match, show error
					RegisterErrorText.text = "Error: Password's don't Match";
				}
			}
			else
			{
				//Password too short so show error
				RegisterErrorText.text = "Error: Password too Short";
			}
		}
		else
		{
			//Username too short so show error
			RegisterErrorText.text = "Error: Username too Short";
		}
	}

	public void BackBtn ()
	{
		ResetAllUIElements();
		loginP.gameObject.SetActive(true);
		registerP.gameObject.SetActive(false);
	}

	public void RegisterBtn ()
	{
		ResetAllUIElements();
		loginP.gameObject.SetActive(false);
		registerP.gameObject.SetActive(true);
	}
}
