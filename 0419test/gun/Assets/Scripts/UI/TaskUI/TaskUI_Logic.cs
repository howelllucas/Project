using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace EZ
{

    public partial class TaskUI
    {
        GunCard_TableItem gunCardRes;
        //private Dictionary<int, GunChipUI_GunChip> gunChipDic = new Dictionary<int, GunChipUI_GunChip>();
        //private Dictionary<int, int> chipDic = new Dictionary<int, int>();
        private List<TaskUI_TaskItemUi> taskItemList = new List<TaskUI_TaskItemUi>();
        private int chipProgress = 0;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            ShowTaskInfo();
            TokenUI tokenUI = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUI?.SetBgImageActive(false);
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            RegisterListeners();

            NextCampBtn.button.onClick.AddListener(OnNextCampClick);
            
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskDataChange, UpdateTasks);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.TaskFinish, ShowTaskInfo);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskDataChange, UpdateTasks);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.TaskFinish, ShowTaskInfo);
        }
        public override void Recycle()
        {
            base.Recycle();
            TokenUI tokenUI = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUI?.SetBgImageActive(true);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
            TokenUI tokenUI = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUI?.SetBgImageActive(true);
        }

        public void HideRoot()
        {
            Root.gameObject.SetActive(false);
            TokenUI tokenUI = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUI?.SetBgImageActive(true);
        }

        public void ResetRoot()
        {
            Root.gameObject.SetActive(true);
            TokenUI tokenUI = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUI?.SetBgImageActive(false);
        }

        private void ShowTaskInfo(int taskNum = 0)
        {
            ClearShowList();

            var campRes = TableMgr.singleton.CampSetTable.GetItemByID(CampsiteMgr.singleton.Id);
            if (campRes == null)
                return;

            Leveltxt.text.text = string.Format("Rank {0}", PlayerDataMgr.singleton.GetPlayerLevel());

            var count = CampTaskMgr.singleton.GetCompleteTaskCount();
            if (count >= campRes.taskCount)
            {
                NextCampBtn.gameObject.SetActive(true);
                CampMapBtn.gameObject.SetActive(false);
                ProgressRoot.gameObject.SetActive(false);
            }
            else
            {
                TaskCount.text.text = string.Format("{0}/{1}", count, campRes.taskCount);
                TaskProgress.image.fillAmount = (float)count / campRes.taskCount;
                NextCampBtn.gameObject.SetActive(false);
                CampMapBtn.gameObject.SetActive(true);
                ProgressRoot.gameObject.SetActive(true);
            }

            var taskDic = CampTaskMgr.singleton.GetTaskDic(CampsiteMgr.singleton.Id);
            foreach (var task in taskDic.Values)
            {
                var res = TableMgr.singleton.CampTaskTable.GetItemByID(task.taskID);
                if (res == null)
                    continue;

                var taskItem = TaskItemUi.GetInstance();
                taskItem.Init(task);
                taskItem.transform.SetParent(TaskRoot.gameObject.transform);
                taskItem.gameObject.SetActive(true);
                taskItem.transform.SetAsLastSibling();
                taskItemList.Add(taskItem);
            }

        }

        private void ClearShowList()
        {
            foreach (var obj in taskItemList)
            {
                TaskItemUi.CacheInstance(obj);
            }
            taskItemList.Clear();

        }

        private void UpdateTasks(int taskIndex)
        {
            foreach (var obj in taskItemList)
            {
                obj.RefreshData();
            }
        }

        private void OnNextCampClick()
        {
            int id;
            if (!CampsiteMgr.singleton.HasNext(out id))
                return;

            //CampsiteMgr.singleton.RequestCreateNew(id, (rst, tip)=> {
            //    if (rst == CampsiteRequestResult.Success)
            //    {
            //        Global.gApp.gGameCtrl.ChangeToMainScene(3);
            //        ShowTaskInfo();

            //        Debug.Log("NextCamp");
            //    }
            //});

            CampsiteMgr.singleton.RequestFinishCampsite((rst, rewards) =>
            {
                if(rst == CampsiteRequestResult.Success)
                {
                    //temp
                    //打开ui
                    foreach (var reward  in rewards)
                    {
                        Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CurrencyChange, GameGoodsMgr.singleton.Goods2Currency(reward.type));
                    }

                    //关闭ui
                    {
                        Global.gApp.gGameCtrl.ChangeToMainScene(3, true);
                        ShowTaskInfo();

                        Debug.Log("NextCamp");
                    }
                }
            });
        }
    }
}