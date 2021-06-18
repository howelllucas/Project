using Game.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    public class IdleRewardDataKey
    {
        public static readonly string START_TIME = "startTime";
        public static readonly string SETTLEMENT_TIME = "settlementTime";
        public static readonly string SETTLEMENT_DATAS = "settlementDatas";
        public static readonly string QUICK_IDLE_INDEX = "quickIdleIndex";
    }

    public class IdleRewardData : ModelDataBase
    {
        public long startTimestamp;
        public long settlementTimestamp;
        public List<IdleRewardSettlementData> settlementList = new List<IdleRewardSettlementData>();
        public int quickIdleIndex = 1;

        public void UpdateSettlementList(List<IdleRewardSettlementData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (i >= settlementList.Count)
                {
                    IdleRewardSettlementData data = new IdleRewardSettlementData();
                    data.Copy(list[i]);
                    settlementList.Add(data);
                }
                else
                {
                    settlementList[i].Copy(list[i]);
                }
            }

            for (int i = settlementList.Count - 1; i >= list.Count; i--)
            {
                settlementList.RemoveAt(i);
            }
        }

        public List<IdleRewardSettlementData> CloneSettlementList()
        {
            List<IdleRewardSettlementData> list = new List<IdleRewardSettlementData>();
            for (int i = 0; i < settlementList.Count; i++)
            {
                IdleRewardSettlementData data = new IdleRewardSettlementData();
                data.Copy(settlementList[i]);
                list.Add(data);
            }
            return list;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == IdleRewardDataKey.START_TIME)
                {
                    startTimestamp = PTUtil.JsonData2Timestamp(data[key]);
                }
                else if (key == IdleRewardDataKey.SETTLEMENT_TIME)
                {
                    settlementTimestamp = PTUtil.JsonData2Timestamp(data[key]);
                }
                else if (key == IdleRewardDataKey.SETTLEMENT_DATAS)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; i++)
                    {
                        IdleRewardSettlementData settlement = new IdleRewardSettlementData();
                        settlement.InitWithJson(subJson[i]);
                        settlementList.Add(settlement);
                    }
                }
                else if (key == IdleRewardDataKey.QUICK_IDLE_INDEX)
                {
                    quickIdleIndex = (int)data[key];
                }
            }
            return true;
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [IdleRewardDataKey.START_TIME] = startTimestamp,
                [IdleRewardDataKey.SETTLEMENT_TIME] = settlementTimestamp,
                [IdleRewardDataKey.QUICK_IDLE_INDEX] = quickIdleIndex,
            };
            if (settlementList.Count > 0)
            {
                JsonData settlementListJson = new JsonData();
                settlementListJson.SetJsonType(JsonType.Array);
                for (int i = 0; i < settlementList.Count; i++)
                {
                    settlementListJson.Add(settlementList[i].GetJsonData());
                }
                ret[IdleRewardDataKey.SETTLEMENT_DATAS] = settlementListJson;
            }
            return ret;
        }
    }

    public class IdleRewardSettlementDataKey
    {
        public static readonly string DURATION = "duration";
        public static readonly string GOLD_PER_MINUTE = "goldPerMinute";
    }

    public class IdleRewardSettlementData : ModelDataBase
    {
        public int duration;//minute
        public int goldPerMinute;

        public void Copy(IdleRewardSettlementData src)
        {
            duration = src.duration;
            goldPerMinute = src.goldPerMinute;
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [IdleRewardSettlementDataKey.DURATION] = duration,
                [IdleRewardSettlementDataKey.GOLD_PER_MINUTE] = goldPerMinute,
            };

            return ret;
        }
        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == IdleRewardSettlementDataKey.DURATION)
                {
                    duration = (int)data[key];
                }
                else if (key == IdleRewardSettlementDataKey.GOLD_PER_MINUTE)
                {
                    goldPerMinute = (int)data[key];
                }
            }
            return true;
        }
    }
}