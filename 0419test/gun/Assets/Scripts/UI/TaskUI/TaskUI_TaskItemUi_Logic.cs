using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;

namespace EZ
{
    public partial class TaskUI_TaskItemUi
    {
        private TaskInfo taskInfo;
        private List<GameObject> starList = new List<GameObject>();

        private void Awake()
        {
            InitNode();
        }
        public void Init(TaskInfo info)
        {
            taskInfo = info;
            RefreshData();
        }

        public void RefreshData()
        {
            if (taskInfo == null)
                return;

            var taskRes = TableMgr.singleton.CampTaskTable.GetItemByID(taskInfo.taskID);
            if (taskRes == null)
                return;

            var typeRes = TableMgr.singleton.CampTaskTypeTable.GetItemByID(taskInfo.taskType);
            if (typeRes == null)
                return;

            TitleText.text.text = CampTaskMgr.singleton.GetName(taskInfo.index);
            //Num.text.text = item.reward_count.ToString();
            icon.image.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon((GoodsType)taskRes.reward);
            switch ((GoodsType)taskRes.reward)
            {
                case GoodsType.GOLD_MINUTE:
                    {
                        var value = (System.Numerics.BigInteger)IdleRewardMgr.singleton.GetGoldPerMinute() * taskRes.reward_count;
                        Num.text.text = value.ToSymbolString();
                    }
                    break;
                default:
                    {
                        Num.text.text = taskRes.reward_count.ToString();
                    }
                    break;
            }
            if (CampTaskMgr.singleton.isReach(taskInfo.index))
            {
                FinishTaskBtn.gameObject.SetActive(true);
            }
            else if (typeRes.progress == 1)
            {
                var curCount = CampTaskMgr.singleton.GetData(taskInfo.index);
                prNum.text.text = string.Format("{0}/{1}", curCount.ToSymbolString(),
                                                taskInfo.conditionValue.ToSymbolString());
                passProgress.image.fillAmount = (float)curCount / (float)taskInfo.conditionValue;
                taskProgress.gameObject.SetActive(true);
                FinishTaskBtn.gameObject.SetActive(false);
            }
            else
            {
                taskProgress.gameObject.SetActive(false);
                FinishTaskBtn.gameObject.SetActive(false);
            }
        }

        private void InitNode()
        {
            TaskBtn.button.onClick.AddListener(OnClick);
            FinishTaskBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.TaskInfoUI, taskInfo);
        }

    }
}