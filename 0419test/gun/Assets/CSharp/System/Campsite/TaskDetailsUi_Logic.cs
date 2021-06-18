using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;

namespace EZ
{
    public partial class TaskDetailsUi
    {
        int m_TaskIndex;
        private RectTransform m_FollowRectTsf;
        private RectTransform m_ParentRectTsf;
        private Action m_CloseCallBack = null;
        // todo  增加 奖励 领取状态。此时身上无任务。
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_FollowRectTsf = m_TaskAdaptNode.gameObject.GetComponent<RectTransform>();
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            m_ParentRectTsf = parentCanvas.GetComponent<RectTransform>();

            gameObject.AddComponent<DelayCallBack>().SetAction(TouchClose, 5);
            base.ChangeLanguage();
        }
        public void InitDetailsInfo(NpcQuestItemDTO taskItem , int taskIndex,Transform adaptNode,Action closeCallBack)
        {
            m_CloseCallBack = closeCallBack;
            m_TaskIndex = taskIndex;
            m_FollowRectTsf.anchoredPosition = UiTools.WorldToRectPos(gameObject, adaptNode.position, m_ParentRectTsf);
            CampTasksItem campTasksItem = Global.gApp.gGameData.CampTasksConfig.Get(taskItem.npcQuestId);
            if(campTasksItem != null)
            {
                TitleText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(campTasksItem.title);
                prNum.text.text = ((int)taskItem.cur).ToString() +"/" + campTasksItem.taskCondition[2].ToString();
                m_passProgress.image.fillAmount = (float)taskItem.cur / campTasksItem.taskCondition[2];


                ItemItem conditonItem = Global.gApp.gGameData.ItemData.Get((int)campTasksItem.taskCondition[1]);
                if (conditonItem != null)
                {
                    taskIcon.image.sprite = Resources.Load(conditonItem.image_grow, typeof(Sprite)) as Sprite;
                }
                else
                {
                    if (campTasksItem.taskCondition[0] == FilterTypeConstVal.KILL_ZOMBIE)
                    {
                        taskIcon.image.sprite = Resources.Load(CommonResourceConstVal.CAMP_KILL_ICON, typeof(Sprite)) as Sprite;
                    }
                    else if (campTasksItem.taskCondition[0] == FilterTypeConstVal.GET_ITEM_BY_TYPE)
                    {
                        taskIcon.image.sprite = Resources.Load(CommonResourceConstVal.CAMP_GET_TYPE_ITEM_ICON, typeof(Sprite)) as Sprite;
                    }
                    else
                    {
                        taskIcon.image.sprite = Resources.Load(CommonResourceConstVal.CAMP_KILL_ICON, typeof(Sprite)) as Sprite;
                    }
                }
                ItemItem rewardItem = Global.gApp.gGameData.ItemData.Get(int.Parse(campTasksItem.reward[0]));
                if(rewardItem!= null)
                {
                    icon.image.sprite = Resources.Load(rewardItem.image_grow, typeof(Sprite)) as Sprite;
                }
                if (rewardItem.id == SpecialItemIdConstVal.GOLD)
                {
                    double itemNum = double.Parse(campTasksItem.reward[1]);
                    Gold_paramsItem cfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                    itemNum *= cfg.coinParams;
                    Num.text.text = UiTools.FormateMoneyUP(itemNum);
                }
                else
                {
                    Num.text.text = UiTools.FormateMoneyUP(double.Parse(campTasksItem.reward[1]));
                }
            }
            if(taskItem.state == NpcState.UnReceived)
            {
                WaitRewardBtn.gameObject.SetActive(false);
                RewardBtn.gameObject.SetActive(true);
                RewardBtn.button.onClick.AddListener(ReceiveReward);
            }
            else
            {
                WaitRewardBtn.gameObject.SetActive(true);
                RewardBtn.gameObject.SetActive(false);
            }
        }
        public override void TouchClose()
        {
            if(m_CloseCallBack != null)
            {
                m_CloseCallBack();
            }
            base.TouchClose();
        }
        private void ReceiveReward()
        {
            Global.gApp.gSystemMgr.GetNpcMgr().ReceiveQuestReward(m_TaskIndex, RewardBtn.rectTransform.position);
            TouchClose();
        }
    }
}
