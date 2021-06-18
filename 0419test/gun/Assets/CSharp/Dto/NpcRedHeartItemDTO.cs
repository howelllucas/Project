//npc任务数据对象
using EZ;
using EZ.DataMgr;
using System;
using UnityEngine;

public class NpcRedHeartItemDTO
{
    //npcId
    public string npcId;
    // 上次ReadHeart刷新的天
    public double lastFreshDay;
    // 上次worker刷新的天
    public double lastWorkerFreshDay;
    // 矿工产出相关
    public double lastWorkerFreshTime;
    // 掉落的 砖石数量
    public int dropDiamondNum;
    // 今天掉落了多少次钻石
    public int curDayDropDiamondTimes;
    // 今天是否刷新过了
    public bool WorkerHasFocusOn;
    // 是否被暂停
    public bool WorkerHasFrozen;
    // 红心相关
    //上次刷新时间
    public double lastFreshTime;
    // 掉落的 红心数量
    public int dropHeartNum;
    public int curDayDropHeartNum;
    public int forceAddHeartNumCount;
    // 今天是否刷新过了
    public bool HasFocusOn;
    // 临时记录标签
    public bool LockHasFocusOnTemp;
    // 是否被暂停
    public bool HasFrozen;
    //结算消耗
    public double DailyConsumeBalanceTime;
    public NpcRedHeartItemDTO() { }
    public NpcRedHeartItemDTO(string npcId)
    {
        this.npcId = npcId;
        dropHeartNum = 0;
        curDayDropDiamondTimes = 0;
        lastFreshTime = DateTimeUtil.GetMills(DateTime.Now);
        lastWorkerFreshTime = lastFreshTime;
        DailyConsumeBalanceTime = lastFreshTime;
        lastFreshDay = DateTimeUtil.GetMills(DateTime.Today);
        lastWorkerFreshDay = lastFreshDay;
    }
    public double BalanceConsume()
    {
        if (!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(3))
        {
            return 0;
        }
        double curTime = DateTimeUtil.GetMills(DateTime.Now);
        double dtTime = curTime - DailyConsumeBalanceTime;
        if(dtTime > 0)
        {
            if (dtTime > 3600000)
            {
                DailyConsumeBalanceTime = curTime;
                return dtTime;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Debug.LogError(" 玩家 修改时间 ");
            return 0;
        }
    }
    public void ResetRedHeartTime()
    {
        lastFreshTime = DateTimeUtil.GetMills(DateTime.Now);
        lastFreshDay = DateTimeUtil.GetMills(DateTime.Today);
    }
    public void ResetConsume()
    {
        DailyConsumeBalanceTime = DateTimeUtil.GetMills(DateTime.Now);
    }
    public void FreshDropHeart(int maxTime)
    {
        if (!HasFrozen)
        {
            if(!Global.gApp.gSystemMgr.GetCampGuidMgr().GetStepFinished(1))
            {
                return;
            }
            int dtTime = Global.gApp.gGameData.CampNpcConfig.Get(npcId).FallDurTime[0];
            if(dtTime > 0)
            {
                // 最大数量限制
                int maxDropTotal = maxTime / dtTime;
                double curTime = DateTimeUtil.GetMills(DateTime.Now);
                // 当前红心最大储量
                if (dropHeartNum - forceAddHeartNumCount >= maxDropTotal)
                {
                    lastFreshTime = curTime;
                    lastFreshDay = DateTimeUtil.GetMills(DateTime.Today);
                    return;
                }

                string dayMaxDrop = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_HEART_DAYFALL_MAX).content;
                int maxDropCountDaily = int.Parse(dayMaxDrop);
                int newDtTime = dtTime * 1000;


                DateTime now = DateTime.Today;
                DateTime lastFreshD = DateTimeUtil.GetDate(lastFreshDay);
                int addDayNum = (now - lastFreshD).Days;
                if (addDayNum > 0)
                {
                    curDayDropHeartNum = 0;
                    lastFreshDay = DateTimeUtil.GetMills(DateTime.Today);
                }
                else
                {
                    maxDropCountDaily = maxDropCountDaily - curDayDropHeartNum;
                }
                // 最大掉落 等于在当天可掉落最大数量 以及 总最多还可以掉落多少取最小值
                maxDropCountDaily = Mathf.Min(maxDropCountDaily, maxDropTotal - dropHeartNum + forceAddHeartNumCount);
                // 已经不能再掉落了。就刷新一下时间
                if (maxDropCountDaily <= 0)
                {
                    lastFreshTime = curTime;
                    return;
                }
                // 开始掉落 最多掉落 maxDropCount 个
                double curTotalTime = curTime - lastFreshTime;
                for (int i = 1; i <= maxDropCountDaily; i++)
                {
                    if(curTotalTime >= newDtTime)
                    {
                        curTotalTime -= newDtTime;
                        dropHeartNum++;
                        curDayDropHeartNum++;
                    }
                    else
                    {
                        lastFreshTime = curTime - curTotalTime;
                        return;
                    }
                }
                lastFreshTime = curTime;
            }
        }
    }
    public void FreshWorkerState()
    {
        if (!WorkerHasFrozen)
        {
            string maxTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_WORKER_REWARD_MAXTIME).content;
            int maxTimeInt = int.Parse(maxTimeStr);
            string dtTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_WORKER_DUR).content;
            int dtTimeInt = int.Parse(dtTimeStr);


