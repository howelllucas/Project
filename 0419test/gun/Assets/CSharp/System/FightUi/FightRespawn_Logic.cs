using EZ.DataMgr;
using EZ.Util;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class FightRespawn
    {

        private int m_RemainFrames = 0;
        private bool m_ShowAD = false;


        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_RemainFrames = Application.targetFrameRate * 6;
            Btn1txt.text.text = "×" + TableMgr.singleton.ValueTable.battle_revive_cost;

            Btn1.button.onClick.AddListener(OnClicRevive);
            ADBtn.button.onClick.AddListener(OnAdBtn);
            CloseBtn.button.onClick.AddListener(OnGiveUpBtnClick);
            RegisterListeners();

            base.ChangeLanguage();
        }

        private void LateUpdate()
        {
            if (m_ShowAD)
            {
                return;
            }

            if (m_RemainFrames > 0)
            {
                m_RemainFrames--;
                RemainSec.text.text = (m_RemainFrames / Application.targetFrameRate).ToString();
            }
            else
            {
                OnGiveUpBtnClick();
            }
        }

        private void AfterAD()
        {
            m_ShowAD = false;
        }

        private void OnClicRevive()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_MDT_REVIVE);
            //double curCount = GameItemFactory.GetInstance().GetItem(FightResultManager.REVIVE_ITEMID);
            //if (curCount < FightResultManager.REVIVE_ITEMCOUNT)
            //{
            //    //Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1005);
            //    m_ShowAD = true;

            //    //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_NO_MDT);
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.NoMDTUI);
            //    return;
            //}
            //ItemDTO reduceItemDTO = new ItemDTO(FightResultManager.REVIVE_ITEMID, FightResultManager.REVIVE_ITEMCOUNT, BehaviorTypeConstVal.OPT_RESPAWN);
            //GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);

            //if (GameGoodsMgr.singleton < FightResultManager.REVIVE_ITEMCOUNT)
            //{
            //    //Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1005);
            //    m_ShowAD = true;

            //    //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_NO_MDT);
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.NoMDTUI);
            //    return;
            //}
            CancelClock();
            GameGoodsMgr.singleton.RequestCostGameGoods((rst, detail) =>
            {
                if (rst == GoodsRequestResult.Success)
                {
                    OnClickEnd(true);
                    FightResultManager.instance.SetRetryType(FightResultManager.RetryType.MDT);
                }
                else if (rst == GoodsRequestResult.DataFail_NotEnough)
                {
                    Global.gApp.gUiMgr.OpenPanel(Wndid.NoDiamondUI);
                }

            }, (int)GoodsType.DIAMOND, TableMgr.singleton.ValueTable.battle_revive_cost);


        }

        private void CancelClock()
        {
            m_ShowAD = true;
            RemainSec.gameObject.SetActive(false);
        }

        private void OnGiveUpBtnClick()
        {
            base.TouchClose();
            FightResultManager.instance.SelectRevive(false);
        }

        private void OnAdBtn()
        {

            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_AD_REVIVE);
            m_ShowAD = true;
            //调起广告
            AdManager.instance.ShowRewardVedio(OnClickEnd, AdShowSceneType.REVIVE, 0, 0, 0);
            //OnClickEnd(true);
        }

        private void OnClickEnd(bool res)
        {
            if (res)
            {
                base.TouchClose();
                FightResultManager.instance.SelectRevive(true);
                FightResultManager.instance.SetRetryType(FightResultManager.RetryType.AD);
            }


            AfterAD();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.AfterAD, AfterAD);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.AfterAD, AfterAD);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

    }
}
