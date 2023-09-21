#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class BYDataTableMaker
{


    [MenuItem("Assets/BY/Create binary File form CSV (text file)", false, 1)]
    private static void CreateBinaryFile()
    {
        foreach (UnityEngine.Object obj in Selection.objects)
        {
            TextAsset csvFile = (TextAsset)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(csvFile));

            ScriptableObject scriptableTable = ScriptableObject.CreateInstance(tableName);
            if (scriptableTable == null)
                return;
     
            AssetDatabase.CreateAsset(scriptableTable, "Assets/_Project/Resources/Configs/" + tableName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            BYDataBase by = (BYDataBase)scriptableTable;
            by.CreateBinaryFile(csvFile);
            EditorUtility.SetDirty(by);
        }
    }

}
#endif