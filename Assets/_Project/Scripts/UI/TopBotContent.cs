using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopBotContent : SingletonMono<TopBotContent>
{
    [SerializeField] private GameObject Topbar;

    [SerializeField] private List<Sprite> lsAvatar = new List<Sprite>();
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private TextMeshProUGUI txtGem;


    [SerializeField] private GameObject Botbar;

    private void OnEnable()
    {
        Setup();
        Debug.Log(SaveManager.Instance.SaveGame.curAvatarIndex);
    }

    public void Setup()
    {
        imgAvatar.sprite = lsAvatar[SaveManager.Instance.SaveGame.curAvatarIndex];
        txtCoin.text = SaveManager.Instance.SaveGame.coin.ToString();
        txtGem.text = SaveManager.Instance.SaveGame.gem.ToString();
    }

    public void UpdateCurrence()
    {
        txtCoin.text = SaveManager.Instance.SaveGame.coin.ToString();
        txtGem.text = SaveManager.Instance.SaveGame.gem.ToString();
    }

    public void UpdateAvatar()
    {
        imgAvatar.sprite = lsAvatar[SaveManager.Instance.SaveGame.curAvatarIndex];
    }


    public void HideTopbar()
    {
        Topbar.SetActive(false);
    }

    public void HideCurrence()
    {
        Topbar.transform.GetChild(1).gameObject.SetActive(false);
        Topbar.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void HideAvatar()
    {
        Topbar.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void hideBotBar()
    {
        Botbar.SetActive(false);
    }

    
}
