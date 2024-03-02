using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "ChestConfig", menuName = "Config/ChestConfig",order =0)]
public class ChestConfig : ScriptableObject
{
    public ChestConfigData[] Datas;
    private static ChestConfig instance;

    public static void Setup()
    {

        instance = Resources.Load<ChestConfig>("Configs/ChestConfig");
        Debug.Log(instance == null ? "NULL" : "Not NULL");
    }

    public static ChestConfigData GetConfigByID(int ChestID)
    {
        if (instance == null)
        {
            return null;
        }
        return instance.Datas.FirstOrDefault(Config => Config.id == ChestID);
    }
}

[Serializable]
public class ChestConfigData
{
    public int id;
    public List<ChestItem> chestItems = new List<ChestItem>();
}
[Serializable]
public class ChestItem
{
    public int itemId;
    public float rate;
    public int amout;
}
