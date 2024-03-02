using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;
using System;
using UnityEngine.UI;

public class UIDailyGift : BaseUI
{
    [SerializeField] private TextMeshProUGUI txtDay;
    [SerializeField] private Image imgReward;
    [SerializeField] private TextMeshProUGUI txtReward;
    [SerializeField] private GameObject imgLock;
    [SerializeField] private GameObject txtMissDay;
    [SerializeField] private GameObject content;


    [SerializeField] private GameObject btnClaim;
    [SerializeField] private GameObject btnClainX2;
    [SerializeField] private GameObject btnReClaim;

    private int currentIdex;
    private int today;

    private DailygiftParam dailygiftParam;

    [SerializeField] List<ItemDailyGift> lsItemDailyGifts = new List<ItemDailyGift>();


    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        dailygiftParam = (DailygiftParam)param;
        Debug.Log(SaveManager.Instance.SaveGame.currentDay + " " + SaveManager.Instance.SaveGame.currentWeek);

        // Reset week
        if((SaveManager.Instance.SaveGame.currentDay - SaveManager.Instance.SaveGame.currentWeek) / 7 >= 1)
        {
            SaveManager.Instance.SaveGame.currentWeek = DateTime.Now.DayOfYear - ((SaveManager.Instance.SaveGame.currentDay - SaveManager.Instance.SaveGame.currentWeek) % 7);
            for (int i = 0; i < lsItemDailyGifts.Count; i++)
            {
                lsItemDailyGifts[i].isClaim = false;
            }
        }

        today = SaveManager.Instance.SaveGame.currentDay - SaveManager.Instance.SaveGame.currentWeek;
        for(int i = 0; i < lsItemDailyGifts.Count; i++)
        {
            lsItemDailyGifts[i].SetupItem(i - today, i, dailygiftParam.lsDailygiftItemDatas[i]);
        }
        currentIdex = today;
        if (lsItemDailyGifts[currentIdex].isClaim)
        {
            content.SetActive(false);
        }
        else
        {
            content.SetActive(true);
            btnClaim.SetActive(true);
            btnClainX2.SetActive(true);
            btnReClaim.SetActive(false);
            imgLock.SetActive(false);
            txtMissDay.SetActive(false);
            imgReward.sprite = lsItemDailyGifts[today].imgReward.sprite;
            imgReward.SetNativeSize();
            if (dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestValue == 0)
            {
                imgReward.transform.localScale = new Vector3(2f, 2f, 0);
            }
            txtDay.text = "DAY " + (today + 1).ToString();
            txtReward.text = dailygiftParam.lsDailygiftItemDatas[today].coinValue > 0 ? dailygiftParam.lsDailygiftItemDatas[today].coinValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[today].gemValue > 0 ? dailygiftParam.lsDailygiftItemDatas[today].gemValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[today].ChestIndex + " " + "X" + dailygiftParam.lsDailygiftItemDatas[today].ChestValue.ToString()));
        }



    }

    public void Onclick_day(int day)
    {
        currentIdex = day;
        if (currentIdex == today)
        {
            btnClaim.SetActive(true);
            btnClainX2.SetActive(true);
            btnReClaim.SetActive(false);
            imgLock.SetActive(false);
            txtMissDay.SetActive(false);
            imgReward.sprite = lsItemDailyGifts[today].imgReward.sprite;
            imgReward.SetNativeSize();
            if (dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestValue == 0)
            {
                imgReward.transform.localScale = new Vector3(2f, 2f, 0);
            }
            txtDay.text = "DAY " + (currentIdex + 1).ToString();
            txtReward.text = dailygiftParam.lsDailygiftItemDatas[currentIdex].coinValue > 0 ? dailygiftParam.lsDailygiftItemDatas[currentIdex].coinValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[currentIdex].gemValue > 0 ? dailygiftParam.lsDailygiftItemDatas[currentIdex].gemValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestIndex + "\n" + "X" + dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestValue.ToString()));
        }
        else
        {
            btnClaim.SetActive(false);
            btnClainX2.SetActive(false);
            btnReClaim.SetActive(true);
            imgLock.SetActive(true);
            txtMissDay.SetActive(true);
            imgReward.sprite = lsItemDailyGifts[currentIdex].imgReward.sprite;
            imgReward.SetNativeSize();
            if (dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestValue == 0)
            {
                imgReward.transform.localScale = new Vector3(2f, 2f, 0);
            }
            txtDay.text = "DAY " + (currentIdex + 1).ToString();
            txtReward.text = dailygiftParam.lsDailygiftItemDatas[currentIdex].coinValue > 0 ? dailygiftParam.lsDailygiftItemDatas[currentIdex].coinValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[currentIdex].gemValue > 0 ? dailygiftParam.lsDailygiftItemDatas[currentIdex].gemValue.ToString() :
                (dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestIndex + " " + "X" + dailygiftParam.lsDailygiftItemDatas[currentIdex].ChestValue.ToString()));
        }

    }

    public void Onclick_Claim()
    {
        lsItemDailyGifts[currentIdex].ClaimItem();
        UIManager.Instance.ShowUI(UIIndex.UIMain);
        UIManager.Instance.HideUI(this);
    }

    public void Onclick_Reclaim()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            lsItemDailyGifts[currentIdex].ClaimItem();
            UIManager.Instance.ShowUI(UIIndex.UIMain);
            UIManager.Instance.HideUI(this);
        });
    }

    public void Onclick_ClaimX2()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            lsItemDailyGifts[currentIdex].ClaimItem();
            UIManager.Instance.ShowUI(UIIndex.UIMain);
            UIManager.Instance.HideUI(this);
        });
    }
    public void OnClose()
    {
        UIManager.Instance.ShowUI(UIIndex.UIMain);
        UIManager.Instance.HideUI(this);
    }

    public bool CheckClaimed()
    {
        for(int i = 0; i <= currentIdex; i++)
        {
            if (!lsItemDailyGifts[i].isClaim) return true;
        }
        return false;
    }
}

public class DailygiftParam : UIParam
{
    public List<DailygiftItemData> lsDailygiftItemDatas = new List<DailygiftItemData>();
}

[Serializable]
public class DailygiftItemData
{
   public int gemValue;
   public int coinValue;
   public ChestIndex ChestIndex;
   public int ChestValue;
}