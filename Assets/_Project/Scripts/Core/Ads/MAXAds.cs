using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MAXAds : IGameAds
{
    private string sdkKey;

    private bool hasRewarded = false;

    private Action<string, double> rewardCallBack;

    private Action customRewardCallback = null;

    private Action watchFailed = null;

    private Action openedCallback = null;

    private Action closedCallback = null;

    private double rewardAmount;

    private string rewardType = "";

    private string bannerAdUnitID = "";

    private string interstitialAdUnitID = "";

    private string rewardedAdUnitID = "";

    private int interstitialRetryAttempt;

    private int rewardedRetryAttempt;

    private MonoBehaviour target = null;

    public MAXAds(MonoBehaviour target, string sdkKey, string bannerIOSAds, string bannerAndroidAds, string interIOSAds, string interAndroidAds,
        string rewardIOSAds, string rewardAndroidAds, Action<string, double> rewardCallback, Action openedCallback, Action closedCallback)
    {
        this.target = target;
        this.sdkKey = sdkKey;
#if UNITY_ANDROID
        bannerAdUnitID = bannerAndroidAds;
        interstitialAdUnitID = interAndroidAds;
        rewardedAdUnitID = rewardAndroidAds;
#else
        bannerAdUnitID = bannerIOSAds;
        interstitialAdUnitID = interIOSAds;
        rewardedAdUnitID = rewardIOSAds;
#endif
        this.rewardCallBack = rewardCallback;
        this.openedCallback = openedCallback;
        this.closedCallback = closedCallback;

    }



    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += configuration =>
        {
            //Init banner
            //InitBanner();
            //Init inter
            InitInterstitial();
            //Init Reward
            InitRewardedVideo();
        };

        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();
    }

    #region Init

    void InitBanner()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerOnAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerOnAdLoadFailedEvent;

        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.CreateBanner(bannerAdUnitID, MaxSdkBase.BannerPosition.BottomCenter);
            ShowBanner();
        }
    }

    void InitInterstitial()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += Interstitial_OnAdLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += Interstitial_OnAdLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += Interstitial_OnAdClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += Interstitial_OnAdDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += Interstitial_OnAdHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += Interstitial_OnAdDisplayFailedEvent;

        LoadInterstitial();
    }

    void InitRewardedVideo()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += Rewarded_OnAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += Rewarded_OnAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += Rewarded_OnAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += Rewarded_OnAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += Rewarded_OnAdDisplayFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Rewarded_OnAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += Rewarded_OnAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += Rewarded_OnAdRevenuePaidEvent;

        LoadRewardedVideo();
    }

    #endregion

    void IGameAds.Update()
    {
        throw new NotImplementedException();
    }


    #region Interstitial
    public bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(interstitialAdUnitID);
    }

    public void LoadInterstitial()
    {
        if (!string.IsNullOrEmpty(interstitialAdUnitID))
        {
            MaxSdk.LoadInterstitial(interstitialAdUnitID);
        }
    }
    public void ShowInterstitial(Action finished)
    {
        if (!string.IsNullOrEmpty(interstitialAdUnitID))
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitID);
        }
    }
    #endregion

    #region Banner

    public void ShowBanner()
    {
        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.ShowBanner(bannerAdUnitID);
        }
    }

    public void HideBanner()
    {
        if (!string.IsNullOrEmpty(bannerAdUnitID))
        {
            MaxSdk.HideBanner(bannerAdUnitID);
        }
    }

    #endregion

    #region Rewarded Video

    public bool isRewardReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitID);
    }

    public void LoadRewardedVideo()
    {
        if (!string.IsNullOrEmpty(rewardedAdUnitID))
        {
            MaxSdk.LoadRewardedAd(rewardedAdUnitID);
        }
    }
    public void ShowRewardedVideo(Action finished, Action watchFailed)
    {
        if (!string.IsNullOrEmpty(rewardedAdUnitID))
        {
            customRewardCallback = finished;
            this.watchFailed = watchFailed;
            if (isRewardReady())
            {
                MaxSdk.ShowRewardedAd(rewardedAdUnitID);
            }
        }
    }
    #endregion

    #region Callback
    private void Rewarded_OnAdRevenuePaidEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("Rewarded_OnAdRevenuePaidEvent");
    }

    private void Rewarded_OnAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    {
        hasRewarded = true;

    }

    private void Rewarded_OnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (hasRewarded)
        {
            if (customRewardCallback != null)
            {
                customRewardCallback();
                customRewardCallback = null;
            }
            else
            {
                rewardCallBack(rewardType, rewardAmount);
            }

            hasRewarded = false;
        }
        else
        {
            watchFailed?.Invoke();

        }

        if (closedCallback != null)
        {
            closedCallback();
        }
        LoadRewardedVideo();
    }

    private void Rewarded_OnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        LoadRewardedVideo();
    }

    private void Rewarded_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (openedCallback != null)
        {
            openedCallback();
        }
    }

    private void Rewarded_OnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {

    }

    private void Rewarded_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log("Rewarded_OnAdLoadFailedEvent");
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        if (target != null)
        {
            target.Invoke("LoadRewardedVideo", (float)retryDelay);
        }
    }

    private void Rewarded_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("Rewarded_OnAdLoadedEvent");
        rewardedRetryAttempt = 0;
    }





    private void Interstitial_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        if (target)
        {
            target.Invoke("LoadInterstitial", (float)retryDelay);
        }
    }

    private void Interstitial_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        interstitialRetryAttempt = 0;
    }

    private void Interstitial_OnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        LoadInterstitial();
    }

    private void Interstitial_OnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        LoadInterstitial();
        if (closedCallback != null)
        {
            closedCallback();
        }

        if (customRewardCallback != null)
        {
            customRewardCallback();
            customRewardCallback = null;
        }
    }

    private void Interstitial_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        if (openedCallback != null)
        {
            openedCallback();
        }

    }

    private void Interstitial_OnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {

    }



    private void BannerOnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
    {
        Debug.Log("BannerOnAdLoadFailedEvent: " + info.Message);
    }

    private void BannerOnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo info)
    {
        Debug.Log("BannerOnAdLoadedEvent");
    }

    

    
    #endregion

}
