using Game.Data;
using Game.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

namespace Game
{
    public class IdleRewardMgr : Singleton<IdleRewardMgr>
    {
        public delegate void ClaimRewardCallback(GoodsRequestResult result, BigInteger rewardCount);

        public const float DEFAULT_HOUR = 1f;
        public const float EACH_HERO_ADD_HOUR = 0.5f;
        public const int QUICK_IDLE_MINUTES = 120;
        public const int QUICK_IDLE_MAX_COUNT = 3;
        public bool IsStart { get { return isStart; } }
        private bool isStart;
        private DateTime startTime;
        private DateTime settlementTime;
        private List<IdleRewardSettlementData> settlementList;
        private int quickIdleIndex = 1;
        private float settlementTimer;
        private Action<bool> extraActOnServerFixed;

        public void Init()
        {
            if (PlayerDataMgr.singleton.DB.idleRewardData.startTimestamp > 0)
            {
                isStart = true;
                startTime = PTUtil.Timestamp2UtcDateTime(PlayerDataMgr.singleton.DB.idleRewardData.startTimestamp);
                settlementTime = PTUtil.Timestamp2UtcDateTime(PlayerDataMgr.singleton.DB.idleRewardData.settlementTimestamp);
                settlementList = PlayerDataMgr.singleton.DB.idleRewardData.CloneSettlementList();
                quickIdleIndex = PlayerDataMgr.singleton.DB.idleRewardData.quickIdleIndex;
                CalcSettlement();
                FixedSettlementTimer();
            }
            else
            {
                isStart = false;
                CheckStart();
            }
            DateTimeMgr.singleton.onLocalTimeFixed -= CheckSettlementOnServerFixed;
            DateTimeMgr.singleton.onLocalTimeFixed += CheckSettlementOnServerFixed;
            DateTimeMgr.singleton.onTimeRefresh -= OnDayRefresh;
            DateTimeMgr.singleton.onTimeRefresh += OnDayRefresh;
        }

        private void CheckStart()
        {
            if (isStart)
                return;
            if (DateTimeMgr.singleton.HasFixedServer)
            {
                StartReward();
            }
            else
            {
                DateTimeMgr.singleton.onLocalTimeFixed -= RefreshOnServerFixed;
                DateTimeMgr.singleton.onLocalTimeFixed += RefreshOnServerFixed;
            }
        }

        private void StartReward()
        {
            startTime = DateTimeMgr.singleton.UtcNow;
            settlementTime = startTime;
            settlementList = new List<IdleRewardSettlementData>();
            quickIdleIndex = 1;
            isStart = true;
            UpdateRecord();
        }

        private void RefreshOnServerFixed(bool success)
        {
            if (success)
            {
                CheckStart();
                DateTimeMgr.singleton.onLocalTimeFixed -= RefreshOnServerFixed;
            }
        }

        private void CheckSettlementOnServerFixed(bool success)
        {
            if (success)
            {
                if (isStart)
                {
                    DateTime maxTime = startTime + new TimeSpan(0, Mathf.FloorToInt(GetMaxIdleHour() * 60), 0);
                    DateTime checkSettlementTime = DateTimeMgr.singleton.UtcNow;
                    if (checkSettlementTime > maxTime)
                        checkSettlementTime = maxTime;
                    bool needFixed = false;
                    while (settlementTime > checkSettlementTime)
                    {
                        needFixed = true;
                        if (settlementList.Count <= 0)
                        {
                            settlementTime = startTime;
                            break;
                        }

                        var lastSettlement = settlementList[settlementList.Count - 1];
                        settlementTime -= new TimeSpan(0, lastSettlement.duration, 0);
                        settlementList.RemoveAt(settlementList.Count - 1);
                    }
                    if (settlementTime < startTime)
                        settlementTime = startTime;
                    if (needFixed)
                    {
                        CalcSettlement();
                        UpdateRecord();
                    }
                }
            }

            extraActOnServerFixed?.Invoke(success);
            extraActOnServerFixed -= extraActOnServerFixed;
        }

