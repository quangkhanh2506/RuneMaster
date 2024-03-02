using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        Debug.Log("inspector GUI");
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputFields();
        EditorGUILayout.Space();
        if(ShapeDataInstance.board != null && ShapeDataInstance.colums>0 && ShapeDataInstance.rows > 0)
        {
            Debug.Log("Draw table");
            DrawBoardtable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Board"))
        {
            ShapeDataInstance.Clear();
        }
    }

    private void DrawColumnsInputFields()
    {
        var columnsTemp = ShapeDataInstance.colums;
        var rowsTemp = ShapeDataInstance.rows;
        ShapeDataInstance.colums = EditorGUILayout.IntField("Colums", ShapeDataInstance.colums);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);

        if((ShapeDataInstance.colums!=columnsTemp || ShapeDataInstance.rows!=rowsTemp) && ShapeDataInstance.colums>0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardtable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (int i = 0; i < ShapeDataInstance.rows; i++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for (int j = 0; j < ShapeDataInstance.colums; j++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[i].column[j], dataFieldStyle);
                ShapeDataInstance.board[i].column[j] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();

        }
    }
}

