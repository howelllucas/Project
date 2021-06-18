using LitJson;
using System;
using System.Collections.Generic;
using System.Numerics;
using Game.Util;

namespace Game.Data
{
    public class CampTaskDataKey
    {
        public static readonly string CAMPID = "campID";
        public static readonly string TASKCOUNT = "taskCount";
        public static readonly string FIXEDTASKS = "fixedTasks";
        public static readonly string COMPLETETASKS = "completeTasks";
        public static readonly string TASKDIC = "taskDic";
        public static readonly string TASKCDDIC = "taskCdDic";
    }

    public class CampTaskData : ModelDataBase
    {
        public int campID = 0;
        public int taskCount = 0;                                                       //营地任务数量
        public Dictionary<int, TaskInfo> taskDic = new Dictionary<int, TaskInfo>();     //营地任务列表
        public List<FixedTask> fixedTasks = new List<FixedTask>();                      //指定任务列表    
        public List<FixedTask> completeTasks = new List<FixedTask>();                   //完成任务列表    
        public Dictionary<int, int> taskCdDic = new Dictionary<int, int>();             //营地任务CD列表

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [CampTaskDataKey.CAMPID] = campID,
                [CampTaskDataKey.TASKCOUNT] = taskCount,
            };

            JsonData fixedTasksJson = new JsonData();
            fixedTasksJson.SetJsonType(JsonType.Array);
            foreach (var task in fixedTasks)
            {
                fixedTasksJson.Add(task.GetJsonData());
            }
            ret[CampTaskDataKey.FIXEDTASKS] = fixedTasksJson;

            JsonData completeTasksJson = new JsonData();
            completeTasksJson.SetJsonType(JsonType.Array);
            foreach (var task in completeTasks)
            {
                completeTasksJson.Add(task.GetJsonData());
            }
            ret[CampTaskDataKey.COMPLETETASKS] = completeTasksJson;

            JsonData taskJson = new JsonData();
            taskJson.SetJsonType(JsonType.Array);
            foreach (var key in taskDic.Keys)
            {
                taskJson.Add(taskDic[key].GetJsonData());
            }
            ret[CampTaskDataKey.TASKDIC] = taskJson;

            if (taskCdDic.Count > 0)
            {
                JsonData taskCDJson = new JsonData();
                //taskCDJson.SetJsonType(JsonType.Array);
                foreach (var item in taskCdDic)
                {
                    taskCDJson[item.Key.ToString()] = item.Value;
                }

                ret[CampTaskDataKey.TASKCDDIC] = taskCDJson;
            }

            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {

            foreach (var key in data.Keys)
            {
                if (key == CampTaskDataKey.TASKCOUNT)
                {
                    taskCount = (int)data[key];
                }
                else if (key == CampTaskDataKey.CAMPID)
                {
                    campID = (int)data[key];
                }
                else if (key == CampTaskDataKey.COMPLETETASKS)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        var task = new FixedTask();
                        task.InitWithJson(subJson[i]);
                        completeTasks.Add(task);
                    }
                }
                else if (key == CampTaskDataKey.FIXEDTASKS)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        var task = new FixedTask();
                        task.InitWithJson(subJson[i]);
                        fixedTasks.Add(task);
                    }
                }
                else if (key == CampTaskDataKey.TASKDIC)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        TaskInfo task = new TaskInfo();
                        task.InitWithJson(subJson[i]);
                        taskDic[task.index] = task;
                    }
                }
                else if (key == CampTaskDataKey.TASKCDDIC)
                {
                    taskCdDic.Clear();
                    var taskCdDicJson = data[key];
                    foreach (var idStr in taskCdDicJson.Keys)
                    {
                        int cd;
                        if (int.TryParse(idStr, out cd))
                        {
                            taskCdDic[cd] = (int)taskCdDicJson[idStr];
                        }
                    }
                }
            }
            return true;
        }
    }


    public class TaskInfoKey
    {
        public static readonly string TASKID = "taskID";
        public static readonly string TASKTYPE = "taskType";
        public static readonly string TASKPARAM = "taskParam";
        public static readonly string INDEX = "index";
        public static readonly string NUMBER = "number";
        public static readonly string CONDITIONVALUE = "conditionValue";
        public static readonly string CURVALUE = "curValue";
        public static readonly string ISFINISH = "isFinish";
    }

    public class TaskInfo : ModelDataBase
    {
        public int taskID = 0;                  //任务id
        public int taskType = 0;                //任务类型
        public int taskParam = -1;              //任务参数
        public int index = 0;                   //任务位置索引
        public int number = 0;                  //任务序号
        public BigInteger conditionValue = 0;   //条件数值
        public BigInteger curValue = 0;         //当前数值
        public bool isFinish = false;           //是否完成

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [TaskInfoKey.TASKID] = taskID,
                [TaskInfoKey.TASKTYPE] = taskType,
                [TaskInfoKey.TASKPARAM] = taskParam,
                [TaskInfoKey.INDEX] = index,
                [TaskInfoKey.NUMBER] = number,
                [TaskInfoKey.CONDITIONVALUE] = conditionValue.ToString(),
                [TaskInfoKey.CURVALUE] = curValue.ToString(),
                [TaskInfoKey.ISFINISH] = isFinish,
            };
            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == TaskInfoKey.TASKID)
                {
                    taskID = (int)data[key];
                }
                else if (key == TaskInfoKey.TASKTYPE)
                {
                    taskType = (int)data[key];
                }
                else if (key == TaskInfoKey.TASKPARAM)
                {
                    taskParam = (int)data[key];
                }
                else if (key == TaskInfoKey.INDEX)
                {
                    index = (int)data[key];
                }
                else if (key == TaskInfoKey.NUMBER)
                {
                    number = (int)data[key];
                }
                else if (key == TaskInfoKey.CONDITIONVALUE)
                {
                    BigInteger value;
                    if (BigInteger.TryParse(data[key].ToString(), out value))
                    {
                        conditionValue = value;
                    }
                }
                else if (key == TaskInfoKey.CURVALUE)
                {
                    BigInteger value;
                    if (BigInteger.TryParse(data[key].ToString(), out value))
                    {
                        curValue = value;
                    }
                }
                else if (key == TaskInfoKey.ISFINISH)
                {
                    isFinish = (bool)data[key];
                }
            }
            return true;
        }
    }

    public class FixedTaskKey
    {
        public static readonly string TASKID = "taskID";
        public static readonly string TASKPARAM = "taskParam";
    }

    public class FixedTask : ModelDataBase
    {
        public int taskID = 0;
        public int taskParam = -1;

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [TaskInfoKey.TASKID] = taskID,
                [TaskInfoKey.TASKPARAM] = taskParam,
            };
            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == TaskInfoKey.TASKID)
                {
                    taskID = (int)data[key];
                }
                else if (key == TaskInfoKey.TASKPARAM)
                {
                    taskParam = (int)data[key];
                }
                
            }
            return true;
        }
    }
}