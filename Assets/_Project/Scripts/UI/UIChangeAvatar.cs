using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

public class UIChangeAvatar : BaseUI
{
    [SerializeField] private HorizontalScrollSnap horizontalScrollSnap;
    [SerializeField] private List<Sprite> lsSprite = new List<Sprite>();
    [SerializeField] private List<ItemAvatar> lsItemAvatars = new List<ItemAvatar>();
    [SerializeField] private TextMeshProUGUI txtUnLock;
    [SerializeField] private GameObject btnUnlock;
    [SerializeField] private Button btnSelect;
    private int idAvatar;
    

    private void OnEnable()
    {
        OnSetUp();
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        btnUnlock.SetActive(false);
        idAvatar = SaveManager.Instance.SaveGame.curAvatarIndex;
        for (int i = lsSprite.Count / 2; i < lsSprite.Count; i++)
        {
            lsItemAvatars[i].islock = true;
        }
        for (int i = 0; i < lsSprite.Count; i++)
        {
            lsItemAvatars[i].Setup(lsSprite[i]);
        }

        horizontalScrollSnap.GoToScreen(idAvatar);
        lsItemAvatars[idAvatar].Select();
        
    }

    public void On_Select()
    {
        lsItemAvatars[idAvatar].Setup();
        idAvatar = horizontalScrollSnap.CurrentPage;
        lsItemAvatars[idAvatar].Select();
        if (lsItemAvatars[idAvatar].islock)
        {
            btnUnlock.SetActive(true);
            btnSelect.interactable = false;
            txtUnLock.text = "Need to unlock " + (HeroIndex)(idAvatar-1);
        }
        else
        {
            btnSelect.interactable = true;
            btnUnlock.SetActive(false);
        }
    }


    public void On_SelectAvatar()
    {
        SaveManager.Instance.SaveGame.curAvatarIndex = idAvatar;
        //Change avatar in topbar

        UIManager.Instance.HideUI(this);
    }
    public void On_UnlockAvatar()
    {
        //switch to UI UNlock

        UIManager.Instance.HideUI(this);
    }

}
