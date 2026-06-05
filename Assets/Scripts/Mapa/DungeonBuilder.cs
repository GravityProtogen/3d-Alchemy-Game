using UnityEngine;
using System.Collections.Generic;

public class DungeonBuilder : MonoBehaviour
{
    public List<GameObject> piecePrefabs = new();

    public DungeonPiece targetPiece;

    [HideInInspector]
    public int targetConnectionIndex;

    [HideInInspector]
    public int selectedPrefabIndex;

    [HideInInspector]
    public int selectedConnectionIndex;
}