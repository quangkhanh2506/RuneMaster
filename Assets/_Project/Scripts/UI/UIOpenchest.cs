using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class UIOpenchest : BaseUI
{
    [SerializeField] HorizontalScrollSnap horizontalScrollSnap;
    private OpenChestParam openChestParam;
    private int indexReward;
    [SerializeField] List<GameObject> lsReward = new List<GameObject>();
    [SerializeField] private Sprite IconCoin;
    [SerializeField] private Sprite IconGem;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        

        indexReward = 0;
        openChestParam = (OpenChestParam)param;

        StartCoroutine(SetupReward());


        indexReward = GetRamdomReward();
        StartCoroutine(DecreaseSpeedAndGoToScreen());
    }

    private IEnumerator SetupReward()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < lsReward.Count; i++)
        {
            int indexitem = i % 13;
            var ItemReward = ItemConfig.GetConfigByID(openChestParam.lsOpenChestRewards[indexitem].id);
            lsReward[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemReward.sprite;
            lsReward[i].transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
            if (ItemReward.idItem < 3)
            {
                lsReward[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "+" + openChestParam.lsOpenChestRewards[indexitem].amount.ToString();

                if (ItemReward.idItem == 1)
                {
                    lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = IconCoin;
                    lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                }
                else
                {
                    lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = IconGem;
                    lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                }
                lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                lsReward[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ItemReward.name + "\n" + "+" + openChestParam.lsOpenChestRewards[indexitem].amount.ToString();
                lsReward[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private int GetRamdomReward()
    {
        float dicroll = Random.Range(0f, 100f);

        for (int i = 0; i < openChestParam.lsOpenChestRewards.Count; i++)
        {
            if (openChestParam.lsOpenChestRewards[i].rate >= dicroll) return i;
            dicroll -= openChestParam.lsOpenChestRewards[i].rate;
        }

        return 0;
    }

    public void On_ClickClaim()
    {
        UIManager.Instance.ShowUI(UIIndex.UIChest);
        UIManager.Instance.HideUI(this);
    }

    public void On_ClickClaimX2()
    {
        UIManager.Instance.ShowUI(UIIndex.UIChest);
        UIManager.Instance.HideUI(this);
    }

    private IEnumerator DecreaseSpeedAndGoToScreen()
    {
        //horizontalScrollSnap.transitionSpeed = 50;
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < 4; i++)
        {
            horizontalScrollSnap.GoToScreen(indexReward);
            yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
            Debug.Log(indexReward);
            indexReward += 7;
        }
        horizontalScrollSnap.RestartOnEnable = true;
    }

}

public class OpenChestParam : UIParam
{
    public List<OpenChestReward> lsOpenChestRewards = new List<OpenChestReward>();
}

public class OpenChestReward
{
    public int id;
    public float rate;
    public int amount;
}
