using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonBuilder))]
public class DungeonBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonBuilder builder =
            (DungeonBuilder)target;

        if (GUILayout.Button("Spawn Corridor"))
        {
            SpawnCorridor(builder);
        }
    }

    ConnectionDirection Opposite(ConnectionDirection dir)
    {
        switch (dir)
        {
            case ConnectionDirection.Norte:
                return ConnectionDirection.Sul;

            case ConnectionDirection.Sul:
                return ConnectionDirection.Norte;

            case ConnectionDirection.Leste:
                return ConnectionDirection.Oeste;

            case ConnectionDirection.Oeste:
                return ConnectionDirection.Leste;

            default:
                return dir;
        }
    }

    void SpawnCorridor(DungeonBuilder builder)
    {
        ConnectionPoint target =
            builder.targetConnection;

        if (target == null)
        {
            Debug.Log("Assign a Target Connection.");
            return;
        }

        if (builder.corridorPrefab == null)
        {
            Debug.Log("Assign a Corridor Prefab.");
            return;
        }

        GameObject corridor =
            (GameObject)PrefabUtility.InstantiatePrefab(
                builder.corridorPrefab
            );

        DungeonPiece piece =
            corridor.GetComponent<DungeonPiece>();

        if (piece == null)
        {
            Debug.Log("Corridor prefab is missing DungeonPiece.");
            return;
        }

        ConnectionDirection neededDirection =
            Opposite(target.direction);

        ConnectionPoint corridorConnection = null;

        foreach (var c in piece.connections)
        {
            if (c.direction == neededDirection)
            {
                corridorConnection = c;
                break;
            }
        }

        if (corridorConnection == null)
        {
            Debug.Log(
                "No connection found with direction: " +
                neededDirection
            );
            return;
        }

        Vector3 offset =
            corridor.transform.position -
            corridorConnection.transform.position;
        Debug.Log("Target Position: " + target.transform.position);
        Debug.Log("Corridor Connection Position: " + corridorConnection.transform.position);
        Debug.Log("Corridor Pivot Position: " + corridor.transform.position);
        Debug.Log("Offset: " + offset);
        corridor.transform.position =
            target.transform.position +
            offset;

        Debug.Log(
            "Snapped using: " +
            corridorConnection.name
        );
    }
}