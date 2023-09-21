using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BYDataBase : ScriptableObject
{
 
    public virtual void CreateBinaryFile(TextAsset csvFile_)
    {

    }
}
public class BYDataTable<T> : BYDataBase
{

    [SerializeField]
    public List<T> records = new List<T>();
#if UNITY_EDITOR
   

    public override void CreateBinaryFile(TextAsset csvFile_)
    {
        string[,] grid = SplitCsvGrid(csvFile_.text);
        if (records != null)
            records.Clear();
        else
            records = new List<T>();
        //Debug.Log("size = " + (1 + grid.GetUpperBound(0)) + "," + (1 + grid.GetUpperBound(1)));
        for (int row = 1; row < grid.GetUpperBound(1); row++)
        {
            FieldInfo[] myFieldInfo;
            Type myType = typeof(T);
            // Get the type and fields of FieldInfoClass.
            myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Display the field information of FieldInfoClass.
            string jsonTemp = string.Empty;
            jsonTemp += "{";
            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                if (myFieldInfo[i].FieldType == typeof(System.String))
                {
                    jsonTemp += "\"" + myFieldInfo[i].Name + "\":\"" + grid[i, row] + "\"";

                }
                else
                {
                    jsonTemp += "\"" + myFieldInfo[i].Name + "\":" + grid[i, row];

                }

                if (i < myFieldInfo.Length - 1)
                {
                    jsonTemp += ",";

                }
            }
            jsonTemp += "}";

            Debug.Log(jsonTemp);

            T data = JsonUtility.FromJson<T>(jsonTemp);
            records.Add(data);
        }
    }



    // splits a CSV file into a 2D string array
    static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }
#endif

}
