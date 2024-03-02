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
    [SerializeField] Button btnRating;
    private int starRating;
    private RatingParam ratingParam;

    private void OnEnable()
    {
        OnSetUp(new RatingParam { gem = 100 });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        ratingParam = (RatingParam)param;
        txtGem.text = "+" + ratingParam.gem;

        if (SaveManager.Instance.SaveGame.isRating)
        {
            btnRating.interactable = false;
        }
    }

    public void OnClickRating(int index)
    {
        starRating = index;
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
        SaveManager.Instance.SaveGame.isRating = true;
        SaveManager.Instance.SaveGame.starRating = starRating;
        //Add gem into savegame
        SaveManager.Instance.SaveGame.gem += ratingParam.gem;
        SaveManager.Instance.Save();

        UIManager.Instance.HideUI(this);
    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.HideUI(this);
    }
}
