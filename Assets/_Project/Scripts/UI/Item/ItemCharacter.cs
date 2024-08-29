using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharacter : MonoBehaviour
{
    [SerializeField] public GameObject imgRim;
    [SerializeField] public GameObject btnBuy;
    [SerializeField] public GameObject GroupBtnUpgrade;

    public void SelectCharacterBought()
    {
        imgRim.SetActive(true);
        GroupBtnUpgrade.SetActive(true);
        btnBuy.SetActive(false);
    }

    public void SelectCharacterNotBuy()
    {
        imgRim.SetActive(true);
        GroupBtnUpgrade.SetActive(false);
        btnBuy.SetActive(true);
    }

    public void NoneSelect()
    {
        imgRim.SetActive(false);
        GroupBtnUpgrade.SetActive(false);
        btnBuy.SetActive(false);
    }
}
