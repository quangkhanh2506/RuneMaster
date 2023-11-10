using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class SaveManager : SingletonMono<SaveManager>
{
    private string gameSaveFileName = "/game-save.json";

    public SaveGame SaveGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        LoadGame();
    }

    public SaveGame Init()
    {
        Debug.Log("Init Save Manager");
        try
        {
            if (SaveGame == null)
            {
                string gameSavePath = GetGameSavePath();
                if (File.Exists(gameSavePath))
                {
                    var s = FileHelper.LoadFileWithPassword(gameSavePath, "", true);
                    try
                    {
                        SaveGame = JsonConvert.DeserializeObject<SaveGame>(s);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Parse Game Save Error: " + e.Message);
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to Load saved game due to: " + e.Message);
        }
        return SaveGame;
    }

    private string GetGameSavePath()
    {
        return Application.persistentDataPath + gameSaveFileName;
    }

    public SaveGame LoadGame()
    {
        if (SaveGame == null)
        {
            Init();
        }
        SaveGame.Init(Application.version);
        StartCoroutine(WaitforSave(5f));

        return SaveGame;
    }

    public void Save()
    {
        string gameSavePath = GetGameSavePath();
        string content = JsonConvert.SerializeObject(SaveGame, Formatting.Indented);
        File.WriteAllText(gameSavePath, content);
    }

    private void OnApplicationQuit()
    {
        SaveGame.isFirstOpen = true;
        Save();
    }

    private IEnumerator WaitforSave(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Save();
        }
    }
}