        public TimeSpan GetIdleTimeSpan()
        {
            if (!isStart)
                return new TimeSpan();
            var result = DateTimeMgr.singleton.UtcNow - startTime;
            var max = new TimeSpan(0, Mathf.FloorToInt(GetMaxIdleHour() * 60), 0);
            if (result > max)
                result = max;
            return result;
        }

        public float GetMaxIdleHour()
        {
            int heroNum = PlayerDataMgr.singleton.GetCollectCardCount();
            return GetMaxIdleHour(heroNum);
        }

        public float GetMaxIdleHour(int heroNum)
        {
            return DEFAULT_HOUR + heroNum * EACH_HERO_ADD_HOUR;
        }

        public float GetProcess()
        {
            float result = 0;
            if (isStart)
            {
                result = (float)GetIdleTimeSpan().TotalHours / GetMaxIdleHour();
            }

            return result;
        }

        public void RequestClaimIdleReward(ClaimRewardCallback callback)
        {
            if (!isStart)
            {
                callback?.Invoke(GoodsRequestResult.Undefine, 0);
                return;
            }

            extraActOnServerFixed += (fixSuccess) =>
            {
                if (fixSuccess)
                {
                    var rewardGold = GetRewardGold();
                    if (rewardGold <= 0)
                    {
                        callback?.Invoke(GoodsRequestResult.Success, 0);
                    }

                    GameGoodsMgr.singleton.RequestAddGameGoods((GoodsRequestResult result, List<GameGoodData> realAddGoods, HashSet<string> tips) =>
                    {
                        if (result == GoodsRequestResult.Success)
                        {
                            Debug.Log("Idle reward gold:" + rewardGold.ToSymbolString());
                            if ((DateTimeMgr.singleton.UtcNow - settlementTime).TotalSeconds > 61f)
                            {
                                settlementTime = DateTimeMgr.singleton.UtcNow;
                            }
                            startTime = settlementTime;
                            settlementList.Clear();
                            UpdateRecord();
                            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.IdleRewardClaim);

                            CampTaskMgr.singleton.AddTaskData(TaskType.Explore_Gold, 1);
                        }
                        callback?.Invoke(result, rewardGold);
                    },
                    (int)GoodsType.GOLD, rewardGold);
                }
                else
                {
                    callback?.Invoke(GoodsRequestResult.NetFail, 0);
                }
            };
            DateTimeMgr.singleton.FixedLocalTime();
        }

        public int GetGoldPerMinute()
        {
            //return 1000;//temp
            var maxUnlockStageId = PlayerDataMgr.singleton.GetMaxUnlockStageID();
            var data = TableMgr.singleton.LevelTable.GetItemByID(maxUnlockStageId);
            if (data != null)
                return data.idleSpeed;
            return 0;
        }

        public BigInteger GetRewardGold()
        {
            if (!isStart || settlementList == null)
                return 0;

            BigInteger result = 0;
            for (int i = 0; i < settlementList.Count; i++)
            {
                result += settlementList[i].duration * settlementList[i].goldPerMinute;
            }
            return result;
        }

        private void CalcSettlement()
        {
            DateTime maxTime = startTime + new TimeSpan(0, Mathf.FloorToInt(GetMaxIdleHour() * 60), 0);
            DateTime newSettlementTime = DateTimeMgr.singleton.UtcNow;
            if (newSettlementTime > maxTime)
                newSettlementTime = maxTime;
            int durationInMinute = Mathf.FloorToInt((float)(newSettlementTime - settlementTime).TotalMinutes);
            if (durationInMinute <= 0)
                return;
            AddSettlementData(durationInMinute, GetGoldPerMinute());
            settlementTime += new TimeSpan(0, durationInMinute, 0);
            UpdateRecord();

        }

