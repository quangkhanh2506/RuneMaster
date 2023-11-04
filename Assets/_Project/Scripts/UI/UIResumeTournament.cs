using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;
using UnityEngine.UI;
using System;

public class ResumeTournamentParam : UIParam
{
    public HeroIndex HeroIndex;
    public TournamentMatchIndex MatchIndex;
}
public class UIResumeTournament : BaseUI
{
    [SerializeField] public List<Sprite> lsSpritesCharacter = new List<Sprite>();
    [SerializeField] public TextMeshProUGUI txtMatch;
    [SerializeField] public Image imgCharacter;

    private void OnEnable()
    {
        OnSetUp(new ResumeTournamentParam { HeroIndex = HeroIndex.AutoBot, MatchIndex = TournamentMatchIndex.Quarter_Final });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        ResumeTournamentParam resumeTournamentParam = (ResumeTournamentParam)param;
        imgCharacter.sprite = lsSpritesCharacter[(int)resumeTournamentParam.HeroIndex];
        txtMatch.text = resumeTournamentParam.MatchIndex.ToString().Replace("_", " ");
    }

    public void OnResume()
    {
        // Resume currrent match

        UIManager.Instance.HideUI(this);
    }
    public void OnNew()
    {
        // Create new match

        UIManager.Instance.HideUI(this);
    }
}
