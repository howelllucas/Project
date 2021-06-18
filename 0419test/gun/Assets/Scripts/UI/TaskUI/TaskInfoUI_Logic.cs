using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;
using System.Numerics;

namespace EZ
{
    public partial class TaskInfoUI
    {
        private TaskInfo taskInfo;
        private Task_TableItem taskRes;
        private List<GameObject> starList = new List<GameObject>();

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            taskInfo = arg as TaskInfo;
            if (taskInfo == null)
                return;

            ShowTask();

        }
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            GotoBtn.button.onClick.AddListener(OnGotoClick);
            FinishBtn.button.onClick.AddListener(OnFinishClick);
            CloseBtn.button.onClick.AddListener(TouchClose);

            RegisterListeners();
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void ShowTask()
        {
            if (taskInfo == null)
                return;

            taskRes = TableMgr.singleton.CampTaskTable.GetItemByID(taskInfo.taskID);
            if (taskRes == null)
                return;

            var typeRes = TableMgr.singleton.CampTaskTypeTable.GetItemByID(taskInfo.taskType);
            if (typeRes == null)
                return;

            TitleText.text.text = CampTaskMgr.singleton.GetDesc(taskInfo.index);

            icon.image.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon((GoodsType)taskRes.reward);
            switch ((GoodsType)taskRes.reward)
            {
                case GoodsType.GOLD_MINUTE:
                    {
                        var value = (BigInteger)IdleRewardMgr.singleton.GetGoldPerMinute() * taskRes.reward_count;
                        Num.text.text = value.ToSymbolString();
                    }
                    break;
                default:
                    {
                        Num.text.text = taskRes.reward_count.ToString();
                    }
                    break;
            }

            if (typeRes.progress == 1)
            {
                var curCount = CampTaskMgr.singleton.GetData(taskInfo.index);
                prNum.text.text = string.Format("{0}/{1}", curCount.ToSymbolString(),
                                                taskInfo.conditionValue.ToSymbolString());
                passProgress.image.fillAmount = (float)curCount / (float)taskInfo.conditionValue;
                taskProgress.gameObject.SetActive(true);

                var type = (TaskType)taskRes.type;
                if (type == TaskType.Supplies_Add || type == TaskType.Buildings_Level)
                {
                    GotoBtn.gameObject.SetActive(false);
                }
                else
                {
                    GotoBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                taskProgress.gameObject.SetActive(false);
                GotoBtn.gameObject.SetActive(false);
            }

            if (CampTaskMgr.singleton.isReach(taskInfo.index))
            {
                FinishBtn.gameObject.SetActive(true);
                GotoBtn.gameObject.SetActive(false);
            }
        }

        private void RegisterListeners()
        {

        }

        private void UnRegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.GunCardOpt, OnGunDataChange);
        }

        private void OnFinishClick()
        {
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, (GoodsType)taskRes.reward,
                        taskRes.reward_count, icon.gameObject.transform.position);

            if (CampTaskMgr.singleton.FinishTask(taskInfo.index))
            {
                TouchClose();
            }
        }

        private void OnGotoClick()
        {
            switch ((TaskType)taskRes.type)
            {
                case TaskType.LevelUp_Gun:
                case TaskType.Cost_Gold:
                case TaskType.Fuse_Gun:
                case TaskType.Epic_Gun:
                case TaskType.Lv10_Gun:
                    {
                        Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.CommonUIIndexChange, 1);
                        TouchClose();
                    }
                    break;
                case TaskType.Open_Box:
                    {
                        Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CommonUIIndexChange4Param, 3, "box");
                        TouchClose();
                    }
                    break;
                case TaskType.Finish_Battle:
                case TaskType.Quick_Reward:
                case TaskType.Get_Gold:
                case TaskType.Explore_Gold:
                    {
                        Global.gApp.gUiMgr.OpenPanel(Wndid.ExpeditionUI);
                        TouchClose();
                    }
                    break;
                case TaskType.Occupy_Building:
                    {
                        Global.gApp.gUiMgr.OpenPanel(Wndid.CampsiteUnlockUI, taskInfo.taskParam.ToString());
                        TouchClose();
                    }
                    break;
                case TaskType.LvUp_Building:
                case TaskType.Auto_Level:
                case TaskType.Equip_Gun:
                    {
                        Global.gApp.gUiMgr.OpenPanel(Wndid.CampsitePointUI, taskInfo.taskParam.ToString());
                        Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.CommonUIIndexChange, 2);
                        TouchClose();
                    }
                    break;
                default:
                    break;
            }

        }
    }
}