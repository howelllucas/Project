using LitJson;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Game.Util;

namespace Game.Data
{
    //用户数据
    public class PlayerDataKey
    {
        public static readonly string GUID = "guid";                                    // 用户id
        public static readonly string CREATE_TIME = "createTime";                       // 创建时间
        public static readonly string SERVER_TIME_DATA = "serverTimeData";              // 服务器时间数据
        public static readonly string REFRESH_TIME = "refreshTime";                       // 刷新时间
        public static readonly string CUR_GUIDE_ID = "curGuideID";                      // 当前引导id
        public static readonly string CUR_GUIDE_STAGE_INDEX = "curGuideStageIndex";
        public static readonly string CUR_STAGE_ID = "curStageID";                      // 当前关卡id
        public static readonly string MAX_UNLOCK_STAGE_ID = "maxUnlockStageID";         // 最大解锁关卡ID
        public static readonly string CUR_BOX_ID = "curBoxID";                          // 当前宝箱id
        public static readonly string STAGE_DATAS = "stage_datas";                      // 关卡信息
        //public static readonly string CURRENCYS = "currencys";                          // 货币列表
        //public static readonly string CURRENCY_REGTIMES = "currencyRegtimes";           // 货币上次回复时间
        public static readonly string CARD_DATAS = "cardDatas";                         // 卡牌列表
        public static readonly string USE_WEAPON_ID = "useWeaponID";                  // 当前使用ID
        public static readonly string FUSED_CARD_LIST = "fusedCardList";              // 融合卡组列表
        public static readonly string COMPLETE_TASKS = "completeTasks";                 // 完成任务列表
        public static readonly string REWARD_CARDS = "rewardCards";                     // 卡牌奖励列表
        public static readonly string BOXDRAWDIC = "boxDrawDic";                     // 宝箱PRD抽取次数
        public static readonly string BOXDRAWCOUNT = "boxDrawCount";                     // 宝箱抽取次数
        public static readonly string LEVEL = "level";                     // 玩家等级

        public static readonly string CURRENCY_DIAMOND = "currency_diamond";            // 钻石
        public static readonly string CURRENCY_POWER = "currency_power";                // 体力
        public static readonly string CURRENCY_GOLD = "currency_gold";                  // 金币
        public static readonly string CURRENCY_WANTED = "currency_wanted";              // 通缉令
        public static readonly string CURRENCY_KEY = "currency_key";                    // 钥匙

        public static readonly string CURRENCYREGTIMES_POWER = "currencyregtimes_power";            // 体力

        public static readonly string CAMPTASKDATA = "campTaskData";               //营地任务数据
        public static readonly string CAMPSITEDATA = "campsiteData";                //营地数据
        public static readonly string IDLE_REWARD_DATA = "idleRewardData";          //挂机奖励数据

        public static readonly string SHOP_DATA = "shopData";           //商店数据

        public static readonly string CHAPTERDATA = "chapterData";           //章节数据

        public static readonly string OPEN_MODULES = "openModules";     //开放功能

        public static readonly string FINISH_DIALOGUE_LIST = "finishDialogueList";     //完成剧情对话
    }
    [Serializable]
    public class PlayerData : ModelDataBase
    {
        public Guid guid;           //用户id
        public DateTime createTime; //创建时间
        public ServerTimeData serverTimeData = null;    //服务器时间数据
        public DateTime refreshTime;          //数据刷新时间
        public int curStageID;      //当前关卡id
        public int maxUnlockStageID;      //最大解锁关卡
        public int curGuideStageIndex;  //当前引导关id
        public int curGuideID;      //当前引导id
        public int curBoxID;        //当前宝箱id
        public int level;           //玩家等级
        public int exp;             //玩家经验

        public Dictionary<CurrencyType, int> currencys = new Dictionary<CurrencyType, int>();   // 货币列表
        public Dictionary<CurrencyType, BigInteger> bigCurrencys = new Dictionary<CurrencyType, BigInteger>();//超大货币列表
        public Dictionary<CurrencyType, DateTime> currencyRegtimes = new Dictionary<CurrencyType, DateTime>();//货币上次回复时间
        public Dictionary<int, GunCardData> cardDatas = new Dictionary<int, GunCardData>();    //卡牌列表
        public Dictionary<int, StageData> stageDatas = new Dictionary<int, StageData>();    //关卡列表
        public int useWeaponID;    // 当前使用武器ID
        public List<int> fusedCardList = new List<int>();              //融合卡牌列表
        public Dictionary<int, int> boxDrawDic = new Dictionary<int, int>();    //宝箱PRD抽取次数
        public int boxDrawCount;        // 宝箱抽取次数

