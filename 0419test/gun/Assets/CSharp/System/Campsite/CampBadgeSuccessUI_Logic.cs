using EZ.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class CampBadgeSuccessUI
    {
        private CampNewGuidBase m_CampNewGuidBase;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_CampNewGuidBase = arg as CampNewGuidBase;
            InitNode();
            base.ChangeLanguage();
        }

        private void InitNode()
        {
            CampStepItem campStepItem = m_CampNewGuidBase.GetCampStepItem();
            CampBadgeItem campBadgeItem = Global.gApp.gGameData.CampBadgeConfig.Get(campStepItem.brageId);
            m_CloseBtn.button.onClick.AddListener(TouchClose);
            m_BrageDes.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(campBadgeItem.title);
            //m_titletxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(campBadgeItem.title);
            m_BrageIcon.image.sprite = Resources.Load(campBadgeItem.activeIcon, typeof(Sprite)) as Sprite;
        }

        public override void TouchClose()
        {
            m_CampNewGuidBase.BrageClose();
            base.TouchClose();
        }
    }
}
