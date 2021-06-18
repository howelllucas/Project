using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    //关卡数据
    public class StageDataKey
    {
        public static readonly string STAGE_ID = "stageID";                     // 关卡id
        public static readonly string FINISH = "finish";                        // 是否完成
        public static readonly string MAX_WAVES = "maxWaves";                   // 最高波数
        public static readonly string FINISH_WAVES = "finishWaves";             // 完成波数
        public static readonly string ENTER_TIMES = "enterTimes";               // 进入关卡次数
        public static readonly string IS_UNLOCK = "is_unlock";                  // 是否解锁
        public static readonly string STAR_LIST = "starList";                  // 获得关卡星级
    }

    [Serializable]
    public class StageData : ModelDataBase
    {
        public int stageID = 0;           //关卡id
        public bool finish = false;       //是否完成
        public int maxWaves = 0;          //最高波数
        public int finishWaves = 0;       //完成波数
        public int enterTimes = 0;        //进入关卡次数
        public bool isUnlock = false;     //是否解锁
        public List<int> starList = new List<int>();    //获得关卡星级

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [StageDataKey.STAGE_ID] = stageID,
                [StageDataKey.FINISH] = finish,
                [StageDataKey.MAX_WAVES] = maxWaves,
                [StageDataKey.FINISH_WAVES] = finishWaves,
                [StageDataKey.ENTER_TIMES] = enterTimes,
                [StageDataKey.IS_UNLOCK] = isUnlock,
            };

            JsonData starListJson = new JsonData();
            starListJson.SetJsonType(JsonType.Array);
            foreach (var star in starList)
            {
                starListJson.Add(star);
            }
            ret[StageDataKey.STAR_LIST] = starListJson;

            return ret;
        }
        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == StageDataKey.STAGE_ID)
                {
                    stageID = (int)data[key];
                }
                else if (key == StageDataKey.FINISH)
                {
                    finish = (bool)data[key];
                }
                else if (key == StageDataKey.MAX_WAVES)
                {
                    maxWaves = (int)data[key];
                }
                else if (key == StageDataKey.FINISH_WAVES)
                {
                    finishWaves = (int)data[key];
                }
                else if (key == StageDataKey.ENTER_TIMES)
                {
                    enterTimes = (int)data[key];
                }
                else if (key == StageDataKey.IS_UNLOCK)
                {
                    isUnlock = (bool)data[key];
                }
                else if (key == StageDataKey.STAR_LIST)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        starList.Add((int)subJson[i]);
                    }
                }
            }
            return true;
        }
    }

    //关卡数据
    public class ChapterDataKey
    {
        public static readonly string STARTSTAGEID = "startStageID";                     // 起始关卡id
        public static readonly string STAR_LIST = "starList";                  // 领取奖励星级列表
    }

    [Serializable]
    public class ChapterData : ModelDataBase
    {
        public int startStageID = 0;           //起始关卡id
        public List<int> starList = new List<int>();    //领取奖励星级列表

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [ChapterDataKey.STARTSTAGEID] = startStageID,

            };

            JsonData starListJson = new JsonData();
            starListJson.SetJsonType(JsonType.Array);
            foreach (var star in starList)
            {
                starListJson.Add(star);
            }
            ret[ChapterDataKey.STAR_LIST] = starListJson;

            return ret;
        }
        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == ChapterDataKey.STARTSTAGEID)
                {
                    startStageID = (int)data[key];
                }                
                else if (key == StageDataKey.STAR_LIST)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        starList.Add((int)subJson[i]);
                    }
                }
            }
            return true;
        }
    }
}