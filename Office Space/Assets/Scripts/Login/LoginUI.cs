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

	public Transform mainMount;
	MenuCamGuide menucam;

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
	void Start () 
	{
		menucam = FindObjectOfType<MenuCamGuide> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Login_LoginButtonPressed()
    {
        if (!GameMaster.Instance.OfflineMode)
            GameMaster.Instance.OfflineMode = false;

        //Called when player presses button to Login

        //Get the username and password the player entered
        playerUsername = LoginUsernameField.text;
        playerPassword = LoginPasswordField.text;

        //Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
        if (playerUsername != "" && playerUsername != null && playerPassword != "" && playerPassword != null)
        {
            if (GameMaster.Instance.DBManager.CheckUsername(playerUsername) && playerPassword == GameMaster.Instance.DBManager.GetPassword(playerUsername))
            {
                //Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
                loginP.gameObject.SetActive(false);
                loadingP.gameObject.SetActive(true);
				StartCoroutine (MainMenus ());

                GameMaster.Instance.CurrentUsername = playerUsername;
            }
            else
            {
                //Details incorrect
                LoginErrorText.text = "Error: Please check details";
            }
        }
        else
        {
            //Details not filled in
            LoginErrorText.text = "Error: Please enter both username and password";
        }
    }

	public void Register_RegisterButtonPressed ()
	{
        if (!GameMaster.Instance.OfflineMode)
            GameMaster.Instance.OfflineMode = false;

        //Called when the player presses the button to register

        //Get the username and password and repeated password the player entered
        playerUsername = RegisterUsernameField.text;
		playerPassword = RegisterPasswordField.text;

		string confirmedPassword = RegisterConfirmPasswordField.text;

		//Make sure username and password are long enough
		if (playerUsername.Length > 3 && playerUsername.Length <= 15)
		{
            if (!GameMaster.Instance.DBManager.CheckUsername(playerUsername))
            {
                if (playerPassword.Length > 5 && playerPassword.Length <= 25)
                {
                    //Check the two passwords entered match
                    if (playerPassword == confirmedPassword)
                    {
                        //Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
                        registerP.gameObject.SetActive(false);
                        loadingP.gameObject.SetActive(true);
						StartCoroutine (MainMenus ());

                        DBPlayer player = new DBPlayer()
                        {
                            Username = playerUsername,
                            Experience = 0,
                            Money = 0f
                        };

                        if (GameMaster.Instance.DBManager.AddPlayer(player, playerPassword))
                            Debug.Log("*Player added!");
                    }
                    else
                    {
                        //Passwords don't match, show error
                        RegisterErrorText.text = "Error: Passwords do not match";
                    }
                }
                else
                {
                    //Password too short so show error
                    RegisterErrorText.text = "Error: Password must contain 6-25 characters";
                }
            }
            else
            {
                //Username taken
                RegisterErrorText.text = "Error: Username has already been taken";
            }
		}
		else
		{
			//Username too short so show error
			RegisterErrorText.text = "Error: Username must contain 4-15 characters";
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
	IEnumerator MainMenus()
	{
		yield return new WaitForSeconds(2);
		menucam.setMount (mainMount);
	}
}
