using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ;
using Game.Data;

namespace Game
{
    public class CampsitePointMgr
    {
        public const int MAX_LVUP_ONCE = 50;//一次最大升级级数
        public const int QUICK_REWARD_MINUTES = 120;//待定
        public const int QUICK_REWARD_MAX_COUNT = 7;//待定
        public int campId { get; private set; }
        public int index { get; private set; }
        public CampBuilding_TableItem buildingRes { get; private set; }
        internal int UnlockStage { get { return unlockStage; } }
        private int unlockStage;
        /// <summary>
        /// 自动化等级
        /// </summary>
        public int AutoLv { get { return autoLv; } }
        private int autoLv;
        private double rewardBase;
        /// <summary>
        /// 点位基础结算周期
        /// </summary>
        public float RewardIntervalBase
        {
            get
            {
                return rewardIntervalBase;
            }
        }
        private float rewardIntervalBase;
        private double lvUpCostBase;

        /// <summary>
        /// 卡牌基础收益加成
        /// </summary>
        public double RewardFactorByCardBase { get { return rewardFactorByCardBase; } }
        private double rewardFactorByCardBase;
        /// <summary>
        /// 卡牌技能收益加成
        /// </summary>
        public double RewardFactorByCardSkill { get { return rewardFactorByCardSkill; } }
        private double rewardFactorByCardSkill;
        /// <summary>
        /// 卡牌收益总加成
        /// </summary>
        public double RewardFactorByCardTotal { get { return RewardFactorByCardBase * RewardFactorByCardSkill; } }

        /// <summary>
        /// 卡牌技能周期加成
        /// </summary>
        public float RewardIntervalFactorByCardSkill { get { return rewardIntervalFactorByCardSkill; } }
        private float rewardIntervalFactorByCardSkill;
        /// <summary>
        /// 卡牌周期总价成
        /// </summary>
        public float RewardIntervalFactorByCardTotal { get { return RewardIntervalFactorByCardSkill; } }

        /// <summary>
        /// 玩家收益加成
        /// </summary>
        public float RewardFactorByPlayer { get { return PlayerDataMgr.singleton.GetCampRewardFactorByPlayer(); } }

        /// <summary>
        /// 点位基础收益
        /// </summary>
        public double RewardBase
        {
            get
            {
                return rewardBase * campId * Math.Pow(1 + TableMgr.singleton.ValueTable.building_production_k1, lv - 1);
            }
        }

        /// <summary>
        /// 一次结算收益
        /// </summary>
        public double OnceRewardVal
        {
            get
            {
                return RewardBase * RewardFactorByCardTotal * RewardFactorByPlayer;
            }
        }
        /// <summary>
        /// 点位收益结算周期
        /// </summary>
        public float RewardInterval
        {
            get
            {
                return rewardIntervalBase / RewardIntervalFactorByCardTotal;
            }
        }
        /// <summary>
        /// 每秒收益率
        /// </summary>
        public double RawardRate { get { return OnceRewardVal / (RewardInterval <= 0 ? 1 : RewardInterval); } }

        public double LvUpCostBase { get { return CalcLvUpOnceBaseCost(lv); } }

        public bool isFrozen { get; private set; }
        public bool isUnlock { get; private set; }
        public bool isAuto { get; private set; }
        public bool isFight { get; private set; }
        public int equipGunId { get; private set; }
        public int quickRewardRemainder { get { return QUICK_REWARD_MAX_COUNT - quickRewardTimes + 1; } }
        private int quickRewardTimes = 1;

        private double timer;
        private double settlementReward_Nonauto;
        private double settlementReward_Auto;

        private int lv = 1;
        public int Lv { get { return lv; } }

        /// <summary>
        /// 可领取收益
        /// </summary>
        public double SettlementRewardVal
        {
            get
            {
                return settlementReward_Auto > 0 ? settlementReward_Auto : settlementReward_Nonauto;
            }
        }

        private bool isInOfflineState;
        /// <summary>
        /// 结算进度
        /// </summary>
        public float Progress
        {
            get { return Mathf.Clamp01((float)(timer / RewardInterval)); }
        }

        internal bool isDirty { get; set; }

