using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;


public class DefeatParam : UIParam
{
    public int coin;
}
public class UIDefeat : BaseUI
{
    private DefeatParam defeatParam;
    [SerializeField] private TextMeshProUGUI txtcoin;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        AdsManager.Instance.HideBanner();
        defeatParam = (DefeatParam)param;
        txtcoin.text = "x" + defeatParam.coin.ToString();
    }
    public void Claim()
    {
        // save coin
        SaveManager.Instance.SaveGame.coin += defeatParam.coin;
        SaveManager.Instance.Save();
        Grid.Instance.ResetMatch();
        UIManager.Instance.ShowUI(UIIndex.UIMain);
        UIManager.Instance.HideUI(UIIndex.UIGame_normal);
        UIManager.Instance.HideUI(this);
    }
    public void ClaimX2()
    {
        //Show ads
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            // save x2 coin
            defeatParam.coin *= 2;
            SaveManager.Instance.SaveGame.coin += defeatParam.coin;
            SaveManager.Instance.Save();
            Grid.Instance.ResetMatch();
            UIManager.Instance.ShowUI(UIIndex.UIMain);
            UIManager.Instance.HideUI(UIIndex.UIGame_normal);
            UIManager.Instance.HideUI(this);
            
        });
    }


}
