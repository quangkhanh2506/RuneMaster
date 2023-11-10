using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameAds
{
    void Init();

    void Update();



    void ShowBanner();

    void HideBanner();



    bool isRewardReady();

    void LoadRewardedVideo();

    void ShowRewardedVideo(Action finished, Action watchFailed);



    bool IsInterstitialReady();

    void LoadInterstitial();

    void ShowInterstitial(Action finished);
}
