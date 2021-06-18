using EZ.Data;
using EZ.DataMgr;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampTaskDetails_DayTaskItemUI
    {
        public NpcQuestItemDTO m_DTO;
        private int m_Index;
        public int m_ShowIndex;
        public CampTaskDetails m_Parent;
        public void InitNode(NpcQuestItemDTO dto, int index, CampTaskDetails parent)
        {
            m_DTO = dto;
            m_Index = index;
            m_Parent = parent;
            CampTasksItem taskCfg = Global.gApp.gGameData.CampTasksConfig.Get(m_DTO.npcQuestId);
            gameObject.SetActive(true);
            if (taskCfg.taskCondition[0] == FilterTypeConstVal.GET_ITEM)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(Convert.ToInt32(taskCfg.taskCondition[1]));
                IName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(itemCfg.sourceLanguage);
                TargetIcon.image.sprite = Resources.Load(itemCfg.image_grow, typeof(Sprite)) as Sprite;
                TaskTargetText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(taskCfg.describe), Global.gApp.gGameData.GetTipsInCurLanguage(itemCfg.sourceLanguage));
                TaskTargetAmount.text.text = taskCfg.taskCondition[taskCfg.taskCondition.Length - 1].ToString();
            }
            else if (taskCfg.taskCondition[0] == FilterTypeConstVal.KILL_ZOMBIE)
            {
                IName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(4265);
                TargetIcon.image.sprite = Resources.Load(CommonResourceConstVal.CAMP_KILL_ICON, typeof(Sprite)) as Sprite;
                TaskTargetText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(taskCfg.describe);
                TaskTargetAmount.text.text = taskCfg.taskCondition[taskCfg.taskCondition.Length - 1].ToString();
            }
            else if (taskCfg.taskCondition[0] == FilterTypeConstVal.GET_ITEM_BY_TYPE)
            {
                IName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(4268);
                TargetIcon.image.sprite = Resources.Load(CommonResourceConstVal.CAMP_GET_TYPE_ITEM_ICON, typeof(Sprite)) as Sprite;
                TaskTargetText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(taskCfg.describe);
                TaskTargetAmount.text.text = taskCfg.taskCondition[taskCfg.taskCondition.Length - 1].ToString();
            }
            else
            {
                Debug.LogError("该条件类型未告知如何显示 taskCfg.taskCondition[0] = " + taskCfg.taskCondition[0]);
            }
            string cur = m_DTO.cur > taskCfg.taskCondition[taskCfg.taskCondition.Length - 1] ? taskCfg.taskCondition[taskCfg.taskCondition.Length - 1].ToString() : m_DTO.cur.ToString();
            Amount.text.text = cur + "/" + taskCfg.taskCondition[taskCfg.taskCondition.Length - 1].ToString();
            Progress.image.fillAmount = Convert.ToSingle(m_DTO.cur / taskCfg.taskCondition[taskCfg.taskCondition.Length - 1]);

            if (int.Parse(taskCfg.reward[0]) == SpecialItemIdConstVal.GOLD)
            {
                Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                AwardAmount.text.text = UiTools.FormateMoneyUP(double.Parse(taskCfg.reward[1]) * gpiCfg.coinParams);
            }
            else
            {
                AwardAmount.text.text = UiTools.FormateMoneyUP(int.Parse(taskCfg.reward[1]));
            }

            ItemItem awardItem = Global.gApp.gGameData.ItemData.Get(int.Parse(taskCfg.reward[0]));
            AwardIcon.image.sprite = Resources.Load(awardItem.image_grow, typeof(Sprite)) as Sprite;
            ChangeStateAndIndex();
            Btn.button.onClick.AddListener(OnReceive);
        }

        public void ChangeStateAndIndex()
        {
            OnGoing.gameObject.SetActive(m_DTO.state == NpcState.OnGoing);
            Received.gameObject.SetActive(m_DTO.state == NpcState.Received);
            int showIndex = m_Index;
            if (m_DTO.state == NpcState.UnReceived)
            {
                showIndex += 10000;
            }
            else if (m_DTO.state == NpcState.OnGoing)
            {
                showIndex += 20000;
            }
            else
            {
                showIndex += 30000;
            }
            m_ShowIndex = showIndex;
        }

        private void OnReceive()
        {
            if (m_DTO.state != NpcState.UnReceived)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3036);
                return;
            }
            Global.gApp.gSystemMgr.GetNpcMgr().ReceiveQuestReward(m_Index, AwardIcon.rectTransform.position);
            ChangeStateAndIndex();

            m_Parent.ChangeShowIndex();
        }

       
    }
}
