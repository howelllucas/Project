using EZ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public enum DateTimeRefreshType
    {
        OneDay,
        TwoDay,
        OneWeek,
    }

    public class DateTimeMgr : Singleton<DateTimeMgr>
    {
        private const float FIX_MIN_INTERVAL = 1f;//最小请求间隔(s) 避免频繁请求

        public bool HasFixedServer
        {
            get
            {
                return PlayerDataMgr.singleton.DB.serverTimeData != null;
            }
        }
        public DateTime Now
        {
            get
            {
                return DateTime.Now + ts;
            }
        }
        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow + ts;
            }
        }

        public DateTime RefreshTime_OneDay { get { return HasFixedServer ? PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneDay : DateTime.MinValue; } }
        //public DateTime RefreshTime_TwoDay { get { return HasFixedServer ? PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_TwoDay : DateTime.MinValue; } }
        //public DateTime RefreshTime_OneWeek { get { return HasFixedServer ? PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneWeek : DateTime.MinValue; } }

        private TimeSpan ts;

        private const string serverPath = "https://gdcasapi.yuechuan.net/timezone";
        private Coroutine request = null;

        public event Action<bool> onLocalTimeFixed;

        private const DayOfWeek WEEK_REFRESH_DATE = DayOfWeek.Monday;
        public event Action<DateTimeRefreshType> onTimeRefresh;

        private float updateTimer;

        private float lastFixTime = -1;
        /// <summary>
        /// 必须在PlayerDataMgr及InternetMgr之后调用
        /// </summary>
        public void Init()
        {
            if (PlayerDataMgr.singleton.DB.serverTimeData == null)
            {
                FixedLocalTime(true);
            }
            else
            {
                ts = new TimeSpan(PlayerDataMgr.singleton.DB.serverTimeData.localTimeSpan);
                FixedLocalTime(true);
            }
        }

        public void FixedLocalTime(bool force = false)
        {
            //避免频繁请求 上一次请求成功后的一定时间内认为时间不需要修正
            if (lastFixTime >= 0 && Time.realtimeSinceStartup - lastFixTime <= FIX_MIN_INTERVAL)
            {
                onLocalTimeFixed?.Invoke(true);
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ServerTimeFixed, true);
                return;
            }

            if (InternetMgr.singleton.IsInternetConnect())
            {
                if (request == null)
                    request = IEnumeratorHelper.Start(RequestServerTime(force));
            }
            else
            {
                //broadcast
                onLocalTimeFixed?.Invoke(false);
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ServerTimeFixed, false);
                //UI.UIMgr.singleton.BoradCast(UIEventType.SERVER_TIME, false);
                if (force)
                {
                    InternetMgr.singleton.funcOnNetworkStateChange -= OnNetworkStateChange;
                    InternetMgr.singleton.funcOnNetworkStateChange += OnNetworkStateChange;
                }
            }
        }

        private void OnNetworkStateChange(bool isConnect)
        {
            if (isConnect)
            {
                if (request == null)
                    request = IEnumeratorHelper.Start(RequestServerTime(true));

                InternetMgr.singleton.funcOnNetworkStateChange -= OnNetworkStateChange;
            }
        }

        IEnumerator RequestServerTime(bool force = false)
        {
            using (var wr = UnityWebRequest.Get(serverPath))
            {
                if (!force || HasFixedServer)
                {
                    wr.timeout = 3;
                }
                wr.SendWebRequest();
                while (!wr.isDone)
                {
                    yield return null;
                }
                if (wr.isNetworkError || wr.isHttpError)
                {
                    Debug.Log("ServerTimeRequestFail:" + wr.error);
                    onLocalTimeFixed?.Invoke(false);
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ServerTimeFixed, false);
                    //UI.UIMgr.singleton.BoradCast(UIEventType.SERVER_TIME, false);
                    //broadcast
                }
                else
                {
                    var handler = wr.downloadHandler;
                    UpdateTime(handler.text);
                }
            }

            request = null;

            if (force && !HasFixedServer)
            {
                FixedLocalTime(true);
            }
        }

        private void UpdateTime(string serverTimeStr)
        {
            DateTime serverUtc;
            bool result = DateTime.TryParseExact(serverTimeStr.Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out serverUtc);
            //bool result = DateTime.TryParse(serverTimeStr, out serverUtc);
            if (result)
            {
                lastFixTime = Time.realtimeSinceStartup;
#if !GAME_DEBUG
                ts = serverUtc - DateTime.UtcNow;
#endif
                if (PlayerDataMgr.singleton.DB.serverTimeData == null)
                {
                    PlayerDataMgr.singleton.DB.serverTimeData = new Data.ServerTimeData();
                }
                PlayerDataMgr.singleton.DB.serverTimeData.localTimeSpan = ts.Ticks;
                PlayerDataMgr.singleton.NotifySaveData();
                onLocalTimeFixed?.Invoke(true);
                CheckRefresh();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ServerTimeFixed, true);

                //UI.UIMgr.singleton.BoradCast(UIEventType.SERVER_TIME, true);
                Debug.Log("FixedTimeFinish:" + serverTimeStr);
            }
            else
            {
                onLocalTimeFixed?.Invoke(false);
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ServerTimeFixed, false);
                //UI.UIMgr.singleton.BoradCast(UIEventType.SERVER_TIME, false);
                Debug.Log("ServerTimeParseFail:" + serverTimeStr);
            }
            //broadcast
        }

        private void CheckRefresh()
        {
            var now = Now;
            if (now >= PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneDay)
            {
                var refreshTime = CalcNextRefreshTime_Day(1);
                PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneDay = refreshTime;
                onTimeRefresh?.Invoke(DateTimeRefreshType.OneDay);
                PlayerDataMgr.singleton.NotifySaveData();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.DateTimeRefresh, DateTimeRefreshType.OneDay);
                Debug.Log("Refresh One Day:" + refreshTime);
            }

            //if (now >= PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_TwoDay)
            //{
            //    var refreshTime = CalcNextRefreshTime_Day(2);
            //    PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_TwoDay = refreshTime;
            //    onTimeRefresh?.Invoke(DateTimeRefreshType.TwoDay);
            //    PlayerDataMgr.singleton.NotifySaveData();
            //    Debug.Log("Refresh Two Day:" + refreshTime);
            //}

            //if (now >= PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneWeek)
            //{
            //    var refreshTime = CalcNextRefreshTime_Week(WEEK_REFRESH_DATE);
            //    PlayerDataMgr.singleton.DB.serverTimeData.refreshTime_OneWeek = refreshTime;
            //    onTimeRefresh?.Invoke(DateTimeRefreshType.OneWeek);
            //    PlayerDataMgr.singleton.NotifySaveData();
            //    Debug.Log("Refresh One Week:" + refreshTime);
            //}
        }

        private DateTime CalcNextRefreshTime_Week(DayOfWeek refreshDate)
        {
            var now = Now;
            var dayOfWeek = now.DayOfWeek;
            var delta = refreshDate - dayOfWeek;
            if (delta < 0)
                delta += 7;
            DateTime refreshTime = new DateTime(now.Year, now.Month, now.Day + delta,
                TableMgr.singleton.ValueTable.refresh_data_time, 0, 0);
            if (refreshTime <= now)
            {
                refreshTime = refreshTime.AddDays(7);
            }
            return refreshTime;
        }

        private DateTime CalcNextRefreshTime_Day(int dayInterval)
        {
            var now = Now;
            int dayOfInterval = 0;
            if (dayInterval > 1)
            {
                var standard = now - new DateTime(1970, 1, 1);
                dayOfInterval = (int)((long)(Math.Floor(standard.TotalDays)) % dayInterval);
                if (dayOfInterval < 0)
                    dayOfInterval += dayInterval;
            }
            DateTime refreshTime = new DateTime(now.Year, now.Month, now.Day + dayOfInterval,
                TableMgr.singleton.ValueTable.refresh_data_time, 0, 0);
            if (refreshTime <= now)
            {
                refreshTime = refreshTime.AddDays(dayInterval);
            }
            return refreshTime;
        }

        public void Update()
        {
            if (!HasFixedServer)
                return;

            updateTimer += Time.unscaledDeltaTime;
            if (updateTimer >= 1f)//每秒检测
            {
                var now = Now;
                if (now >= RefreshTime_OneDay/* || now >= RefreshTime_TwoDay || now >= RefreshTime_OneWeek*/)
                {
                    FixedLocalTime(true);
                }
                updateTimer -= 1f;
            }
        }
    }
}