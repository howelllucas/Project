using EZ.Data;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class MatExchangeNode : MonoBehaviour
    {
        private List<WeaponRaiseUI_ExchangeMatItemUI> m_ShowItemList = new List<WeaponRaiseUI_ExchangeMatItemUI>();
        WeaponRaiseUI m_WeaponRaiseUi;
        public void Init(WeaponRaiseUI weaponUi)
        {
            m_WeaponRaiseUi = weaponUi;
            RegisterListeners();
        }
        public void DisableItems()
        {
            gameObject.SetActive(false);
            ClearShowList();
        }
        public void ShowItems()
        {
            gameObject.SetActive(true);
            ClearShowList();
            ShowScrollItem();
        }
        //刷新ShowScrollItem
        private void ShowScrollItem()
        {
            ClearShowList();
            m_WeaponRaiseUi.ExchangeMatItemUI.gameObject.SetActive(false);
            CampShopItem[] shopItems = Global.gApp.gGameData.CampShopConfig.items;
            foreach(CampShopItem shopItem in shopItems)
            {
                WeaponRaiseUI_ExchangeMatItemUI itemUI = m_WeaponRaiseUi.ExchangeMatItemUI.GetInstance();
                itemUI.transform.SetSiblingIndex(shopItem.heartNum);
                itemUI.Init(shopItem, this);
                m_ShowItemList.Add(itemUI);
            }
        }
        public void FresAllItem()
        {
            foreach(WeaponRaiseUI_ExchangeMatItemUI weaponRaiseUI_ExchangeMatItemUI in m_ShowItemList)
            {
                weaponRaiseUI_ExchangeMatItemUI.UIFresh();
            }
        }
        private void ClearShowList()
        {
            foreach (WeaponRaiseUI_ExchangeMatItemUI obj in m_ShowItemList)
            {
                obj.Recycle();
                m_WeaponRaiseUi.ExchangeMatItemUI.CacheInstance(obj);
            }
            m_ShowItemList.Clear();
        }
        public void UIFresh()
        {
            foreach (WeaponRaiseUI_ExchangeMatItemUI itemUI in m_ShowItemList)
            {
                itemUI.UIFresh();
            }
        }
        public RectTransform GetViewPoint2()
        {
            return m_WeaponRaiseUi.Viewport2.rectTransform;
        }
        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
        }
        public void Destroy()
        {
            UnRegisterListeners();
        }
    }
}

