using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPicker : MonoBehaviour 
{

	public Texture2D colourSprite;
    SpriteRenderer sprite;
	GameObject Selector;

	public Color textureColour;

	private Rect textureRect = new Rect (120,190,220,100);

	void Start()
	{

		sprite = GetComponent<SpriteRenderer>();

		sprite.color = Color.black;
	}


	void OnGUI()
	{
		GUI.DrawTexture (textureRect, colourSprite);

		if (Event.current.type == EventType.MouseUp)
		{
			Vector2 mousePostion = Event.current.mousePosition;

			if (mousePostion.x < textureRect.x || mousePostion.x > textureRect.xMax || mousePostion.y < textureRect.y || mousePostion.y > textureRect.yMax)
			{
				return;
			}

			float textureUPosition = (mousePostion.x - textureRect.x) / textureRect.width;
			float textureVPosition = 1.0f - ((mousePostion.y - textureRect.y) / textureRect.height);

			textureColour = colourSprite.GetPixelBilinear (textureUPosition, textureVPosition);
			sprite.color = textureColour;
		}
	}

}
