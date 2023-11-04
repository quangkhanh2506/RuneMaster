using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;

public class RemoveAdsParam : UIParam
{
    public int coin;
}

public class UIRemoveAds : BaseUI
{
    [SerializeField] public TextMeshProUGUI txtCoin;

    private void OnEnable()
    {
        OnSetUp(new RemoveAdsParam { coin = 6000 });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        RemoveAdsParam removeAdsParam = (RemoveAdsParam)param;
        txtCoin.text = removeAdsParam.coin.ToString();       
    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        // Save coin

        // Purchase


        UIManager.Instance.HideUI(this);
    }
}
