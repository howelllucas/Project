using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class NoMDTUI 
    {
        private int m_RandomMax;
        private List<ItemDTO> m_RandomList = new List<ItemDTO>();

        private List<ShopItem> mGetMDTConfig;
        private int mAdsTimes = 0;
        private int mLastKey = 0;
        private int mTodayKey = 0;
        private int m_AdAddMDTCD;


        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();
            RegisterListeners();

            GetMDTBtn.button.onClick.AddListener(OnADGetMDTBtn);
            CloseBtn.button.onClick.AddListener(OnCloseBtn);
            Ad4Mdt.button.onClick.AddListener(OnADGetMDTBtn);

            base.ChangeLanguage();
        }

        
        private void InitNode()
        {
            m_AdAddMDTCD = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.AD_ADD_MDT_CD).content);

            mGetMDTConfig = Global.gApp.gGameData.ShopTypeMapData[ChargeShowTypeConstVal.MDT];

            mLastKey = Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTData;
            mAdsTimes = Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTDatatTimes;
            FlushMDTData();

        }

        private void UIFresh()
        {
            
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
            base.Release();
        }


        private void FlushMDTData()
        {
            mTodayKey = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
            //FillNewDay();
            FlushMDTUI();
        }
        private void FillNewDay()
        {
            if (mTodayKey == mLastKey) return;
            mLastKey = mTodayKey;
            Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTData = mTodayKey;
            mAdsTimes = 0;
            Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTDatatTimes = mAdsTimes;
        }
        private void FlushMDTUI()
        {
            //看广告得狗牌
            //for (int i = 0; i < mGetMDTConfig.Count; i++)
            //{
            //    RectTransform_Container timesRT = ReflectionUtil.GetValueByProperty<NoMDTUI, RectTransform_Container>("mdtItem" + (i + 1), this);
            //    timesRT.rectTransform.GetChild(1).gameObject.SetActive(mAdsTimes == i);
            //    //timesRT.rectTransform.GetChild(2).GetComponent<Image>().sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(getMDTConfig.contents[i * 2]);
            //    timesRT.rectTransform.GetChild(3).GetComponent<Text>().text = mGetMDTConfig[i].goodsNum.ToString();
            //    timesRT.rectTransform.GetChild(4).gameObject.SetActive(mAdsTimes > i);
            //}

            //progress.image.fillAmount = (float)mAdsTimes / mGetMDTConfig.Count;

            //Ad4Mdt 的UI
            totalChance.text.text = "/ " + mGetMDTConfig.Count.ToString();
            int leftTimes = mGetMDTConfig.Count - mAdsTimes;
            leftChance.text.text = leftTimes.ToString();
            newHas.gameObject.SetActive(leftTimes > 0);
            m_num.text.text = leftTimes.ToString();
            if (mAdsTimes == mGetMDTConfig.Count)
            {
                mdtNum.text.text = "0";
            }
            else
            {
                mdtNum.text.text = mGetMDTConfig[mAdsTimes].goodsNum.ToString();
            }

            for (int i = 1; i <= mGetMDTConfig.Count; i++)
            {
                RectTransform_Image_Container rt = ReflectionUtil.GetValueByProperty<NoMDTUI, RectTransform_Image_Container>("m" + i, this);
                rt.gameObject.SetActive(i <= mGetMDTConfig.Count - mAdsTimes);
            }

            double leftMills = Global.gApp.gSystemMgr.GetMiscMgr().LastVideoMDTDataMills + m_AdAddMDTCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
            MdtCDbg.gameObject.SetActive(false);
            if (leftMills > 0 && leftTimes > 0)
            {
                DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
                int leftSecs = (int)Math.Round(leftMills / 1000);

                MdtCDbg.gameObject.SetActive(true);
                MdtCDText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3059), EZMath.FormateTime(leftSecs));
                dcb.SetAction(() =>
                {
                    leftMills = Global.gApp.gSystemMgr.GetMiscMgr().LastVideoMDTDataMills + m_AdAddMDTCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
                    leftSecs = (int)Math.Round(leftMills / 1000);
                    if (leftSecs > 0)
                    {
                        MdtCDbg.gameObject.SetActive(true);
                        MdtCDText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3059), EZMath.FormateTime(leftSecs));
                    }
                    else
                    {
                        MdtCDbg.gameObject.SetActive(false);
                    }
                }, 1f);
                dcb.SetIgnoreSceneTimeScale(true);
                dcb.SetCallTimes(leftSecs);
            }

            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.DogeAdTimesChanged);
        }


        private void OnADGetMDTBtn()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.NO_MDT_AD);
            if (mAdsTimes == mGetMDTConfig.Count)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3044);
                return;
            }
            double leftMills = Global.gApp.gSystemMgr.GetMiscMgr().LastVideoMDTDataMills + m_AdAddMDTCD * 1000 - DateTimeUtil.GetMills(DateTime.Now);
            if (leftMills > 0)
            {
                int leftSecs = (int)Math.Round(leftMills / 1000);
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3059, EZMath.FormateTime(leftSecs));
                return;
            }

            //调起广告
            AdManager.instance.ShowRewardVedio(CompleteAds, AdShowSceneType.NO_MDT_GET, 0, 0, 0);
            //CompleteAds(true);
        }

        private void CompleteAds(bool ended)
        {
            //Global.gApp.gMsgDispatcher.RemoveListener<bool>(MsgIds.ViewAdCallBack, CompleteAds);
            if (ended)
            {

                //发货
                int itemId = mGetMDTConfig[mAdsTimes].goodsType;
                float itemNum = mGetMDTConfig[mAdsTimes].goodsNum;
                //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.8f);
                ItemDTO addItemDTO = new ItemDTO(itemId, itemNum, BehaviorTypeConstVal.OPT_WATCH_ADS_4_MDT);
                addItemDTO.paramStr1 = mAdsTimes.ToString();
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, itemId, (int)itemNum, GetMDTBtn.rectTransform.position);
                GameItemFactory.GetInstance().AddItem(addItemDTO);
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);


                mAdsTimes++;
                Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTDatatTimes = mAdsTimes;
                Global.gApp.gSystemMgr.GetMiscMgr().LastVideoMDTDataMills = DateTimeUtil.GetMills(DateTime.Now);


                FlushMDTData();
            }
            
        }

        private void OnCloseBtn()
        {
            OnClick();
            Global.gApp.gUiMgr.ClosePanel(Wndid.NoMDTUI);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.AfterAD);
        }

    }
}
