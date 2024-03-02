using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;

public class VictoryParam : UIParam
{
    public int coin;
}
public class UIVictory : BaseUI
{
    [SerializeField] public TextMeshProUGUI txtcoin;
    private VictoryParam victoryParam;

    private void OnEnable()
    {
        OnSetUp(new VictoryParam { coin = 1000 });
    }
    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        AdsManager.Instance.HideBanner();
        victoryParam = (VictoryParam)param;
        txtcoin.text = "x" + victoryParam.coin.ToString();
    }
    public void Claim()
    {
        // save coin
        SaveManager.Instance.SaveGame.coin += victoryParam.coin;
        SaveManager.Instance.Save();
        UIManager.Instance.HideUI(this);
    }
    public void ClaimX2()
    {
        //Show ads
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            // save x2 coin
            victoryParam.coin *= 2;
            SaveManager.Instance.SaveGame.coin += victoryParam.coin;
            SaveManager.Instance.Save();
            UIManager.Instance.HideUI(this);
        });
    }
}
