using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Core
{
    public class UIManager : SingletonMono<UIManager>
    {
        private Dictionary<UIIndex, BaseUI> dicUI = new Dictionary<UIIndex, BaseUI>();
        public UIIndex currentUIIndex;
        public List<BaseUI> lsShow = new List<BaseUI>();
        public List<BaseUI> lsUI = new List<BaseUI>();
        public void Init(Action callback)
        {
            for (int i = 0; i < lsUI.Count; i++)
            {
                GameObject goDialog = lsUI[i].gameObject;
                goDialog.transform.SetParent(transform, false);

                RectTransform rectTrans = goDialog.GetComponent<RectTransform>();
                rectTrans.offsetMax = rectTrans.offsetMin = Vector2.zero;

                BaseUI baseUI = goDialog.GetComponent<BaseUI>();
                baseUI.OnInit();

                dicUI.Add(baseUI.uiIndex, baseUI);

                goDialog.SetActive(false);
            }

            if (callback != null)
            {
                callback();
            }
        }

        public void ShowUI(UIIndex uiIndex, UIParam param = null, Action callback = null)
        {
            BaseUI dialog = dicUI[uiIndex];
            if (!lsShow.Contains(dialog))
            {
                dialog.ShowUI(param, callback);
                lsShow.Add(dialog);
            }
           
        }

        public void HideUI(BaseUI ui, Action callback = null)
        {
            ui.HideUI(callback);
            lsShow.Remove(ui);
        }

        public void HideUI(UIIndex uiIndex, Action callback = null)
        {
            BaseUI ui = FindUI(uiIndex);
            if (ui != null)
            {
                ui.HideUI(callback);
                lsShow.Remove(ui);
            }
        }

        public void HideAllUI(Action callback = null)
        {
            for (int i = 0; i < lsShow.Count - 1; i++)
            {
                lsShow[i].HideUI(null);

            }
            if (lsShow.Count > 0)
            {
                lsShow[lsShow.Count - 1].HideUI(callback);
            }
            
            lsShow.Clear();
        }

        public BaseUI FindUIVisible(UIIndex uiIndex)
        {
            for (int i = 0; i < lsShow.Count; i++)
            {
                if (lsShow[i].uiIndex == uiIndex)
                    return lsShow[i];
            }
            return null;
        }

        public BaseUI FindUI(UIIndex uiIndex)
        {
            return dicUI[uiIndex];
        }
    }

    public class UIParam
    {

    }

    public class ResultParam : UIParam
    {
        public bool isWin;
        public int valueCoinResult;
        public int valueGemResult;
    }

    public class NoInternetParam : UIParam
    {
        public bool isNoInternet;
    }

    public class MissionParam : UIParam
    {
        public bool isBackToMenu;
        public bool isAnimation;
    }

    public class GiftParam : UIParam
    {
        public int valueCoinReward;
        public int valueGemReward;
        public bool isOfflineEarning;
    }
    
    public enum UIIndex
    {
        UIGameplay,
        UILoading,
        UIResult,
        UIMission,
        UIMain,
        UIUpgrade,
        UIPrivacy,
        UIGift,
        UIBooster,
        UIPause,
        UIRate,
        UINoInternet,
        UILeaderBoard,
        UISetting,
        COUNT,
    }
}