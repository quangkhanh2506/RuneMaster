using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UISecondChance : BaseUI
{
    private float timer;
    [SerializeField] private Image imgFill;
    [SerializeField] private GameObject btnRevivalByGem;
    [SerializeField] private GameObject btnRevivalByAds;
    [SerializeField] private TextMeshProUGUI GemToBuy;
    private int gem;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        SecondChanceParam secondChanceParam = (SecondChanceParam)param;
        gem = secondChanceParam.gem;
        GemToBuy.text = secondChanceParam.gem.ToString();

        timer = secondChanceParam.countdown;
        imgFill.fillAmount = 1;

        if (SaveManager.Instance.SaveGame.resetGame)
        {
            btnRevivalByAds.SetActive(true);
            btnRevivalByGem.SetActive(true);
            btnRevivalByAds.transform.localPosition = new Vector3(195, 0);
            btnRevivalByGem.transform.localPosition = new Vector3(-195, 0);
            SaveManager.Instance.SaveGame.resetGame = false;
        }

        btnRevivalByGem.GetComponent<Button>().interactable = SaveManager.Instance.SaveGame.gem <= gem ? false : true;

        DOTween.To(() => imgFill.fillAmount, x => imgFill.fillAmount = x, 0, timer).OnComplete(() =>
        {
            UIManager.Instance.HideUI(this);
            //UIManager.Instance.ShowUI();
        });
    }

    public void On_ClickRevivalByGem()
    {
        if (SaveManager.Instance.SaveGame.gem <= gem)
        {
            SaveManager.Instance.SaveGame.gem -= gem;
            SaveManager.Instance.Save();
            btnRevivalByGem.SetActive(false);
            btnRevivalByAds.transform.localPosition = Vector3.zero;
            DOTween.KillAll();
            UIManager.Instance.HideUI(this);
        }
    }
    public void On_ClickRevivalByAds()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            btnRevivalByAds.SetActive(false);
            btnRevivalByGem.transform.localPosition = Vector3.zero;
            DOTween.KillAll();
            UIManager.Instance.HideUI(this);
        });
    }
}

public class SecondChanceParam : UIParam
{
    public int gem;
    public float countdown;
}