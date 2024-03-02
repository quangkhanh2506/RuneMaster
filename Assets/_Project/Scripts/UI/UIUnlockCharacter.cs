using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Core;

public class UnlockCharacterParam : UIParam
{
    public HeroIndex HeroIndex;
}

public class UIUnlockCharacter : BaseUI
{
    [SerializeField] public List<Sprite> lsSpriteCharacters = new List<Sprite>();
    [SerializeField] public Image imgCharacter;
    [SerializeField] public TextMeshProUGUI txtName;

    private void OnEnable()
    {
        OnSetUp(new UnlockCharacterParam { HeroIndex = HeroIndex.BigBoar });
    }
    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        UnlockCharacterParam unlockCharacterParam = (UnlockCharacterParam)param;
        //Change image Character
        imgCharacter.sprite = lsSpriteCharacters[(int)unlockCharacterParam.HeroIndex];
        imgCharacter.SetNativeSize();
        //Change name Character
        txtName.text = unlockCharacterParam.HeroIndex.ToString();

        //Save ID character

    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.HideUI(this);
    }
}


