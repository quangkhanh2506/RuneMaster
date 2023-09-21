using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
namespace Core
{
    public class SaveManager : SingletonMono<SaveManager>
    {
        private float gameSaveInterval = 10.0f;
        private string gameSaveFilename = "/game-save.json";

        private float timeSinceSave;
        private GameSave gameSave;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (gameSave != null)
            {
                timeSinceSave += Time.deltaTime;

                if (timeSinceSave >= gameSaveInterval)
                {
                    timeSinceSave = 0;
                    SaveGame();
                }
            }
        }
        public GameSave Init()
        {

            try
            {
                if (gameSave == null)
                {
                    string gameSavePath = GetGameSavePath();
                    //Debug.LogError(">>> gameSavePath: " + gameSavePath);
                    if (File.Exists(gameSavePath))
                    {
                        Debug.Log("Loading game save : " + gameSavePath);
                        var s = FileHelper.LoadFileWithPassword(gameSavePath, "", true);

                        try
                        {
                            gameSave = JsonConvert.DeserializeObject<GameSave>(s);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(">>> Parse GameSave error: " + e.Message);
                        }

                        //Debug.LogError(">> Get Money: " + _gameState.GetMoney());

                    }
                    else
                    {
                        Debug.Log("Game save not found, starting a new game!");
                        if (gameSave == null)
                        {
                            gameSave = new GameSave();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Faled to load saved game due to: " + ex.ToString());
            }
            //_gameState = null;
           

            return gameSave;
        }
        public GameSave LoadGame()
        {
            //gameSave = null;
            if (gameSave == null)
            {
                Init();
            }

            gameSave.Init(Application.version);
            return gameSave;
        }

        public void SaveGame()
        {
            string gameSavePath = GetGameSavePath();
            string content = JsonConvert.SerializeObject(gameSave, Formatting.Indented);
            File.WriteAllText(gameSavePath, content);
        }

        private string GetGameSavePath()
        {
            return Application.persistentDataPath + gameSaveFilename;
        }

        private void OnApplicationQuit()
        {
            gameSave.isFirstOpen = true;
        }
    }
}