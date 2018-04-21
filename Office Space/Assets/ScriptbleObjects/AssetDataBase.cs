using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetDB", menuName = "Database")]
public class PieceData : ScriptableObject
{
    private const int defaultGridSize = 3;

    [Range(1, 5)]
    public int gridSize = defaultGridSize;

    public CellRow[] cells = new CellRow[defaultGridSize];

    [System.Serializable]
    public class CellRow
    {
        public ScriptableObject[] row = new ScriptableObject[defaultGridSize];
    }
}