        internal void Init(int campId, int index, int buildingId, int autoLv, int lastAutoLv)
        {
            this.campId = campId;
            this.index = index;
            this.buildingRes = TableMgr.singleton.CampBuildingTable.GetItemByID(buildingId);
            this.autoLv = autoLv;
            var cycleRes = TableMgr.singleton.CampBuildingCycleTable.GetItemByID(index + 1);
            if (cycleRes != null)
            {
                double rewardBaseModelFactor = CalcModelFactorByCardBase(lastAutoLv);

                rewardBase = cycleRes.productionInitValue * cycleRes.billingTime;
                rewardBase *= rewardBaseModelFactor;
                rewardIntervalBase = cycleRes.billingTime;
                lvUpCostBase = cycleRes.productionInitValue * rewardBaseModelFactor;//表中物资每秒产出基础值
                lvUpCostBase *= campId;
                lvUpCostBase *= RewardFactorByPlayer * CalcModelFactorByCardBase(autoLv) * CalcModelFactorBtCardSkill(autoLv);
            }
        }

        private int CalcModelRarity(int lv)
        {
            int rarity = 1;
            if (lv > 100)
                rarity = 3;
            else if (lv > 60)
                rarity = 2;

            return rarity;
        }

        private double CalcModelFactorByCardBase(int lv)
        {
            double factor = 1;
            if (lv > 0)
            {
                int rarity = CalcModelRarity(lv);

                int[] rewardRarityFactor = new int[] { 3, 6, 12 };

                factor = rewardRarityFactor[rarity - 1] * (1 + (lv - 1) *
                TableMgr.singleton.ValueTable.card_productionBonus_rate);
            }
            return factor;
        }

        private double CalcModelFactorBtCardSkill(int lv)
        {
            double factor = 1;
            if (lv > 0)
            {
                int rarity = CalcModelRarity(lv);

                int skillLv = rarity + PlayerDataMgr.singleton.CalcCampSkillAdd(lv);
                factor = skillLv + 1;
            }
            return factor;
        }

        internal void InitByCreate()
        {
            RefreshCardData();
            SetUnlockStage();
        }

        private void SetUnlockStage()
        {
            this.unlockStage = TableMgr.singleton.LevelTable.GetCampUnlockStage(this.autoLv);
        }

        internal void SetUnlockStage(int stageId)
        {
            this.unlockStage = stageId;
            isDirty = true;
        }

        internal void LoadSaveData(CampsitePointData data)
        {
            this.isUnlock = data.isUnlock;
            this.unlockStage = data.unlockStage;
            this.equipGunId = data.equipGunId;
            this.quickRewardTimes = data.quickRewardTimes;
            this.timer = data.timer;
            this.settlementReward_Nonauto = data.settlementReward_Nonauto;
            this.settlementReward_Auto = data.settlementReward_Auto;
            this.lv = data.lv;
            RefreshCardData();
            RefreshAutoState();
        }

        internal void FillSaveData(CampsitePointData buffer)
        {
            buffer.index = this.index;
            buffer.isUnlock = this.isUnlock;
            buffer.unlockStage = this.unlockStage;
            buffer.equipGunId = this.equipGunId;
            buffer.quickRewardTimes = this.quickRewardTimes;
            buffer.timer = this.timer;
            buffer.settlementReward_Nonauto = this.settlementReward_Nonauto;
            buffer.settlementReward_Auto = this.settlementReward_Auto;
            buffer.lv = this.lv;
        }

        internal void OnDayRefresh(DateTimeRefreshType type)
        {
            if (type == DateTimeRefreshType.OneDay)
            {
                quickRewardTimes = 1;
            }
        }

        internal void CheckFrozen(int maxUnlockPointIndex)
        {
            bool isFrozen = true;
            if (isUnlock || maxUnlockPointIndex == index - 1)
            {
                isFrozen = false;
            }
            if (this.isFrozen != isFrozen)
            {
                this.isFrozen = isFrozen;
                isDirty = true;
            }
        }

        internal void CheckUnlock(int finishStage)
        {
            if (this.isUnlock)
                return;

            if (finishStage == unlockStage)
            {
                this.isUnlock = true;
                timer = 0;
                RefreshFightState();
                isDirty = true;
            }
        }

        internal void SetUnlock()
        {
            if (isUnlock)
                return;
            isUnlock = true;
            timer = 0;
            RefreshFightState();
            isDirty = true;

            CampTaskMgr.singleton.AddTaskData(TaskType.Occupy_Building, buildingRes.id, index);
        }

        internal void LvUp(int upVal)
        {
            lv += upVal;
            isDirty = true;
        }

        /// <summary>
        /// 升级花费
        /// </summary>
        /// <param name="upVal"></param>
        /// <returns></returns>
        public double GetLvUpCost(int upVal)
        {
            double result = 0;
            for (int i = 0; i < upVal; i++)
            {
                result += CalcLvUpOnceCost(lv + i);
            }
            return result;
        }

