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
	Item[] shopItem;

	[HideInInspector]
	public ShopInventoryUi shopI;

	public int slot;
	public Quaternion rot;

	public GameObject hud;

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
		shopItem = Itemmanager.shopitem;

		//showCanvas ();

		canvas.enabled = false;


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
				shopI.setItems (slot, pos1, rot);
				hud.SetActive (false);
				//GameMaster.Instance.UIMode = true;

			}
		}
	}

	public void SpawnObject(Rigidbody prefab,Vector3 post, Quaternion rotn,int slots,Item item)
	{
		if (items [slots] != null) 
		{
			Debug.Log ("Bloop");
			Destroy (items[slots].gameObject);
			Debug.Log ("spawn"+slots.ToString ());
		}
			
		Rigidbody rPrefab;
		rPrefab = Instantiate(prefab,post,rotn) as Rigidbody;
		items [slots] = rPrefab;
		shopCanvas.SetActive (false);
		shopItem [slots] = item;

		canvas = transform.GetComponent<Canvas> ();

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
			showCanvas();
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
			isInsideTrigger = false;
			OpenPanel.SetActive(false);
			cube.SetActive (false);
			canvas.enabled = false;
			if (shopCanvas.activeInHierarchy)
				shopCanvas.SetActive (false);
			hud.SetActive (true);
		}
	}

	public void showCanvas()
	{
		Transform nameText = canvas.transform.Find ("name");
		name = nameText.GetComponent<TextMeshProUGUI> ();

		Transform priceText = canvas.transform.Find ("Price");
		price = priceText.GetComponent<TextMeshProUGUI> ();

		Transform qualityText = canvas.transform.Find ("Quality");
		quality = qualityText.GetComponent<TextMeshProUGUI> ();

		if (items[slot] == null)
		{
			name.SetText ("Nothing");
			price.SetText ("");
			quality.SetText ("");
		} 
		else 
		{
			Debug.Log ("Name"+slot.ToString ());
			name.SetText (shopItem [slot].Name);
			price.SetText (shopItem [slot].UnitCost.ToString());
			quality.SetText (shopItem [slot].Quality.ToString());
		}
	}

	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}

}
