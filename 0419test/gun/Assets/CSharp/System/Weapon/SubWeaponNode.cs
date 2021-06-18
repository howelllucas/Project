using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class SubWeaponNode : MonoBehaviour
    {
        private const int m_AskNum = 4;
        private List<WeaponUI_SubWeaponItemUI> m_ShowItemList = new List<WeaponUI_SubWeaponItemUI>();
        WeaponUI m_MainWeaponUi;
        public void Init(WeaponUI weaponUi)
        {
            m_MainWeaponUi = weaponUi;
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
            ShowScrollItem(ItemTypeConstVal.SUB_WEAPON, 1);
        }
        //刷新ShowScrollItem
        private void ShowScrollItem(int itemType, int showOrder)
        {
            ClearShowList();
            m_MainWeaponUi.SubWeaponItemUI.gameObject.SetActive(false);
            if (!Global.gApp.gGameData.ItemTypeMapData.ContainsKey(itemType))
            {
                return;
            }
            int curIdx = 0;
            int count = 0;
            
            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[itemType].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[itemType][i];
                if (Convert.ToUInt32(itemConfig.opencondition[0]) == FilterTypeConstVal.CAMP && !Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig))
                {
                    continue;
                }

                WeaponUI_SubWeaponItemUI itemUI = m_MainWeaponUi.SubWeaponItemUI.GetInstance();
                itemUI.Init(itemConfig, showOrder, this);
                m_ShowItemList.Add(itemUI);
                if (itemConfig.name.Equals(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon()))
                {
                    curIdx = i;
                }
                count = i;
            }

            for (int i = 0; i < m_AskNum; i++)
            {
                WeaponUI_SubWeaponItemUI itemUI = m_MainWeaponUi.SubWeaponItemUI.GetInstance();
                m_ShowItemList.Add(itemUI);
                itemUI.transform.SetSiblingIndex(1 * (showOrder * 10000 + 10000));
                itemUI.gameObject.SetActive(true);
                itemUI.Item.gameObject.SetActive(false);
                itemUI.AskIcon.gameObject.SetActive(true);
            }
            m_MainWeaponUi.SubWeaponContent.rectTransform.localPosition = new Vector3(m_MainWeaponUi.SubWeaponContent.rectTransform.localPosition.x, m_MainWeaponUi.SubWeaponContent.rectTransform.localPosition.y - 1, m_MainWeaponUi.SubWeaponContent.rectTransform.localPosition.z);
        }
        private void ClearShowList()
        {
            foreach (WeaponUI_SubWeaponItemUI obj in m_ShowItemList)
            {
                obj.Recycle();
                m_MainWeaponUi.SubWeaponItemUI.CacheInstance(obj);
            }
            m_ShowItemList.Clear();

        }

        //装备武器操作
        public void Equip(string subWeaponName)
        {
            foreach (WeaponUI_SubWeaponItemUI obj in m_ShowItemList)
            {
                if (obj.GetWeaponName() != null && obj.GetWeaponName().Equals(subWeaponName))
                {
                    obj.SubEquip.gameObject.SetActive(true);
                    obj.Mask.gameObject.SetActive(true);
                    Global.gApp.gSystemMgr.GetWeaponMgr().SetCurSubWeapon(subWeaponName);
                    ////新获得的武器设置为拥有
                    //if (Global.gApp.gSystemMgr.GetSubWeaponMgr().GetSubWeaponOpenState(subWeaponName) == WeaponStateConstVal.NEW)
                    //{
                    //    Global.gApp.gSystemMgr.GetSubWeaponMgr().SetSubWeaponOpenState(subWeaponName, WeaponStateConstVal.EXIST);
                    //}
                }
                else
                {
                    obj.Mask.gameObject.SetActive(false);
                    obj.SubEquip.gameObject.SetActive(false);
                }
            }
        }
        public void UIFresh()
        {
            foreach (WeaponUI_SubWeaponItemUI itemUI in m_ShowItemList)
            {
                itemUI.UIFresh();
            }
        }

        public bool IsHave()
        {
            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[ItemTypeConstVal.SUB_WEAPON].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[ItemTypeConstVal.SUB_WEAPON][i];
                if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig))
                {
                    return true;
                }
            }
            return false;
        }

        public RectTransform GetViewPoint2()
        {
            return m_MainWeaponUi.Viewport2.rectTransform;
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

