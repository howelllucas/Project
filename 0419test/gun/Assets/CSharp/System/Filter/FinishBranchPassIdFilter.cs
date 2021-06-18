
using EZ;
using EZ.Data;
using EZ.DataMgr;
using System;

public class FinishBranchPassIdFilter : BaseFilter
{
    private static int m_BranchInitId = 400000;
    public override bool Filter(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetPassMgr().CheckBranchPassEnterd(Convert.ToInt32(condition[1]) + m_BranchInitId);
    }

    public override string GetUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(4484);
        return string.Format(tip, condition[1]);
    }
    public override string GetTinyUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(4484);
        return string.Format(tip, condition[1]);
    }
    public override string GetMiddleUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(4484);
        return string.Format(tip, condition[1]);
    }
    public override string GetLeftTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(4484);
        return string.Format(tip, condition[1]);
    }
    public override double GetDefault(float[] condition)
    {
        return 0;
    }
}
