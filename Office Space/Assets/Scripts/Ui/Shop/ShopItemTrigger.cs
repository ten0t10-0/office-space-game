using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemTrigger : MonoBehaviour {

	public GameObject shopCanvas;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	GameObject cube;
	//public GameObject canvas;
	public Canvas canvas;
	Canvas tempcanvas;
	TextMeshProUGUI name, price,quality;

	ShopManagerClass Itemmanager;

	Rigidbody[] items;
	//Item[] shopItem;

	[HideInInspector]
	public ShopInventoryUi shopI;

	public int slot;
	public Quaternion rot;

	public GameObject hud;
	public Animator hudO;

//	bool firstPerson = false;

	Vector3 pos1;

	void Awake ()
	{
		Itemmanager = FindObjectOfType<ShopManagerClass> ();
	}

	// Use this for initialization
	void Start () 
	{
		shopI = GameObject.Find("ShopCanvas").transform.Find("Inventory").gameObject.GetComponent<ShopInventoryUi>();
		cube = transform.Find("Cube").gameObject;

		items = Itemmanager.items;
		//shopItem = Itemmanager.shopitem;

		//showCanvas ();

		canvas.enabled = false;

//		if (Camera.main.GetComponent<CameraController> ().CameraMode == CameraMode.FirstPerson)
//			firstPerson = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				pos1 = cube.transform.position;
				shopCanvas.SetActive (true);
				OpenPanel.SetActive (false);
				shopI.setItems (slot, pos1, rot,tempcanvas);
				hudO.SetBool ("UIO", true);
				hud.SetActive (false);
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.Static);

			}
		}
	}

	public void SpawnObject(Rigidbody prefab,Vector3 post, Quaternion rotn,int slots,Item item,Canvas tcanvas,int inventory)
	{
		if (items [slots] != null) 
		{
			Debug.Log ("Bloop");
			Destroy (items[slots].gameObject);
			Debug.Log ("spawn"+slots.ToString ());
			GameMaster.Instance.Player.Business.MoveItemsToInventory(slots);
		}
			
		Rigidbody rPrefab;
		rPrefab = Instantiate(prefab,post,rotn) as Rigidbody;
		items [slots] = rPrefab;
		shopCanvas.SetActive (false);
		Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.ThirdPerson);
		//shopItem [slots] = item;
		GameMaster.Instance.Player.Business.MoveItemsToShop (inventory, slots);

		showCanvas(tcanvas, slots);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			tempcanvas = canvas;
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
			cube.SetActive (true);
			canvas.enabled = true;
			showCanvas(canvas,slot);
			Debug.Log ("Enter" + slot);


		}
	}
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);

		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			hudO.SetBool ("UIO", false);
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
			cube.SetActive (false);
			canvas.enabled = false;
			if (shopCanvas.activeInHierarchy) 
			{
				shopCanvas.SetActive (false);
//				if (firstPerson == true)
				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.ThirdPerson);
//				else
//				Camera.main.GetComponent<CameraController> ().ChangeMode (CameraMode.FirstPerson);
			}
			hud.SetActive (true);
		}
	}

	public void showCanvas(Canvas tcanvas,int slots)
	{
		Transform nameText = tcanvas.transform.Find ("name");
		name = nameText.GetComponent<TextMeshProUGUI> ();

		Transform priceText = tcanvas.transform.Find ("Price");
		price = priceText.GetComponent<TextMeshProUGUI> ();

		Transform qualityText = tcanvas.transform.Find ("Quality");
		quality = qualityText.GetComponent<TextMeshProUGUI> ();

		Debug.Log (slot);

		if (items[slots] == null)
		{
			name.SetText ("Nothing");
			price.SetText ("");
			quality.SetText ("");
		} 
		else 
		{
			Debug.Log ("Name"+slot.ToString ());
			name.SetText (GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay [slots].Name);
			price.SetText (GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay[slots].UnitCost.ToString());
			quality.SetText (GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay [slots].Quality.ToString());
		}
	}
//	public void UpdateCanvas(Canvas tcanvas,int slots)
//	{
//		Transform nameText = tcanvas.transform.Find ("name");
//		name = nameText.GetComponent<TextMeshProUGUI> ();
//
//		Transform priceText = tcanvas.transform.Find ("Price");
//		price = priceText.GetComponent<TextMeshProUGUI> ();
//
//		Transform qualityText = tcanvas.transform.Find ("Quality");
//		quality = qualityText.GetComponent<TextMeshProUGUI> ();
//
//		name.SetText (shopItem [slots].Name);
//		price.SetText (shopItem [slots].UnitCost.ToString());
//		quality.SetText (shopItem [slots].Quality.ToString());
//	
//	}

	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}

}
