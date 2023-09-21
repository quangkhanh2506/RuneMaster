using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;


[Serializable]
public class GameSave
{
    public string FirstVersion = "";
    public bool isFirstOpen;
    
    // Setting
    public bool VibrateOn;
    public bool MusicOn;
    public bool SoundOn;

    // Currency
    public int Coin = 50;
    public int Gem;
    
    public int CurrentGemReward;
    
    // public List<int> GirlFriendSkinBoughts = new List<int>();
    // public List<int> BoyFriendSkinBoughts = new List<int>();

    // Current Level Map
    public int CurrentLevel;
    
    // Tower
    public int CurrentCoinBuyTower = 5;
    public int CurrentLevelTower = 0;
    public int HighestLevelTower = 1;
    
    public List<int> lsIndexTowers = new List<int>();
    
    // Boooster
    public int CurrentSelectBooster = -1;
    public int CurrentValueBuyBooster = 1500;

    public List<int> lsBoughtBoosters = new List<int>();
    
    // Mission
    public int CurrentGroupMission = 2;
    public int CurrentProgressMissionStep = 0;
    public bool CanGetStepRewardMission = false;
    
    // Leader board
    public string name = "YOU";
    public int idLeaderboard = 8860;
    public int score;
    
    // Upgrade
    public List<int> lsLevelUpgrades = new List<int>();
    
    
    // Mission
    public List<int> lsCurrentIndexOfGroupMissions = new List<int>();
    public List<int> lsIndexGroupMissions = new List<int>();
    
    
    // DateTime save
    public double EndOpenChestTime;
    
    // Rating
    public int CurrentCountRating = 0;
    public bool ShowRating = true;
    // Remove ads
    public bool isRemoveAds = false;
    
    public void Init(string version)
    {
        if (FirstVersion == "")
        {
            FirstVersion = version;
        }
        int firstOpen = PlayerPrefs.GetInt("FirstOpen", 0);
#if UNITY_EDITOR
        //firstOpen = 0;
#endif
        
        if (firstOpen == 0)
        {
            VibrateOn = true;
            ShowRating = true;
            Coin = 50;
            Gem = 0;
            
            isRemoveAds = false;
            
            CurrentLevel = 1;
            CurrentLevelTower = 0;
            CurrentCoinBuyTower = 5;
            // BoyFriendSkinBoughts.Add(0);
            // GirlFriendSkinBoughts.Add(0);
            
            HighestLevelTower = 1;
            isFirstOpen = false;
            
            lsIndexTowers.Clear();
            
        }
        firstOpen++;
        PlayerPrefs.SetInt("FirstOpen", firstOpen);
    }
}