using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveGame
{

    private List<HeroIndex> lsIDHeroPurchased = new List<HeroIndex>();
    public RankIdex curRankIdex;
    public HeroIndex curHeroIndex;
    public int curAvatarIndex;
    public int coin;
    public int gem;
    public TournamentMatchIndex TournamentMatchIndex;
    public int chestEpic;
    public int chestFree;
    public int chestHeroic;
    public int chestLegendary;
    public bool isRating;
    public int starRating;
    public string name;

    public int currentDay;
    public int currentWeek;

    public string FirstVersion;

    public bool resetGame;

    public double timerFreeChest;

    public int numberOpenFreeChest;
    public int numberOpenEpicChest;
    public int numberOpenHeroicChest;
    public int numberOpenLengendaryChest;

    public void Init(string Version)
    {
        if(FirstVersion == "")
        {
            FirstVersion = Version;
        }

        int firstOpen = PlayerPrefs.GetInt("FirstOpen", 0);
        Debug.Log(firstOpen);
        if (firstOpen == 0)
        {
            Debug.Log("Init file save game");
            coin = 0;
            gem = 3000;
            curHeroIndex = HeroIndex.AutoBot;
            curRankIdex = RankIdex.Bronze;
            curAvatarIndex = (int)HeroIndex.AutoBot + 1;
            lsIDHeroPurchased.Add(HeroIndex.AutoBot);
            chestEpic = 0;
            chestFree = 0;
            chestHeroic = 0;
            chestLegendary = 0;
            isRating = false;
            starRating = 0;
            name = "Player";
            resetGame = true;
            currentDay = DateTime.Now.DayOfYear;
            currentWeek = DateTime.Now.DayOfYear;

            timerFreeChest = 0;
            numberOpenFreeChest = 0;
            numberOpenEpicChest = 0;
            numberOpenHeroicChest = 0;
            numberOpenLengendaryChest = 0;

        }
        firstOpen++;
        PlayerPrefs.SetInt("FirstOpen", firstOpen);
    }
}
