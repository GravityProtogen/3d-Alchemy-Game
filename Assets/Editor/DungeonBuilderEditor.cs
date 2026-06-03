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

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Straight"))
        {
            SpawnPiece(
                builder,
                builder.straightPrefab
            );
        }

        if (GUILayout.Button("Spawn Corner"))
        {
            SpawnPiece(
                builder,
                builder.cornerPrefab
            );
        }

        if (GUILayout.Button("Spawn Room"))
        {
            SpawnPiece(
                builder,
                builder.roomPrefab
            );
        }

        if (GUILayout.Button("Spawn T Junction"))
        {
            SpawnPiece(
                builder,
                builder.tJunctionPrefab
            );
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

    void SpawnPiece(DungeonBuilder builder, GameObject prefab)
    {
        ConnectionPoint target =
            builder.targetConnection;

        if (target == null)
        {
            Debug.Log("Assign a Target Connection.");
            return;
        }

        if (prefab == null)
        {
            Debug.Log("Assign a Prefab.");
            return;
        }

        GameObject corridor =
            (GameObject)PrefabUtility.InstantiatePrefab(
                prefab
            );

        DungeonPiece piece =
            corridor.GetComponent<DungeonPiece>();

        if (piece == null)
        {
            Debug.Log("Corridor prefab is missing DungeonPiece.");
            DestroyImmediate(corridor);
            return;
        }

        if (piece.connections.Count == 0)
            {
                Debug.Log("Piece has no connections.");

                DestroyImmediate(corridor);
                return;
            }

            ConnectionPoint corridorConnection = null;

            ConnectionDirection neededDirection =
                Opposite(target.direction);

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
                corridorConnection = piece.connections[0];

                Debug.Log(
                    "Fallback connection used: " +
                    corridorConnection.name
                );
            }

        // ROTATE TO FACE THE TARGET CONNECTION

        Quaternion rotation =
        Quaternion.FromToRotation(
            corridorConnection.transform.forward,
            -target.transform.forward
        );

        corridor.transform.rotation =
            rotation * corridor.transform.rotation;

        // SNAP AFTER ROTATION

        Vector3 delta =
        target.transform.position -
        corridorConnection.transform.position;

        corridor.transform.position += delta;

        // AUTO-SELECT NEXT FREE CONNECTION

        foreach (var c in piece.connections)
        {
            if (c != corridorConnection)
            {
                builder.targetConnection = c;

                Debug.Log(
                    "Next target: " +
                    c.name +
                    " direction=" +
                    c.direction
                );

                break;
            }
        }

        EditorUtility.SetDirty(builder);

        Debug.Log(
            "Snapped using: " +
            corridorConnection.name
        );
    }
}