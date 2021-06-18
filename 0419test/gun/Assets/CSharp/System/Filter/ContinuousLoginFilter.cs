
using EZ;

public class ContinuousLoginFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {

        int cur = Global.gApp.gSystemMgr.GetMiscMgr().GetContinousLoginDayNum();
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
        float cur = Global.gApp.gSystemMgr.GetMiscMgr().GetContinousLoginDayNum();
        bool isUpdate = false;
        if (condition.Length > 1)
        {
            if(questItemDTO.cur < cur)
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
        return null;
    }

    public override double GetDefault(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetMiscMgr().GetContinousLoginDayNum();
    }

}
