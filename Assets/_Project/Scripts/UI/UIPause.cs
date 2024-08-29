using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class UIPause : BaseUI
{
    public void Click_CloseUI()
    {
        Time.timeScale = 1;
        UIManager.Instance.HideUI(this);
    }
    public void Click_GiveUp()
    {
        Time.timeScale = 1;
        UIManager.Instance.HideAllUI();
        UIManager.Instance.ShowUI(UIIndex.UIMain);
    }
    public void CLick_Restart()
    {
        Time.timeScale = 1;
        Grid.Instance.ResetMatch();
        UIManager.Instance.HideUI(this);
    }
}
