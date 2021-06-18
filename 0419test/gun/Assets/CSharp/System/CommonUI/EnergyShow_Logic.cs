using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class EnergyShow
    {
        private int m_CurRemainSeconds = 0;
        private double m_LastCheckMills = 0;

        private int m_CheckMills = 500;

        private float mProgressY = 0;
        private float mMaxProgressParam = 0;

        private ItemDTO m_BuyCostMDT;
        private ItemDTO m_BuyAddEnergy;
        private ItemDTO m_AdAddEnergy;
        private int m_AdAddEnergyTimesLimit;

        private int m_AdAddEnergyCD;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            Init();
            RegisterListeners();
            FlushUI();
            base.ChangeLanguage();
        }

        private void Update()
        {
            FreshTime();
        }

        public void Init()
        {
            mProgressY = m_Pronum.rectTransform.sizeDelta.y;
            mMaxProgressParam = m_Pronum.rectTransform.sizeDelta.x;

            m_BuyCostMDT = new ItemDTO(SpecialItemIdConstVal.MDT,
                double.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_BUY_ONCE).content), BehaviorTypeConstVal.OPT_MDT_EXCHANGE_ENERGY);
            m_BuyAddEnergy = new ItemDTO(SpecialItemIdConstVal.ENERGY,
                double.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_BUY_ADD).content), BehaviorTypeConstVal.OPT_MDT_EXCHANGE_ENERGY);
            m_AdAddEnergy = new ItemDTO(SpecialItemIdConstVal.ENERGY,
                double.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_AD_ADD).content), BehaviorTypeConstVal.OPT_MDT_EXCHANGE_ENERGY);
            m_AdAddEnergyCD = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.AD_ADD_ENERGY_CD).content);

            EnergyChanged(GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.ENERGY));
            FlushCD();

            m_Btn1txt.text.text = string.Format("×{0}", (int)m_BuyCostMDT.num);
            EnergyNum.text.text = string.Format("×{0}", (int)m_BuyAddEnergy.num);
            AdEnergyNum.text.text = string.Format("×{0}", (int)m_AdAddEnergy.num);
            Btn1.button.onClick.AddListener(OnEnergy);
            Btn2.button.onClick.AddListener(TouchClose);
            AdBtn.button.onClick.AddListener(OnAd);
            m_AdAddEnergyTimesLimit = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_AD_ADD_TIMES).content);
            AdText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3043), m_AdAddEnergyTimesLimit - Global.gApp.gSystemMgr.GetMiscMgr().adEnegyTimes);
        }

        private void FreshTime()
        {
            double nowMills = DateTimeUtil.GetMills(DateTime.Now);
            if (m_LastCheckMills + m_CheckMills > nowMills)
            {
                return;
            }
            m_LastCheckMills = nowMills;

            GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_LIMIT);
            double limit = double.Parse(limitConfig.content);
            if (Global.gApp.gSystemMgr.GetBaseAttrMgr().GetEnergy() >= limit)
            {
                string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3079);
                if (!tip.Equals(Timeneed.text.text))
                {
                    Timeneed.text.text = tip;
                }
                return;
            }

            GeneralConfigItem freshConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_FRESH_SECONDS);
            int freshMills = int.Parse(freshConfig.content) * 1000;

            double addNum = (nowMills - Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLastEnergyTime()) / freshMills;
            addNum = Math.Ceiling(addNum);

            DateTime nextDate = DateTimeUtil.GetDate(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLastEnergyTime() + freshMills * addNum);
            double remainMills = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLastEnergyTime() + freshMills * addNum - nowMills;

            int remainSeconds = (int)Math.Ceiling(remainMills / 1000);
            if (remainSeconds > 0 && m_CurRemainSeconds != remainSeconds)
            {
                m_CurRemainSeconds = remainSeconds;
                int hours = m_CurRemainSeconds / DateTimeUtil.m_Hour_Secs;
                int mimutes = (m_CurRemainSeconds % DateTimeUtil.m_Hour_Secs) / DateTimeUtil.m_Minute_Secs;
                int seconds = (m_CurRemainSeconds % DateTimeUtil.m_Hour_Secs) % DateTimeUtil.m_Minute_Secs;
                Timeneed.text.text = mimutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');

            }

        }

        private void OnEnergy()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.NO_ENERGY_MDT);
            if (!GameItemFactory.GetInstance().CanReduce(m_BuyCostMDT))
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.CommonUIIndexChange, 0);
                TouchClose();
                return;
            }
            GameItemFactory.GetInstance().ReduceItem(m_BuyCostMDT);

            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);

            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_BuyAddEnergy.itemId, (int)m_BuyAddEnergy.num, Btn1.rectTransform.position);
            GameItemFactory.GetInstance().AddItem(m_BuyAddEnergy);

        }

        private void OnAd()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.NO_ENERGY_AD);
            if (Global.gApp.gSystemMgr.GetMiscMgr().adEnegyTimes >= m_AdAddEnergyTimesLimit)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3044);
                return;
            }
            double leftMills = Global.gApp.gSystemMgr.GetMiscMgr().lastAdEnegyMills + m_AdAddEnergyCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
            if (leftMills > 0)
            {
                int leftSecs = (int)Math.Round(leftMills / 1000);
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3059, EZMath.FormateTime(leftSecs));
                return;
            }
            //调用广告sdk
            AdManager.instance.ShowRewardVedio(CompleteAd, AdShowSceneType.GET_ENERGY, 0, 0, 0);
            //CompleteAd(true);
        }

        private void CompleteAd(bool res)
        {
            if (res)
            {
                //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_AdAddEnergy.itemId, (int)m_AdAddEnergy.num * 3, AdBtn.rectTransform.position);
                GameItemFactory.GetInstance().AddItem(m_AdAddEnergy);
                Global.gApp.gSystemMgr.GetMiscMgr().adEnegyTimes += 1;

                Global.gApp.gSystemMgr.GetMiscMgr().lastAdEnegyMills = DateTimeUtil.GetMills(DateTime.Now);
                AdText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3043), m_AdAddEnergyTimesLimit - Global.gApp.gSystemMgr.GetMiscMgr().adEnegyTimes);
                FlushCD();
            }
            

        }

        private void EnergyChanged(double val)
        {
            FlushUI();
        }

        private void FlushUI()
        {
            GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_LIMIT);
            float limit = float.Parse(limitConfig.content);
            float cur = (float)Global.gApp.gSystemMgr.GetBaseAttrMgr().GetEnergy();

            float param = cur / limit * mMaxProgressParam;
            m_Pronum.image.rectTransform.sizeDelta = new Vector2(param > mMaxProgressParam ? mMaxProgressParam : param, mProgressY);
            string tmpEng = cur >= limit ? string.Format("<color=#ff426c>{0}/{1}</color>", cur, limit) : string.Format("{0}/{1}", cur, limit); ;
            m_Energytxt.text.text = tmpEng;


        }

        private void FlushCD()
        {
            double leftMills = Global.gApp.gSystemMgr.GetMiscMgr().lastAdEnegyMills + m_AdAddEnergyCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
            if (leftMills > 0 && m_AdAddEnergyTimesLimit - Global.gApp.gSystemMgr.GetMiscMgr().adEnegyTimes > 0)
            {
                DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
                int leftSecs = (int)Math.Round(leftMills / 1000);
                AdBtntxt.text.text = EZMath.FormateTime(leftSecs);
                if (leftSecs > 0)
                {
                    m_AdBtn.button.interactable = false;
                }
                dcb.SetCallTimes(leftSecs);
                dcb.SetAction(() =>
                {
                    leftMills = Global.gApp.gSystemMgr.GetMiscMgr().lastAdEnegyMills + m_AdAddEnergyCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
                    leftSecs = (int)Math.Round(leftMills / 1000);
                    if (leftSecs > 0)
                    {
                        AdBtntxt.text.text = EZMath.FormateTime(leftSecs);
                        m_AdBtn.button.interactable = false;
                    }
                    else
                    {
                        AdBtntxt.text.text = "Get";
                        m_AdBtn.button.interactable = true;
                    }
                }, 1f);
            }
            else
            {
                AdBtntxt.text.text = "Get";
            }
        }



        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.EnergyChanged, EnergyChanged);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.EnergyChanged, EnergyChanged);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }


    }
}