        internal int CalcMaxLvUpVal(double budget)
        {
            int maxUpVal = 0;
            var cost = CalcLvUpOnceCost(lv + maxUpVal);
            while (budget >= cost)
            {
                maxUpVal++;
                budget -= cost;
                cost = CalcLvUpOnceCost(lv + maxUpVal);
            }
            return maxUpVal;
        }

        private double CalcLvUpOnceBaseCost(int curVal)
        {
            //return lvUpCostBase * campId * (Mathf.Pow(curVal, TableMgr.singleton.ValueTable.building_cost_k1));
            double cost = lvUpCostBase * Math.Pow(1 + TableMgr.singleton.ValueTable.building_production_k1, curVal - 1);
            cost *= 10;//等待时间
            return cost;
        }

        private double CalcLvUpOnceCost(int curVal)
        {
            return CalcLvUpOnceBaseCost(curVal);//*SkillFactor
        }

        internal void EquipGun(int gunId)
        {
            if (!isUnlock)
                return;
            if (this.equipGunId != gunId)
            {
                this.equipGunId = gunId;
                RefreshCardData();
                RefreshAutoState();
                CampsiteMgr.singleton.NotifyRefreshSaveData();
                isDirty = true;

                CampTaskMgr.singleton.AddTaskData(TaskType.Equip_Gun, gunId, index);
            }
        }

        internal void RemoveGun()
        {
            int gunId = 0;
            if (this.equipGunId != gunId)
            {
                this.equipGunId = gunId;
                RefreshCardData();
                RefreshAutoState();

                CampsiteMgr.singleton.NotifyRefreshSaveData();
                isDirty = true;
            }
        }

        internal void OnGunDataRefresh(int gunId)
        {
            if (!isUnlock)
                return;
            if (equipGunId == gunId)
            {
                RefreshCardData();
                RefreshAutoState();
                isDirty = true;
                CampTaskMgr.singleton.AddTaskData(TaskType.Equip_Gun, gunId, index);
            }
        }

        internal void SetOfflineState(bool isInOfflineState)
        {
            this.isInOfflineState = isInOfflineState;
            RefreshFightState();
        }

        private void RefreshCardData()
        {
            rewardFactorByCardBase = 1f;
            rewardFactorByCardSkill = 1f;
            rewardIntervalFactorByCardSkill = 1f;
            if (equipGunId > 0)
            {
                CalcFactorsByCard(equipGunId, out rewardFactorByCardBase, out rewardFactorByCardSkill, out rewardIntervalFactorByCardSkill);
            }
        }

