
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class WeaponRaiseUI
    {
        private WeaponRaiseNode m_WeaponNode;
        private MatExchangeNode m_ExchangeNode;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            CommonUI commonui = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            if(commonui != null)
            {
                commonui.SetOrderLayer(30);
            }
            OnExChangeMat();
            UIFresh();
            base.ChangeLanguage();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            RegisterListeners();

            m_WeaponNode = GetComponentInChildren<WeaponRaiseNode>();
            m_WeaponNode.Init(this);

            m_ExchangeNode = GetComponentInChildren<MatExchangeNode>();
            m_ExchangeNode.Init(this);
            InitNode();
            
        }
        private void InitNode()
        {
            //WeaponTab.button.onClick.AddListener(OnMainWeapon);
            //MatExchangeTab.button.onClick.AddListener(OnExChangeMat);
            CloseBtn.button.onClick.AddListener(TouchClose);
        }
        
        private void OnMainWeapon()
        {
            if (m_weapon_img_active.gameObject.activeSelf)
            {
                return;
            }
            SetAllNodeVisible(false);
            m_WeaponNode.ShowItems();
            m_ExchangeNode.DisableItems();
            m_weapon_img_active.gameObject.SetActive(true);
            m_matexchange_img_active.gameObject.SetActive(false);
            UIFresh();
        }
        private void OnExChangeMat()
        {
            if (m_matexchange_img_active.gameObject.activeSelf)
            {
                return;
            }
            SetAllNodeVisible(false);
            m_WeaponNode.DisableItems();
            m_ExchangeNode.ShowItems();
            m_weapon_img_active.gameObject.SetActive(false);
            m_matexchange_img_active.gameObject.SetActive(true);
            UIFresh();
        }
        private void SetAllNodeVisible(bool enable)
        {
            m_WeaponNode.gameObject.SetActive(enable);
        }
        public RectTransform GetViewPoint2()
        {
            return Viewport2.rectTransform;
        }
        public void UIFresh()
        {
        }
        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
        }
        public override void Release()
        {
            CommonUI commonui = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            if (commonui != null)
            {
                commonui.ResetOrderLayer();
            }
            UnRegisterListeners();
            if (m_WeaponNode != null)
            {
                m_WeaponNode.Destroy();
            }
            if(m_ExchangeNode != null)
            {
                m_ExchangeNode.Destroy();
            }
            base.Release();
        }
    }
}
