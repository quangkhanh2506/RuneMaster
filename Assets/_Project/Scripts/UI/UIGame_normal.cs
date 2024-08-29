using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class UIGame_normal : BaseUI
{
    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        Grid.Instance.SetupGameplay();
    }

    public void Click_PauseGame()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowUI(UIIndex.UIPause);
    }
}
