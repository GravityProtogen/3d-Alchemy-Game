using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonBuilder))]
public class DungeonBuilderEditor : Editor
{
    // GUI do Inspetor
    public override void OnInspectorGUI()
    {
        DungeonBuilder builder =
            (DungeonBuilder)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("piecePrefabs")
        );

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("targetPiece")
        );
        DrawTargetConnectionDropdown(
            builder
        );

        GUILayout.Space(10);

        DrawPrefabDropdown(builder);

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Prefab"))
        {
            if (builder.piecePrefabs.Count == 0)
            {
                Debug.Log("No prefabs assigned.");
                return;
            }

            SpawnPiece(
                builder,
                builder.piecePrefabs[
                    builder.selectedPrefabIndex
                ]
            );
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(builder);
        }
    }
    void DrawPrefabDropdown( DungeonBuilder builder )
    {
        if (builder.piecePrefabs.Count == 0)
            return;

        string[] prefabNames =
            new string[
                builder.piecePrefabs.Count
            ];

        for (int i = 0;
            i < builder.piecePrefabs.Count;
            i++)
        {
            prefabNames[i] =
                builder.piecePrefabs[i] != null
                ? builder.piecePrefabs[i].name
                : "Missing Prefab";
        }

        builder.selectedPrefabIndex =
            EditorGUILayout.Popup(
                "Prefab",
                builder.selectedPrefabIndex,
                prefabNames
            );

        GameObject selectedPrefab =
            builder.piecePrefabs[
                builder.selectedPrefabIndex
            ];

        if (selectedPrefab == null)
            return;

        DungeonPiece piece =
            selectedPrefab.GetComponent<DungeonPiece>();

        if (piece == null)
            return;

        piece.RefreshConnections();

        string[] connectionNames =
            new string[
                piece.connections.Count
            ];

        for (int i = 0;
            i < piece.connections.Count;
            i++)
        {
            connectionNames[i] =
                piece.connections[i].name +
                " (" +
                piece.connections[i].direction +
                ")";
        }

        builder.selectedConnectionIndex =
            EditorGUILayout.Popup(
                "Connection",
                builder.selectedConnectionIndex,
                connectionNames
            );
    }
    void DrawTargetConnectionDropdown( DungeonBuilder builder )
    {
        if (builder.targetPiece == null)
            return;

        builder.targetPiece.RefreshConnections();

        if (builder.targetPiece.connections.Count == 0)
            return;

        string[] connectionNames =
            new string[
                builder.targetPiece.connections.Count
            ];

        for (int i = 0;
            i < builder.targetPiece.connections.Count;
            i++)
        {
            ConnectionPoint cp =
                builder.targetPiece.connections[i];

            connectionNames[i] =
                cp.name +
                " (" +
                cp.direction +
                ")";
        }

        builder.targetConnectionIndex =
            EditorGUILayout.Popup(
                "Target Connection",
                builder.targetConnectionIndex,
                connectionNames
            );
    }

    void SpawnPiece(DungeonBuilder builder, GameObject prefab)
    {
        if (builder.piecePrefabs.Count == 0)
        {
            Debug.Log("No prefabs assigned.");
            return;
        }
        if (builder.targetPiece == null)
        {
            Debug.Log("Assign a Target Piece.");
            return;
        }

        if (
            builder.targetConnectionIndex >=
            builder.targetPiece.connections.Count
        )
        {
            Debug.Log("Invalid target connection.");
            return;
        }

        ConnectionPoint target =
            builder.targetPiece.connections[
                builder.targetConnectionIndex
            ];

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
            Debug.Log("Prefab is missing DungeonPiece.");
            DestroyImmediate(corridor);
            return;
        }

        if (piece.connections.Count == 0)
            {
                Debug.Log("Piece has no connections.");

                DestroyImmediate(corridor);
                return;
            }

            if (builder.selectedConnectionIndex >= piece.connections.Count)
                {
                    Debug.Log("Invalid connection index.");
                    DestroyImmediate(corridor);
                    return;
                }

                ConnectionPoint corridorConnection =
                    piece.connections[
                        builder.selectedConnectionIndex
                    ];

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

    }
}