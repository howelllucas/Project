
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class WeaponUI
    {
        private WeaponNode m_WeaponNode;
        private SubWeaponNode m_SubWeaponNode;
        private PetNode m_PetNode;

        private int[] m_OpenLevels;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            OnMainWeapon();
            weapon_lock.gameObject.SetActive(Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[0]);
            //subweapon_lock.gameObject.SetActive(Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[1] && !m_SubWeaponNode.IsHave());
            //pet_lock.gameObject.SetActive(Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[2] && !m_PetNode.IsHave());

            weapon_txt.gameObject.SetActive(!weapon_lock.gameObject.activeSelf);
            //subweapon_txt.gameObject.SetActive(!subweapon_lock.gameObject.activeSelf);
            //pet_txt.gameObject.SetActive(!pet_lock.gameObject.activeSelf);

            UIFresh();

            NewbieGuideButton[] newBieButtons = m_Tabs.gameObject.GetComponentsInChildren<NewbieGuideButton>();
            foreach (NewbieGuideButton newBieButton in newBieButtons)
            {
                newBieButton.OnStart();
            }

            base.ChangeLanguage();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            RegisterListeners();
            m_WeaponNode = GetComponentInChildren<WeaponNode>();
            m_WeaponNode.Init(this);

            //m_SubWeaponNode = GetComponentInChildren<SubWeaponNode>();
            //m_SubWeaponNode.Init(this);

            //m_PetNode = GetComponentInChildren<PetNode>();
            //m_PetNode.Init(this);

            GeneralConfigItem gci = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.WEAPON_OPEN_PASS);
            m_OpenLevels = new int[gci.contents.Length];
            for (int i = 0; i < gci.contents.Length; i ++)
            {
                m_OpenLevels[i] = int.Parse(gci.contents[i]);
            }

            InitNode();
            
        }
        private void InitNode()
        {
            WeaponTab.button.onClick.AddListener(OnMainWeapon);
            //SubWeaponTab.button.onClick.AddListener(OnSubWeapon);
            //PetTab.button.onClick.AddListener(OnPet);
        }
        
        

        private void OnMainWeapon()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_MAIN);
            if (Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[0])
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByID, 1004);
                return;
            }
            
            SetAllNodeVisible(false);
            m_WeaponNode.ShowItems();
            //m_SubWeaponNode.DisableItems();
            //m_PetNode.DisableItems();
            m_weapon_img_active.gameObject.SetActive(true);
            //subweapon_img_active.gameObject.SetActive(false);
            //pet_img_active.gameObject.SetActive(false);

            UIFresh();
        }
        private void OnSubWeapon()
        {
			//InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_SUB);
            if (Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[1] && !m_SubWeaponNode.IsHave())
            {
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3022, m_OpenLevels[1].ToString());
                return;
            }
            if (subweapon_img_active.gameObject.activeSelf)
            {
                return;
            }
            SetAllNodeVisible(false);
            m_WeaponNode.DisableItems();
            m_SubWeaponNode.ShowItems();
            m_PetNode.DisableItems();
            m_weapon_img_active.gameObject.SetActive(false);
            subweapon_img_active.gameObject.SetActive(true);
            pet_img_active.gameObject.SetActive(false);
            //Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3078);

            UIFresh();
        }
        private void OnPet()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_PET);
            if (Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() < m_OpenLevels[2] && !m_PetNode.IsHave())            {
                
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3022, m_OpenLevels[2].ToString());
                return;
            }
            if (pet_img_active.gameObject.activeSelf)
            {
                return;
            }
            SetAllNodeVisible(false);
            m_WeaponNode.DisableItems();
            m_SubWeaponNode.DisableItems();
            m_PetNode.ShowItems();
            m_weapon_img_active.gameObject.SetActive(false);
            subweapon_img_active.gameObject.SetActive(false);
            pet_img_active.gameObject.SetActive(true);
            //Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3078);

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
            bool haveNewMain = Global.gApp.gSystemMgr.GetWeaponMgr().HaveNewWeapon(ItemTypeConstVal.BASE_MAIN_WEAPON);
            bool haveNewSub = Global.gApp.gSystemMgr.GetWeaponMgr().HaveNewWeapon(ItemTypeConstVal.SUB_WEAPON);
            bool haveNewPet = Global.gApp.gSystemMgr.GetWeaponMgr().HaveNewWeapon(ItemTypeConstVal.PET);

            bool canUpdateMain = Global.gApp.gSystemMgr.GetWeaponMgr().CanUpdateWeapon(ItemTypeConstVal.BASE_MAIN_WEAPON);
            bool canUpdateSub = Global.gApp.gSystemMgr.GetWeaponMgr().CanUpdateWeapon(ItemTypeConstVal.SUB_WEAPON);
            bool canUpdatePet = Global.gApp.gSystemMgr.GetWeaponMgr().CanUpdateWeapon(ItemTypeConstVal.PET);

            NewWp.gameObject.SetActive(haveNewMain && !weapon_img_active.gameObject.activeSelf);
            NewSubWp.gameObject.SetActive(haveNewSub && !subweapon_img_active.gameObject.activeSelf);
            NewPet.gameObject.SetActive(haveNewPet && !pet_img_active.gameObject.activeSelf);

            TabWeaponUp.gameObject.SetActive(canUpdateMain);
            TabSubWeaponUp.gameObject.SetActive(canUpdateSub);
            TabPetUp.gameObject.SetActive(canUpdatePet);
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
            UnRegisterListeners();
            if (m_WeaponNode != null)
            {
                m_WeaponNode.Destroy();
            }
            if(m_SubWeaponNode != null)
            {
                m_SubWeaponNode.Destroy();
            }
            if(m_PetNode != null)
            {
                m_PetNode.Destroy();
            }
            base.Release();
        }
    }
}
