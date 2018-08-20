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
	TextMeshProUGUI name, price;

	[HideInInspector]
	public ShopInventoryUi shopI;

	public Rigidbody[] items = new Rigidbody[16];
	public Item[] shopitem = new Item[16];

	public int slot;
	public Quaternion rot;

	public GameObject hud;

	Vector3 pos1;

	void Awake ()
	{
		
	}

	// Use this for initialization
	void Start () 
	{
		shopI = GameObject.Find("ShopCanvas").transform.Find("Inventory").gameObject.GetComponent<ShopInventoryUi>();
		cube = transform.Find("Cube").gameObject;

		//GameObject tempObject = GameObject.Find("Canvas");
		//canvas = transform.Find("Canvas").GetComponent<Canvas>();

//		Transform nameText = canvas.transform.Find ("name");
//		name = nameText.GetComponent<TextMeshProUGUI> ();
//
//		Transform priceText = canvas.transform.Find ("Price");
//		price = nameText.GetComponent<TextMeshProUGUI> ();

//		name = transform.Find ("name").GetComponent<TMP_Text> ();
//		price = canvas.transform.Find ("Price").GetComponent<TMP_Text> ();
//		quality = canvas.transform.Find ("Quality").GetComponent<TMP_Text> ();
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
			}
		}
	}

	public void SpawnObject(Rigidbody prefab,Vector3 post, Quaternion rotn,int slot,Item item)
	{
		Rigidbody rPrefab;
		rPrefab = Instantiate(prefab,post,rotn) as Rigidbody;
		items [slot] = rPrefab;
		shopCanvas.SetActive (false);
		shopitem [slot] = item;
//		name.SetText (item.Name.ToString());
//		Debug.Log (item.Name.ToString ());
//		quality.SetText (item.Quality.ToString ());
//		price.SetText (item.UnitCost.ToString ());
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
			cube.SetActive (true);
//			canvas.enabled = true;
			showCanvas();
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
	//		canvas.enabled = false;
			if (shopCanvas.activeInHierarchy)
				shopCanvas.SetActive (false);
			hud.SetActive (true);
		}
	}

	public void showCanvas()
	{
		canvas.enabled = true;
		canvas.GetComponent<RectTransform> ().localPosition = pos1;

		Transform nameText = canvas.transform.Find ("name");
		name = nameText.GetComponent<TextMeshProUGUI> ();
		name.SetText (shopitem [slot].Name);

//		Canvas n;
//		n = Instantiate (canvas, pos1, rot);
	}

	private bool IsOpenPanelActive
	{
		get
		{
			return OpenPanel.activeInHierarchy;
		}
	}
}
