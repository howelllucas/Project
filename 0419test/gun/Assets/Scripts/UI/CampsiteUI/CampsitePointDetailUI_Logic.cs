using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class CampsitePointDetailUI
    {
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            var pointDataMgr = arg as CampsitePointMgr;
            Title.text.text = pointDataMgr.buildingRes.buildingName;
            RewardBase.CampDetailItem.SetValue(UiTools.FormateMoney(pointDataMgr.RewardBase));
            RewardPlayer.CampDetailItem.SetTitle(LanguageMgr.GetText("CampDetail_Rule_Rank", PlayerDataMgr.singleton.GetPlayerLevel()));
            RewardPlayer.CampDetailItem.SetValue(string.Format("x{0}", pointDataMgr.RewardFactorByPlayer));
            RewardSettleTxt.text.text = UiTools.FormateMoney(pointDataMgr.OnceRewardVal);
            IntervalBase.CampDetailItem.SetValue(LanguageMgr.GetText("CampDetail_Text_Time", pointDataMgr.RewardIntervalBase));
            IntervalSettleTxt.text.text = LanguageMgr.GetText("CampDetail_Text_Time", pointDataMgr.RewardInterval);
            CostBase.CampDetailItem.SetValue(UiTools.FormateMoney(pointDataMgr.LvUpCostBase));
            CostSettleTxt.text.text = UiTools.FormateMoney(pointDataMgr.GetLvUpCost(1));
            string autoColorHex = ColorUtility.ToHtmlStringRGB(pointDataMgr.isAuto ? Color.green : Color.red);
            AutoSettleTxt.text.color = pointDataMgr.isAuto ? Color.green : Color.red;
            AutoSettleTxt.text.text = LanguageMgr.GetText(pointDataMgr.isAuto ? "CampDetail_Rule_AutoOpen" : "CampDetail_Rule_AutoClose");

            if (pointDataMgr.equipGunId > 0)
            {
                var gunRes = TableMgr.singleton.GunCardTable.GetItemByID(pointDataMgr.equipGunId);
                var cardSprite = Global.gApp.gResMgr.LoadAssets<Sprite>(gunRes.icon);
                RewardCardBase.CampDetailItem.SetIcon(cardSprite);
                RewardCardBase.CampDetailItem.SetValue(string.Format("x{0}", pointDataMgr.RewardFactorByCardBase));
                var rewardFactorByCardSkill = pointDataMgr.RewardFactorByCardSkill;
                if (rewardFactorByCardSkill > 1)
                {
                    RewardCardSkill.CampDetailItem.SetIcon(cardSprite);
                    RewardCardSkill.CampDetailItem.SetValue(string.Format("x{0}", rewardFactorByCardSkill));
                }
                else
                {
                    RewardCardSkill.gameObject.SetActive(false);
                }

                var intervalFactorByCardSkill = pointDataMgr.RewardIntervalFactorByCardSkill;
                if (intervalFactorByCardSkill > 1)
                {
                    IntervalCardSkill.CampDetailItem.SetIcon(cardSprite);
                    IntervalCardSkill.CampDetailItem.SetValue(string.Format("x{0}", intervalFactorByCardSkill));
                }
                else
                {
                    IntervalCardSkill.gameObject.SetActive(false);
                }
                var gunLv = PlayerDataMgr.singleton.GetCardLevel(pointDataMgr.equipGunId);
                AutoCardLv.CampDetailItem.SetValue(string.Format("<color=#{0}>{1}</color>/{2}", autoColorHex, gunLv, pointDataMgr.AutoLv));
            }
            else
            {
                RewardCardBase.gameObject.SetActive(false);
                RewardCardSkill.gameObject.SetActive(false);
                IntervalCardSkill.gameObject.SetActive(false);
                AutoCardLv.CampDetailItem.SetValue(string.Format("<color=#{0}>{1}</color>/{2}", autoColorHex, 0, pointDataMgr.AutoLv));
            }

        }
    }
}