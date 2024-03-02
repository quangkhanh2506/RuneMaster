using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAvatar : MonoBehaviour
{
    public Image imgAvatar;
    public GameObject imgRim;
    public GameObject imgSelect;
    public GameObject imgObscure;

    public bool islock = false;

    public void Setup(Sprite sprite = null)
    {
        if (sprite != null)
        {
            imgAvatar.sprite = sprite;
        }
        
        imgRim.SetActive(true);
        imgSelect.SetActive(false);
        if (islock)
        {
            imgObscure.SetActive(true);
        }
        else
        {
            imgObscure.SetActive(false);
        }

    }
    public void Select()
    {
        if (!islock)
        {
            imgRim.SetActive(false);
        }
        imgSelect.SetActive(true);
    }
}
