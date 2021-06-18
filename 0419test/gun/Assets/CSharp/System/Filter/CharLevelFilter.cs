
using EZ;
using EZ.Data;

public class CharLevelFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
        if (condition.Length > 1 && curCharLevel >= condition[1])
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
        float cur = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
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
        int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3021);
        return string.Format(tip, condition[1]);
    }
    public override double GetDefault(float[] condition)
    {
        return Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
    }


}
