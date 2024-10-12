using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDailyGift : MonoBehaviour
{
    [SerializeField] Image imgBgCircle;
    [SerializeField] List<Sprite> lsSpritesBG = new List<Sprite>();

    [SerializeField] TextMeshProUGUI txtDailyGiftContent;
    public Image imgReward;
    [SerializeField] List<Sprite> lsSpritesCoin = new List<Sprite>();
    [SerializeField] List<Sprite> lsSpritesGem = new List<Sprite>();
    [SerializeField] List<Sprite> lsSpritesChest = new List<Sprite>();

    [SerializeField] GameObject txtCoin;
    [SerializeField] GameObject txtGem;
    [SerializeField] GameObject txtChest;

    public bool isClaim = false;

    public void SetupItem(int isDay, int day, DailygiftItemData dailygiftItemData)
    {
        if (isDay == 0)
        {
            if (isClaim)
            {
                GetComponent<Button>().interactable = false;
                txtDailyGiftContent.text = "CLAIM";
                imgBgCircle.sprite = lsSpritesBG[0];
            }
            else
            {
                GetComponent<Button>().interactable = true;
                imgBgCircle.sprite = lsSpritesBG[2];
                txtDailyGiftContent.text = "DAY " + (day+1);
            }

        }
        else if (isDay > 0)
        {
            GetComponent<Button>().interactable = false;
            imgBgCircle.sprite = lsSpritesBG[1];
            txtDailyGiftContent.text = "DAY " + (day + 1);
        }
        else
        {
            if (!isClaim)
            {
                GetComponent<Button>().interactable = true;
                imgBgCircle.sprite = lsSpritesBG[1];
                txtDailyGiftContent.text = "DAY " + (day + 1);
            }
        }
        if (dailygiftItemData.coinValue > 0)
        {
            imgReward.sprite = lsSpritesCoin[2];
            imgReward.SetNativeSize();
            txtCoin.SetActive(true);
            txtCoin.GetComponent<TextMeshProUGUI>().text = dailygiftItemData.coinValue.ToString();
            txtChest.SetActive(false);
            txtGem.SetActive(false);
        }
        else if (dailygiftItemData.gemValue > 0)
        {
            imgReward.sprite = lsSpritesGem[2];
            imgReward.SetNativeSize();
            txtCoin.SetActive(false);
            txtChest.SetActive(false);
            txtGem.SetActive(true);
            txtGem.GetComponent<TextMeshProUGUI>().text = dailygiftItemData.gemValue.ToString();
        }
        else
        {
            imgReward.sprite = lsSpritesChest[(int)dailygiftItemData.ChestIndex];
            imgReward.SetNativeSize();
            imgReward.transform.localScale = new Vector3(1.2f, 1.2f, 0);
            txtCoin.SetActive(false);
            txtChest.SetActive(true);
            txtGem.GetComponent<TextMeshProUGUI>().text = dailygiftItemData.ChestIndex + " " + "X" + dailygiftItemData.ChestValue.ToString();
            txtGem.SetActive(false);
            
        }

    }

    public void ClaimItem()
    {
        GetComponent<Button>().interactable = false;
        txtDailyGiftContent.text = "CLAIM";
        imgBgCircle.sprite = lsSpritesBG[0];
        isClaim = true;
    }
}