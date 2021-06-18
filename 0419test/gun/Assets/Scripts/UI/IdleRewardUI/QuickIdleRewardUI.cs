using Game;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public class QuickIdleRewardUI : BaseUi
    {
        public Text descTxt;
        public Text rewardGoldTxt;
        public Text clockTxt;
        public Text remainderTxt;
        public Text costTxt;
        public Button freeClaimBtn;
        public Button diamondClaimBtn;
        public Button invalidBtn;
        public Button closeBtn;
        private bool isClick;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            freeClaimBtn.onClick.AddListener(OnClaimBtnClick);
            diamondClaimBtn.onClick.AddListener(OnClaimBtnClick);
            closeBtn.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            descTxt.text = LanguageMgr.GetText("QuickPage_Text_Desc", IdleRewardMgr.QUICK_IDLE_MINUTES / 60);
            rewardGoldTxt.text = IdleRewardMgr.singleton.GetQuickIdleRewardGold().ToSymbolString(); //string.Format("{0}h", IdleRewardMgr.QUICK_IDLE_MINUTES / 60);
            RefreshState();
            InvokeRepeating("UpdateClock", 0, 1f);
        }

        public override void Release()
        {
            base.Release();
            CancelInvoke();
        }

        private void UpdateClock()
        {
            if (DateTimeMgr.singleton.HasFixedServer)
            {
                var clock = DateTimeMgr.singleton.RefreshTime_OneDay - DateTimeMgr.singleton.Now;
                clockTxt.text = LanguageMgr.GetText("QuickPage_Text_Reset", (int)clock.TotalHours, clock.Minutes, clock.Seconds);
            }
            else
            {
                clockTxt.text = "";
            }
        }

        private void RefreshState()
        {
            int remainder = IdleRewardMgr.singleton.GetQuickIdleRemainder();
            //remainderTxt.text = LanguageMgr.GetText("Idle_Fast_Tips0", remainder);
            remainderTxt.gameObject.SetActive(false);
            int cost;
            if (remainder <= 0 || !IdleRewardMgr.singleton.GetQuickIdleCost(out cost))
            {
                freeClaimBtn.gameObject.SetActive(false);
                diamondClaimBtn.gameObject.SetActive(false);
                invalidBtn.gameObject.SetActive(true);
                return;
            }

            invalidBtn.gameObject.SetActive(false);
            if (cost <= 0)
            {
                freeClaimBtn.gameObject.SetActive(true);
                diamondClaimBtn.gameObject.SetActive(false);
            }
            else
            {
                freeClaimBtn.gameObject.SetActive(false);
                diamondClaimBtn.gameObject.SetActive(true);
                costTxt.text = cost.ToString();
            }
        }

        internal override void OnDateTimeRefresh(DateTimeRefreshType type)
        {
            base.OnDateTimeRefresh(type);
            if (type == DateTimeRefreshType.OneDay)
            {
                isClick = false;
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, LanguageMgr.GetText("Active_Award_Data"));
                TouchClose();
            }

        }

        private void OnClaimBtnClick()
        {
            if (isClick)
                return;

            isClick = true;
            IdleRewardMgr.singleton.RequestClaimQuickIdleReward(ClaimRewardEffect);
        }
        private void ClaimRewardEffect(GoodsRequestResult result, BigInteger rewardCount)
        {
            if (result == GoodsRequestResult.Success)
            {
                if (rewardCount > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, GoodsType.GOLD, rewardCount.GetPropEffectValue(), rewardGoldTxt.transform.position);
                }
                RefreshState();
            }
            else if (result == GoodsRequestResult.DataFail_NotEnough)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.NoDiamondUI);
            }
            else if (result == GoodsRequestResult.NetFail)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "Active_Award_No_Network");
            }
            isClick = false;
        }
    }
}