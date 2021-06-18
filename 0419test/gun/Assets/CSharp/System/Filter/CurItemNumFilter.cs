
using EZ;
using EZ.Data;
using EZ.DataMgr;

public class CurItemNumFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        int itemId = (int)condition[1];
        return GameItemFactory.GetInstance().GetItem(itemId) >= condition[2];
    }

    public override bool FilterQuest(float[] condition, float[] param)
    {
        return Filter(condition);
    }

    public override string GetUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3087);
        ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get((int)condition[1]);
        return string.Format(tip, itemConfig.gamename, condition[2]);
    }
    public override string GetTinyUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3088);
        ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get((int)condition[1]);
        return string.Format(tip, itemConfig.gamename, condition[2]);
    }
    public override string GetMiddleUnfinishTips(float[] condition)
    {
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3089);
        ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get((int)condition[1]);
        return string.Format(tip, itemConfig.gamename, condition[2]);
    }
    public override string GetLeftTips(float[] condition)
    {
        int itemId = (int)condition[1];
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3087);
        ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get((int)condition[1]);
        return string.Format(tip, condition[1] + condition[2] - GameItemFactory.GetInstance().GetItem(itemId), itemConfig.gamename);
    }
    public override double GetDefault(float[] condition)
    {
        int itemId = (int)condition[1];
        return GameItemFactory.GetInstance().GetItem(itemId);
    }
}
