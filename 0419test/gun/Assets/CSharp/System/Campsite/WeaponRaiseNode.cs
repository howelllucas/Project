using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class WeaponItemContainer: IComparable<WeaponItemContainer>
    {
        ItemItem m_Item;
        public WeaponItemContainer(ItemItem item)
        {
            m_Item = item;
        }
        public ItemItem GetItemItem()
        {
            return m_Item;
        }
        public int CompareTo(WeaponItemContainer other)
        {
            if(other == null)
            {
                return -1;
            }
            ItemItem otherItem = other.GetItemItem();
            int qualityX = Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_Item);
            int qualityY = Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(otherItem);
            if (qualityX < qualityY)
            {
                return -1;
            }
            else if (qualityX > qualityY)
            {
                return 1;
            }
            else
            {
                if (m_Item.showorder < otherItem.showorder)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    public class WeaponRaiseNode : MonoBehaviour {
        private List<WeaponRaiseUI_WeaponItemUI> m_ShowItemList = new List<WeaponRaiseUI_WeaponItemUI>();
        private List<WeaponItemContainer> m_SortWeaponList = new List<WeaponItemContainer>();
        WeaponRaiseUI m_WeaponRaiseUi;
        public void Init(WeaponRaiseUI weaponUi)
        {
            m_WeaponRaiseUi = weaponUi;
            GenerateShowWeapon();
            RegisterListeners();
        }
        private void GenerateShowWeapon()
        {
            if(m_SortWeaponList.Count > 0)
            {
                return;
            }
            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[ItemTypeConstVal.BASE_MAIN_WEAPON].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[ItemTypeConstVal.BASE_MAIN_WEAPON][i];
                if (itemConfig.supercondition.Length > 1 && Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig.name) > WeaponStateConstVal.NONE)
                {
                    m_SortWeaponList.Add(new WeaponItemContainer(itemConfig));
                }
            }
            m_SortWeaponList.Sort();
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
            ShowScrollItem(ItemTypeConstVal.BASE_MAIN_WEAPON, 1);
        }
        //刷新ShowScrollItem
        private void ShowScrollItem(int itemType, int showOrder)
        {
            ClearShowList();
            if (!Global.gApp.gGameData.ItemTypeMapData.ContainsKey(itemType))
            {
                return;
            }
            m_WeaponRaiseUi.WeaponItemUI.gameObject.SetActive(false);
            foreach(WeaponItemContainer itemConfigContainer in m_SortWeaponList)
            {
                WeaponRaiseUI_WeaponItemUI itemUI = m_WeaponRaiseUi.WeaponItemUI.GetInstance();
                itemUI.Init(itemConfigContainer.GetItemItem(), showOrder, this);
                m_ShowItemList.Add(itemUI);
            }
        }
        private void ClearShowList()
        {
            foreach (WeaponRaiseUI_WeaponItemUI obj in m_ShowItemList)
            {
                obj.Recycle();
                m_WeaponRaiseUi.WeaponItemUI.CacheInstance(obj);
            }
            m_ShowItemList.Clear();

        }
        public void UIFresh()
        {
            foreach (WeaponRaiseUI_WeaponItemUI itemUI in m_ShowItemList)
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
