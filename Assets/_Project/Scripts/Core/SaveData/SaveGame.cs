using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveGame
{
    public bool isFirstOpen;

    private List<HeroIndex> lsIDHeroPurchased = new List<HeroIndex>();
    public RankIdex RankIdex;
    public HeroIndex HeroIndex;
    public int coin;
    public int gem;
    public TournamentMatchIndex TournamentMatchIndex;

    public string FirstVersion;

    public void Init(string Version)
    {
        if(FirstVersion == "")
        {
            FirstVersion = Version;
        }

        int firstOpen = PlayerPrefs.GetInt("FirstOpen", 0);
        if (firstOpen == 0)
        {
            coin = 0;
            gem = 0;
            HeroIndex = HeroIndex.AutoBot;
            RankIdex = RankIdex.Bronze;
            lsIDHeroPurchased.Add(HeroIndex.AutoBot);

            isFirstOpen = false;
        }
        firstOpen++;
        PlayerPrefs.SetInt("FirstOpen", firstOpen);
    }
}
