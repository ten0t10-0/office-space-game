using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Mesh Group", menuName = "Player Mesh Group")]
public class PlayerMeshGroup : ScriptableObject
{
    public Mesh Clothing;
    public Mesh Body;
}
