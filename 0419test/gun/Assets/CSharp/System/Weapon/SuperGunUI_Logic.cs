
using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class SuperGunUI
    {
        private string m_CM_COLOR = "#FDE47EFF";

        ItemItem m_Item;
        WeaponRaiseUI_WeaponItemUI m_WeaponUI_WeaponItemUI;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            m_WeaponUI_WeaponItemUI = arg as WeaponRaiseUI_WeaponItemUI;
            m_Item = m_WeaponUI_WeaponItemUI.GetItemConfig();
            base.Init(name, info, arg);
            base.ChangeLanguage();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            RegisterListeners();
            InitNode();

        }
        private void InitNode()
        {
            m_CloseBtn.button.onClick.AddListener(TouchClose);
            m_Btn1.button.onClick.AddListener(TouchClose);
            m_Btn2.button.onClick.AddListener(UpdateQuality);
            m_fromGun.image.sprite = Resources.Load(m_Item.image_grow, typeof(Sprite)) as Sprite;
            m_toGun.image.sprite = Resources.Load(m_Item.image_grow + "_s", typeof(Sprite)) as Sprite;



            int itemId = Convert.ToInt32(m_Item.supercondition[1]);
            double unlockItemNum = Convert.ToDouble(m_Item.supercondition[2]);
            m_Cost1.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemId) < unlockItemNum, m_CM_COLOR);
            m_Cost1.text.text = unlockItemNum.ToString();

            double itemCount = GameItemFactory.GetInstance().GetItem(itemId);
            string iconPath = string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId);
            m_Icon1.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(iconPath);



            int itemId2 = Convert.ToInt32(m_Item.supercondition[3]);
            double unlockItemNum2 = Convert.ToDouble(m_Item.supercondition[4]);
            m_Cost2.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemId2) < unlockItemNum2, m_CM_COLOR);
            m_Cost2.text.text = unlockItemNum2.ToString();

            double itemCount2 = GameItemFactory.GetInstance().GetItem(itemId2);
            string iconPath2 = string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId2);
            m_Icon2.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(iconPath2);
        }
        private void UpdateQualityCall(int errorCode)
        {
            if(errorCode == 0)
            {
                m_WeaponUI_WeaponItemUI.UIFresh(true);
                Global.gApp.gUiMgr.OpenPanel(Wndid.WeaponRaiseSucess_UI, m_WeaponUI_WeaponItemUI.GetItemConfig());
                TouchClose();
            }
        }
        private void UpdateQuality()
        {
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_Item) == 0)
            {
                Global.gApp.gSystemMgr.GetWeaponMgr().UpdateQualityLv(m_Item, UpdateQualityCall);
            }
        }
        private void RegisterListeners()
        {

        }
        private void UnRegisterListeners()
        {

        }
        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