            int maxDropCount = maxTimeInt / dtTimeInt;
            int newDtTime = dtTimeInt * 1000;
            DateTime now = DateTime.Today;
            DateTime lastFresh = DateTimeUtil.GetDate(lastWorkerFreshDay);
            int addDayNum = (now - lastFresh).Days;
            // 隔了一天 按
            if (addDayNum > 0)
            {
                lastWorkerFreshDay = DateTimeUtil.GetMills(DateTime.Today);
                curDayDropDiamondTimes = 0;
            }
            else
            {
                if(maxDropCount - curDayDropDiamondTimes <= 0)
                {
                    return;
                }
            }
            string[] rewardContent = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_WORKER_REWARD_CONTENT).contents;
            string[] workerRewardNum = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_WORKER_REWARD_NUM).contents;
            string[] campWorkerRewardWeight = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_WORKER_REWARD_WEIGHT).contents;

            double curTime = DateTimeUtil.GetMills(DateTime.Now);
            double curTotalTime = curTime - lastWorkerFreshTime;
            int foreachTimes = (int)(curTotalTime / newDtTime);
            foreachTimes = Mathf.Min(foreachTimes, 1000);
            for (int i = 1; i <= foreachTimes; i++)
            {
                if(curDayDropDiamondTimes >= maxDropCount)
                {
                    lastWorkerFreshTime = curTime;
                    return;
                }
                if (curTotalTime >= newDtTime)
                {
                    curTotalTime -= newDtTime;
                    // 计算掉落 
                    int curVal = UnityEngine.Random.Range(0, 1001);
                    int calCal = 0;
                    int findIndex = 0;
                    foreach (string weight in campWorkerRewardWeight)
                    {
                        calCal += int.Parse(weight);
                        if(calCal >= curVal)
                        {
                            break;
                        }
                        else
                        {
                            findIndex++;
                        }
                    }
                    int rewardNum = int.Parse(workerRewardNum[findIndex]);
                    if (rewardNum > 0)
                    {
                        int rewardId = int.Parse(rewardContent[findIndex]);
                        curDayDropDiamondTimes++;
                        if(rewardId == SpecialItemIdConstVal.MDT)
                        {
                            dropDiamondNum += rewardNum;
                        }
                    }
                }
                else
                {
                    lastWorkerFreshTime = curTime - curTotalTime;
                    return;
                }
            }
            lastWorkerFreshTime = curTime;
        }
    }
    public bool ForceAddRedHeart(int count)
    {
        int dtTime = Global.gApp.gGameData.CampNpcConfig.Get(npcId).FallDurTime[0];
        if(dtTime > 0)
        {
            dropHeartNum += count;
            forceAddHeartNumCount += count;
            return true;
        }
        return false;
    }
    public int PickUpHeart(int num)
    {
        dropHeartNum -= num;
        forceAddHeartNumCount -= num;
        if(forceAddHeartNumCount < 0)
        {
            forceAddHeartNumCount = 0;
        }
        if (dropHeartNum >= 0)
        {
            return num;
        }
        else
        {
            Debug.LogError(" 领取的数量 异常 ");
            int addNum = dropHeartNum;
            dropHeartNum = 0;
            return addNum;
        }
    }
    public int PickUpDiamond()
    {
        if (dropDiamondNum > 0)
        {
            int addNum = dropDiamondNum;
            dropDiamondNum = 0;
            return addNum;
        }
        else
        {
            return 0;
        }
    }
    public void CheckDropHeartRrozen()
    {
        if (HasFocusOn)
        {
            if (HasFrozen)
            {
                lastFreshTime = DateTimeUtil.GetMills(DateTime.Now);
                lastFreshDay = DateTimeUtil.GetMills(DateTime.Today);
            }
            HasFrozen = false;
        }
        else
        {
            HasFrozen = true;
        }
    }
    public void CheckWorkerRrozenStae()
    {
        if (this.npcId == SpecialItemIdConstVal.NPC_WORKER_STR || this.npcId == SpecialItemIdConstVal.NPC_WORKER01_STR)
        {
            if (WorkerHasFocusOn)
            {
                if (WorkerHasFrozen)
                {
                    lastWorkerFreshTime = DateTimeUtil.GetMills(DateTime.Now);
                }
                WorkerHasFrozen = false;
            }
            else
            {
                this.WorkerHasFrozen = true;
            }
        }
        else
        {
            this.WorkerHasFrozen = true;
        }
    }
}
