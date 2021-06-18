
using EZ;
using EZ.Data;
using EZ.DataMgr;

public class FirstPurchaseFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase == 1;
    }

    public override string GetUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3092);
        return tip;
    }
    public override string GetTinyUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3092);
        return tip;
    }
    public override string GetMiddleUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3092);
        return tip;
    }
    public override string GetLeftTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3092);
        return tip;
    }
    public override double GetDefault(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase;
    }
}
