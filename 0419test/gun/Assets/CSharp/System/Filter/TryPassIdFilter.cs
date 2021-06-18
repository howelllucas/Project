
using EZ;
using EZ.Data;
using System;

public class TryPassIdFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        float cur = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() % Convert.ToInt32(initPassIdConfig.content);
        int enterTimes = Global.gApp.gSystemMgr.GetPassMgr().GetCurEnterTimes();
        if (enterTimes == 0)
        {
            cur = cur - 1;
        }

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
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        float cur = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId() % Convert.ToInt32(initPassIdConfig.content);
        int enterTimes = Global.gApp.gSystemMgr.GetPassMgr().GetCurEnterTimes();
        if (enterTimes == 0)
        {
            cur = cur - 1;
        }
       
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

    public override double GetDefault(float[] condition)
    {
        float cur = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
        GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
        int enterTimes = Global.gApp.gSystemMgr.GetPassMgr().GetCurEnterTimes();
        if (enterTimes == 0)
        {
            return cur % Convert.ToInt32(initPassIdConfig.content) - 1;
        } else
        {
            return cur % Convert.ToInt32(initPassIdConfig.content);
        }
        
    }
}
