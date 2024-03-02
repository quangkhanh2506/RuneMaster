using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;
using UnityEngine.UI;

public class GoldMemberShipParam : UIParam
{
    public int gold;
    public int gem;
    public ChestIndex chestIndex;
    public int numberChest;
}

public class UIGoldMemberShipReward : BaseUI
{
    [SerializeField] public TextMeshProUGUI txtGold;
    [SerializeField] public TextMeshProUGUI txtGem;
    [SerializeField] public TextMeshProUGUI txtNumberchest;
    [SerializeField] public List<Sprite> lsSpriteChests=new List<Sprite>();
    [SerializeField] public Image imgChest;
    private GoldMemberShipParam goldMemberShipParam;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        goldMemberShipParam = (GoldMemberShipParam)param;
        txtGold.text = goldMemberShipParam.gold.ToString();
        txtGem.text = goldMemberShipParam.gem.ToString();
        txtNumberchest.text = goldMemberShipParam.numberChest.ToString();
        imgChest.sprite = lsSpriteChests[(int)goldMemberShipParam.chestIndex];
        imgChest.SetNativeSize();
        
    }

    public void OnClaim()
    {
        //Save gold and gem, chest into savegame
        SaveManager.Instance.SaveGame.coin += goldMemberShipParam.gold;
        SaveManager.Instance.SaveGame.gem += goldMemberShipParam.gem;
        switch (goldMemberShipParam.chestIndex)
        {
            case ChestIndex.Epic:
                SaveManager.Instance.SaveGame.chestEpic += goldMemberShipParam.numberChest;
                break;
            case ChestIndex.Heroic:
                SaveManager.Instance.SaveGame.chestHeroic += goldMemberShipParam.numberChest;
                break;
            case ChestIndex.Legendary:
                SaveManager.Instance.SaveGame.chestLegendary += goldMemberShipParam.numberChest;
                break;
            default:
                SaveManager.Instance.SaveGame.chestFree += goldMemberShipParam.numberChest;
                break;
        }
        //Show UI chest

        UIManager.Instance.HideUI(this);

        
    }
}
