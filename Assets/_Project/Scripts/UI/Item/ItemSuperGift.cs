using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSuperGift : MonoBehaviour
{
    [SerializeField] private Image imgCircleBG;
    public Image imgReward;
    public TextMeshProUGUI txtReward;
    public GameObject imgIcon;
    [SerializeField] private TextMeshProUGUI txtReceive;
    [SerializeField] private GameObject btnClaim;

    [SerializeField] private List<Sprite> lsSpritesCircleBG = new List<Sprite>();
    

    public void Click_Claim()
    {
        imgCircleBG.sprite = lsSpritesCircleBG[0];
        btnClaim.SetActive(false);
        txtReceive.text = "RECIEVED";
    }


    public void Not_Claim()
    {
        imgCircleBG.sprite = lsSpritesCircleBG[1];
        btnClaim.SetActive(false);
        txtReceive.text = "CLAIM";
    }

    public void Claim()
    {
        imgCircleBG.sprite = lsSpritesCircleBG[2];
        btnClaim.SetActive(true);
    }
}
