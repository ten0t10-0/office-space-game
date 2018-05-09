using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Clothing", menuName = "Player Customization/Item/Clothing")]
public class PlayerClothing : ScriptableObject
{
    public PlayerClothingCategory Category;
    public PlayerMeshGroup MeshGroup;
}
