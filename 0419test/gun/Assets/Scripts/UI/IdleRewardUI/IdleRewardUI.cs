using Game;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public class IdleRewardUI : BaseUi
    {
        public CanvasGroup canvasGroup;
        public Text rewardGoldTxt;
        public Text rewardSpeedTxt;
        public Text idleMaxHourTxt;
        public Text idleTimeTxt;
        public Slider idleProcess;
        public Text maxDurationDocTxt;
        public Text collectCardCountTxt;
        public Button claimBtn;
        public Button quickBtn;
        public Button closeBtn;
        private bool isClick = false;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            claimBtn.onClick.AddListener(OnClaimBtnClick);
            quickBtn.onClick.AddListener(OnQuickIdleBtnClick);
            closeBtn.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InvokeRepeating("RefreshInfo", 0f, 1f);
        }

        public override void Release()
        {
            base.Release();
            CancelInvoke();
        }

        void RefreshInfo()
        {
            rewardGoldTxt.text = IdleRewardMgr.singleton.GetRewardGold().ToSymbolString();
            //rewardSpeedTxt.text = LanguageMgr.GetText("Idle_UI_IdleBenefit", IdleRewardMgr.singleton.GetGoldPerMinute().ToSymbolString(1));
            var maxIdleHour = IdleRewardMgr.singleton.GetMaxIdleHour();
            idleMaxHourTxt.text = LanguageMgr.GetText("IdelPage_Text_MaxTime", maxIdleHour);
            var idleTimeSpan = IdleRewardMgr.singleton.GetIdleTimeSpan();
            idleTimeTxt.text = string.Format("{0:00}:{1:00}:{2:00}",
                Mathf.FloorToInt((float)idleTimeSpan.TotalHours), idleTimeSpan.Minutes, idleTimeSpan.Seconds);
            var idleHour = idleTimeSpan.TotalHours;
            idleProcess.value = (float)idleHour / maxIdleHour;
            maxDurationDocTxt.text = LanguageMgr.GetText("IdelPage_Text_Tips", IdleRewardMgr.EACH_HERO_ADD_HOUR);
            collectCardCountTxt.text = LanguageMgr.GetText("IdelPage_Text_Number", PlayerDataMgr.singleton.GetCollectCardCount());
        }

        private void OnClaimBtnClick()
        {
            if (isClick)
                return;

            isClick = true;
            canvasGroup.interactable = false;
            IdleRewardMgr.singleton.RequestClaimIdleReward(ClaimRewardEffect);
        }

        private void ClaimRewardEffect(GoodsRequestResult result, BigInteger rewardCount)
        {
            if (result == GoodsRequestResult.Success)
            {
                if (rewardCount > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, GoodsType.GOLD, rewardCount.GetPropEffectValue(), rewardGoldTxt.transform.position);
                }
                TouchClose();
            }
            else if (result == GoodsRequestResult.NetFail)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "Active_Award_No_Network");
            }
            isClick = false;
            canvasGroup.interactable = true;
        }

        private void OnQuickIdleBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.QuickIdleRewardUI);
        }
    }
}