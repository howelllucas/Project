using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class WeaponNode : MonoBehaviour {
        private List<WeaponUI_WeaponItemUI> m_ShowItemList = new List<WeaponUI_WeaponItemUI>();
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
            int curIdx = 0;
            int count = 0;
            m_MainWeaponUi.WeaponItemUI.gameObject.SetActive(false);
            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[itemType].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[itemType][i];
                if (Convert.ToUInt32(itemConfig.opencondition[0]) == FilterTypeConstVal.CAMP && !Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig))
                {
                    continue;
                }

                WeaponUI_WeaponItemUI itemUI = m_MainWeaponUi.WeaponItemUI.GetInstance();
                itemUI.Init(itemConfig, showOrder, this);
                m_ShowItemList.Add(itemUI);
                if (itemConfig.name.Equals(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon()))
                {
                    curIdx = i;
                }
                count = i;
            }

            if (curIdx > count - 3)
            {
                curIdx = count - 5;
            }
            else
            {
                curIdx = Mathf.Max(0, curIdx - 3);
            }
            m_MainWeaponUi.WeaponContent.rectTransform.localPosition = new UnityEngine.Vector3(m_MainWeaponUi.WeaponContent.rectTransform.localPosition.x, curIdx * 255, m_MainWeaponUi.WeaponContent.rectTransform.localPosition.z);
        }
        private void ClearShowList()
        {
            foreach (WeaponUI_WeaponItemUI obj in m_ShowItemList)
            {
                obj.Recycle();
                m_MainWeaponUi.WeaponItemUI.CacheInstance(obj);
            }
            m_ShowItemList.Clear();

        }

        //装备武器操作
        public void Equip(string weaponName)
        {
            foreach (WeaponUI_WeaponItemUI obj in m_ShowItemList)
            {
                if (obj.GetWeaponName() != null && obj.GetWeaponName().Equals(weaponName))
                {
                    obj.Equip.gameObject.SetActive(true);
                    obj.EquipBg.gameObject.SetActive(true);
                    Global.gApp.gSystemMgr.GetWeaponMgr().SetCurMainWeapon(weaponName);
                    //新获得的武器设置为拥有
                    if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(weaponName) == WeaponStateConstVal.NEW)
                    {
                        Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(weaponName, WeaponStateConstVal.EXIST);
                        obj.NewIMG.gameObject.SetActive(false);
                        obj.NewWeapon.gameObject.SetActive(false);
                    }
                }
                else
                {
                    obj.Equip.gameObject.SetActive(false);
                    obj.EquipBg.gameObject.SetActive(false);
                }
            }
        }
        public void UIFresh()
        {
            foreach (WeaponUI_WeaponItemUI itemUI in m_ShowItemList)
            {
                itemUI.UIFresh();
            }
        }
        public RectTransform GetViewPoint2()
        {
            return m_MainWeaponUi.Viewport2.rectTransform;
        }

        // 新手引导 待定
        private void ChangeScrollView4Guide()
        {
            int curIdx = 0;
            int curGuideId = Global.gApp.gSystemMgr.GetNewbieGuideMgr().GetCurGuideId();
            NewbieGuideItem config = Global.gApp.gGameData.NewbieGuideData.Get(curGuideId);
            if (config == null)
            {
                return;
            }

            for (int i = 0; i < m_ShowItemList.Count; i++)
            {
                if (m_ShowItemList[i].GetWeaponName() != null && !m_ShowItemList[i].GetWeaponName().Equals(GameConstVal.EmepyStr) && m_ShowItemList[i].GetWeaponName().Equals(config.param))
                {
                    curIdx = i;
                    break;
                }
            }
            m_MainWeaponUi.WeaponContent.rectTransform.localPosition = new UnityEngine.Vector3(m_MainWeaponUi.WeaponContent.rectTransform.localPosition.x, curIdx * 255, m_MainWeaponUi.WeaponContent.rectTransform.localPosition.z);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.ChangeScrollView4Guide, ChangeScrollView4Guide);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.ChangeScrollView4Guide, ChangeScrollView4Guide);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
        }
        public void Destroy()
        {
            UnRegisterListeners();
        }
    }
}
