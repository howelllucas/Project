
namespace EZ
{
    public class MsgIds
    {
        public static int MonsterDead = 1001;// 怪物死亡
        public static int WaveStart = 1002;//某个波次开始
        public static int WaveEnded = 1003;// 某个波次结束
        public static int AllWaveEnded = 1016;// 所有的波次结束

        public static int AddPlayerHpUi = 1000;// 增加 主角血量hp 显示ui
        public static int MainRoleHpChange = 1004;// 主角血量方式改变
        public static int MonsterHpChanged = 1009;// 怪物血量发生改变
        public static int AddMonsterHpUi = 1010;// 增加 血量hp 显示ui 
        public static int AddFightUICountItem = 1011;// 道具倒计时的icon
        public static int UpdatePlayerEnergy = 1029; // 体力消耗
        public static int FocusGameObject = 1031;
        public static int FreshGunPower = 1039; //枪械能量条

        public static int FightGainGold = 1005;// 战中获取金币
        public static int GoldChanged = 1006;// 金币改变 广播
        public static int DiamondChanged = 1007;// 钻石改变广播
        public static int RedHeardChanged = 1056;
        public static int GainProp = 1008;// 获取道具
        //public static int GainDelayShow = 1032;// 延迟特效
        public static int UIFresh = 1033;// UI刷新
        public static int FightUiProgress = 1045;//战斗模块 倒计时提醒
        public static int DogeAdTimesChanged = 1046;// UI刷新
        public static int EnergyAdTimesChanged = 1047;// UI刷新
        public static int CreateTaskIcon = 1048;// UI刷新
        public static int TaskIconLight = 1049;// UI刷新
        public static int WpnChipCountChanged = 1053;// UI 刷新 碎片 

        public static int CollectingProp = 1025;// 收集中道具
        public static int ExpChanged = 1015;  // 游戏 经验 改变广播
        public static int ActiveProp = 1017;// 战中激活道具 一般都是 跟胜利条件挂钩
        public static int FightUiTopTips = 1018;// 战提示具 提示
        public static int FightUiModeCountDownTips = 1026;//战斗模块 倒计时提醒


        public static int ShowGameTipsByID = 1012;// tips 广播。参数是id
        public static int ShowGameTipsByStr = 1013;// tips 广播 参数是 str
        public static int ShowGameTipsByIDAndParam = 1014;// 广播。参数是id和参数
        public static int AddGameGuideUi = 1034;// 广播 增加引导的手指引
        public static int rmGameGuideUi = 1035;//删除 引导的手指引
        public static int AddGuidePlotUi = 1036;//显示引导提示
        public static int RmGuidePlotUi = 1037;//删除引导提示
        public static int ChangeScrollView4Guide = 1038;//为了引导 调整滑动窗口显示位置

        public static int AddPetName = 1054;//

        public static int TimeCoolDown = 1015;// 游戏倒计时广播
        public static int ShowFightPlotByID = 1023;// tips 广播。参数是id
        public static int ShowFightPlotByStr = 1024;// tips 广播。参数是id
        public static int ShowRewardGetEffect = 1030;// 主界面 获得 奖励效果

        public static int TriggerCollider = 1019;//碰到某个机关

        public static int PointArrowAngle = 1020;//ui 指示箭头的角度

        public static int TaskModeBegin = 1021;// 监听 任务模块开启，
        public static int TaskModeEnd = 1022;// 监听 任务模块关闭，
        public static int TaskModeCountDown = 1050;// 监听 某些任务模块倒计时开始，

        public static int EnergyChanged = 1027;// 能量改变 广播
        public static int MDTChanged = 1028;// 狗牌改变广播

        public static int AfterAD = 1040;// 广告结束广播
        public static int HideGameGuideAD = 1041;// 隐藏游戏引导
        public static int CommonUIIndexChange = 1042;//主界面页签切换 
        public static int ShowAni = 1043;//播放指定动画 
        public static int CommonUIIndexChange4Param = 1044;//主界面页签切换
        public static int CheckGameGuide = 1051;//检查新手引导
        // camp 任务
        public static int TaskStateChanged = 1052;// 任务状态修改
        public static int CampRedTips = 1057;// 任务状态修改

        //物品变化统一接口
        public static int ItemChanged = 1999;
        public static int ViewAdCallBack = 2000;

        // 多语言变化
        public static int Language = 1055;

        private static int MaxID = 1058;// 不断增大 ，给人看的，添加id 的时候记得 标注一下 

        private static int NewMsgFlag = 3000;//项目新增事件起点
        public static int CampsitePointDataChange = 3001;//营地点位解锁 <index>
        public static int CampsitePointApplyReward = 3002;//营地点位收取奖励 <index,reward>
        public static int CampsiteTotalRewardChange = 3003;//营地总收益变化

        public static int StageProgressChange = 3010;//关卡进度变化
        public static int GunCardDataChange = 3020;//枪械卡牌数据变化 <gunId>
        public static int GunCardOpt = 3021;//枪械卡牌操作

        public static int CurrencyChange = 3030;//货币变化 <CurrencyType>

        public static int PurchaseDataRefresh = 3040;//内购数据刷新
        public static int PurchaseSuccess = 3041;//内购购买成功<isRemedy, productId, rewardList>
        public static int PurchaseFail = 3042;//内购购买失败<status, productId>

        public static int ShopBuySuccess = 3050;//商店购买成功<shopId>

        public static int ServerTimeFixed = 3060;//服务器时间同步<success>
        public static int DateTimeRefresh = 3061;//x日刷新<DateTimeRefreshType>

        public static int IdleRewardClaim = 3070;//金币挂机领取

        public static int TaskDataChange = 3080;//任务数据变化
        public static int TaskFinish = 3081;//任务完成

        public static int StarReward = 3090;//关卡星级奖励
        public static int ChapterChange = 3091;//章节切换


        public static int UIPanelOpen = 4001;//打开UI事件<panelName>
        public static int UIPanelClose = 4002;//关闭UI事件<panelName>
        public static int UIPanelBackToTop = 4003;//UI回到顶层<panelName>
        public static int TopUIBeCovered = 4004;//顶层UI被覆盖<panelName>

        public static int ModuleOpen = 4010;//模块开放<GameModuleType>
    }
}
