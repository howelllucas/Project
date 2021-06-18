
using EZ;
using EZ.Data;

public class SumLoginDayFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        if (condition.Length > 1 && cur >= condition[1])
        {
            return true;
        }
        return false;
    }

    public override bool FilterQuest(float[] condition, float[] param)
    {
        QuestItemDTO questItemDTO = null;
        if (param.Length >= 1)
        {
            questItemDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetQuestItemDTO((int)param[0]);
        }
        if (questItemDTO == null)
        {
            return false;
        }
        float cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        bool isUpdate = false;
        if (condition.Length > 1)
        {
            if (questItemDTO.cur < cur)
            {
                questItemDTO.cur = cur;
                isUpdate = true;
            }

            if (questItemDTO.cur >= condition[1] && questItemDTO.state == QuestStateConstVal.UNFINISH)
            {
                questItemDTO.state = QuestStateConstVal.CAN_RECEIVE;
                isUpdate = true;
            }
        }
        return isUpdate;
    }

    public override string GetUnfinishTips(float[] condition)
    {
        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3023);
        return string.Format(tip, condition[1]);
    }
    public override string GetTinyUnfinishTips(float[] condition)
    {
        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3049);
        //return string.Format(tipsConfig.txtcontent, condition[1] - cur);
        if (condition[1] == 2)
        {
            tip = Global.gApp.gGameData.GetTipsInCurLanguage(3065);
            return tip;
        }
        return string.Format(tip, condition[1]);
    }

    public override string GetMiddleUnfinishTips(float[] condition)
    {
        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3062);
        //return string.Format(tipsConfig.txtcontent, condition[1] - cur);
        if (condition[1] == 2)
        {
            tip = Global.gApp.gGameData.GetTipsInCurLanguage(3066);
            return tip;
        }
        return string.Format(tip, condition[1]);
    }
    public override string GetLeftTips(float[] condition)
    {
        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3064);
        return string.Format(tip, condition[1] - cur);
    }


    public override double GetDefault(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetMiscMgr().GetSumLoginDayNum();
    }

}
