using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;
using System;
using UnityEngine.UI;

public class UIChest : BaseUI
{
    [SerializeField] List<GameObject> lsChest = new List<GameObject>();
    [SerializeField] TextMeshProUGUI timer; 
   

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        ChestConfig.Setup();
        ItemConfig.Setup();
        TimeSpan ts1 = new TimeSpan(System.DateTime.Now.Ticks);
        double countdown = ts1.TotalSeconds;
        UpdateTimer(countdown);
        UpdateChest();
        StartCoroutine(CountDown());
    }

    private void UpdateTimer(double countdown)
    {
        double time = SaveManager.Instance.SaveGame.timerFreeChest;

        if ((1800 - countdown + time) < 0)
        {
            timer.gameObject.SetActive(false);
            return;
        }
        timer.gameObject.SetActive(true);
        timer.text = ((int)(1800 - countdown + time) / 60).ToString("00") + ":" + ((1800 - countdown + time)%60).ToString("00");
    }

    private IEnumerator CountDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            TimeSpan ts1 = new TimeSpan(System.DateTime.Now.Ticks);
            double countdown = ts1.TotalSeconds;
            UpdateTimer(countdown);
        }
    }


    private void UpdateChest()
    {
        if (SaveManager.Instance.SaveGame.chestFree == 0)
        {
            lsChest[1].transform.GetChild(2).gameObject.SetActive(false);
            lsChest[1].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            lsChest[1].transform.GetChild(2).gameObject.SetActive(true);
            lsChest[1].transform.GetChild(3).gameObject.SetActive(false);
            lsChest[1].transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x"
                + SaveManager.Instance.SaveGame.chestEpic + " " + "Chests";
        }

        if (SaveManager.Instance.SaveGame.chestEpic == 0)
        {
            lsChest[1].transform.GetChild(2).gameObject.SetActive(false);
            lsChest[1].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            lsChest[1].transform.GetChild(2).gameObject.SetActive(true);
            lsChest[1].transform.GetChild(3).gameObject.SetActive(false);
            lsChest[1].transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x"
                + SaveManager.Instance.SaveGame.chestEpic + " " + "Chests";
        }
        if (SaveManager.Instance.SaveGame.chestHeroic == 0)
        {
            lsChest[2].transform.GetChild(2).gameObject.SetActive(false);
            lsChest[2].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            lsChest[2].transform.GetChild(2).gameObject.SetActive(true);
            lsChest[2].transform.GetChild(3).gameObject.SetActive(false);
            lsChest[2].transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x"
                + SaveManager.Instance.SaveGame.chestHeroic + " " + "Chests";
        }
        if (SaveManager.Instance.SaveGame.chestLegendary == 0)
        {
            lsChest[3].transform.GetChild(2).gameObject.SetActive(false);
            lsChest[3].transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            lsChest[3].transform.GetChild(2).gameObject.SetActive(true);
            lsChest[3].transform.GetChild(3).gameObject.SetActive(false);
            lsChest[3].transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x"
                + SaveManager.Instance.SaveGame.chestLegendary + " " + "Chests";
        }
    }

    public void On_ClickNoAds()
    {

    }

    public void On_ClickFreeGems()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            SaveManager.Instance.SaveGame.gem += 25;
        });
    }

    public void On_ClickOpen(int index)
    {
        List<OpenChestReward> openChestRewards = new List<OpenChestReward>();
        if (index == 1)
        {
            SaveManager.Instance.SaveGame.chestEpic -= 1;
            var lsItemChest = ChestConfig.GetConfigByID(2).chestItems;
            float totalRateCurrence = 0, totalrateCurrenceX;
            for (int i = 0; i < lsItemChest.Count; i++)
            {
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    totalRateCurrence += lsItemChest[i].rate;
                }
            }

            totalrateCurrenceX = ((float)(9 - SaveManager.Instance.SaveGame.numberOpenEpicChest) * totalRateCurrence + 50 * SaveManager.Instance.SaveGame.numberOpenEpicChest) / 9f;

            
            for (int i = 0; i < lsItemChest.Count; i++)
            {
                OpenChestReward reward = new OpenChestReward();
                reward.amount = lsItemChest[i].amout;
                reward.id = lsItemChest[i].itemId;
                if (SaveManager.Instance.SaveGame.numberOpenFreeChest == 10) totalrateCurrenceX = 0;
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    reward.rate = (lsItemChest[i].rate / totalRateCurrence) * totalrateCurrenceX;
                }
                else
                {
                    reward.rate = (lsItemChest[i].rate / (100 - totalRateCurrence)) * (100 - totalrateCurrenceX);
                }
                openChestRewards.Add(reward);
            }
        }
        else if (index == 2)
        {
            SaveManager.Instance.SaveGame.chestHeroic -= 1;
            var lsItemChest = ChestConfig.GetConfigByID(3).chestItems;
            float totalRateCurrence = 0, totalrateCurrenceX;
            for (int i = 0; i < lsItemChest.Count; i++)
            {
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    totalRateCurrence += lsItemChest[i].rate;
                }
            }

            totalrateCurrenceX = ((float)(9 - SaveManager.Instance.SaveGame.numberOpenHeroicChest) * totalRateCurrence + 50 * SaveManager.Instance.SaveGame.numberOpenHeroicChest) / 9f;


            for (int i = 0; i < lsItemChest.Count; i++)
            {
                OpenChestReward reward = new OpenChestReward();
                reward.amount = lsItemChest[i].amout;
                reward.id = lsItemChest[i].itemId;
                if (SaveManager.Instance.SaveGame.numberOpenFreeChest == 10) totalrateCurrenceX = 0;
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    reward.rate = (lsItemChest[i].rate / totalRateCurrence) * totalrateCurrenceX;
                }
                else
                {
                    reward.rate = (lsItemChest[i].rate / (100 - totalRateCurrence)) * (100 - totalrateCurrenceX);
                }
                openChestRewards.Add(reward);
            }
        }
        else if (index == 3)
        {
            SaveManager.Instance.SaveGame.chestLegendary -= 1;
            var lsItemChest = ChestConfig.GetConfigByID(4).chestItems;
            float totalRateCurrence = 0, totalrateCurrenceX;
            for (int i = 0; i < lsItemChest.Count; i++)
            {
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    totalRateCurrence += lsItemChest[i].rate;
                }
            }

            totalrateCurrenceX = ((float)(9 - SaveManager.Instance.SaveGame.numberOpenLengendaryChest) * totalRateCurrence + 50 * SaveManager.Instance.SaveGame.numberOpenLengendaryChest) / 9f;


            for (int i = 0; i < lsItemChest.Count; i++)
            {
                OpenChestReward reward = new OpenChestReward();
                reward.amount = lsItemChest[i].amout;
                reward.id = lsItemChest[i].itemId;
                if (SaveManager.Instance.SaveGame.numberOpenFreeChest == 10) totalrateCurrenceX = 0;
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    reward.rate = (lsItemChest[i].rate / totalRateCurrence) * totalrateCurrenceX;
                }
                else
                {
                    reward.rate = (lsItemChest[i].rate / (100 - totalRateCurrence)) * (100 - totalrateCurrenceX);
                }
                openChestRewards.Add(reward);
            }
        }
        else if (index == 0)
        {
            SaveManager.Instance.SaveGame.chestFree -= 1;
            var lsItemChest = ChestConfig.GetConfigByID(1).chestItems;
            float totalRateCurrence = 0, totalrateCurrenceX;
            for (int i = 0; i < lsItemChest.Count; i++)
            {
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    totalRateCurrence += lsItemChest[i].rate;
                }
            }

            totalrateCurrenceX = ((float)(9 - SaveManager.Instance.SaveGame.numberOpenFreeChest) * totalRateCurrence + 50 * SaveManager.Instance.SaveGame.numberOpenFreeChest) / 9f;

            for (int i = 0; i < lsItemChest.Count; i++)
            {
                OpenChestReward reward = new OpenChestReward();
                reward.amount = lsItemChest[i].amout;
                reward.id = lsItemChest[i].itemId;
                if (SaveManager.Instance.SaveGame.numberOpenFreeChest == 10) totalrateCurrenceX = 0;
                if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
                {
                    reward.rate = (lsItemChest[i].rate / totalRateCurrence) * totalrateCurrenceX;
                }
                else
                {
                    reward.rate = (lsItemChest[i].rate / (100 - totalRateCurrence)) * (100 - totalrateCurrenceX);
                }
                openChestRewards.Add(reward);
            }
        }




        UIManager.Instance.ShowUI(UIIndex.UIOpenChest, new OpenChestParam
        {
            lsOpenChestRewards = openChestRewards
        });
        UIManager.Instance.HideUI(this);
    }

    public void On_ClickOpenAds()
    {
        var lsItemChest = ChestConfig.GetConfigByID(1).chestItems;
        float totalRateCurrence = 0, totalrateCurrenceX;
        for(int i = 0; i < lsItemChest.Count; i++)
        {
            if (lsItemChest[i].itemId == 1 || lsItemChest[i].itemId == 2)
            {
                totalRateCurrence += lsItemChest[i].rate;
            }
            
        }
            
        totalrateCurrenceX = ((float)(9 - SaveManager.Instance.SaveGame.numberOpenFreeChest) * totalRateCurrence + 50 * SaveManager.Instance.SaveGame.numberOpenFreeChest) / 9f;

        List<OpenChestReward> openChestRewards = new List<OpenChestReward>();
        for (int i = 0; i < lsItemChest.Count; i++)
        {
            OpenChestReward reward = new OpenChestReward();
            reward.amount = lsItemChest[i].amout;
            reward.id = lsItemChest[i].itemId;
            if (SaveManager.Instance.SaveGame.numberOpenFreeChest == 10) totalrateCurrenceX = 0;
            if (lsItemChest[i].itemId==1 || lsItemChest[i].itemId == 2)
            {
                reward.rate = (lsItemChest[i].rate / totalRateCurrence) * totalrateCurrenceX;
            }
            else
            {
                reward.rate = (lsItemChest[i].rate / (100-totalRateCurrence)) * (100-totalrateCurrenceX);
            }
            openChestRewards.Add(reward);
        }

        AdsManager.Instance.ShowRewardedAds(() =>
        {
            UIManager.Instance.ShowUI(UIIndex.UIOpenChest, new OpenChestParam
            {
                lsOpenChestRewards = openChestRewards
            });

            lsChest[0].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
            TimeSpan ts1 = new TimeSpan(System.DateTime.Now.Ticks);
            SaveManager.Instance.SaveGame.timerFreeChest = ts1.TotalSeconds;

            UIManager.Instance.HideUI(this);
        });
    }

    public void On_ClickBuyx1(int index)
    {
        if (SaveManager.Instance.SaveGame.gem < 10 * index)
        {
            return;
        }

        SaveManager.Instance.SaveGame.gem -= 10*index;
        if (index == 1)
        {
            SaveManager.Instance.SaveGame.chestEpic += 1;
        }
        else if (index == 2)
        {
            SaveManager.Instance.SaveGame.chestHeroic += 1;
        }
        else if (index == 4)
        {
            SaveManager.Instance.SaveGame.chestLegendary += 1;
        }
        SaveManager.Instance.Save();
        UpdateChest();
    }
    public void On_ClickBuyx10(int index)
    {
        int price = index * 100 - index * 10;

        if (SaveManager.Instance.SaveGame.gem < price)
        {
            return;
        }


        SaveManager.Instance.SaveGame.gem -= price;
        if (index == 1)
        {
            SaveManager.Instance.SaveGame.chestEpic += 10;
        }
        else if (index == 2)
        {
            SaveManager.Instance.SaveGame.chestHeroic += 10;
        }
        else if (index == 4)
        {
            SaveManager.Instance.SaveGame.chestLegendary += 10;
        }
        SaveManager.Instance.Save();
        UpdateChest();
    }
}
