using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using TMPro;

public class UISuperGift : BaseUI
{
    [SerializeField] private List<ItemSuperGift> lsItemSuperGifts = new List<ItemSuperGift>();
    [SerializeField] private List<Sprite> lsSpritesCoin = new List<Sprite>();
    [SerializeField] private List<Sprite> lsSpritesGem = new List<Sprite>();
    [SerializeField] private List<Sprite> lsSpritesChest = new List<Sprite>();
    [SerializeField] private UIMainMenu uIMainMenu;
    private SuperGiftParam superGiftParam;
    private int indexReward = 0;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        superGiftParam = (SuperGiftParam)param;
        if (indexReward == 3) indexReward = 0;
        for(int i = 0; i < 3; i++)
        {
            if (i == indexReward) lsItemSuperGifts[i].Claim();
            else if (i > indexReward) lsItemSuperGifts[i].Not_Claim();
            else lsItemSuperGifts[i].Click_Claim();
            if (superGiftParam.lsRewardSuperGifts[i].coin > 0)
            {
                lsItemSuperGifts[i].txtReward.text = superGiftParam.lsRewardSuperGifts[i].coin.ToString();
                lsItemSuperGifts[i].imgReward.sprite = lsSpritesCoin[1];
                lsItemSuperGifts[i].imgReward.SetNativeSize();
                lsItemSuperGifts[i].imgReward.transform.localScale = new Vector3(2f, 2f, 0);
                lsItemSuperGifts[i].imgIcon.SetActive(true);
                lsItemSuperGifts[i].imgIcon.GetComponent<Image>().sprite = lsSpritesCoin[0];
            }
            else if (superGiftParam.lsRewardSuperGifts[i].gem > 0)
            {
                lsItemSuperGifts[i].txtReward.text = superGiftParam.lsRewardSuperGifts[i].gem.ToString();
                lsItemSuperGifts[i].imgReward.sprite = lsSpritesGem[1];
                lsItemSuperGifts[i].imgReward.SetNativeSize();
                lsItemSuperGifts[i].imgReward.transform.localScale = new Vector3(2f, 2f, 0);
                lsItemSuperGifts[i].imgIcon.SetActive(true);
                lsItemSuperGifts[i].imgIcon.GetComponent<Image>().sprite = lsSpritesGem[0];
            }
            else
            {
                lsItemSuperGifts[i].txtReward.text = superGiftParam.lsRewardSuperGifts[i].amountChest.ToString();
                lsItemSuperGifts[i].imgReward.sprite = lsSpritesChest[(int)superGiftParam.lsRewardSuperGifts[i].chestIndex];
                lsItemSuperGifts[i].imgReward.SetNativeSize();
                lsItemSuperGifts[i].imgReward.transform.localScale = new Vector3(1f, 1f, 0);
                lsItemSuperGifts[i].imgIcon.SetActive(false);
            }
        }
    }

    public void Onclick_Claim()
    {
        lsItemSuperGifts[indexReward].Click_Claim();
        
        if (superGiftParam.lsRewardSuperGifts[indexReward].coin > 0)
        {
            SaveManager.Instance.SaveGame.coin += superGiftParam.lsRewardSuperGifts[indexReward].coin;
            SaveManager.Instance.Save();
        }
        else if (superGiftParam.lsRewardSuperGifts[indexReward].gem > 0)
        {
            SaveManager.Instance.SaveGame.gem += superGiftParam.lsRewardSuperGifts[indexReward].gem;
            SaveManager.Instance.Save();
        }
        else
        {
            switch (superGiftParam.lsRewardSuperGifts[indexReward].chestIndex)
            {
                case ChestIndex.Epic:
                    SaveManager.Instance.SaveGame.chestEpic += superGiftParam.lsRewardSuperGifts[indexReward].amountChest;
                    break;
                case ChestIndex.Heroic:
                    SaveManager.Instance.SaveGame.chestHeroic += superGiftParam.lsRewardSuperGifts[indexReward].amountChest;
                    break;
                case ChestIndex.Legendary:
                    SaveManager.Instance.SaveGame.chestLegendary += superGiftParam.lsRewardSuperGifts[indexReward].amountChest;
                    break;
                default:
                    SaveManager.Instance.SaveGame.chestFree += superGiftParam.lsRewardSuperGifts[indexReward].amountChest;
                    break;
            }
            SaveManager.Instance.Save();
        }
        indexReward++;
        lsItemSuperGifts[indexReward].Claim();
    }
    public void OnClose()
    {
        if (indexReward > 2)
        {
            //Turn off button supergift
            uIMainMenu.btnSuperGift.SetActive(false);

        }
        UIManager.Instance.HideUI(this);
    }
}

public class SuperGiftParam : UIParam
{
    public List<RewardSuperGift> lsRewardSuperGifts = new List<RewardSuperGift>();
}
public class RewardSuperGift
{
    public int coin;
    public int gem;
    public ChestIndex chestIndex;
    public int amountChest;
}