        private void CalcFactorsByCard(int cardId, out double rewardFactorByCardBase, out double rewardFactorByCardSkill, out float intervalFactorByCardSkill)
        {
            rewardFactorByCardBase = 1f;
            rewardFactorByCardSkill = 1f;
            intervalFactorByCardSkill = 1f;
            if (cardId <= 0)
                return;

            rewardFactorByCardBase *= PlayerDataMgr.singleton.GetCampRewardFactorByCard(cardId);
            var skillId = PlayerDataMgr.singleton.GetCampSkillID(cardId);
            if (skillId > 0)
            {
                var skillData = TableMgr.singleton.CampGunSkillTable.GetItemByID(skillId);
                if (skillData != null && skillData.campBuilding == buildingRes.id)
                {
                    switch (skillData.type)
                    {
                        case 1:
                            {
                                rewardFactorByCardSkill *= (skillData.value);
                            }
                            break;
                        case 2:
                            {
                                if (skillData.value > 0 && !Mathf.Approximately(skillData.value, 0f))
                                    intervalFactorByCardSkill = skillData.value;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 技能是否对当前点位生效
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool CheckIsValidSkill(int skillId)
        {
            if (skillId <= 0)
                return false;
            var skillData = TableMgr.singleton.CampGunSkillTable.GetItemByID(skillId);
            return skillData != null && skillData.campBuilding == buildingRes.id;
        }
        /// <summary>
        /// 获取指定卡牌在当前点位的增益
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="totalRewardFactor"></param>
        /// <param name="totalIntervalFactor"></param>
        public void GetCardTotalFactorOnPoint(int cardId, out double totalRewardFactor, out float totalIntervalFactor)
        {
            double rewardFactorBase;
            double rewardFactorSkill;
            float intervalFactorSkill;
            CalcFactorsByCard(cardId, out rewardFactorBase, out rewardFactorSkill, out intervalFactorSkill);
            totalRewardFactor = rewardFactorBase * rewardFactorSkill;
            totalIntervalFactor = intervalFactorSkill;
        }

        private void RefreshAutoState()
        {
            if (!isUnlock)
                return;

            bool isAuto = CheckCardIsAuto(equipGunId);

            if (this.isAuto != isAuto)
            {
                this.isAuto = isAuto;
                settlementReward_Nonauto = 0;
                RefreshFightState();

                CampTaskMgr.singleton.AddTaskData(TaskType.Auto_Level, AutoLv, index);
            }
        }

        /// <summary>
        /// 放置指定卡牌可否自动化
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public bool CheckCardIsAuto(int cardId)
        {
            if (cardId <= 0)
                return false;

            var gunData = PlayerDataMgr.singleton.GetGunCardData(cardId);
            if (gunData != null && gunData.level >= autoLv)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void SettlementReward(double dt)
        {
            if (!isUnlock)
                return;
            if (dt <= 0)
                return;

            timer += dt;
            if (timer >= RewardInterval)
            {
                if (isAuto)
                {
                    var loop = Math.Floor(timer / RewardInterval);
                    settlementReward_Auto += loop * OnceRewardVal;
                    timer -= loop * RewardInterval;
                    isDirty = true;
                    //Debug.Log(string.Format("point {0} settlement reward {1}", index, SettlementRewardVal));
                }
                else
                {
                    if (settlementReward_Nonauto <= 0)
                    {
                        settlementReward_Nonauto = OnceRewardVal;
                        RefreshFightState();
                        isDirty = true;
                        //Debug.Log(string.Format("point {0} settlement reward {1}", index, SettlementRewardVal));
                    }
                    timer = RewardInterval;
                }
            }
        }

        private void RefreshFightState()
        {
            var isFight = !isInOfflineState && (isAuto || timer < RewardInterval);
            if (this.isFight != isFight)
            {
                this.isFight = isFight;
                isDirty = true;
            }
        }

        internal void ClaimReward()
        {
            if (isAuto)
            {
                ApplyReward();
            }
            else
            {
                if (SettlementRewardVal > 0)
                {
                    ApplyReward();
                    timer = 0;
                    RefreshFightState();
                }
            }
        }

        private void ApplyReward()
        {
            var reward = SettlementRewardVal;
            settlementReward_Nonauto = 0;
            settlementReward_Auto = 0;
            AddReward(reward);
        }

        internal double GetOfflineReward(double dt)
        {
            if (!isUnlock)
                return 0;
            double totalTime = timer + dt;
            double totalReward = 0;
            if (totalTime >= RewardInterval)
            {
                if (isAuto)
                {
                    var loop = Math.Floor(totalTime / RewardInterval);
                    double onceReward = OnceRewardVal;
                    totalReward = onceReward * loop;
                }
                else
                {
                    if (settlementReward_Nonauto > 0)
                        totalReward = settlementReward_Nonauto;
                    else
                    {
                        double onceReward = OnceRewardVal;
                        totalReward = onceReward;
                    }
                }
            }
            return totalReward;
        }

        internal void AddOfflineTime(double dt)
        {
            SettlementReward(dt);
        }

        internal double GetQuickRewardVal()
        {
            var dt = QUICK_REWARD_MINUTES * 60f;
            var rewardRate = OnceRewardVal / RewardInterval;
            return rewardRate * dt;
        }

        internal bool ClaimQuickReward()
        {
            if (!isUnlock)
                return false;
            var reward = GetQuickRewardVal();
            AddReward(reward);
            quickRewardTimes++;
            CampsiteMgr.singleton.NotifyRefreshSaveData();
            return true;
        }

        internal bool GetQuickRewardCost(out int diamond)
        {
            diamond = 0;
            if (quickRewardTimes > QUICK_REWARD_MAX_COUNT)
            {
                return false;
            }
            diamond = 20 * (quickRewardTimes);

            return true;
        }

        private void AddReward(double rewardVal)
        {
            if (rewardVal <= 0)
                return;

            Debug.Log(string.Format("Point{0} add reward: {1}", index + 1, rewardVal));
            CampsiteMgr.singleton.AddReward(rewardVal);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampsitePointApplyReward, index, rewardVal);
            isDirty = true;
        }

    }

    public enum CampsiteRequestResult
    {
        Undefine,
        Success,
        NetFail,
        DataFail_NoCampId,
        DataFail_NotEnough,
    }

    //所有需要的时间校验在管理器外部调用 校验成功后再调用管理器方法
    public class CampsiteMgr : Singleton<CampsiteMgr>
    {
        public delegate void ClaimRewardCallback(CampsiteRequestResult result);
        public delegate void CreateNewCallback(CampsiteRequestResult result);
        public delegate void FinishCampsiteCallback(CampsiteRequestResult result, List<GameGoodData> reward);

        public const int SHOW_CLAIMALL_ID = 3;//开始显示一键领取的营地id
        public const float OFFLINE_HOUR_BASE = 1f;//离线挂机基础上限 单位 小时
        public const float OFFLINE_HOUR_EACH_CARD = 0.5f;//每张卡牌提供的离线挂机时间 单位 小时
        private const double OFFLINE_CHECK_THRESHOLD = 300;//单位 秒
        private const float RECORD_INTERVAL = 1f;//存档间隔 秒
        private int id;
        public int Id { get { return id; } }
        private string sceneName;
        public string SceneName { get { return sceneName; } }
        private CampsitePointMgr[] points;
        public CampsitePointMgr[] Points { get { return points; } }
        private double totalRewardVal;
        public double TotalRewardVal { get { return totalRewardVal; } }
        public double TotalRewardRate
        {
            get
            {
                if (points == null)
                    return 0;
                double result = 0;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].isUnlock)
                        result += points[i].RawardRate;
                }
                return result;
            }
        }

        public int NewUnlockPointIndex { get; private set; } = -1;

        private DateTime lastRecordTime;
        private bool hasOfflineReward;

        private float timer;
        private float recordTimer;
        private int maxUnlockIndex;

        private bool isDirty;

        private Action<bool> fixedServerTimeCallback;

        public void Init()
        {
            RegisterListeners();
            if (!LoadSaveData())
            {

            }
            else
            {
                if (!hasOfflineReward)
                {
                    double dt; double reward;
                    CheckOfflineReward(out dt, out reward);
                }
                CheckPointsFrozen();
            }
            timer = Time.realtimeSinceStartup;
            recordTimer = Time.realtimeSinceStartup;
        }

        public override void ClearSingleton()
        {
            base.ClearSingleton();
            UnRegisterListeners();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<bool>(MsgIds.ServerTimeFixed, OnServerTimeFixed);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ServerTimeFixed);
            Global.gApp.gMsgDispatcher.AddListener<DateTimeRefreshType>(MsgIds.DateTimeRefresh, OnDayRefresh);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.DateTimeRefresh);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.GunCardDataChange, OnGunDataChange);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.GunCardDataChange);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<bool>(MsgIds.ServerTimeFixed, OnServerTimeFixed);
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.GunCardDataChange, OnGunDataChange);
            Global.gApp.gMsgDispatcher.RemoveListener<DateTimeRefreshType>(MsgIds.DateTimeRefresh, OnDayRefresh);
        }

        public void OnGameStart()
        {
            if (id <= 0)
            {
                CreateNew(1);//第一次进入游戏会进行服务器时间校验 因此不做重复校验
            }
        }

        private bool LoadSaveData()
        {
            CampsiteData data = PlayerDataMgr.singleton.DB.campsiteData;//从DB取值
            if (data != null)
            {
                var res = TableMgr.singleton.CampSetTable.GetItemByID(data.id);
                if (res == null)
                    return false;
                if (res.CampBuildingArr.Length != data.points.Length)
                    return false;

                int lastAutoLv = 0;

                this.id = data.id;
                this.sceneName = res.sceneName;
                this.totalRewardVal = data.totalRewardVal;
                this.lastRecordTime = data.lastRecordTime;
                this.hasOfflineReward = data.hasOfflineReward;
                this.points = new CampsitePointMgr[data.points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    var pData = data.points[i];
                    CampsitePointMgr p = new CampsitePointMgr();
                    p.Init(id, i, res.CampBuildingArr[i], res.AutoLevelArr[i], lastAutoLv);
                    p.LoadSaveData(pData);
                    p.SetOfflineState(hasOfflineReward);
                    this.points[i] = p;
                    lastAutoLv = res.AutoLevelArr[i];
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckPointsFrozen()
        {
            if (points != null)
            {
                maxUnlockIndex = -1;
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].isUnlock)
                    {
                        maxUnlockIndex = i;
                    }
                    else
                    {
                        points[i].CheckFrozen(maxUnlockIndex);
                    }
                }
            }
        }

        public bool CheckOfflineReward(out double dt, out double reward)
        {
            var ts = DateTimeMgr.singleton.UtcNow - lastRecordTime;
            dt = ts.TotalSeconds;
            dt = Math.Min(dt, GetMaxOfflineHour() * 60 * 60);
            reward = 0;
            if (points == null || dt <= 0)
                return false;
            if (dt <= OFFLINE_CHECK_THRESHOLD)
            {
                SettlementPointsReward(dt);
                lastRecordTime = DateTimeMgr.singleton.UtcNow;
                NotifyRefreshSaveData();
                return false;
            }
            for (int i = 0; i < points.Length; i++)
            {
                reward += points[i].GetOfflineReward(dt);
            }
            if (reward >= 1)
            {
                hasOfflineReward = true;
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].SetOfflineState(true);
                }
                Debug.Log("OfflineStart!");
                return true;
            }
            else
            {
                SettlementPointsReward(dt);
                lastRecordTime = DateTimeMgr.singleton.UtcNow;
                NotifyRefreshSaveData();
                return false;
            }
        }

        public void RequestClaimOfflineReward(ClaimRewardCallback callback)
        {
            fixedServerTimeCallback += (success) =>
              {
                  if (success)
                  {
                      AddOfflineReward();
                      callback?.Invoke(CampsiteRequestResult.Success);
                  }
                  else
                  {
                      callback?.Invoke(CampsiteRequestResult.NetFail);
                  }
              };
            DateTimeMgr.singleton.FixedLocalTime();
        }

        private void AddOfflineReward()
        {
            var ts = DateTimeMgr.singleton.UtcNow - lastRecordTime;
            var dt = ts.TotalSeconds;
            dt = Math.Min(dt, GetMaxOfflineHour() * 60 * 60);
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].AddOfflineTime(dt);
                    points[i].SetOfflineState(false);
                }
            }
            lastRecordTime = DateTimeMgr.singleton.UtcNow;
            hasOfflineReward = false;
            NotifyRefreshSaveData();
            Debug.Log("OfflineEnd! dt = " + dt);
        }

        public float GetMaxOfflineHour()
        {
            int heroNum = PlayerDataMgr.singleton.GetCollectCardCount();
            return GetMaxOfflineHour(heroNum);
        }

        public float GetMaxOfflineHour(int heroNum)
        {
            return OFFLINE_HOUR_BASE + heroNum * OFFLINE_HOUR_EACH_CARD;
        }

        public bool CanClaimAll()
        {
            return id >= SHOW_CLAIMALL_ID;
        }

        public void ClaimAllReward()
        {
            if (points == null)
                return;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].ClaimReward();
            }
        }

        public bool HasNext(out int nextId)
        {
            nextId = id + 1;
            return TableMgr.singleton.CampSetTable.GetItemByID(nextId) != null;
        }
        
        public void RequestFinishCampsite(FinishCampsiteCallback callback)
        {
            int nextCampId;
            if (!HasNext(out nextCampId))
            {
                callback.Invoke(CampsiteRequestResult.DataFail_NoCampId, null);
                return;
            }

            fixedServerTimeCallback += (success) =>
            {
                if (success)
                {
                    var res = TableMgr.singleton.CampSetTable.GetItemByID(id);
                    GameGoodsMgr.singleton.RequestAddGameGoods((rst, realAdd, tips) =>
                    {
                        if (rst == GoodsRequestResult.Success)
                        {
                            CreateNew(nextCampId);
                            callback?.Invoke(CampsiteRequestResult.Success, realAdd);
                        }
                        else
                        {
                            callback?.Invoke(CampsiteRequestResult.NetFail, null);
                        }

                    }, res.reward, res.reward_count, res.reward_type);


                }
                else
                {
                    callback?.Invoke(CampsiteRequestResult.NetFail, null);
                }
            };

            DateTimeMgr.singleton.FixedLocalTime();
        }

        private void CreateNew(int campId)
        {
            if (id == campId)
                return;

            var res = TableMgr.singleton.CampSetTable.GetItemByID(campId);
            if (res == null)
                return;

            id = campId;
            sceneName = res.sceneName;
            points = new CampsitePointMgr[res.CampBuildingArr.Length];
            int lastAutoLv = 0;
            for (int i = 0; i < res.CampBuildingArr.Length; i++)
            {
                CampsitePointMgr p = new CampsitePointMgr();
                p.Init(id, i, res.CampBuildingArr[i], res.AutoLevelArr[i], lastAutoLv);
                p.InitByCreate();
                if (campId == 1 && i < TableMgr.singleton.ValueTable.CampLevel_1Arr.Length)
                {
                    p.SetUnlockStage(TableMgr.singleton.ValueTable.CampLevel_1Arr[i]);
                }
                else if (campId == 2 && i < TableMgr.singleton.ValueTable.CampLevel_2Arr.Length)
                {
                    p.SetUnlockStage(TableMgr.singleton.ValueTable.CampLevel_2Arr[i]);
                }
                points[i] = p;
                lastAutoLv = res.AutoLevelArr[i];
            }
            totalRewardVal = 0;
            lastRecordTime = DateTimeMgr.singleton.UtcNow;
            hasOfflineReward = false;
            NotifyRefreshSaveData();
            CheckPointsFrozen();
            PlayerDataMgr.singleton.SetPlayerLevel(id);
        }

        internal void NotifyRefreshSaveData()
        {
            isDirty = true;
        }

        private void RefreshSaveData()
        {
            CampsiteData data = PlayerDataMgr.singleton.DB.campsiteData;
            if (data == null)
            {
                data = new CampsiteData();
            }
            data.id = this.id;
            data.totalRewardVal = this.totalRewardVal;
            data.lastRecordTime = this.lastRecordTime;
            data.hasOfflineReward = this.hasOfflineReward;
            CampsitePointData[] pointsBuffer = data.points;
            if (data.points == null || data.points.Length != this.points.Length)
            {
                data.points = new CampsitePointData[this.points.Length];
            }
            for (int i = 0; i < this.points.Length; i++)
            {
                CampsitePointData buffer = pointsBuffer != null && i < pointsBuffer.Length ? pointsBuffer[i] : new CampsitePointData();
                this.points[i].FillSaveData(buffer);
                data.points[i] = buffer;
            }

            PlayerDataMgr.singleton.DB.campsiteData = data;
            PlayerDataMgr.singleton.NotifySaveData();
        }

        public void Update()
        {
            if (isDirty)
            {
                RefreshSaveData();
                isDirty = false;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                RequestFinishCampsite((result, rewards) =>
                 {
                     if (result == CampsiteRequestResult.Success)
                     {
                         foreach (var reward in rewards)
                         {
                             Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CurrencyChange, GameGoodsMgr.singleton.Goods2Currency(reward.type));
                         }
                         Global.gApp.gGameCtrl.ChangeToMainScene(3, true);
                     }
                 });
            }

            float dt = Time.realtimeSinceStartup - timer;
            timer = Time.realtimeSinceStartup;

            //离线收益清空后才计算在线收益
            if (hasOfflineReward)
                return;

            lastRecordTime = lastRecordTime.AddSeconds(dt);

            if (points == null)
                return;

            SettlementPointsReward(dt);
            CheckPointsDirty();

            if (Time.realtimeSinceStartup - recordTimer >= RECORD_INTERVAL)
            {
                NotifyRefreshSaveData();
                recordTimer = Time.realtimeSinceStartup;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                UnlockNext();
            }
        }

        private void SettlementPointsReward(double dt)
        {
            if (points == null)
                return;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].SettlementReward(dt);
            }
        }

        private void CheckPointsDirty()
        {
            if (points == null)
                return;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].isDirty)
                {
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampsitePointDataChange, i);
                    NotifyRefreshSaveData();
                    points[i].isDirty = false;
                }
            }
        }

        public bool HasPoint(int index)
        {
            return points != null && index >= 0 && index < points.Length;
        }

        public CampsitePointMgr GetPointByIndex(int index)
        {
            if (HasPoint(index))
            {
                return points[index];
            }

            return null;
        }

        public void AddReward(double value)
        {
            totalRewardVal += value;
            NotifyRefreshSaveData();
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampsiteTotalRewardChange);

            CampTaskMgr.singleton.AddTaskData(TaskType.Supplies_Add, (System.Numerics.BigInteger)value);
        }

        private bool CostReward(double value)
        {
            if (value > totalRewardVal)
            {
                return false;
            }

            totalRewardVal -= value;
            NotifyRefreshSaveData();
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CampsiteTotalRewardChange);
            return true;
        }

        public int GetNextUnlockPoint(int index)
        {
            int nextIndex = index + 1;
            if (nextIndex > maxUnlockIndex)
                nextIndex = 0;
            return nextIndex;
        }

        public int GetLastUnlockPoint(int index)
        {
            int lastIndex = index - 1;
            if (lastIndex < 0)
                lastIndex = maxUnlockIndex;
            return lastIndex;
        }

        public bool CheckCardIsOccupied(int cardId, out int occupiedPointIndex)
        {
            occupiedPointIndex = 0;
            if (points == null)
                return false;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].equipGunId == cardId)
                {
                    occupiedPointIndex = points[i].index;
                    return true;
                }
            }
            return false;
        }

        public int GetUnlockNextStageId()
        {
            if (points == null)
                return 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (!points[i].isUnlock)
                {
                    return points[i].UnlockStage;
                }
            }

            return 0;
        }

        public void UnlockNext()
        {
            if (points == null)
                return;
            for (int i = 0; i < points.Length; i++)
            {
                if (!points[i].isUnlock)
                {
                    points[i].SetUnlock();
                    NewUnlockPointIndex = i;
                    break;
                }
            }
            CheckPointsFrozen();
        }

        public void ResetNewUnlockPointIndex()
        {
            NewUnlockPointIndex = -1;
        }

        public bool HasPointCanSetGun()
        {
            if (points == null)
                return false;
            for (int i = 0; i < points.Length; i++)
            {
                if (PointCanSetGun(points[i].index))
                    return true;
            }
            return false;
        }
        #region 点位操作接口
        public void LvUpPoint(int pointIndex, int upVal)
        {
            if (upVal <= 0)
                return;

            var point = GetPointByIndex(pointIndex);
            if (point == null)
                return;
            var cost = point.GetLvUpCost(upVal);
            if (!CostReward(cost))
            {
                //tip
                return;
            }
            point.LvUp(upVal);
            Debug.Log(string.Format("point {0} lv up {1} cur = {2}", pointIndex, upVal, point.Lv));
            NotifyRefreshSaveData();

            CampTaskMgr.singleton.AddTaskData(TaskType.LvUp_Building, upVal, pointIndex);
            CampTaskMgr.singleton.AddTaskData(TaskType.Buildings_Level, upVal);
        }

        public int GetPointMaxLvUpVal(int pointIndex)
        {
            var point = GetPointByIndex(pointIndex);
            if (point == null)
                return 0;
            return point.CalcMaxLvUpVal(totalRewardVal);
        }

        public void PointEquipGun(int pointIndex, int gunId)
        {
            int occupiedPointIndex;
            if (CheckCardIsOccupied(gunId, out occupiedPointIndex))
            {
                PointRemoveGun(occupiedPointIndex);
            }
            GetPointByIndex(pointIndex)?.EquipGun(gunId);
        }

        public void PointRemoveGun(int pointIndex)
        {
            GetPointByIndex(pointIndex)?.RemoveGun();
        }

        public bool PointCanSetGun(int pointIndex)
        {
            var point = GetPointByIndex(pointIndex);
            if (point == null)
                return false;
            if (!point.isUnlock || point.equipGunId > 0)
                return false;

            var limitGunType = point.buildingRes.gunType;
            var gunCards = PlayerDataMgr.singleton.GetCardsByType(limitGunType);
            for (int i = 0; i < gunCards.Count; i++)
            {
                int occupiedPointIndex;
                if (!CheckCardIsOccupied(gunCards[i], out occupiedPointIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public void ClaimPointReward(int pointIndex)
        {
            GetPointByIndex(pointIndex)?.ClaimReward();
        }

        public double GetPointQuickRewardVal(int pointIndex)
        {
            var point = GetPointByIndex(pointIndex);
            if (point == null)
                return 0;
            return point.GetQuickRewardVal();
        }

        public bool ClaimPointQuickReward(int pointIndex)
        {
            var point = GetPointByIndex(pointIndex);
            if (point == null)
                return false;
            return point.ClaimQuickReward();
        }

        public bool GetPointQuickRewardCost(int pointIndex, out int diamond)
        {
            var point = GetPointByIndex(pointIndex);
            if (point != null)
                return point.GetQuickRewardCost(out diamond);

            diamond = 0;
            return false;
        }
        #endregion
        private void OnGunDataChange(int gunId)
        {
            if (points == null)
                return;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].OnGunDataRefresh(gunId);
            }
        }

        private void OnServerTimeFixed(bool success)
        {
            fixedServerTimeCallback?.Invoke(success);
            fixedServerTimeCallback -= fixedServerTimeCallback;
        }

        private void OnDayRefresh(DateTimeRefreshType type)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].OnDayRefresh(type);
                }
                NotifyRefreshSaveData();
            }
        }

        //由CampsiteFightScene胜利时调用
        public void OnCampsiteLevelWin()
        {
            UnlockNext();
        }

        public void OnApplicationPause(bool pause)
        {

        }
    }
}