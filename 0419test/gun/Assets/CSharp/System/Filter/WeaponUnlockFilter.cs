
using EZ;
using EZ.Data;
using System;

public class WeaponUnlockFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        int unlockNum = 0;
        foreach (ItemItem cfg in Global.gApp.gGameData.ShowOrderGun[Convert.ToInt32(condition[1])].Values)
        {
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg))
            {
                unlockNum++;
                if (unlockNum == condition[2])
                {
                    return true;
                }
            }
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

        double unlockNum = GetDefault(condition);

        bool isUpdate = false;
        if (condition.Length > 1)
        {
            if (questItemDTO.cur < unlockNum)
            {
                questItemDTO.cur = unlockNum;
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
        return "";
    }

    public override double GetDefault(float[] condition)
    {
        int unlockNum = 0;
        foreach (ItemItem cfg in Global.gApp.gGameData.ShowOrderGun[Convert.ToInt32(condition[1])].Values)
        {
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg))
            {
                unlockNum++;
            }
        }

        return unlockNum;

    }

    public override bool JudgeNewbieButton(float[] condition, NewbieGuideItem nConfig, NewbieGuideButton nButton)
    {

        int unlockNum = 0;
        ItemItem weaponCfg = null;
        foreach (ItemItem cfg in Global.gApp.gGameData.ShowOrderGun[Convert.ToInt32(condition[1])].Values)
        {
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(cfg))
            {
                unlockNum++;
                if (unlockNum == condition[2])
                {
                    weaponCfg = cfg;
                    break;
                }
            }
        }
        if (!nConfig.param.Equals("*"))
        {
            return true;
        }
        if (weaponCfg == null)
        {
            return false;
        }
        return weaponCfg.name.Equals(nButton.Param);
    }
}