        public CampTaskData campTaskData = new CampTaskData();                  //营地任务数据
        public CampsiteData campsiteData = null;          //营地数据
        public IdleRewardData idleRewardData = new IdleRewardData();

        public ShopData shopData = new ShopData();      //商店数据

        public ChapterData chapterData = new ChapterData();      //章节数据

        public List<int> openModules = new List<int>();

        public List<int> finishDialogueList = new List<int>();  //完成剧情对话

        public PlayerData()
        {
            guid = Guid.Empty;
            curStageID = 0;
            curGuideStageIndex = 1;
            curBoxID = 0;
            level = 0;
            exp = 0;
            boxDrawCount = 0;

            currencys[CurrencyType.DIAMOND] = 0;
            bigCurrencys[CurrencyType.GOLD] = 0;
            currencys[CurrencyType.KEY] = 0;
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [PlayerDataKey.GUID] = guid.ToString(),
                [PlayerDataKey.CREATE_TIME] = PTUtil.DateTime2Timestamp(createTime),
                [PlayerDataKey.REFRESH_TIME] = PTUtil.DateTime2Timestamp(refreshTime),
                [PlayerDataKey.CUR_GUIDE_ID] = curGuideID,
                [PlayerDataKey.CUR_STAGE_ID] = curStageID,
                [PlayerDataKey.MAX_UNLOCK_STAGE_ID] = maxUnlockStageID,
                [PlayerDataKey.CUR_GUIDE_STAGE_INDEX] = curGuideStageIndex,
                [PlayerDataKey.CUR_BOX_ID] = curBoxID,
                [PlayerDataKey.USE_WEAPON_ID] = useWeaponID,
                [PlayerDataKey.CURRENCY_DIAMOND] = currencys[CurrencyType.DIAMOND],
                [PlayerDataKey.CURRENCY_GOLD] = bigCurrencys[CurrencyType.GOLD].ToString(),
                [PlayerDataKey.CURRENCY_KEY] = currencys[CurrencyType.KEY],
                [PlayerDataKey.BOXDRAWCOUNT] = boxDrawCount,
                [PlayerDataKey.LEVEL] = level,

                [PlayerDataKey.CAMPTASKDATA] = campTaskData.GetJsonData(),
                [PlayerDataKey.IDLE_REWARD_DATA] = idleRewardData.GetJsonData(),

                [PlayerDataKey.SHOP_DATA] = shopData.GetJsonData(),
                [PlayerDataKey.CHAPTERDATA] = chapterData.GetJsonData(),
            };
            if (serverTimeData != null)
            {
                ret[PlayerDataKey.SERVER_TIME_DATA] = serverTimeData.GetJsonData();
            }
            //JsonData currencysJson = new JsonData();
            //currencysJson.SetJsonType(JsonType.Array);
            //foreach (var key in currencys.Keys)
            //{
            //    currencysJson[key.ToString()] = currencys[key];
            //}
            //ret[PlayerDataKey.CURRENCYS] = currencysJson;

            //JsonData currencyRegtimesJson = new JsonData();
            //currencyRegtimesJson.SetJsonType(JsonType.Array);
            //foreach (var key in currencyRegtimes.Keys)
            //{
            //    currencyRegtimesJson[key.ToString()] = currencyRegtimes[key].ToString();
            //}
            //ret[PlayerDataKey.CURRENCY_REGTIMES] = currencyRegtimesJson;

            JsonData stageDatasJson = new JsonData();
            stageDatasJson.SetJsonType(JsonType.Array);
            foreach (var key in stageDatas.Keys)
            {
                stageDatasJson.Add(stageDatas[key].GetJsonData());
            }
            ret[PlayerDataKey.STAGE_DATAS] = stageDatasJson;

