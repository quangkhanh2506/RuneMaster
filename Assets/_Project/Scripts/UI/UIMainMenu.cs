using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIMainMenu : BaseUI
{
    public GameObject btnDailyGift;
    public GameObject btnSuperGift;
    public GameObject btnGoldMemberShip;
    [SerializeField] public GameObject BotMode;
    [SerializeField] public GameObject SelectCharacter;
    [SerializeField] public GameObject BotTournamet;
    [SerializeField] public GameObject Heart;
    [SerializeField] public GameObject TopBotCanvas;
    [SerializeField] public HorizontalScrollSnap ScrollSnapCharacter;
    [SerializeField] public List<ItemCharacter> lsItemCharacters = new List<ItemCharacter>();
    [SerializeField] public List<Sprite> LsSpritesCharacters = new List<Sprite>();
    [SerializeField] public Image imgCharacter;

    private int idCharacter;


    private void OnEnable()
    {
        OnSetUp();
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);

        UIDailyGift uIDailyGift = (UIDailyGift)UIManager.Instance.FindUI(UIIndex.UIDailyGift);
        BotMode.SetActive(true);
        SelectCharacter.SetActive(false);
        BotTournamet.SetActive(false);
        Heart.SetActive(false);
        TopBotCanvas.SetActive(true);
        btnDailyGift.SetActive(uIDailyGift.CheckClaimed());
        idCharacter = (int)SaveManager.Instance.SaveGame.curHeroIndex;
        imgCharacter.sprite = LsSpritesCharacters[idCharacter];
        imgCharacter.SetNativeSize();
    }

    public void On_ClickDailyGift()
    {
        TopBotCanvas.SetActive(false);
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
        //TopBotCanvas.SetActive(false);
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

    public void On_ClickMatch()
    {
        BotMode.SetActive(false);
        SelectCharacter.SetActive(true);
        BotTournamet.SetActive(false);
        Heart.SetActive(true);

        ScrollSnapCharacter.GoToScreen(idCharacter);

        lsItemCharacters[idCharacter].SelectCharacterBought();
        imgCharacter.sprite = LsSpritesCharacters[idCharacter];
        imgCharacter.SetNativeSize();

        
    }

    public void On_ClickTournament()
    {
        BotMode.SetActive(false);
        BotTournamet.SetActive(true);
    }

    public void playGame()
    {

        TopBotCanvas.SetActive(false);
        UIManager.Instance.ShowUI(UIIndex.UIVS);
        UIManager.Instance.HideUI(this);
    }

    public void On_ClickBackAtSelectHero()
    {

        BotMode.SetActive(true);
        SelectCharacter.SetActive(false);
        Heart.SetActive(false);
    }

    public void On_ClickBackAtTournamet()
    {
        BotMode.SetActive(true);
        BotTournamet.SetActive(false);
    }

    public void On_clickSelectCharacter()
    {
        lsItemCharacters[idCharacter].NoneSelect();
        idCharacter = ScrollSnapCharacter.CurrentPage;
        if (SaveManager.Instance.SaveGame.lsIDHeroPurchased.Contains((HeroIndex)idCharacter))
        {
            lsItemCharacters[idCharacter].SelectCharacterBought();
            imgCharacter.sprite = LsSpritesCharacters[idCharacter];
            imgCharacter.SetNativeSize();
            SaveManager.Instance.SaveGame.curHeroIndex = (HeroIndex)idCharacter;
        }
        else
        {
            lsItemCharacters[idCharacter].SelectCharacterNotBuy();
        }
    }

    public void BuyCharacter()
    {
        SaveManager.Instance.SaveGame.curHeroIndex = (HeroIndex)idCharacter;
        SaveManager.Instance.SaveGame.lsIDHeroPurchased.Add((HeroIndex)idCharacter);
        lsItemCharacters[idCharacter].SelectCharacterBought();
        imgCharacter.sprite = LsSpritesCharacters[idCharacter];
        imgCharacter.SetNativeSize();
    }

}
