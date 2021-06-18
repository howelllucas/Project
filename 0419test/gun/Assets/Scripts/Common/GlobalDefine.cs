

namespace Game
{
    //图集命名
    public class SpriteAtlasNames
    {
        public const string ACom = "atlas_acom";
        public const string CARD = "atlas_card";
        public const string BONUS_ICON = "atlas_bonus_icon";
        public const string HERO_BONUS_ICON = "atlas_hero_bonus_icon";
        public const string HERO_ICON = "atlas_hero_icon";
        public const string HERO_REWARD_ICON = "atlas_hero_icon";
    }

    //界面事件类型
    public enum UIEventType
    {
        CURRENCY = 1,           //货币刷新
        TASK = 2,               //任务刷新
        STAGE = 3,              //关卡刷新
        CARD = 4,               //卡牌刷新
        OPEN_BOX = 5,           //打开宝箱
        BATTLE = 6,             //战斗UI刷新
        LOGIN_REWARD = 7,       //登录奖励刷新
        WANTED_TASK = 8,        //悬赏任务刷新
        AD_REWARD_READY = 9,    //奖励视频加载完成
        LABYRINTH_UPDATE = 10,          //迷宫刷新
        LABYRINTH_FINISH = 11,          //迷宫完成
        DPS = 12,               //DPS刷新
        PLAYER_LEVEL = 13,      //玩家等级刷新
        TOWER_PROP = 14,        //塔模式道具使用
        PURCHASE_PRODUCT = 15,  //商品数据刷新
        PURCHASE_BUY = 16,      //商品购买完成


        PAUSE_STATE = 999,
        CLOSE_UI = 1000,        //关闭UI
        SERVER_TIME = 1001,     //系统时间刷新
        ONE_DAY_REFRESH = 1002, //每日刷新
        TWO_DAY_REFRESH = 1003, //每两日刷新
        ONE_WEEK_REFRESH = 1004,    //每周刷新
        SHOW_BUFFER_CLEAR = 1005,   //缓存UI展示完毕
    }


    //游戏货币类型
    public enum CurrencyType
    {
        Undefine = 0,           //未定义

        GOLD = 1,               //金币
        DIAMOND = 2,            //钻石
        KEY = 3,                //钥匙
    }

    //任务类型
    public enum TaskType
    {
        Finish_Battle = 1,	//完成战斗
        Occupy_Building = 2,     //占领建筑：{X}
        Equip_Gun = 3,      //给{buildingName}守护者装备{X}级枪
        LevelUp_Gun = 4,	//{gun_type}升级{X}次
        Supplies_Add = 5,	    //物资储备
        Cost_Gold = 6,	    //消耗金币
        Quick_Reward = 7,	        //使用{0}次快速收益
        Open_Box = 8,	    //开启宝箱
        Get_Gold = 9,	    //收集金币
        LvUp_Building = 10,     //将{buildingName}升至{X}级
        Buildings_Level = 11,     //营地内建筑的总等级达到{X}级
        Auto_Level = 12,        //开启自动化{0}
        Fuse_Gun = 13,        //装备融合武器
        Epic_Gun = 14,        //装备史诗武器
        Explore_Gold = 15,        //领取野外探索奖励
        Lv10_Gun = 16,        //拥有10级武器
    }

    //游戏物品类型
    public enum GoodsType
    {
        NONE = -1,

        DIAMOND = 1,            //钻石
        GOLD = 2,               //金币
        KEY = 7,                //钥匙


        CARD = 4,               //卡牌
        BOX = 5,                //宝箱*

        RANDOM_CARD = 9,         //随机卡牌
        CARD_CHIP = 10,         //卡牌碎片
        GOLD_MINUTE = 11,         //金币时间(挂机n分钟等价金币)
        RANDOM_CARD_CHIP = 12,  //随机卡牌碎片
        CAMPSITE_REWARD = 13,   //营地物资
    }

    //游戏品质类型
    public enum CardQualityType
    {
        NONE = -1,

        RARE = 1,                   //稀有（橙）
        EPIC = 2,                   //史诗（紫）
        LEGEND = 3,                 //传奇（金）
    }


    public enum LevelStarType
    {
        NONE = -1,

        Level_Pass = 101,                   //通关
        Life_50 = 102,                   //血量高于50%以上通关
        Life_100 = 103,                  //血量100%通关
        Revive_0 = 104,                 //未使用复活通关
        Epic_Gun = 105,                 //使用史诗武器通关
        Legend_Gun = 106,                 //使用传说武器通关
        Gun_Type_1 = 107,                 //使用弩通关
        Gun_Type_2 = 108,                 //使用狙击枪通关
        Gun_Type_3 = 109,                 //使用来复枪通关
        Gun_Type_4 = 110,                 //使用特殊枪通关
        Gun_Level = 111,                 //使用X级以上武器通关
    }

    //红点提示类型
    public enum RedTipsType
    {
        None = 0,

