using EZ;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace Game
{
    public class CampTaskMgr : Singleton<CampTaskMgr>
    {
        private CampTaskData campTaskData = null;
        private CampSet_TableItem campSetRes = null;
        private int taskWeight = 0;
        private Dictionary<int, Task_TableItem> taskDic = new Dictionary<int, Task_TableItem>();

        public void Init()
        {
            campTaskData = PlayerDataMgr.singleton.DB.campTaskData;

            campSetRes = TableMgr.singleton.CampSetTable.GetItemByID(CampsiteMgr.singleton.Id);
        }

        void SetCamp(int id)
        {
            campSetRes = TableMgr.singleton.CampSetTable.GetItemByID(id);
            if (campSetRes == null)
                return;

            campTaskData.campID = id;
            campTaskData.taskCount = campSetRes.taskCount;
            campTaskData.taskDic.Clear();
            campTaskData.fixedTasks.Clear();
            campTaskData.completeTasks.Clear();

            for (int i = 0; i < campSetRes.FixedTaskArr.Length; ++i)
            {
                //campTaskData.fixedTasks.Add(campSetRes.FixedTaskArr[i]);
                campTaskData.fixedTasks.Add(new FixedTask
                {
                    taskID = campSetRes.FixedTaskArr[i],
                    taskParam = campSetRes.TaskParamArr[i]
                });
            }

            PlayerDataMgr.singleton.NotifySaveData();
        }

        public int GetContainTaskCount(int type)
        {
            int count = 0;
            foreach (var task in campTaskData.taskDic.Values) 
            {
                if (task.taskType == type)
                    count++;
            }

            return count;
        }

        public Dictionary<int, TaskInfo> GetTaskDic(int campID)
        {
            if (campID != campTaskData.campID || campSetRes == null)
            {
                SetCamp(campID);
            }

            if ((campTaskData.taskCount > 0 || campSetRes.taskCount == 0) &&
                campTaskData.taskDic.Count <= campSetRes.taskField)
            {
                var count = campSetRes.taskField - campTaskData.taskDic.Count;

                if (count > 0)
                {
                    if (campTaskData.fixedTasks.Count > 0)
                    {//创建指定任务
                        for (int i = 0; i < campTaskData.fixedTasks.Count; ++i)
                        {
                            var taskID = campTaskData.fixedTasks[i].taskID;
                            var task = TableMgr.singleton.CampTaskTable.GetItemByID(taskID);
                            if (task == null)
                                continue;

                            //if (GetContainTaskCount(task.type) > 0)
                            //    continue;

                            if (!CreateTask(task, campTaskData.fixedTasks[i].taskParam))
                                continue;

                            campTaskData.fixedTasks.RemoveAt(i);
                            i--;
                            campTaskData.taskCount--;
                            count--;
                            if (count <= 0)
                                break;
                        }

                        count = campSetRes.taskField - campTaskData.taskDic.Count;
                        PlayerDataMgr.singleton.NotifySaveData();
                    }
                }

                if (count > 0 && GetContainTaskCount((int)TaskType.LvUp_Building) <= 0)
                {//优先创建升级建筑任务

                    //for (int i = 0; i < count; ++i)
                    //{
                    //var task = TableMgr.singleton.CampTaskTable.GetItemByID((int)TaskType.LvUp_Building);
                    var task = GetTaskByType(TaskType.LvUp_Building);
                    if (task != null/* && !GetContainTaskCount(task.type)*/)
                    {
                        if (CreateTask(task))
                        {
                            campTaskData.taskCount--;

                        }
                    }
                    //}

                    count = campSetRes.taskField - campTaskData.taskDic.Count;
                    PlayerDataMgr.singleton.NotifySaveData();
                }

                if (count > 0 && GetContainTaskCount((int)TaskType.Occupy_Building) <= 0)
                {//优先创建占领建筑任务
                    //var task = TableMgr.singleton.CampTaskTable.GetItemByID((int)TaskType.Occupy_Building);
                    var task = GetTaskByType(TaskType.Occupy_Building);
                    if (task != null/* && !GetContainTaskCount(task.type)*/)
                    {
                        if (CreateTask(task))
                        {
                            campTaskData.taskCount--;
                            count = campSetRes.taskField - campTaskData.taskDic.Count;
                            PlayerDataMgr.singleton.NotifySaveData();
                        }
                    }
                }

                if (count > 0)
                {//随机任务
                    for (int i = 0; i < count;)
                    {
                        ResetTaskRandom();
                        var task = RandomTask();
                        if (!CreateTask(task))
                            continue;

                        campTaskData.taskCount--;
                        i++;
                    }

                    PlayerDataMgr.singleton.NotifySaveData();
                }
            }


            return campTaskData.taskDic;
        }

        private Task_TableItem GetTaskByType(TaskType type)
        {
            List<Task_TableItem> taskList = new List<Task_TableItem>();
            foreach (Task_TableItem task in TableMgr.singleton.CampTaskTable.getEnumerator())
            {
                if (task.level != campSetRes.taskLevel)
                    continue;

                if ((int)type != task.type)
                    continue;

                taskList.Add(task);
            }

            Debug.Log("taskList " + taskList.Count);
            if (taskList.Count <= 0)
                return null;

            var index = Random.Range(0, taskList.Count);

            return taskList[index];
        }

        private void ResetTaskRandom()
        {
            taskWeight = 0;
            taskDic.Clear();
            var lastType = 0;
            if (campTaskData.completeTasks.Count > 0)
            {
                lastType = campTaskData.completeTasks[campTaskData.completeTasks.Count - 1].taskID;
            }

            foreach (Task_TableItem task in TableMgr.singleton.CampTaskTable.getEnumerator())
            {
                if (task.level != campSetRes.taskLevel)
                    continue;

                if (task.weight <= 0)
                    continue;

                if (GetContainTaskCount(task.type) > 0)
                    continue;

                if (lastType == task.type)
                    continue;

                int cd = 0;
                if (campTaskData.taskCdDic.TryGetValue(task.type, out cd))
                {
                    if (cd > 0)
                        continue;
                }

                if ((TaskType)task.type == TaskType.Equip_Gun || (TaskType)task.type == TaskType.Supplies_Add)
                {
                    var pointData = CampsiteMgr.singleton.GetPointByIndex(0);
                    if (pointData == null)
                        continue;

                    if (!pointData.isUnlock)
                        continue;
                }

                taskWeight += task.weight;
                taskDic[taskWeight] = task;
            }

            Debug.Log("taskDic " + taskDic.Count);
        }

        private Task_TableItem RandomTask()
        {
            var rate = Random.Range(0, taskWeight);
            var all = 0;
            foreach (var item in taskDic)
            {
                all += item.Key;
                if (rate < all)
                {
                    return item.Value;
                }
            }

            return null;
        }

        private bool CreateTask(Task_TableItem taskRes, int param = -1)
        {
            if (taskRes == null)
                return false;

            var index = GetTaskIndex();
            if (index <= 0)
                return false;

            var info = CreateTaskInfo(taskRes, param);
            if (info == null)
                return false;

            info.index = index;
            info.number = campTaskData.taskDic.Count + campTaskData.completeTasks.Count + 1;
            campTaskData.taskDic[index] = info;

            Debug.Log("CreateTask " + info.number);
            //for (int i = 0; i < campTaskData.taskDic.Count; ++i)
            //{
            //    campTaskData.taskDic[i].index = i;
            //}
            //if (campTaskData.taskDic.Count < TableMgr.singleton.ValueTable.camp_task_max_count)

            //info.index = campTaskData.taskDic.Count;

            //int index = 0;
            //for(int i = 0;i < campTaskData.taskDic.Count;++i)
            //{
            //    if (i != campTaskData.taskDic[i].index)
            //    {
            //        index = i;
            //        break;
            //    }
            //    index++;
            //}

            //info.index = index;
            //campTaskData.taskDic.Insert(index, info);
            //Debug.Log("CreateTask " + index);

            return true;
        }

        private TaskInfo CreateTaskInfo(Task_TableItem task, int param)
        {
            var taskTypeRes = TableMgr.singleton.CampTaskTypeTable.GetItemByID(task.type);
            if (taskTypeRes == null)
                return null;

            var info = new TaskInfo();
            info.taskID = task.id;
            info.taskType = task.type;

            if (param > 0)
                info.taskParam = param - 1;

            switch ((TaskType)task.type)
            {
                //case TaskType.Finish_Battle:
                //    {
                //        info.conditionValue = GetFinishTaskCount(task.type) + task.value + campSetRes.taskDifficulty;
                //    }
                //    break;
                case TaskType.Occupy_Building:
                    {
                        if (info.taskParam >= 0)
                        {
                            var pointData = CampsiteMgr.singleton.GetPointByIndex(info.taskParam);
                            if (pointData == null)
                                return null;

                            info.conditionValue = 1;
                            return info;
                        }
                        else
                        {
                            CampsitePointMgr pointData = null;
                            for (int i = 0; i < campSetRes.CampBuildingArr.Length; ++i)
                            {
                                pointData = CampsiteMgr.singleton.GetPointByIndex(i);
                                if (pointData == null)
                                    continue;

                                if (!pointData.isUnlock)
                                {
                                    info.taskParam = i;
                                    info.conditionValue = 1;
                                    return info;
                                }
                            }
                        }

                        return null;

                    }
                case TaskType.Equip_Gun:
                    {
                        if (info.taskParam < 0)
                        {
                            List<int> buildingList = new List<int>();
                            for (int i = 0; i < campSetRes.CampBuildingArr.Length; ++i)
                            {
                                var pointData = CampsiteMgr.singleton.GetPointByIndex(i);
                                if (pointData == null)
                                    continue;

                                if (pointData.isUnlock)
                                {
                                    buildingList.Add(campSetRes.CampBuildingArr[i]);
                                }
                            }

                            if (buildingList.Count > 0)
                                info.taskParam = BaseRandom.Next(0, buildingList.Count);
                            else
                                return null;
                        }

                        info.conditionValue = GetFinishTaskCount(task.type) * task.value *
                                                campSetRes.taskDifficulty +
                                                campSetRes.AutoLevelArr[info.taskParam];
                    }
                    break;
                case TaskType.LvUp_Building:
                    {
                        if (info.taskParam < 0)
                        {
                            //var index = GetFinishTaskCount(task.type) + GetContainTaskCount(task.type);
                            //Debug.Log("index " + index);
                            //var pointData = CampsiteMgr.singleton.GetPointByIndex(index);
                            //if (pointData == null)
                            //    return null;

                            //if (!pointData.isUnlock)
                            //{
                            //    return null;
                            //}
                            int index = -1;
                            for (int i = 0; i < campSetRes.CampBuildingArr.Length; ++i)
                            {
                                var pointData = CampsiteMgr.singleton.GetPointByIndex(i);
                                if (pointData == null)
                                    continue;

                                if (pointData.isUnlock)
                                {
                                    index = i;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (index <= GetFinishTaskParam(task.type))
                                return null;

                            info.taskParam = index;
                        }

                        info.conditionValue = (int)(task.value * campSetRes.taskDifficulty *
                                                campSetRes.id * 0.3f) + 5;
                    }
                    break;
                case TaskType.Buildings_Level:
                    {
                        info.conditionValue = task.value * campSetRes.taskDifficulty *
                                                campSetRes.id / 2;
                    }
                    break;
                case TaskType.LevelUp_Gun:
                    {
                        if (info.taskParam < 0)
                        {
                            var typeList = PlayerDataMgr.singleton.GetGunCardTypeList();
                            if (typeList.Count <= 0)
                                return null;

                            info.taskParam = typeList[BaseRandom.Next(0, typeList.Count)];
                        }
                        info.conditionValue = GetFinishTaskCount(task.type) * 2 + task.value + campSetRes.taskDifficulty;
                    }
                    break;
                case TaskType.Supplies_Add:
                    {
                        //if (CampsiteMgr.singleton.TotalRewardRate <= 0.0f)
                        //{
                        //    return null;
                        //}

                        info.conditionValue = (BigInteger)(CampsiteMgr.singleton.TotalRewardRate *
                                                task.value * campSetRes.taskDifficulty);
                    }
                    break;
                case TaskType.Cost_Gold:
                case TaskType.Get_Gold:
                    {
                        info.conditionValue = (BigInteger)(IdleRewardMgr.singleton.GetGoldPerMinute() *
                                                (GetFinishTaskCount(task.type) + 1) * task.value * 0.5f *
                                                campSetRes.taskDifficulty);
                    }
                    break;
                case TaskType.Quick_Reward:
                case TaskType.Open_Box:
                case TaskType.Fuse_Gun:                
                case TaskType.Explore_Gold:
                case TaskType.Lv10_Gun:
                case TaskType.Finish_Battle:
                    {
                        info.conditionValue = task.value;
                    }
                    break;
                case TaskType.Epic_Gun:
                    {
                        info.conditionValue = 1;
                    }
                    break;
                case TaskType.Auto_Level:
                    {
                        if (info.taskParam < 0)
                        {
                            int index = 0;
                            for (int i = 0; i < campSetRes.CampBuildingArr.Length; ++i)
                            {
                                var pointData = CampsiteMgr.singleton.GetPointByIndex(i);
                                if (pointData == null)
                                    continue;

                                if (pointData.isUnlock)
                                {
                                    index = i;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            info.taskParam = index;
                        }

                        var data = CampsiteMgr.singleton.GetPointByIndex(info.taskParam);
                        if (data == null)
                            return null;
                        info.conditionValue = 1;
                    }
                    break;
            }

            return info;
        }

        private int GetTaskIndex()
        {
            for (int i = 1; i <= campSetRes.taskField; ++i)
            {
                if (!campTaskData.taskDic.ContainsKey(i))
                    return i;
            }

            return 0;
        }

        public TaskInfo GetTaskInfoByIdx(int index)
        {
            TaskInfo task;
            if (!campTaskData.taskDic.TryGetValue(index, out task))
                return null;

            return task;
        }

        public TaskInfo GetTaskInfoByID(int taskID)
        {
            foreach(var task in campTaskData.taskDic.Values)
            {
                if (task.taskID == taskID)
                    return task;
            }

            return null;
        }

        public TaskInfo GetTaskInfoByNumber(int number)
        {
            foreach (var task in campTaskData.taskDic.Values)
            {
                if (task.number == number)
                    return task;
            }

            return null;
        }

        //public TaskInfo GetTaskInfoByType(int type, int param = -1)
        //{
        //    for (int i = 0;i < campTaskData.taskDic.Count;++i)
        //    {
        //        if (campTaskData.taskDic[i].taskType == type &&
        //            campTaskData.taskDic[i].taskParam == param)
        //            return campTaskData.taskDic[i];
        //    }

        //    return null;
        //}

        public int GetFinishTaskCount(int type)
        {
            var count = 0;
            foreach (var task in campTaskData.completeTasks)
            {
                if (task.taskID == type)
                    count++;
            }

            return count;
        }

        public int GetFinishTaskParam(int type)
        {
            var param = -1;
            foreach (var task in campTaskData.completeTasks)
            {
                if (task.taskID == type && param < task.taskParam)
                    param = task.taskParam;
            }

            return param;
        }

        public bool FinishTask(int index)
        {
            TaskInfo task = GetTaskInfoByIdx(index);
            if (task == null)
                return false;

            if (!isReach(index))
                return false;

            var res = TableMgr.singleton.CampTaskTable.GetItemByID(task.taskID);
            if (res == null)
                return false;

            GameGoodsMgr.singleton.RequestAddGameGoods((rst, goods, tips) =>
            {
                if (rst != GoodsRequestResult.Success)
                    return;

                campTaskData.completeTasks.Add(new FixedTask
                {
                    taskID = task.taskType,
                    taskParam = task.taskParam
                });
                campTaskData.taskDic.Remove(index);

                foreach (var key in campTaskData.taskCdDic.Keys.ToArray())
                {
                    if (campTaskData.taskCdDic[key] <= 0)
                        continue;

                    campTaskData.taskCdDic[key] -= 1;
                }

                var typeRes = TableMgr.singleton.CampTaskTypeTable.GetItemByID(res.type);
                if (typeRes != null && typeRes.cd > 0)
                {
                    campTaskData.taskCdDic[res.type] = typeRes.cd;
                }

                PlayerDataMgr.singleton.NotifySaveData();

                EZ.Global.gApp.gMsgDispatcher.Broadcast<int>(EZ.MsgIds.TaskFinish, task.number);

            }, res.reward, res.reward_count, res.reward_type);


            return true;
        }


        /// <summary>
        /// 是否达到完成标准.
        /// </summary>
        /// <returns></returns>
        public bool isReach(int index)
        {
            TaskInfo task = GetTaskInfoByIdx(index);
            if (task == null)
                return false;

            if (task.isFinish)
                return true;

            if (GetData(index) >= task.conditionValue)
                return true;

            return false;
        }

        public int GetTaskReach()
        {
            //foreach (var task in curTaskDic)
            //{
            //    foreach (Task_TableItem task in taskTable.getEnumerator())
            //    {
            //        if (isOver(task.id))
            //            continue;
            //        if (isReach(task.id))
            //            return task.id;
            //    }
            //}

            return 0;
        }

        public void AddTaskData(TaskType type, BigInteger value, int param = -1)
        {
            if (campTaskData == null)
                return;

            //TaskInfo task = GetTaskInfoByType((int)type, param);
            //if (task == null)
            //    return;
            foreach (var task in campTaskData.taskDic.Values)
            {
                if (task.taskType != (int)type)
                    continue;

                if (task.isFinish)
                    continue;

                if (task.taskParam != param)
                    continue;

                task.curValue += value;

                if (isReach(task.index))
                {
                    task.isFinish = true;
                    Global.gApp.gUiMgr.OpenPanel(Wndid.TaskTipsUI, task.index.ToString());
                }
                PlayerDataMgr.singleton.NotifySaveData();

                EZ.Global.gApp.gMsgDispatcher.Broadcast<int>(EZ.MsgIds.TaskDataChange, task.index);
            }

        }
        /// <summary>
        /// 获得当前完成进度值
        /// </summary>
        /// <returns></returns>
        public BigInteger GetData(int index)
        {
            TaskInfo task = GetTaskInfoByIdx(index);
            if (task == null)
                return 0;

            switch ((TaskType)task.taskType)
            {
                case TaskType.Occupy_Building:
                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return 0;

                        if (pointData.isUnlock)
                        {
                            return 1;
                        }
                    }
                    break;
                case TaskType.LvUp_Building:
                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return 0;

                        if (pointData.isUnlock)
                        {
                            return pointData.Lv;
                        }
                    }
                    break;
                case TaskType.Buildings_Level:
                    {
                        int count = 0;
                        for (int i = 0; i < campSetRes.CampBuildingArr.Length; ++i)
                        {
                            var pointData = CampsiteMgr.singleton.GetPointByIndex(i);
                            if (pointData == null)
                                continue;

                            if (pointData.isUnlock)
                            {
                                count += pointData.Lv;
                            }
                        }

                        return count;

                    }
                    break;
                case TaskType.Equip_Gun:
                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return 0;

                        var cardData = PlayerDataMgr.singleton.GetGunCardData(pointData.equipGunId);
                        if (cardData != null)
                        {
                            return cardData.level;
                        }

                    }
                    break;
                case TaskType.Fuse_Gun:
                    {
                        return PlayerDataMgr.singleton.GetFusedCardList().Count;

                    }
                case TaskType.Epic_Gun:
                    {
                        var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(PlayerDataMgr.singleton.GetUseWeaponID());
                        if (cardRes != null)
                        {
                            if (cardRes.rarity >= (int)CardQualityType.EPIC)
                                return 1;
                        }
                    }
                    break;
                case TaskType.Lv10_Gun:
                    {
                        return PlayerDataMgr.singleton.GetCardsByLevel(10).Count;
                    }
                case TaskType.Auto_Level:
                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return 0;

                        if (pointData.isAuto)
                            return 1;
                    }
                    break;
                default:
                    {
                        return task.curValue;
                    }

            }

            return 0;
        }

        /// <summary>
        /// 获得任务描述
        /// </summary>
        /// <returns></returns>
        public string GetDesc(int index)
        {
            TaskInfo task = GetTaskInfoByIdx(index);
            if (task == null)
                return "";

            var item = TableMgr.singleton.CampTaskTypeTable.GetItemByID(task.taskType);
            if (item == null)
            {
                return "";
            }

            switch ((TaskType)task.taskType)
            {
                case TaskType.Occupy_Building:
                case TaskType.LvUp_Building:
                case TaskType.Equip_Gun:
                case TaskType.Auto_Level:
                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return "";

                        return LanguageMgr.GetText(item.tid_desc, pointData.buildingRes.buildingName, task.conditionValue);
                    }
                    break;
                case TaskType.LevelUp_Gun:
                    {
                        var res = TableMgr.singleton.GunTypeTable.GetItemByID(task.taskParam);
                        if (res == null)
                            return "";

                        return LanguageMgr.GetText(item.tid_desc, LanguageMgr.GetText(res.tid_type), task.conditionValue);
                    }
                    break;
                case TaskType.Cost_Gold:
                case TaskType.Get_Gold:
                case TaskType.Supplies_Add:
                    {
                        return LanguageMgr.GetText(item.tid_desc, task.conditionValue.ToSymbolString());
                    }
                    break;

            }

            //if (!string.IsNullOrEmpty(item.desc))
            //    return string.Format(item.desc, task.conditionValue);

            return LanguageMgr.GetText(item.tid_desc, task.conditionValue);
        }

        public string GetName(int index)
        {
            TaskInfo task = GetTaskInfoByIdx(index);
            if (task == null)
                return "";

            var item = TableMgr.singleton.CampTaskTypeTable.GetItemByID(task.taskType);
            if (item == null)
            {
                return "";
            }

            switch ((TaskType)task.taskType)
            {
                case TaskType.Occupy_Building:
                case TaskType.LvUp_Building:
                case TaskType.Auto_Level:

                    {
                        var pointData = CampsiteMgr.singleton.GetPointByIndex(task.taskParam);
                        if (pointData == null)
                            return "";

                        return LanguageMgr.GetText(item.tid_name, pointData.buildingRes.buildingName, task.conditionValue);
                    }
                    break;
                case TaskType.LevelUp_Gun:
                    {
                        var res = TableMgr.singleton.GunTypeTable.GetItemByID(task.taskParam);
                        if (res == null)
                            return "";

                        return LanguageMgr.GetText(item.tid_name, LanguageMgr.GetText(res.tid_type), task.conditionValue);
                    }
                    break;

            }

            //if (!string.IsNullOrEmpty(item.name))
            //    return string.Format(item.name, task.conditionValue);

            return LanguageMgr.GetText(item.tid_name, task.conditionValue);
        }

        public int GetCompleteTaskCount()
        {
            return campTaskData.completeTasks.Count;
        }
    }
}