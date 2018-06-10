using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notifications : MonoBehaviour {

	[SerializeField] //Notifications
	private GameObject NotificationContainer;
	private Transform scrollViewContent;

	void Awake()
	{
		
	}
	// Use this for initialization
	void Start () 
	{
		AddNotifications();
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	public void AddNotifications()
	{
		//		ClearNotifications();
		List <Notification> noteList = GameMaster.Instance.Notifications.GetAll ();

		foreach (Notification note in noteList) 
		{
			if (note.Read == false) 
			{
				GameObject newNote = Instantiate (NotificationContainer, scrollViewContent);
				newNote.transform.Find ("Button/Text").GetComponent<TMP_Text> ().text = note.Text;

				newNote.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate {ReadNotification(note);});
			}
		}
	}

	public void ReadNotification(Notification note)
	{
		note.Read = true;
	}

		public void ClearNotifications()
		{
	
			if (scrollViewContent == null)
			{
				scrollViewContent = transform.Find("Screen/Scroll View/Viewport/Content");
			}
	
			foreach (Transform child in scrollViewContent)
			{
				Destroy(child.gameObject);
			}
		}	
}