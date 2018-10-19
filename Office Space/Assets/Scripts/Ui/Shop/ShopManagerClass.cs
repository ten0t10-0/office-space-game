using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopManagerClass : MonoBehaviour 
{
	public Item[] shopitem = new Item[17];
	public Rigidbody[] items = new Rigidbody[17];
	public Transform[] slots = new Transform[17];
	public Rigidbody gpu, cpu, console, desktop, laptop, game, keyboard, mouse, figure,motherboard;
	Rigidbody prefab;

	ShopInventoryUi shop;


	void Start()
	{

	}

	public void spawnShopItems()
	{
		RemoveAllItems ();
		int i = 0;
		foreach (Item item in GameMaster.Instance.Player.Business.Shop.ItemsOnDisplay) 
		{
			if (item != null) 
			{
				Rigidbody rPrefab;
				rPrefab = Instantiate (StartSpawn(item), slots[i].position, slots[i].rotation) as Rigidbody;
				items [i] = rPrefab;
			}
			i++;
		}
	}

	public Rigidbody StartSpawn(Item item)
	{
		if (item.Subcategory.EnumID == ItemSubcategory.Console)
			prefab = console;

		else if(item.Subcategory.EnumID == ItemSubcategory.ConsoleGame) 
			prefab = game;

		else if (item.Subcategory.EnumID == ItemSubcategory.CPU) 
			prefab = cpu;

		else if (item.Subcategory.EnumID == ItemSubcategory.Desktop) 
			prefab = desktop;

		else if (item.Subcategory.EnumID == ItemSubcategory.figurines) 
			prefab = figure;

		else if (item.Subcategory.EnumID == ItemSubcategory.GPU) 
			prefab = gpu;

		else if(item.Subcategory.EnumID == ItemSubcategory.Keyboard) 
			prefab = keyboard;

		else if (item.Subcategory.EnumID == ItemSubcategory.Laptop) 
			prefab = laptop;

		else if (item.Subcategory.EnumID== ItemSubcategory.Mouse) 
			prefab = mouse;

		else if (item.Subcategory.EnumID == ItemSubcategory.PCGame) 
			prefab = game;

		else if (item.Subcategory.EnumID == ItemSubcategory.MotherBoard) 
			prefab = motherboard;

		return prefab;
	}
	public void RemoveItem(int i)
	{
		Debug.Log("Removing item " + i.ToString());
		Destroy (items[i].gameObject);
		items [i] = null;
	}

	public void RemoveAllItems()
	{
		int i = 0;
		foreach (Rigidbody prefab in items) 
		{
			if (prefab != null) 
			{
				Destroy (items[i].gameObject);
				items [i] = null;
			}
			i++;
		}
	}

}
