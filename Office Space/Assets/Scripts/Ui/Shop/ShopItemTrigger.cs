using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemTrigger : MonoBehaviour {

	public GameObject shopCanvas;

	public GameObject OpenPanel = null;
	private bool isInsideTrigger = false;

	GameObject cube;

	[HideInInspector]
	public ShopInventoryUi shopI;

	//var shopI : ShopInventoryUi;

	public int slot;
	public Vector3 pos;
	public Quaternion rot;

	void Awake ()
	{
		
	}

	// Use this for initialization
	void Start () 
	{

		shopI = GameObject.Find("ShopCanvas").GetComponent<ShopInventoryUi>();
		cube = transform.Find("Cube").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsOpenPanelActive && isInsideTrigger) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				
				shopCanvas.SetActive (true);
				OpenPanel.SetActive (false);
				shopI.setItems (slot, pos, rot);
			}
		}
	}

	public void SpawnObject(Rigidbody prefab,Vector3 post, Quaternion rotn)
	{

		Rigidbody rPrefab;
		rPrefab = Instantiate(prefab,post,rotn) as Rigidbody;
		shopCanvas.SetActive (false);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			isInsideTrigger = true;
			OpenPanel.SetActive(true);
			cube.SetActive (true);

			Debug.Log (slot);
			Debug.Log (pos.ToString ());
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
			if (shopCanvas.activeInHierarchy)
				shopCanvas.SetActive (false);
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
