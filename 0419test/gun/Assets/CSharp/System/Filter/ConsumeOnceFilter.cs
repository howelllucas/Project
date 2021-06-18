
using EZ;
using EZ.Data;

public class ConsumeOnceFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        return false;
    }

    public override bool FilterQuest(float[] condition, float[] param)
    {
        return false;
    }

    public override string GetUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3020);
        return string.Format(tip, condition[2]);
    }

    public override double GetDefault(float[] condition)
    {
        return 0d;
    }
}
