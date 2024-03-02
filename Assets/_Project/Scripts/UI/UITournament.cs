using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using TMPro;

public class TournamentParam : UIParam
{
    public int coin;
    public IconTournamentIndex iconTournamentIndex;
}

public class UITournament : BaseUI
{
    [SerializeField] private List<Sprite> lsIconTournament = new List<Sprite>();
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private Image imgIconTournament;
    private TournamentParam tournamentParam;

    private void OnEnable()
    {
        OnSetUp(new TournamentParam { coin = 300, iconTournamentIndex = IconTournamentIndex.Silver });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        AdsManager.Instance.HideBanner();
        tournamentParam = (TournamentParam)param;
        txtCoin.text = tournamentParam.coin.ToString();
        imgIconTournament.sprite = lsIconTournament[(int)tournamentParam.iconTournamentIndex];

    }

    public void On_CLickClaim()
    {
        SaveManager.Instance.SaveGame.coin += tournamentParam.coin;
        SaveManager.Instance.Save();
        UIManager.Instance.HideUI(this);
    }

    public void On_CLickClaimX2()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            SaveManager.Instance.SaveGame.coin += tournamentParam.coin * 2;
            SaveManager.Instance.Save();
            UIManager.Instance.HideUI(this);
        });
        
    }
}
