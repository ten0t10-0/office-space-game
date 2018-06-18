using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudMain : MonoBehaviour {


	public TextMeshProUGUI money;
	public TextMeshProUGUI time;
	public GameObject order;
	public GameObject hudCanvas;

	[SerializeField] //items
	private GameObject noteContainer;
	private Transform iscrollContent;

	void Start () 
	{
		//money.SetText("$ "+(GameMaster.Instance.Player.Business.Money).ToString());


	}
	

	void Update () 
	{
		time.SetText (GameMaster.Instance.GameTimeString12 ());
        money.SetText("$ " + (GameMaster.Instance.Player.Business.Money).ToString());
		DisplayNotes ();

		if (order.activeInHierarchy)
			orderNew ();
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
			iscrollContent = transform.Find("Phone/Screen/Scroll View/Viewport/Content");
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
			StartCoroutine (orderNew ());
		}
	}
	IEnumerator orderNew()
	{
		yield return new WaitForSeconds(2);
		order.SetActive (false);

	}

}
