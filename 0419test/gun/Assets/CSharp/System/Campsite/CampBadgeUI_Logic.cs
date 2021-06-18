using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{
    public partial class CampBadgeUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            InitNode();
            base.ChangeLanguage();
        }

        private void InitNode()
        {
            m_CloseBtn.button.onClick.AddListener(TouchClose);
            CampBadgeItem[] campBadges  = Global.gApp.gGameData.CampBadgeConfig.items;
            int index = 0;
            foreach (CampBadgeItem campBadgeItem in campBadges)
            {
                CampBadgeUI_BadgeItemUI campBadgeUI_BadgeItemUI = BadgeItemUI.GetInstance();
                campBadgeUI_BadgeItemUI.gameObject.SetActive(true);
                campBadgeUI_BadgeItemUI.Detail.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(campBadgeItem.detail);
                campBadgeUI_BadgeItemUI.Title.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(campBadgeItem.title);
                if (GameItemFactory.GetInstance().GetItem(campBadgeItem.id) > 0)
                {
                    campBadgeUI_BadgeItemUI.Detail.gameObject.SetActive(true);
                    campBadgeUI_BadgeItemUI.Title.gameObject.SetActive(true);
                    campBadgeUI_BadgeItemUI.NoActiveTip.gameObject.SetActive(false);
                    campBadgeUI_BadgeItemUI.Icon.image.sprite = Resources.Load(campBadgeItem.activeIcon, typeof(Sprite)) as Sprite;
                    campBadgeUI_BadgeItemUI.Icon.image.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    campBadgeUI_BadgeItemUI.Detail.gameObject.SetActive(false);
                    campBadgeUI_BadgeItemUI.Title.gameObject.SetActive(false);
                    campBadgeUI_BadgeItemUI.NoActiveTip.gameObject.SetActive(true);
                    campBadgeUI_BadgeItemUI.Icon.image.sprite = Resources.Load(campBadgeItem.noActiveIcon, typeof(Sprite)) as Sprite;
                    campBadgeUI_BadgeItemUI.Icon.image.color = new Color(1, 1, 1, 0.5f);
                }

                campBadgeUI_BadgeItemUI.transform.SetSiblingIndex(index);
                index++;

            }
        }
    }
}
