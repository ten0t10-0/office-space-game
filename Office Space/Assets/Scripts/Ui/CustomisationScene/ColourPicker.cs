using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPicker : MonoBehaviour 
{

	public Texture2D colourSprite;
    SpriteRenderer sprite;
	GameObject Selector;

	public Color textureColour;

	private Rect textureRect = new Rect (160,400,220,100);

	Rect newRect;

	void Start()
	{

		sprite = GetComponent<SpriteRenderer>();

		sprite.color = Color.black;
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
			sprite.color = textureColour;
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
