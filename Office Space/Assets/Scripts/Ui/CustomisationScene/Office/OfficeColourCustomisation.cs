﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeColourCustomisation : MonoBehaviour 
{
	GameObject Selector;
	public GameObject colourCanvas;
	public GameObject hudCanvas,skybox;
	public Texture2D colourSprite;

	public GameObject OfficeWall, shopWall;
	GameObject wall;
	public Transform shopWallMount,officeWallMount;

	Transform WallMount;


	public Color textureColour;

	private Rect textureRect = new Rect (60,300,200,200);

	Rect newRect;


	void Start()
	{
		

	}

	public void OfficeMount()
	{
		WallMount = officeWallMount;
		wall = OfficeWall;

	}

	public void ShopMount()
	{
		wall = shopWall;
		WallMount = shopWallMount;

	}

	public void SetMount()
	{
		FindObjectOfType<CamMounts> ().setMount(WallMount);
	}

	void Update()
	{
		ChangeWallColour();
	}

	public void OpenCustomisation()
	{
		GameMaster.Instance.ModeSetUI();
		GameMaster.Instance.CameraLock = true;
		colourCanvas.SetActive (true);
		hudCanvas.SetActive (false);
		SetMount ();
		textureColour = wall.GetComponent<Renderer> ().material.color;
	}
	public void CloseCustomisation()
	{
		colourCanvas.SetActive (false);
		GameMaster.Instance.ModeSetPlay();
		GameMaster.Instance.CameraLock = false;
		hudCanvas.SetActive (true);

	}
	public void Skybox()
	{
		skybox.SetActive (false);
	}

	void ChangeWallColour()
	{
		Renderer rend = wall.GetComponent<Renderer>();

		rend.material.shader = Shader.Find("_Color");
		rend.material.SetColor("_Color", textureColour);


		rend.material.shader = Shader.Find("Specular");
		rend.material.SetColor("_SpecColor", textureColour);

		GameMaster.Instance.CustomizationManager.Office.MaterialWallsCurrent.color = textureColour;

 

//		rend.material.shader = Shader.Find("Specular");
//		rend.material.SetColor("_SpecColor", Color.red);
	}

	void OnGUI()
	{
		newRect = ResizeGUI (textureRect);
		GUI.DrawTexture (newRect, colourSprite);

		if (Event.current.type == EventType.MouseUp)
		{
			Vector2 mousePostion = Event.current.mousePosition;

			if (mousePostion.x < newRect.x || mousePostion.x > newRect.xMax || mousePostion.y < newRect.y || mousePostion.y > newRect.yMax)
			{
				return;
			}

			float textureUPosition = (mousePostion.x - newRect.x) / newRect.width;
			float textureVPosition = 1.0f - ((mousePostion.y - newRect.y) / newRect.height);

			textureColour = colourSprite.GetPixelBilinear (textureUPosition, textureVPosition);
		}
	}
	Rect ResizeGUI(Rect _rect)
	{
		float FilScreenWidth = _rect.width / 800;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / 600;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / 800) * Screen.width;
		float rectY = (_rect.y / 600) * Screen.height;

		return new Rect (rectX, rectY, rectWidth, rectHeight);
	}

}
