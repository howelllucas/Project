using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using Game.Util;

namespace Game.Data
{
    public class CampsiteDataKey
    {
        public static readonly string ID = "id";
        public static readonly string POINTS = "points";
        public static readonly string TOTAL_REWARD_VAL = "totalRewardVal";
        public static readonly string LAST_RECORD_TIME = "lastRecordTime";
        public static readonly string HAS_OFFLINE_REWARD = "hasOfflineReward";
    }

    public class CampsiteData : ModelDataBase
    {
        public int id;
        public CampsitePointData[] points;
        public double totalRewardVal;
        public DateTime lastRecordTime;
        public bool hasOfflineReward;

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == CampsiteDataKey.ID)
                {
                    id = (int)data[key];
                }
                else if (key == CampsiteDataKey.POINTS)
                {
                    var subJsonArr = data[key];
                    points = new CampsitePointData[subJsonArr.Count];
                    for (int i = 0; i < subJsonArr.Count; i++)
                    {
                        points[i] = new CampsitePointData();
                        points[i].InitWithJson(subJsonArr[i]);
                    }
                }
                else if (key == CampsiteDataKey.TOTAL_REWARD_VAL)
                {
                    totalRewardVal = (double)data[key];
                }
                else if (key == CampsiteDataKey.LAST_RECORD_TIME)
                {
                    lastRecordTime = PTUtil.JsonData2DateTime(data[key]);
                }
                else if (key == CampsiteDataKey.HAS_OFFLINE_REWARD)
                {
                    hasOfflineReward = (bool)data[key];
                }
            }
            return true;
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [CampsiteDataKey.ID] = id,
                [CampsiteDataKey.TOTAL_REWARD_VAL] = totalRewardVal,
                [CampsiteDataKey.LAST_RECORD_TIME] = PTUtil.DateTime2Timestamp(lastRecordTime),
                [CampsiteDataKey.HAS_OFFLINE_REWARD] = hasOfflineReward,
            };

            JsonData pointsJsonArr = new JsonData();
            pointsJsonArr.SetJsonType(JsonType.Array);
            for (int i = 0; i < points.Length; i++)
            {
                pointsJsonArr.Add(points[i].GetJsonData());
            }

            ret[CampsiteDataKey.POINTS] = pointsJsonArr;

            return ret;

        }
    }

    public class CampsitePointDataKey
    {
        public static readonly string INDEX = "index";
        public static readonly string IS_UNLOCK = "isUnlock";
        public static readonly string UNLOCK_STAGE = "UnlockStage";
        public static readonly string EQUIP_GUN_ID = "equipGunId";
        public static readonly string QUICK_REWARD_TIMES = "quickRewardTimes";
        public static readonly string TIMER = "timer";
        public static readonly string REWARD_NONAUTO = "rewardNonauto";
        public static readonly string REWARD_AUTO = "rewardAuto";
        public static readonly string LV = "lv";
    }

    public class CampsitePointData : ModelDataBase
    {
        public int index;
        public bool isUnlock;
        public int unlockStage;
        public int equipGunId;
        public int quickRewardTimes;
        public double timer;
        public double settlementReward_Nonauto;
        public double settlementReward_Auto;
        public int lv;

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == CampsitePointDataKey.INDEX)
                {
                    index = (int)data[key];
                }
                else if (key == CampsitePointDataKey.IS_UNLOCK)
                {
                    isUnlock = (bool)data[key];
                }
                else if (key == CampsitePointDataKey.UNLOCK_STAGE)
                {
                    unlockStage = (int)data[key];
                }
                else if (key == CampsitePointDataKey.EQUIP_GUN_ID)
                {
                    equipGunId = (int)data[key];
                }
                else if (key == CampsitePointDataKey.QUICK_REWARD_TIMES)
                {
                    quickRewardTimes = (int)data[key];
                }
                else if (key == CampsitePointDataKey.TIMER)
                {
                    timer = (double)data[key];
                }
                else if (key == CampsitePointDataKey.REWARD_NONAUTO)
                {
                    settlementReward_Nonauto = (double)data[key];
                }
                else if (key == CampsitePointDataKey.REWARD_AUTO)
                {
                    settlementReward_Auto = (double)data[key];
                }
                else if (key == CampsitePointDataKey.LV)
                {
                    lv = (int)data[key];
                }
            }

            return true;
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [CampsitePointDataKey.INDEX] = index,
                [CampsitePointDataKey.IS_UNLOCK] = isUnlock,
                [CampsitePointDataKey.UNLOCK_STAGE] = unlockStage,
                [CampsitePointDataKey.EQUIP_GUN_ID] = equipGunId,
                [CampsitePointDataKey.QUICK_REWARD_TIMES] = quickRewardTimes,
                [CampsitePointDataKey.TIMER] = timer,
                [CampsitePointDataKey.REWARD_NONAUTO] = settlementReward_Nonauto,
                [CampsitePointDataKey.REWARD_AUTO] = settlementReward_Auto,
                [CampsitePointDataKey.LV] = lv,
            };

            return ret;
        }
    }
}