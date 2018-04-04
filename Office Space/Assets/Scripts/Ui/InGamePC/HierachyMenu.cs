using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierachyMenu : MonoBehaviour 

{
	public int tier = 0;
	public bool shown = false;

	public static List<HierachyMenu> menus = new List<HierachyMenu>();

	public void Awake()
	{
		menus.Add (this);
	}

	public void OnDestroy()
	{
		menus.Remove (this);
	}

	public void Show(bool show)
	{
		if (show)
		{
			foreach (HierachyMenu menu in menus) 
			{
				if ((menu != this) && (menu.shown) && (menu.tier >= tier))
					menu.Show (false);
			}	
		}

				
		shown = show;
		gameObject.SetActive (shown);

	}
	public void Toggle()
	{
		Show (!shown);
	}

}
