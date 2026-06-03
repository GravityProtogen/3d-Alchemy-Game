using UnityEngine;
using System.Collections.Generic;

public class DungeonPiece : MonoBehaviour
{
    public List<ConnectionPoint> connections = new();

    private void Awake()
    {
        RefreshConnections();
    }

    private void OnValidate()
    {
        RefreshConnections();
    }

    public void RefreshConnections()
    {
        connections.Clear();
        connections.AddRange(
            GetComponentsInChildren<ConnectionPoint>()
        );
    }
}