        IdleReward = 1,    //挂机奖励
        QuickIdle = 2,     //快速挂机

        CampsiteSetGun = 3, //营地有可配置武器

        ShopTenBox = 10,     //商店十连抽宝箱
        ShopOneBox = 11,    //商店单抽宝箱

        NewCard = 20,        //新卡牌
        LvUpCard = 21,       //升级卡牌
        StarUpCard = 22,       //升星卡牌
        FuseCard = 23,       //融合卡牌
        StarReward = 24,        //星级奖励
    }

    //技能类型
    public enum SkillType
    {
        Fuse = 1,
        Camp = 2,
    }

    //游戏数据类型
    public enum GameData
    {
        DailyTaskStart = 0,         //日常任务

        DailyWantedTask = 1,          //接受{1}个悬赏任务
        DailyFastIdleReward = 2,          //快速挂机1次
        DailyEnterStage = 3,          //挑战关卡1次
        DailyCardLvUp = 4,            //升级卡牌1次
        DailyADVideo = 5,           //观看视频{1}次
        DailyEnterTower = 6,        //挑战王座之塔1次
        DailyEnterLabyrinth = 7,        //挑战迷宫1次
        DailyOpenGoldBox = 8,        //开金币宝箱{1}次
        DailyCostRMB = 9,            //累计消耗钻石
        DailyPassWaves = 10,        //累计抵抗50波
        DailyGetIdleReward = 11,       //领取挂机收益X次


        DailyTaskEnd,

        WeeklyTaskStart = 100,      //周常任务

        WeeklyPlayerLvUp = 101, //升级玩家等级{1}
        WeeklyWantedTask = 102, //接受{1}个悬赏任务
        WeeklyOpenRMBBox = 103,    //开钻石宝箱{1}次
        WeeklyStageBoss = 104, //击败关卡BOSS{1}次
        WeeklyCostGold = 105, //累计消耗{1}金币
        WeeklyADVideo = 106, //观看视频    {1}    次
        WeeklyLabyrinthBoss1 = 107, //击败迷宫第1层BOSS    {1}    次
        WeeklyLabyrinthBoss3 = 108, //击败迷宫第3层BOSS    {1}    次
        WeeklyEnterTower = 109, //挑战王座之塔1次
        WeeklyCardStarUp = 110, //卡牌升星{1}次
        WeeklyCostRMB = 111, //累计消耗X钻石
        WeeklyCardStarUpRare = 112, //稀有卡牌升星X次
        WeeklyCardStarUpEpic = 113, //史诗卡牌升星X次
        WeeklyCardStarUpLegend = 114, //传说卡牌升星X次
        WeeklyOpenGoldBox = 115,    //开金币宝箱{1}次

        WeeklyTaskEnd,

        MainTaskStart = 1000,       //主线任务

        MainPassStage = 1001,   //● 通关关卡X-3
        MainPassWorld = 1002,   //● 通关第X世界
        MainPassWaves = 1003,   //● 累计完成X波战斗
        MainLabyrinthBoss = 1004,   //● 迷宫中击杀boss次数x次以上
        MainLabyrinthBoss3 = 1005,   //● 击败迷宫第三层boss累计x次以上
        MainCardCount = 1006,  //● 累计获得X张卡牌
        MainCardLevel = 1007,   //● 任意一张卡牌升级到X级
        MainPlayerLevel = 1008, //● 玩家等级达到X级
        MainFastIdleReward = 1009, //● 使用快速挂机X次
        MainGetIdleReward = 1010,  //● 挂机累计获得X枚金币
        MainPassTower = 1011,   //● 通关塔模式X层
        MainADVideo = 1012, //● 累计观看广告X次
        MainOpenRMBBox = 1013,   //● 累计打开X个钻石宝箱
        MainWantedTask = 1014, //累计接受X悬赏任务
        MainCostBoxKey = 1015, //累计使用X抽卡券
        MainCostVedioKey = 1016,     //累计使用X视频券
        MainCostRMB = 1017, //累计使用钻石X
        MainCardMaxStar = 1018, //任意卡牌升到X星
        MainCardAllStar = 1019, //所有卡牌累计升星X
        MainOpenGoldBox = 1020,   //● 累计打开X个金币宝箱

        MainTaskEnd,
    }

    public enum CacheUITriggerScene
    {
        Main,
    }
    //////////////////////////////////////////////////////////////////////////
    public enum DamageType
    {
        Damage,
        Cri,
        Heal,
        PlayerDmg,
    }

    public enum TargetType
    {
        None = 0,
        Front = 1,
        Random = 2,
        LiHighestLife = 3,
        NoAtk = 4,
    }

    public enum GameModuleType
    {
        Undefine,
        BuildInteraction,
        Expedition,
        ShopTab,
        BuildSetGun,
        GunTab,
        GunFuse,
        IdleReward,
    }

    public enum FightResult
    {
        Undefine,
        Win,
        Lose,
    }
}