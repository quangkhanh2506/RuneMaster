using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using TMPro;

public class RatingParam : UIParam
{
    public int gem;
}

public class UIRating : BaseUI
{
    [SerializeField] List<GameObject> lsFillStars = new List<GameObject>();
    [SerializeField] TextMeshProUGUI txtGem;

    private void OnEnable()
    {
        OnSetUp(new RatingParam { gem = 100 });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        RatingParam ratingParam = (RatingParam)param;
        txtGem.text = "+" + ratingParam.gem;

        
    }

    public void OnClickRating(int index)
    {
        for(int i = 0; i < lsFillStars.Count; i++)
        {
            if (i > index)
            {
                lsFillStars[i].SetActive(false);
            }
            else
            {
                lsFillStars[i].SetActive(true);
            }
        }
    }

    public void GetItNow()
    {
        //Show Rating

        //Save data rating

        //Add gem into savegame

        UIManager.Instance.HideUI(this);
    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.HideUI(this);
    }
}
