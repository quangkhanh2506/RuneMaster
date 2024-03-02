using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using Core;

public class SaveManager : SingletonMono<SaveManager>
{
    private string gameSaveFileName = "/game-save.json";

    public SaveGame SaveGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Debug.Log(SaveGame == null ? "Null" : "Not null");
        LoadGame();
        StartCoroutine(ShowUI());
    }

    IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(3f);
        UIManager.Instance.Init(() =>
        {
            UIManager.Instance.ShowUI(UIIndex.UIChest);
        });
    }

    public SaveGame Init()
    {
        Debug.Log("Init Save Manager");
        try
        {
            //if (SaveGame == null)
            {
                string gameSavePath = GetGameSavePath();
                if (File.Exists(gameSavePath))
                {
                    var s = FileHelper.LoadFileWithPassword(gameSavePath, "", true);
                    Debug.Log("init save manager " + s);
                    try
                    {
                        SaveGame = JsonConvert.DeserializeObject<SaveGame>(s);
                    }
                    catch(Exception e)
                    {
                        Debug.LogError("Parse Game Save Error: " + e.Message);
                    }
                }
                else
                {
                    Debug.Log("Game save not found, starting a new game");
                    SaveGame = new SaveGame();
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to Load saved game due to: " + e.Message);
        }
        StartCoroutine(WaitforSave(10f));
        return SaveGame;
    }

    private string GetGameSavePath()
    {
        return Application.persistentDataPath + gameSaveFileName;
    }

    public SaveGame LoadGame()
    {
        //if (SaveGame == null)
        {
            Init();
        }

        SaveGame.Init(Application.version);
        
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
