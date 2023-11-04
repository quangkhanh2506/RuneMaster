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

    private void OnEnable()
    {
        OnSetUp(new GoldMemberShipParam { gold = 5000, gem = 100, chestIndex = ChestIndex.Heroic, numberChest = 2 });
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        GoldMemberShipParam goldMemberShipParam = (GoldMemberShipParam)param;
        txtGold.text = goldMemberShipParam.gold.ToString();
        txtGem.text = goldMemberShipParam.gem.ToString();
        txtNumberchest.text = goldMemberShipParam.numberChest.ToString();
        imgChest.sprite = lsSpriteChests[(int)goldMemberShipParam.chestIndex];
        imgChest.SetNativeSize();
        
        

    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        //Save gold and gem, chest into savegame

        //Show UI chest

        UIManager.Instance.HideUI(this);

        
    }
}
