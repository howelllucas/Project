//NPC数据对象
using EZ;
using EZ.Data;
using System;
using System.Collections.Generic;

public class NpcDTO
{
    //npc任务map
    public List<NpcQuestItemDTO> npcQuestList;
    //上次刷新广告展示时间
    public double lastAdShowTime;
    public double leftAdHeart;
    public int heartAdWatchTimes;
    //上次刷新时间
    public double lastFreshTime;
    //npc的map
    public Dictionary<string, ItemDTO> npcMap;
    //npc的物品map
    public Dictionary<string, ItemDTO> npcAwardMap;
    //npc任务map
    public List<NpcRedHeartItemDTO> npcRedHeartList;
    // 守卫上次刷新的天数
    public double GuardGetRewardDay;
    // 守卫 当天累积获取时间
    public double GuardCurDataRewardTime;
    // 守卫 领取奖励的时间
    public double GuardGetRewardTime;
    // 营地商店商品购买次数
    public Dictionary<string, int> CampShopTimesMap;
    // 进入营地界面的时间
    public double OpenCampTime;
    // 营地buff
    public Dictionary<string, SkillItemDTO> campBuffMap;
    // 营地界面显示等级
    public int campShowLevel;

    public bool showBoyNpc = true;
    public NpcDTO()
    {
        npcQuestList = new List<NpcQuestItemDTO>();
        lastFreshTime = 0d;
        leftAdHeart = 0;
        GuardGetRewardDay = 0;
        lastAdShowTime = 0d;
        heartAdWatchTimes = 0;
        npcMap = new Dictionary<string, ItemDTO>();
        npcAwardMap = new Dictionary<string, ItemDTO>();
        npcRedHeartList = new List<NpcRedHeartItemDTO>();
        CampShopTimesMap = new Dictionary<string, int>();
        campBuffMap = new Dictionary<string, SkillItemDTO>();
        campShowLevel = 1;
        showBoyNpc = true;
    }
    public double AddAdRedHeart(double num)
    {
        leftAdHeart += num;
        return leftAdHeart;
    }
    public double PickRedHeart(double num)
    {
        if(leftAdHeart >= num)
        {
            leftAdHeart -= num;
            return num;
        }
        else
        {
            return 0;
        }
    }
    public void FirstEnterFresh()
    {
        GuardCurDataRewardTime = 0;
        GuardGetRewardTime = DateTimeUtil.GetMills(DateTime.Now);
        GuardGetRewardDay = DateTimeUtil.GetMills(DateTime.Today);
    }
    public double AddGuardReward()
    {
        double rewardCount = GetGuardCurDataRewardCount();
        if(rewardCount > 0)
        {
            GuardGetRewardTime = DateTimeUtil.GetMills(DateTime.Now);
            double canGetRewardTime = GetGuardCurDataRewardLeftTime();
            GuardCurDataRewardTime += canGetRewardTime;
            GuardGetRewardDay = DateTimeUtil.GetMills(DateTime.Today);
        }
        return rewardCount;
    }
    private double GetGuardCurDataRewardLeftTime()
    {
        DateTime now = DateTime.Today;
        DateTime lastFresh = DateTimeUtil.GetDate(GuardGetRewardDay);
        int addDayNum = (now - lastFresh).Days;
        double canGetRewardTime = 0;
        string maxTimeStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_SOLDIER_REWARD_MAX_TIME).content;
        double maxTimeD = double.Parse(maxTimeStr);


        double dtTime = DateTimeUtil.GetMills(DateTime.Now) - GuardGetRewardTime;
        canGetRewardTime = dtTime / 1000;
        // 隔了一天 按
        if (addDayNum > 0)
        {
            GuardCurDataRewardTime = 0;
            canGetRewardTime = Math.Min(maxTimeD, canGetRewardTime);
        }
        else
        {
            if (GuardCurDataRewardTime >= maxTimeD)
            {
                return 0;
            }
            canGetRewardTime = Math.Min((maxTimeD - GuardCurDataRewardTime), canGetRewardTime);
        }
        return canGetRewardTime;
    }
    public double GetGuardCurDataRewardCount()
    {
        double canGetRewardTime = GetGuardCurDataRewardLeftTime();
        string rewardParamStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_SOLDIER_REWARD_PARAMS).content;
        double rewardParamD = double.Parse(rewardParamStr);
        Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
        return canGetRewardTime * rewardParamD * gpiCfg.coinParams;
    }
}
