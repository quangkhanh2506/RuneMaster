using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Config/ItemConfig", order = 2)]
public class ItemConfig : ScriptableObject
{
    public ItemConfigData[] Datas;
    private static ItemConfig instance;

    public static void Setup()
    {
        instance = Resources.Load<ItemConfig>("Configs/ItemConfig");
    }

    public static ItemConfigData GetConfigByID(int itemID)
    {
        if (instance == null)
        {
            return null;
        }
        return instance.Datas.FirstOrDefault(config => config.idItem == itemID);
    }
}

[Serializable]
public class ItemConfigData
{
    public int idItem;
    public string name;
    public Sprite sprite;
}
