using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudMain : MonoBehaviour {


	public GameObject phone;
	public TextMeshProUGUI money;
	public TextMeshProUGUI time;
	public GameObject order;
	public GameObject hudCanvas;
	public Animator animator;

	[SerializeField] //items
	private GameObject noteContainer;
	private Transform iscrollContent;
	PhoneScreen ph;




	void Start () 
	{
		//money.SetText("$ "+(GameMaster.Instance.Player.Business.Money).ToString());
		ph = gameObject.transform.Find("Phone").GetComponent<PhoneScreen>();

	}
	

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			StopCoroutine(PhoneClose());
			phone.SetActive (true);
			animator.SetBool ("PhoneO", true);
			FindObjectOfType<SoundManager>().Play("PhoneO");
			Cursor.lockState = CursorLockMode.None;
			ph.noteCount ();

            GameMaster.Instance.ModeSetUI();
		}
		if (Input.GetKeyDown(KeyCode.N)) 
		{
			animator.SetBool ("PhoneO", false);
			Cursor.lockState = CursorLockMode.Locked;
			StartCoroutine (PhoneClose());

            GameMaster.Instance.ModeSetPlay();
        }


		time.SetText (GameMaster.Instance.GameTimeString12 ());
        money.SetText("$ " + (GameMaster.Instance.Player.Business.Money).ToString());
		DisplayNotes ();

    }

	public void DisplayNotes()
	{
		ClearNote ();
		string names = "";
		List<Order> order = GameMaster.Instance.OrderManager.GetOpenOrders();

		for (int i = 0; i < order.Count; i++)
		{
			GameObject newItem = Instantiate (noteContainer, iscrollContent);

			newItem.transform.Find ("Time").GetComponent<TMP_Text> ().text = order[i].GetTimeRemaining().ToString();
      		newItem.transform.Find ("name").GetComponent<TMP_Text> ().text = order[i].Customer.FullName();

			foreach (OrderItem item in order[i].Items)
			{
				names = names+item.Quantity.ToString()+"x " + item.Name.ToString ()+"\n";
			}
			newItem.transform.Find ("items").GetComponent<TMP_Text> ().text = names;
		}

	}

	public void ClearNote()
	{
		if (iscrollContent == null)
		{
			iscrollContent = transform.Find("Phone/Orders/Scroll View/Viewport/Content");
		}
		foreach (Transform childs in iscrollContent)
		{
			Destroy(childs.gameObject);
		}
	}
	public void orderNotifiation()
	{
		if (hudCanvas.activeInHierarchy) 
		{
			order.SetActive (true);
		}
	}
	IEnumerator PhoneClose()
	{
		yield return new WaitForSeconds(1);
		phone.SetActive (false);
	}



}