        private void AddSettlementData(int duration, int goldPerMinute)
        {
            if (settlementList.Count <= 0)
            {
                settlementList.Add(new IdleRewardSettlementData()
                {
                    duration = duration,
                    goldPerMinute = goldPerMinute
                });
            }
            else
            {
                var last = settlementList[settlementList.Count - 1];
                if (goldPerMinute == last.goldPerMinute)
                    last.duration += duration;
                else
                {
                    settlementList.Add(new IdleRewardSettlementData()
                    {
                        duration = duration,
                        goldPerMinute = goldPerMinute
                    });
                }
            }
        }

        private void UpdateRecord()
        {
            PlayerDataMgr.singleton.DB.idleRewardData.startTimestamp = isStart ?
                PTUtil.UtcDateTime2Timestamp(startTime) : 0;
            PlayerDataMgr.singleton.DB.idleRewardData.settlementTimestamp = isStart ?
                PTUtil.UtcDateTime2Timestamp(settlementTime) : 0;
            PlayerDataMgr.singleton.DB.idleRewardData.UpdateSettlementList(settlementList);
            PlayerDataMgr.singleton.DB.idleRewardData.quickIdleIndex = quickIdleIndex;
            PlayerDataMgr.singleton.NotifySaveData();
        }

        public void Update()
        {
            if (!isStart)
                return;
            settlementTimer += Time.unscaledDeltaTime;

            if (settlementTimer >= 60f)
            {
                CalcSettlement();
                FixedSettlementTimer();
            }
        }

        private void FixedSettlementTimer()
        {
            settlementTimer = (DateTimeMgr.singleton.UtcNow - settlementTime).Seconds;
            Debug.Log("Fixed Settlement Time:" + settlementTimer);
        }

        public bool GetQuickIdleCost(out int diamond)
        {
            diamond = 0;
            //if (quickIdleIndex > QUICK_IDLE_MAX_COUNT)
            //{
            //    return false;
            //}
            if (quickIdleIndex > 1)
                diamond = 50 * (1 << (quickIdleIndex - 2));

            return true;
        }

        public int GetQuickIdleRemainder()
        {
            //int remainder = QUICK_IDLE_MAX_COUNT - quickIdleIndex + 1;
            //return remainder;
            return int.MaxValue;
        }

        public BigInteger GetQuickIdleRewardGold()
        {
            BigInteger rewardGold = QUICK_IDLE_MINUTES;
            rewardGold *= GetGoldPerMinute();
            return rewardGold;
        }

        public void RequestClaimQuickIdleReward(ClaimRewardCallback callback)
        {
            if (!isStart)
            {
                callback?.Invoke(GoodsRequestResult.NetFail, 0);
                return;
            }
            int costDiamond;
            if (!GetQuickIdleCost(out costDiamond))
            {
                callback?.Invoke(GoodsRequestResult.Undefine, 0);
                return;
            }
            var rewardGold = GetQuickIdleRewardGold();
            if (rewardGold <= 0)
            {
                callback?.Invoke(GoodsRequestResult.Success, 0);
                return;
            }

            extraActOnServerFixed += (fixSuccess) =>
            {
                if (fixSuccess)
                {
                    GameGoodsMgr.singleton.RequestCostAddGameGoods((GoodsRequestResult result, List<GameGoodData> realAddGoods, HashSet<string> tips, string failDetail) =>
                    {
                        if (result == GoodsRequestResult.Success)
                        {
                            Debug.Log("Idle quick reward gold:" + rewardGold.ToSymbolString());
                            quickIdleIndex++;
                            UpdateRecord();

                            CampTaskMgr.singleton.AddTaskData(TaskType.Quick_Reward, 1);
                        }
                        callback?.Invoke(result, rewardGold);
                    },
                    (int)GoodsType.DIAMOND, costDiamond, -1,
                    (int)GoodsType.GOLD, rewardGold, -1);
                }
                else
                {
                    callback?.Invoke(GoodsRequestResult.NetFail, 0);
                }
            };
            DateTimeMgr.singleton.FixedLocalTime();
        }

        private void OnDayRefresh(DateTimeRefreshType type)
        {
            if (type == DateTimeRefreshType.OneDay)
            {
                quickIdleIndex = 1;
                UpdateRecord();
            }
        }
    }
}