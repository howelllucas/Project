using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;

namespace EZ
{
    public partial class CampsiteOfflineRewardUI
    {
        private bool isClick;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            ClaimBtn.button.onClick.AddListener(OnClaimBtnClick);
            double dt; double reward;
            if (CampsiteMgr.singleton.CheckOfflineReward(out dt, out reward))
            {
                RewardValTxt.text.text = UiTools.FormateMoney(reward);
                TimeSpan ts = new TimeSpan((long)(10000000 * dt));
                OfflineTimeTxt.text.text = string.Format("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
                OfflineTimeBar.slider.value = Mathf.Clamp01((float)(dt / (CampsiteMgr.singleton.GetMaxOfflineHour() * 60 * 60)));
                MinHourTxt.text.text = "0";
                MaxHourTxt.text.text = LanguageMgr.GetText("IdelPage_Text_MaxTime", CampsiteMgr.singleton.GetMaxOfflineHour());
                MaxDurationDocTxt.text.text = LanguageMgr.GetText("IdelPage_Text_Tips", CampsiteMgr.OFFLINE_HOUR_EACH_CARD);
                CollectCardCountTxt.text.text = LanguageMgr.GetText("IdelPage_Text_Number", PlayerDataMgr.singleton.GetCollectCardCount());
            }
        }

        private void OnClaimBtnClick()
        {
            if (isClick)
                return;

            isClick = true;
            CampsiteMgr.singleton.RequestClaimOfflineReward(HandleClaimCallback);
        }

        private void HandleClaimCallback(CampsiteRequestResult result)
        {
            isClick = false;
            if (result == CampsiteRequestResult.Success)
            {
                CampsiteMgr.singleton.ClaimAllReward();
                TouchClose();
            }
            else if(result == CampsiteRequestResult.NetFail)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, LanguageMgr.GetText("Active_Award_No_Network"));
            }
        }
    }
}