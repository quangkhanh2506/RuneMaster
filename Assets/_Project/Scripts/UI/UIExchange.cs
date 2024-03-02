using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Core;

using UnityEngine.UI;


public class ExchangeParam : UIParam
{
    public int multiplierCoin;
    public int multiplierGem;
}
public class UIExchange : BaseUI
{
    [SerializeField] private TextMeshProUGUI txtGemChange;
    [SerializeField] private TextMeshProUGUI txtCoinChange;

    [SerializeField] private TMP_InputField txtInputPrice;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Button btnMinus;
    [SerializeField] private Button btnExchange;


    private int value;
    private ExchangeParam exchangeParam;

    private void OnEnable()
    {
        OnSetUp(new ExchangeParam { multiplierCoin = 50, multiplierGem = 1 });
        
    }

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        value = 1;
        exchangeParam = (ExchangeParam)param;
        ChangeValue();
        CheckbtnMinus();
        CheckbtnExchange();
    }

    private void CheckbtnMinus()
    {
        if (value < 2)
        {
            btnMinus.interactable = false;
        }
        else
        {
            btnMinus.interactable = true;
        }
    }

    private void CheckbtnExchange()
    {
        if(value * exchangeParam.multiplierGem > SaveManager.Instance.SaveGame.gem)
        {
            btnExchange.interactable = false;
        }
        else
        {
            btnExchange.interactable = true;
        }
    }

    public void OnInputValue()
    {
        
        value = int.Parse(txtInputPrice.text);
        if (value < 1)
        {
            value = 1;
            txtInputPrice.text = value.ToString();
        }
        CheckbtnMinus();
        ChangeValue();
        CheckbtnExchange();
    }

    private void ChangeValue()
    {
        txtGemChange.text = (value * exchangeParam.multiplierGem).ToString();
        txtCoinChange.text = (value * exchangeParam.multiplierCoin).ToString();
        txtPrice.text = value.ToString();
        CheckbtnMinus();
    }

    public void OnclickPlus()
    {
        value++;
        ChangeValue();
        btnMinus.interactable = true;
        CheckbtnExchange();
    }

    public void OnClickMinus()
    {
        if (value > 1)
        {
            btnMinus.interactable = true;
            value--;
            ChangeValue();
        }
        CheckbtnMinus();
    }

    public void OnClickExchange()
    {
        //save coin and gem
        SaveManager.Instance.SaveGame.gem -= value * exchangeParam.multiplierGem;
        SaveManager.Instance.SaveGame.coin += value * exchangeParam.multiplierCoin;
        SaveManager.Instance.Save();
        UIManager.Instance.HideUI(this);
    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.HideUI(this);
    }
}
