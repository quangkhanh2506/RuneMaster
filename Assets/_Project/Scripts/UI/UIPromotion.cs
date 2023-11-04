using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using System;

public class PromotionParam : UIParam
{
    public RankIdex rankIndex;
}

public class UIPromotion : BaseUI
{
    [SerializeField] public List<Sprite> lsSpriteRanks = new List<Sprite>();
    [SerializeField] public Image imgRank;

    private void OnEnable()
    {
        OnSetUp(new PromotionParam { rankIndex = RankIdex.Silver });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        PromotionParam promotionParam = (PromotionParam)param;
        // Change sprite rank
        promotionParam.rankIndex++;
        imgRank.sprite = lsSpriteRanks[(int)promotionParam.rankIndex];
        imgRank.SetNativeSize();

        //Save rank
    }

    //public void UpdateRank()
    //{

    //}


    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.HideUI(this);
    }
}
