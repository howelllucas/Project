
using EZ;
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

public class BaseFilter
{

    public virtual bool Filter(float[] condition)
    {
        return false;
    }

    public virtual bool FilterQuest(float[] condition, float[] param)
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
        double cur = questItemDTO.cur;
        bool isUpdate = false;
        if (condition.Length > 1)
        {
            if (condition.Length != param.Length)
            {
                Debug.Log("FilterQuest condition.Length != param.Length");
                return false;
            }
            for (int i = 1; i < condition.Length - 1; i ++)
            {
                if (condition[i] != param[i])
                {
                    return false;
                }
            }
            questItemDTO.cur = cur + param[param.Length  - 1];
            isUpdate = true;
            if (questItemDTO.cur >= condition[condition.Length - 1] && questItemDTO.state == QuestStateConstVal.UNFINISH)
            {
                questItemDTO.state = QuestStateConstVal.CAN_RECEIVE;
            }
        }
        return isUpdate;
    }

    public virtual bool FilterCampTask(float[] condition, double[] param)
    {
        List<NpcQuestItemDTO> questItemDTOList = Global.gApp.gSystemMgr.GetNpcMgr().GetNpcQuestItemDTOListByQuestId((int)param[0]);
        
        if (questItemDTOList == null || questItemDTOList.Count == 0)
        {
            return false;
        }
        bool isUpdate = false;
        foreach (NpcQuestItemDTO questItemDTO in questItemDTOList)
        {
            double cur = questItemDTO.cur;
            if (condition.Length > 1)
            {
                if (condition.Length != param.Length)
                {
                    Debug.Log("FilterCampTask condition.Length != param.Length");
                    return false;
                }
                for (int i = 1; i < condition.Length - 1; i++)
                {
                    if (condition[i] != param[i])
                    {
                        return false;
                    }
                }
                questItemDTO.cur = cur + param[param.Length - 1];
                isUpdate = true;
                if (questItemDTO.cur >= condition[condition.Length - 1] && questItemDTO.state == NpcState.OnGoing)
                {
                    questItemDTO.state = NpcState.UnReceived;
                }
            }
        }
        return isUpdate;
    }

    public virtual string GetUnfinishTips(float[] condition)
    {
        return null;
    }

    public virtual string GetTinyUnfinishTips(float[] condition)
    {
        return null;
    }
    public virtual string GetMiddleUnfinishTips(float[] condition)
    {
        return null;
    }
    public virtual string GetLeftTips(float[] condition)
    {
        return null;
    }

    public virtual double GetDefault(float[] condition)
    {
        return 0d;
    }

    public virtual bool JudgeNewbieButton(float[] condition, NewbieGuideItem nConfig, NewbieGuideButton nButton)
    {
        return nConfig.param.Equals(nButton.Param);
    }

}
