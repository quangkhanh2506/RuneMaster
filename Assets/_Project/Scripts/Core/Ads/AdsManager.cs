using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdsManager : SingletonMono<AdsManager>
{
    private IGameAds maxAppLovin = null;
    public bool maxApplovinSupport;
    public string maxSDKKey;
    public string maxAndroidBannerID;
    public string maxIOSBannerID;
    public string maxAndroidInterID;
    public string maxIOSInterID;
    public string maxAndroidrewardID;
    public string maxIOSrewardID;

    private void Awake()
    {
        maxAppLovin = new MAXAds(this, maxSDKKey, maxIOSBannerID, maxAndroidBannerID, maxIOSInterID, maxAndroidInterID, maxIOSrewardID,
            maxAndroidrewardID, RewardCallback, OpenedCallback, ClosedCallback);
        maxAppLovin.Init();
        ShowBanner();
    }

    private void ClosedCallback()
    {

    }

    private void OpenedCallback()
    {

    }

    private void RewardCallback(string arg1, double arg2)
    {

    }

    public void ShowBanner()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            maxAppLovin.ShowBanner();
        }
    }

    public void HideBanner()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            maxAppLovin.HideBanner();
        }
    }

    public bool IsInterstitialReady()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            return maxAppLovin.IsInterstitialReady();
        }
        return Application.isEditor;
    }

    public void LoadInterstitial()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            maxAppLovin.LoadInterstitial();
        }
    }

    public void ShowInterstitialAds(Action finished)
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            if (IsInterstitialReady())
            {
                maxAppLovin.ShowInterstitial(finished);
            }
            else
            {
                if (finished != null)
                {
                    finished();
                }
            }
        }

    }

    public bool isRewardReady()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            return maxAppLovin.isRewardReady();
        }
        return Application.isEditor;
    }

    public void LoadRewardedAds()
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            maxAppLovin.LoadRewardedVideo();
        }
    }

    public void ShowRewardedAds(Action finished, Action watchFailed = null)
    {
        if (maxApplovinSupport && maxAppLovin != null)
        {
            if (isRewardReady())
            {
                maxAppLovin.ShowRewardedVideo(finished, watchFailed);
            }
            else
            {
                //Show UI No Internet
            }
        }

    }
}
