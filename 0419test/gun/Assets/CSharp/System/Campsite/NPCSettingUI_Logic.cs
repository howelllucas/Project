using EZ.Data;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class NPCSettingUI
    {
        public Toggle fightToggle;
        public Toggle lockToggle;
        public Button lvUpBtn;
        public Button lvUpMaxBtn;
        private int m_NpcIndex;
        private Action<int, GunCard_TableItem, bool, bool> m_Callback;

        private List<GunCard_TableItem> selectList;
        private int selectIndex;

        private RectTransform m_CanvasRect;
        private CampsitePointMgr pointData;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_OkBtn.button.onClick.AddListener(OnOkBtnClick);
            lvUpBtn.onClick.AddListener(OnLvUpBtnClick);
            lvUpMaxBtn.onClick.AddListener(OnLvUpMaxBtnClick);
            //InitWeaponDropDown();
            m_CanvasRect = GetComponentInParent<Canvas>().rectTransform();
        }

        private void InitWeaponDropDown(int gunType)
        {
            var weaponDropDown = m_DropdownTemp.gameObject.GetComponent<Dropdown>();
            selectList = new List<GunCard_TableItem>();
            selectList.Add(null);
            List<string> selectNameList = new List<string>();
            selectNameList.Add("None");

            var gunCards = PlayerDataMgr.singleton.GetCardsByType(gunType);
            for (int i = 0; i < gunCards.Count; i++)
            {
                GunCard_TableItem item = TableMgr.singleton.GunCardTable.GetItemByID(gunCards[i]);
                selectList.Add(item);
                selectNameList.Add(item.name);
            }

            weaponDropDown.AddOptions(selectNameList);
            weaponDropDown.onValueChanged.AddListener(OnDropdownChange);
        }

        public void Init(Transform adaptNode, int npcIndex, Action<int, GunCard_TableItem, bool, bool> callback)
        {
            m_NpcIndex = npcIndex;
            m_Callback = callback;
            pointData = CampsiteMgr.singleton.GetPointByIndex(npcIndex);
            GunCard_TableItem gunData = null;
            if (pointData.equipGunId > 0)
            {
                gunData = TableMgr.singleton.GunCardTable.GetItemByID(pointData.equipGunId);
            }
            InitWeaponDropDown(pointData.buildingRes.gunType);
            SetTestValues(gunData, false, false);
            m_TitleText.text.text = string.Format("npc{0}, lv{1}", npcIndex + 1, pointData.Lv);
            m_TaskAdaptNode.rectTransform.anchoredPosition = UiTools.WorldToRectPos(gameObject, adaptNode.position, m_CanvasRect);
        }

        public void SetTestValues(GunCard_TableItem gunData, bool isFight, bool isLock)
        {
            var weaponDropDown = m_DropdownTemp.gameObject.GetComponent<Dropdown>();
            int selectIndex = selectList.IndexOf(gunData);
            if (selectIndex >= 0)
            {
                weaponDropDown.value = selectIndex;
            }
            fightToggle.isOn = isFight;
            lockToggle.isOn = isLock;
        }

        private void OnDropdownChange(int index)
        {
            selectIndex = index;
        }

        private void OnOkBtnClick()
        {
            GunCard_TableItem selectItem = null;
            if (selectList != null && selectIndex >= 0 && selectIndex < selectList.Count)
            {
                selectItem = selectList[selectIndex];
            }
            if (pointData != null)
            {
                if (selectItem == null)
                    CampsiteMgr.singleton.PointRemoveGun(pointData.index);
                else
                    CampsiteMgr.singleton.PointEquipGun(pointData.index, selectItem.id);
                //pointData.ClaimReward();
                //pointData.SetTestAuto(fightToggle.isOn);
            }
            TouchClose();
        }

        private void OnLvUpBtnClick()
        {
            var npcIndex = pointData.index;
            CampsiteMgr.singleton.LvUpPoint(pointData.index, 1);
            m_TitleText.text.text = string.Format("npc{0}, lv{1}", npcIndex + 1, pointData.Lv);
        }

        private void OnLvUpMaxBtnClick()
        {
            var lv = CampsiteMgr.singleton.GetPointMaxLvUpVal(pointData.index);

            var npcIndex = pointData.index;
            CampsiteMgr.singleton.LvUpPoint(pointData.index, lv);
            m_TitleText.text.text = string.Format("npc{0}, lv{1}", npcIndex + 1, pointData.Lv);
        }
    }
}