            JsonData cardDatasJson = new JsonData();
            cardDatasJson.SetJsonType(JsonType.Array);
            foreach (var key in cardDatas.Keys)
            {
                cardDatasJson.Add(cardDatas[key].GetJsonData());
            }
            ret[PlayerDataKey.CARD_DATAS] = cardDatasJson;

            JsonData fusedCardListJson = new JsonData();
            fusedCardListJson.SetJsonType(JsonType.Array);
            foreach (var value in fusedCardList)
            {
                fusedCardListJson.Add(value);
            }
            ret[PlayerDataKey.FUSED_CARD_LIST] = fusedCardListJson;

            if (campsiteData != null)
            {
                ret[PlayerDataKey.CAMPSITEDATA] = campsiteData.GetJsonData();
            }

            if (boxDrawDic.Count > 0)
            {
                JsonData boxDrawDicJson = new JsonData();
                //stageDatasJson.SetJsonType(JsonType.Array);
                foreach (var item in boxDrawDic)
                {
                    boxDrawDicJson[item.Key.ToString()] = item.Value;
                }

                ret[PlayerDataKey.BOXDRAWDIC] = boxDrawDicJson;
            }

            if (openModules.Count > 0)
            {
                JsonData openModulesJson = new JsonData();
                openModulesJson.SetJsonType(JsonType.Array);
                for (int i = 0; i < openModules.Count; i++)
                {
                    openModulesJson.Add(openModules[i]);
                }
                ret[PlayerDataKey.OPEN_MODULES] = openModulesJson;
            }

