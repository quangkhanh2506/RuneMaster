using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core
{

    public class BaseUI : MonoBehaviour
    {
        public UIIndex uiIndex;
        RectTransform rectTrans;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
        }

        public void ShowUI(UIParam param = null, Action callback = null)
        {
            gameObject.SetActive(true);
            rectTrans.SetAsLastSibling();
            OnSetUp(param);

            OnShow();
            if (callback != null)
                callback();

        }

        public virtual void OnInit()
        {

        }

        public virtual void OnSetUp(UIParam param = null)
        {

        }

        public virtual void OnShow(UIParam param = null)
        {

        }

        public void HideUI(Action callback = null)
        {
            OnHide();
            gameObject.SetActive(false);
            if (callback != null)
                callback();
        }

        public virtual void OnHide()
        {

        }

        public virtual void OnCloseClick() 
        {
            SoundManager.Instance.PlaySoundFX(SoundIndex.Click);
            UIManager.Instance.HideUI(this);
        }
    }
}