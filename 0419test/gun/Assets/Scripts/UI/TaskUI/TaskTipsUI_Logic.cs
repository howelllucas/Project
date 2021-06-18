using DG.Tweening;
using Game;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class TaskTipsUI
    {
        private TaskInfo taskInfo;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            //FinishTaskBtn.button.onClick.AddListener(OnTaskClick);

            //RegisterListeners();
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            var index = int.Parse(arg as string);
            UpdateTasks(index);
        }

        public override void Recycle()
        {
            //UnRegisterListeners();
            base.Recycle();
        }

        public override void Release()
        {
            //UnRegisterListeners();
            base.Release();
        }

        //private void RegisterListeners()
        //{
        //    UnRegisterListeners();
        //    Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskDataChange, UpdateTasks);
        //}

        //private void UnRegisterListeners()
        //{
        //    Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskDataChange, UpdateTasks);
        //}
    

        private void UpdateTasks(int taskIndex)
        {
            var info = CampTaskMgr.singleton.GetTaskInfoByIdx(taskIndex);
            if (info == null)
                return;

            var taskRes = TableMgr.singleton.CampTaskTable.GetItemByID(info.taskID);
            if (taskRes == null)
                return;

            if (!CampTaskMgr.singleton.isReach(taskIndex))
            {
                return;
            }

            taskInfo = info;
            TitleText.text.text = CampTaskMgr.singleton.GetName(taskInfo.index);
            //FinishText.text.text = CampTaskMgr.singleton.GetDesc(taskInfo.index);
            //Num.text.text = taskRes.reward_count.ToString();
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
            TaskItemUi.gameObject.SetActive(true);

            Invoke("TouchClose", 3.0f);
            //Global.gApp.CurScene.GetTimerMgr().AddTimer(3.0f, 1, (a,b)=> {
            //    TaskItemUi.gameObject.SetActive(false);
            //    TouchClose();
            //});


        }

        //private void ShowTask()
        //{
        //    Global.gApp.gUiMgr.OpenPanel(Wndid.TaskInfoUI, taskInfo);
        //    TouchClose();
        //}
    }
}