            if (finishDialogueList.Count > 0)
            {
                JsonData finishDialogueListJson = new JsonData();
                finishDialogueListJson.SetJsonType(JsonType.Array);
                for (int i = 0; i < finishDialogueList.Count; i++)
                {
                    finishDialogueListJson.Add(finishDialogueList[i]);
                }
                ret[PlayerDataKey.FINISH_DIALOGUE_LIST] = finishDialogueListJson;
            }

            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {

                if (key == PlayerDataKey.CUR_STAGE_ID)
                {
                    curStageID = (int)data[key];
                }
                else if (key == PlayerDataKey.CUR_GUIDE_ID)
                {
                    curGuideID = (int)data[key];
                }
                else if (key == PlayerDataKey.CUR_GUIDE_STAGE_INDEX)
                {
                    curGuideStageIndex = (int)data[key];
                }
                else if (key == PlayerDataKey.CUR_BOX_ID)
                {
                    curBoxID = (int)data[key];
                }
                else if (key == PlayerDataKey.MAX_UNLOCK_STAGE_ID)
                {
                    maxUnlockStageID = (int)data[key];
                }
                else if (key == PlayerDataKey.GUID)
                {
                    guid = Guid.Parse((string)data[key]);
                }
                else if (key == PlayerDataKey.CREATE_TIME)
                {
                    createTime = PTUtil.JsonData2DateTime(data[key]);
                }
                else if (key == PlayerDataKey.REFRESH_TIME)
                {
                    refreshTime = PTUtil.JsonData2DateTime(data[key]);
                }
                else if (key == PlayerDataKey.USE_WEAPON_ID)
                {
                    useWeaponID = (int)data[key];
                }
                else if (key == PlayerDataKey.LEVEL)
                {
                    level = (int)data[key];
                }
                else if (key == PlayerDataKey.BOXDRAWCOUNT)
                {
                    boxDrawCount = (int)data[key];
                }
                else if (key == PlayerDataKey.CURRENCY_DIAMOND)
                {
                    currencys[CurrencyType.DIAMOND] = (int)data[key];
                }
                else if (key == PlayerDataKey.CURRENCY_GOLD)
                {
                    BigInteger gold;
                    if (BigInteger.TryParse(data[key].ToString(), out gold))
                    {
                        bigCurrencys[CurrencyType.GOLD] = gold;
                    }
                }
                else if (key == PlayerDataKey.CURRENCY_KEY)
                {
                    currencys[CurrencyType.KEY] = (int)data[key];
                }
                else if (key == PlayerDataKey.STAGE_DATAS)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        var c = new StageData();
                        c.InitWithJson(subJson[i]);
                        stageDatas[c.stageID] = c;
                    }
                }
                else if (key == PlayerDataKey.CARD_DATAS)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        var c = new GunCardData();
                        c.InitWithJson(subJson[i]);
                        cardDatas[c.cardID] = c;
                    }
                }
                else if (key == PlayerDataKey.FUSED_CARD_LIST)
                {
                    var subJson = data[key];
                    for (int i = 0; i < subJson.Count; ++i)
                    {
                        fusedCardList.Add((int)subJson[i]);
                    }
                }
                else if (key == PlayerDataKey.SERVER_TIME_DATA)
                {
                    serverTimeData = new ServerTimeData();
                    serverTimeData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.CAMPTASKDATA)
                {
                    campTaskData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.CAMPSITEDATA)
                {
                    campsiteData = new CampsiteData();
                    campsiteData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.IDLE_REWARD_DATA)
                {
                    idleRewardData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.SHOP_DATA)
                {
                    shopData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.CHAPTERDATA)
                {
                    chapterData.InitWithJson(data[key]);
                }
                else if (key == PlayerDataKey.BOXDRAWDIC)
                {
                    boxDrawDic.Clear();
                    var boxDrawDicJson = data[key];
                    foreach (var idStr in boxDrawDicJson.Keys)
                    {
                        int boxID;
                        if (int.TryParse(idStr, out boxID))
                        {
                            boxDrawDic[boxID] = (int)boxDrawDicJson[idStr];
                        }
                    }
                }
                else if (key == PlayerDataKey.OPEN_MODULES)
                {
                    openModules.Clear();
                    var subJson = data[key];
                    if (subJson.IsArray)
                    {
                        for (int i = 0; i < subJson.Count; i++)
                        {
                            openModules.Add((int)subJson[i]);
                        }
                    }
                }
                else if (key == PlayerDataKey.FINISH_DIALOGUE_LIST)
                {
                    finishDialogueList.Clear();
                    var subJson = data[key];
                    if (subJson.IsArray)
                    {
                        for (int i = 0; i < subJson.Count; i++)
                        {
                            finishDialogueList.Add((int)subJson[i]);
                        }
                    }
                }
            }

            return true;
        }
    }

    //卡牌数据
    public class CardDataKey
    {
        public static readonly string CARD_ID = "cardID";                       // 卡牌id
        public static readonly string LEVEL = "level";                          // 等级
        public static readonly string COUNT = "count";                          // 数量
        public static readonly string IS_NEW = "isNew";                         // 是否新卡
        public static readonly string STAR = "star";                            // 星级
        public static readonly string FUSESKILLID = "fuseSkillID";              // 融合技能id
        public static readonly string CAMPSKILLID = "campSkillID";              // 营地技能id
    }

    [Serializable]
    public class GunCardData : ModelDataBase
    {
        public int cardID = 0;              //卡牌id
        public int level = 0;               //等级
        public int count = 0;               //数量
        public bool isNew = true;           //是否新卡
        public int star = 0;                //星级
        public int fuseSkillID = 0;         //融合技能id
        public int campSkillID = 0;         //营地技能id


        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [CardDataKey.CARD_ID] = cardID,
                [CardDataKey.LEVEL] = level,
                [CardDataKey.COUNT] = count,
                [CardDataKey.IS_NEW] = isNew,
                [CardDataKey.STAR] = star,
                [CardDataKey.FUSESKILLID] = fuseSkillID,
                [CardDataKey.CAMPSKILLID] = campSkillID,
            };


            return ret;
        }
        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == CardDataKey.CARD_ID)
                {
                    cardID = (int)data[key];
                }
                else if (key == CardDataKey.LEVEL)
                {
                    level = (int)data[key];
                }
                else if (key == CardDataKey.COUNT)
                {
                    count = (int)data[key];
                }
                else if (key == CardDataKey.IS_NEW)
                {
                    isNew = (bool)data[key];
                }
                else if (key == CardDataKey.STAR)
                {
                    star = (int)data[key];
                }
                else if (key == CardDataKey.FUSESKILLID)
                {
                    fuseSkillID = (int)data[key];
                }
                else if (key == CardDataKey.CAMPSKILLID)
                {
                    campSkillID = (int)data[key];
                }
            }
            return true;
        }

        public float GetCampRewardFactor()
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return 1f;

            return cardRes.productionBonus * (1 + (level - 1) *
                    TableMgr.singleton.ValueTable.card_productionBonus_rate);

        }

        public float GetAtk()
        {
            return GetAtk(level, star);
        }

        public float GetAtk(int _level, int _star)
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return 0;

            var atk = cardRes.atk * Mathf.Pow((1 + TableMgr.singleton.ValueTable.card_levelup_dpsrate),
                                                (_level - 1));
            //Debug.Log("atk " + atk);
            atk = atk * Mathf.Pow((1 + TableMgr.singleton.ValueTable.card_star_dpsrate),
                                  (_star - PlayerDataMgr.GetCardInitStar(cardRes.rarity)));

            return atk;
        }

        public float GetAtkSpeed()
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return 0;

            return cardRes.atkSpeed;
        }
        public float DPS()
        {
            return DPS(level, star);
        }
        public float DPS(int _level, int _star)
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return 0;

            return GetAtkSpeed() * GetAtk(_level, _star) * cardRes.bulletParam;
        }
    }



    //卡牌奖励数据
    public class RewardCardKey
    {
        public static readonly string QUALITY = "quality";                          // 等级
        public static readonly string COUNT = "count";                              // 数量

    }

    [Serializable]
    public class RewardCard : ModelDataBase
    {
        public int quality = 0;                 //品质
        public int count = 0;                   //数量


        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [RewardCardKey.QUALITY] = quality,
                [RewardCardKey.COUNT] = count,

            };


            return ret;
        }
        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == RewardCardKey.QUALITY)
                {
                    quality = (int)data[key];
                }
                else if (key == RewardCardKey.COUNT)
                {
                    count = (int)data[key];
                }

            }
            return true;
        }
    }

    public class ServerTimeDataKey
    {
        public static readonly string LOCAL_TIME_SPAN = "localTimeSpan";                // 本地时间与服务器时间差
        public static readonly string REFRESH_TIME_ONE_DAY = "refreshOneDay";                // 每日刷新时间
        public static readonly string REFRESH_TIME_TWO_DAY = "refreshTwoDay";                // 每两日刷新时间
        public static readonly string REFRESH_TIME_ONE_WEEK = "refreshOneWeek";                // 每周刷新时间

    }

    [Serializable]
    public class ServerTimeData : ModelDataBase
    {
        public long localTimeSpan;  //本地时间与服务器时间差
        public DateTime refreshTime_OneDay;
        public DateTime refreshTime_TwoDay;
        public DateTime refreshTime_OneWeek;
        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData()
            {
                [ServerTimeDataKey.LOCAL_TIME_SPAN] = localTimeSpan,
                [ServerTimeDataKey.REFRESH_TIME_ONE_DAY] = PTUtil.DateTime2Timestamp(refreshTime_OneDay),
                [ServerTimeDataKey.REFRESH_TIME_TWO_DAY] = PTUtil.DateTime2Timestamp(refreshTime_TwoDay),
                [ServerTimeDataKey.REFRESH_TIME_ONE_WEEK] = PTUtil.DateTime2Timestamp(refreshTime_OneWeek),
            };
            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (string key in data.Keys)
            {
                if (key == ServerTimeDataKey.LOCAL_TIME_SPAN)
                {
                    localTimeSpan = PTUtil.JsonData2Timestamp(data[key]);
                }
                else if (key == ServerTimeDataKey.REFRESH_TIME_ONE_DAY)
                {
                    refreshTime_OneDay = PTUtil.JsonData2DateTime(data[key]);
                }
                else if (key == ServerTimeDataKey.REFRESH_TIME_TWO_DAY)
                {
                    refreshTime_TwoDay = PTUtil.JsonData2DateTime(data[key]);
                }
                else if (key == ServerTimeDataKey.REFRESH_TIME_ONE_WEEK)
                {
                    refreshTime_OneWeek = PTUtil.JsonData2DateTime(data[key]);
                }
            }
            return true;
        }

    }



    /////////////////////////////////////////////////////////////////////////
    public class ModelDataBase
    {
        public ModelDataBase()
        {
        }

        public virtual bool InitWithJson(JsonData data)
        {
            return true;
        }

        public virtual JsonData GetJsonData()
        {
            return new JsonData();
        }

        // 数据是否变动
        public static bool Dirty
        {
            get { return bDirty; }
            set { bDirty = value; }
        }

        protected static bool bDirty = false;
    }


}
