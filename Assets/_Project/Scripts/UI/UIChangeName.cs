using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using TMPro;

public class UIChangeName : BaseUI
{
    [SerializeField] private TMP_InputField txtInputName;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        txtInputName.placeholder.GetComponent<TextMeshProUGUI>().text = SaveManager.Instance.SaveGame.name;
    }
    public void On_ConfirmName()
    {
        SaveManager.Instance.SaveGame.name = txtInputName.text;
        SaveManager.Instance.Save();
        Debug.Log(SaveManager.Instance.SaveGame.name);
        UIManager.Instance.HideUI(this);
    }
}
