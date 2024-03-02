using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;

public class UIMainMenu : BaseUI
{
    public GameObject btnDailyGift;
    public GameObject btnSuperGift;
    public GameObject btnGoldMemberShip;

    private void OnEnable()
    {
        OnSetUp();
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);

        UIDailyGift uIDailyGift = (UIDailyGift)UIManager.Instance.FindUI(UIIndex.UIDailyGift);
        btnDailyGift.SetActive(uIDailyGift.CheckClaimed());
    }

    public void On_ClickDailyGift()
    {
        UIManager.Instance.ShowUI(UIIndex.UIDailyGift, new DailygiftParam
        {
            lsDailygiftItemDatas = new List<DailygiftItemData>() { new DailygiftItemData { coinValue = 200 },
            new DailygiftItemData{gemValue = 10 },
            new DailygiftItemData{ChestIndex= ChestIndex.Heroic, ChestValue=2},
            new DailygiftItemData{coinValue =200},
            new DailygiftItemData {gemValue=50},
            new DailygiftItemData {ChestIndex= ChestIndex.Legendary, ChestValue=1},
            new DailygiftItemData {coinValue=100}
        }
        });
        UIManager.Instance.HideUI(this);
    }

    public void On_ClickSuperGift()
    {
        UIManager.Instance.ShowUI(UIIndex.UISuperGifts, new SuperGiftParam
        {
            lsRewardSuperGifts = new List<RewardSuperGift>() { new RewardSuperGift { coin = 200 },
            new RewardSuperGift{gem = 10 },
            new RewardSuperGift{chestIndex= ChestIndex.Heroic, amountChest=2},
        }
        });
    }

    public void On_ClickGoldMemberShip()
    {
        UIManager.Instance.ShowUI(UIIndex.UIGoldMemBerShip, new GoldMemberShipParam { gold = 5000, gem = 100, chestIndex = ChestIndex.Heroic, numberChest = 2 });
    }
}
