using EZ.Data;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace EZ.DataMgr
{
    //杂项数据
    public class MiscMgr : BaseDataMgr<MiscDTO>
    {
        public int m_DeleteData = 0;

        public MiscMgr()
        {
            OnInit();
        }
        public override void OnInit()
        {
            base.OnInit();
            Init("misc");

            if (m_Data == null)
            {
                m_Data = new MiscDTO();
                SaveData();
            }
        }

        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }

        private DateTime GetLastLoginDay()
        {
            return DateTimeUtil.GetDate(m_Data.lastLoginDay);
        }

        public int GetSumLoginDayNum()
        {
            return m_Data.sumLoginDay;
        }
        public int GetContinousLoginDayNum()
        {
            return m_Data.continuousLoginDay;
        }

        public int GetRetainDays()
        {
            return m_Data.retainDays;
        }

        public DateTime GetCreateDate()
        {
            return DateTimeUtil.GetDate(m_Data.createDate);
        }

        public long GetLastLoginTime()
        {
            return (long)m_Data.lastLoginTime;
        }

        public double GetLastOffline()
        {
            return m_Data.lastOffline;
        }

        public void SetLastOffline(double mills)
        {
            m_Data.lastOffline = mills;
        }

        #region 在线奖励
        #endregion

        #region 看视频得狗牌

        public int VideoMDTData
        {
            get { return m_Data.VideoMDTData; }
            set {
                m_Data.VideoMDTData = value;
                SaveData();
            }
        }
        public int VideoMDTDatatTimes
        {
            get { return m_Data.VideoMDTDatatTimes; }
            set {
                m_Data.VideoMDTDatatTimes = value;
                SaveData();
            }
        }
        public int VideoMDTDataGift
        {
            get { return m_Data.VideoMDTDataGift; }
            set
            {
                m_Data.VideoMDTDataGift = value;
                SaveData();
            }
        }
        public double LastVideoMDTDataMills
        {
            get { return m_Data.LastVideoMDTDataMills; }
            set
            {
                m_Data.LastVideoMDTDataMills = value;
                SaveData();
            }
        }

        public int FirstMain
        {
            get { return m_Data.firstMain; }
            set
            {
                m_Data.firstMain = value;
                SaveData();
            }
        }
        #endregion


        #region 看视频得能量
        public int adEnegyTimes
        {
            get { return m_Data.AdEnegyTimes; }
            set
            {
                m_Data.AdEnegyTimes = value;
                SaveData();
            }
        }
        public double lastAdEnegyMills
        {
            get { return m_Data.LastAdEnegyMills; }
            set
            {
                m_Data.LastAdEnegyMills = value;
                SaveData();
            }
        }
        #endregion

        #region 看视频开箱子
        public int adBoxTimes
        {
            get { return m_Data.AdBoxTimes; }
            set
            {
                m_Data.AdBoxTimes = value;
                SaveData();
            }
        }
        public double lastAdBoxMills
        {
            get { return m_Data.LastAdBoxMills; }
            set
            {
                m_Data.LastAdBoxMills = value;
                SaveData();
            }
        }
        #endregion

        #region 首充相关
        public int EveryDayFP
        {
            get { return m_Data.EveryDayFP; }
            set
            {
                m_Data.EveryDayFP = value;
                SaveData();
            }
        }
        public int FirstPurchase
        {
            get { return m_Data.FirstPurchase; }
            set
            {
                m_Data.FirstPurchase = value;
                SaveData();
            }
        }

        public Dictionary<string, int> OrderDic
        {
            get { return m_Data.orderDic; }
            set
            {
                m_Data.orderDic = value;
                SaveData();
            }
        }
        #endregion
		public double SumPurchase
        {
            get { return m_Data.sumPurchase; }
            set
            {
                m_Data.sumPurchase = value;
                SaveData();
            }
        }
        public int SumPurchaseTimes
        {
            get { return m_Data.sumPurchaseTimes; }
            set
            {
                m_Data.sumPurchaseTimes = value;
                SaveData();
            }
        }
        public int SumAdTimes
        {
            get { return m_Data.sumAdTimes; }
            set
            {
                m_Data.sumAdTimes = value;
                SaveData();
            }
        }
        public string Language
        {
            get { return m_Data.language; }
            set
            {
                m_Data.language = value;
                SaveData();
            }
        }
        public int ShowTimeGiftToday
        {
            get { return m_Data.showTimeGiftToday; }
            set
            {
                m_Data.showTimeGiftToday = value;
                SaveData();
            }
        }
        public int StartTimesToday
        {
            get { return m_Data.startTimesToday; }
            set
            {
                m_Data.startTimesToday = value;
                SaveData();
            }
        }
        public Dictionary<string, int> PurchaseTimesDic
        {
            get { return m_Data.purchaseTimesDic; }
            set
            {
                m_Data.purchaseTimesDic = value;
                SaveData();
            }
        }
        public string TimeGiftEndTime
        {
            get { return m_Data.timeGiftEndTime; }
            set
            {
                m_Data.timeGiftEndTime = value;
                SaveData();
            }
        }
        public double TimeGiftStartTime
        {
            get { return m_Data.timeGiftStartTime; }
            set
            {
                m_Data.timeGiftStartTime = value;
                SaveData();
            }
        }
        public List<int> Dialogues
        {
            get { return m_Data.Dialogues; }
            set
            {
                m_Data.Dialogues = value;
                SaveData();
            }
        }

        public void AddDialogue(int id)
        {
            Dialogues.Add(id);
            SaveData();
        }
        public void RemoveDialogue(int index)
        {
            Dialogues.RemoveAt(index);
            SaveData();
        }

        public void AfterInit()
        {
            //Debug.Log("AfterInit = " + DateTime.Now.ToString());
            double nowMills = DateTimeUtil.GetMills(DateTime.Now);
            if (m_Data.lastOffline > 0 && m_Data.lastOffline < m_Data.lastLoginTime)
            {
                m_Data.lastOffline = m_Data.lastLoginTime;
            }
            m_Data.lastLoginTime = nowMills;
            DateTime curDate = GetLastLoginDay();
            DateTime now = DateTime.Today;
            int addDayNum = (now - curDate).Days;
            if (addDayNum > 0)
            {
                m_Data.lastLoginDay = DateTimeUtil.GetMills(now);
                m_Data.sumLoginDay = m_Data.sumLoginDay + 1;
                if (addDayNum == 1)
                {
                    m_Data.continuousLoginDay += 1;
                } else
                {
                    m_Data.continuousLoginDay = 1;
                }


                DateTime createDate = GetCreateDate();
                m_Data.retainDays = DateTimeUtil.SubDays(now, createDate);
                m_Data.AdEnegyTimes = 0;
                m_Data.AdBoxTimes = 0;
                m_Data.EveryDayFP = 0;
                m_Data.VideoMDTDatatTimes = 0;
                int mTodayKey = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
                m_Data.VideoMDTData = mTodayKey;

                m_Data.startTimesToday = 0;
                m_Data.showTimeGiftToday = 0;
                SaveData();

                //if (m_Data.retainDays == 1)
                //{
                //    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_login_2nd);
                //} else if(m_Data.retainDays == 6)
                //{
                //    //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_login_7th);
                //}
            }
            StartTimesToday ++;

            if (TimeGiftEndTime == null || !TimeGiftEndTime.Equals(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TIME_GIFT_END_TIME).content))
            {
                TimeGiftEndTime = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TIME_GIFT_END_TIME).content;

                TimeGiftStartTime = DateTimeUtil.GetMills(DateTime.Now);
            }

            if (Convert.ToDateTime(TimeGiftEndTime) < DateTimeUtil.GetDate(TimeGiftStartTime))
            {
                TimeGiftStartTime = 0d;
            }

            //进行任务检测
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.SUM_LOGIN_DAY, 0f);
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.CONSTIOUS_LOGIN, 0f);
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.TEMP_CONSTIOUS_LOGIN, 1f);

            
            if (Language == null || Language.Equals(GameConstVal.EmepyStr))
            {
                Language = UiTools.GetLanguage();
            }
        }
        public void FreshMicInfo()
        {
            m_Data.lastOffline = DateTimeUtil.GetMills(DateTime.Now);
            double curOnLineTime = m_Data.totalOnLineTime;
            curOnLineTime += (DateTimeUtil.GetMills(DateTime.Now) - m_Data.lastLoginTime);
            m_Data.totalOnLineTime = curOnLineTime;
            SaveData();
        }
        public void FreshOnLineTime()
        {
            // 将在线时间设置成 当前时间。那么关闭游戏 的时候，存储totalOnLineTIme 的时候
            // 用当前时间减去它表示经过多长时间
            // 否则 就取两次的时间就行了
            m_Data.totalOnLineTime = m_Data.lastLoginTime - DateTimeUtil.GetMills(DateTime.Now);

            int curDay = m_Data.sumLoginDay;
            int curRewardTimes = m_Data.getTimesEachDay;
            int realTimes = 0;
            int recordDay = 0;
            if (curRewardTimes != 0)
            {
                realTimes = curRewardTimes % 100000;
                recordDay = (curRewardTimes / 100000);
            }
            if (recordDay == curDay)
            {
                realTimes++;
            }
            else
            {
                realTimes = 1;
            }
            m_Data.getTimesEachDay = curDay * 100000 + realTimes;
            SaveData();
        }
        public bool CheckHasOnlineReward()
        {
            double lastLoginTime = m_Data.lastLoginTime;
            double curOnLineTime = m_Data.totalOnLineTime;
            double newOnLineTime = curOnLineTime;
            newOnLineTime += DateTimeUtil.GetMills(DateTime.Now) - lastLoginTime;
            string timeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FALL_BOX_DUR).content;
            float dtTime = float.Parse(timeStr) * 1000;
            if(dtTime > newOnLineTime)
            {
                return false;
            }
            else
            {
                string timesStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FALL_BOX_MAX).content;
                int maxTimes = int.Parse(timesStr);
                int curDay = m_Data.sumLoginDay;
                int curRewardTimes = m_Data.getTimesEachDay;
                int realTimes = 0;
                int recordDay = 0;
                if(curRewardTimes != 0)
                {
                    realTimes = curRewardTimes % 100000;
                    recordDay = (curRewardTimes / 100000); 
                }
                if(recordDay == curDay)
                {
                    return maxTimes > realTimes;
                }
                else
                {
                    return true;
                }
            }
        }

        //查看创建了多少天
        public int GetCreateDays()
        {
            DateTime createDate = GetCreateDate();
            DateTime now = DateTime.Today;
            return DateTimeUtil.SubDays(now, createDate);
        }

        //是否在指定时间内
        public bool IsCreateInDay(int day)
        {
            return DateTimeUtil.GetMills(DateTime.Now) < (m_Data.createDate + day * DateTimeUtil.m_Day_Mills);
        }
         

        public bool IsShowTimeGift(double mls, string key)
        {
            bool openTimeGift = mls > 0;
            bool openTimeGift1 = openTimeGift && NotHaveShowTimeGift(key);
            return openTimeGift1;
        }

        public bool NotHaveShowTimeGift(string key)
        {
            
            GeneralConfigItem timeGift1Cfg = Global.gApp.gGameData.GeneralConfigData.Get(key);
            string[] timeGift1Prm = timeGift1Cfg.content.Split('.');
            ItemItem timeGift1Item = Global.gApp.gGameData.ItemData.Get(int.Parse(timeGift1Prm[2]));
            
            if (ItemTypeConstVal.isWeapon(timeGift1Item.showtype))
            {
                
                return GameItemFactory.GetInstance().GetItem(timeGift1Item.id) == 0d;
            }
            else
            {
                int times1 = 0;
                Global.gApp.gSystemMgr.GetMiscMgr().PurchaseTimesDic.TryGetValue(Global.gApp.gGameData.GeneralConfigData.Get(key).content, out times1);
                
                return times1 == 0;
            }
        }
        public bool GetIsFirstOpenCampUi()
        {
            return !m_Data.HasOpendCampUi;
        }
        public bool GetAndResetHasOpenCampState()
        {
            if (!m_Data.HasOpendCampUi)
            {
                m_Data.HasOpendCampUi = true;
                SaveData();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

