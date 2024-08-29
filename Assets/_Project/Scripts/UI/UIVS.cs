using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class UIVS : BaseUI
{
    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        StartCoroutine(ShowUIGameplay());
    }

    IEnumerator ShowUIGameplay()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideAllUI();
        UIManager.Instance.ShowUI(UIIndex.UIGame_normal);
    }
}
