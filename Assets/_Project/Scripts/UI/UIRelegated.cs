using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class RelagatedParam: UIParam
{
    public RankIdex rankIdex;
}

public class UIRelegated : BaseUI
{
    [SerializeField] public List<Sprite> lsSpriteRanks = new List<Sprite>();
    [SerializeField] public Image currentImgRank;
    [SerializeField] public Image UpdateImgRank;

    private void OnEnable()
    {
        OnSetUp(new RelagatedParam { rankIdex = RankIdex.Gold });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        RelagatedParam relegatedParam = (RelagatedParam)param;
        //change sprite rank
        currentImgRank.sprite = lsSpriteRanks[(int)relegatedParam.rankIdex];
        currentImgRank.SetNativeSize();

        UpdateImgRank.sprite = lsSpriteRanks[(int)relegatedParam.rankIdex - 1];
        UpdateImgRank.SetNativeSize();

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
