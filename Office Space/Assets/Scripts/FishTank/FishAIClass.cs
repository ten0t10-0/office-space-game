using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishAIClass
{

	public string AIGroupName { get { return aiGroupName; } }
	public GameObject ObjectPrefab { get {return prefab;}}
	public int MaxAi { get {return maxAI;}}
	public int SpawnRate { get { return spawnRate; } }
	public int SpawnAmount { get { return spawnAmount; } }

	[SerializeField]
	private string aiGroupName;
	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private int maxAI;
	[SerializeField]
	private int spawnRate;
	[SerializeField]
	private int spawnAmount;
}
