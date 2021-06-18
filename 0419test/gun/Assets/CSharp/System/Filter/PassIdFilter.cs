
using EZ;
using EZ.Data;
using System;

public class PassIdFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        if (condition.Length > 1 && curPassId % Convert.ToInt32(initPassIdConfig.content) - 1 >= condition[1])
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
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        float cur = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() % Convert.ToInt32(initPassIdConfig.content) - 1;
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
        int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3022);
        return string.Format(tip, condition[1]);
    }
    public override string GetTinyUnfinishTips(float[] condition)
    {
        int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3048);
        //return string.Format(tipsConfig.txtcontent, condition[1] + int.Parse(initPassIdConfig.content) - curPassId + 1);
        return string.Format(tip, condition[1]);
    }
    public override string GetMiddleUnfinishTips(float[] condition)
    {
        int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3061);
        //return string.Format(tipsConfig.txtcontent, condition[1] + int.Parse(initPassIdConfig.content) - curPassId + 1);
        return string.Format(tip, condition[1]);
    }
    public override string GetLeftTips(float[] condition)
    {
        int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        string tip = Global.gApp.gGameData.GetTipsInCurLanguage(3063);
        return string.Format(tip, condition[1] + int.Parse(initPassIdConfig.content) - curPassId + 1);
    }

    public override double GetDefault(float[] condition)
    {
        float cur = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        return cur % Convert.ToInt32(initPassIdConfig.content) - 1;
    }
}
