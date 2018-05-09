using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Mesh Group", menuName = "Player Customization/Mesh Group")]
public class PlayerMeshGroup : ScriptableObject
{
    public Mesh BodyMesh;
    public Mesh ClothingMesh;
}
