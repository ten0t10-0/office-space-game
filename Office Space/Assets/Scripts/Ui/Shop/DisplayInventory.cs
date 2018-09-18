using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour 
{
	[SerializeField] //items
	private GameObject ItemContainer;
	private Transform scrollViewContent;

	public TextMeshProUGUI category,label;

	public Button btnDecrease,btnIncrease,cancel,confirm,shop,inventory,nothing; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
