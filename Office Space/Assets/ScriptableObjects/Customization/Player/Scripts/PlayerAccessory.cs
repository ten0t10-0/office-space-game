using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Accessory", menuName = "Player Customization/Item/Accessory")]
public class PlayerAccessory : ScriptableObject
{
    public PlayerAccessoryCategory Category;
    public Mesh Mesh;
}
