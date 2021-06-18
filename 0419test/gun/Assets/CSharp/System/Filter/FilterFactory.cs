
using EZ;
using EZ.Data;
using System.Collections.Generic;

//条件判定工厂
public class FilterFactory
{
    private static FilterFactory m_Instance = new FilterFactory();

    private Dictionary<float, BaseFilter> m_FilterMap = new Dictionary<float, BaseFilter>();
    public FilterFactory()
    {
        m_FilterMap.Add(FilterTypeConstVal.NONE, new NoneFilter());
        m_FilterMap.Add(FilterTypeConstVal.COSUME_ONCE, new ConsumeOnceFilter());
        m_FilterMap.Add(FilterTypeConstVal.CHAR_LEVEL, new CharLevelFilter());
        m_FilterMap.Add(FilterTypeConstVal.PASS_ID, new PassIdFilter());
        m_FilterMap.Add(FilterTypeConstVal.SUM_LOGIN_DAY, new SumLoginDayFilter());
        m_FilterMap.Add(FilterTypeConstVal.KILL_ZOMBIE, new KillZoombieFilter());
        m_FilterMap.Add(FilterTypeConstVal.CONSTIOUS_LOGIN, new ContinuousLoginFilter());
        m_FilterMap.Add(FilterTypeConstVal.WATCH_AD, new WatchAdFilter());
        m_FilterMap.Add(FilterTypeConstVal.TEMP_CONSTIOUS_LOGIN, new TempContinuousLoginFilter());
        m_FilterMap.Add(FilterTypeConstVal.TRY_PASS_ID, new TryPassIdFilter());
        m_FilterMap.Add(FilterTypeConstVal.WEAPON_UNLOCK, new WeaponUnlockFilter());
        m_FilterMap.Add(FilterTypeConstVal.CUR_ITEM_NUM, new CurItemNumFilter());
        m_FilterMap.Add(FilterTypeConstVal.FIRST_PURCHASE, new FirstPurchaseFilter());
        m_FilterMap.Add(FilterTypeConstVal.PURCHASE, new PurchaseFilter());
        m_FilterMap.Add(FilterTypeConstVal.GET_ITEM, new GetItemFilter());
        m_FilterMap.Add(FilterTypeConstVal.GET_ITEM_BY_TYPE, new GetItemByTypeFilter());
        m_FilterMap.Add(FilterTypeConstVal.CAMP, new CampFilter());
        m_FilterMap.Add(FilterTypeConstVal.FINISH_CAMP_STEP, new FinishCampStepFilter());
        m_FilterMap.Add(FilterTypeConstVal.FINISH_BRANCH_PASS_ID, new FinishBranchPassIdFilter());
    }

    public bool Filter(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.Filter(condition);
        } else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return false;
        }
    }

    public bool FilterQuest(float[] condition, float[] param)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.FilterQuest(condition, param);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return false;
        }
    }

    public bool FilterCampTask(float[] condition, double[] param)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.FilterCampTask(condition, param);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return false;
        }
    }

    public string GetUnfinishTips(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.GetUnfinishTips(condition);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return null;
        }
    }

    public string GetTinyUnfinishTips(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.GetTinyUnfinishTips(condition);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return null;
        }
    }

    public string GetMiddleUnfinishTips(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.GetMiddleUnfinishTips(condition);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return null;
        }
    }

    public string GetLeftTips(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.GetLeftTips(condition);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return null;
        }
    }

    public double GetDefault(float[] condition)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.GetDefault(condition);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return 0d;
        }
    }
    public bool JudgeNewbieButton(float[] condition, NewbieGuideItem nConfig, NewbieGuideButton nButton)
    {
        BaseFilter filter;
        if (m_FilterMap.TryGetValue(condition[0], out filter))
        {
            return filter.JudgeNewbieButton(condition, nConfig, nButton);
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3019, condition[0].ToString());
            return false;
        }
    }

    public static FilterFactory GetInstance()
    {
        return m_Instance;
    }

}
