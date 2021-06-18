
using EZ;
using Game.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Game
{

    public class PlayerDataMgr : Singleton<PlayerDataMgr>
    {
        public Data.PlayerData DB { get { return Data.Models.Instance.PlayerData; } }

        public int Diamond { get { return GetCurrency(CurrencyType.DIAMOND); } }
        public BigInteger Gold { get { return GetBigCurrency(CurrencyType.GOLD); } }

        public int Key { get { return GetCurrency(CurrencyType.KEY); } }
        public int NewGuideID { get; set; }

        public int EnterStageID { get; set; } = 0;

        public float StageHpParam { get; set; } = 0;
        public int StageAtkLevel { get; private set; } = 0;
        public int StageGold { get; private set; } = 0;
        public float StageWaveFactor { get; set; } = 0;

        public int AutoEnterStageID { get; set; } = -1;

        public Dictionary<int, List<int>> dropCardDic = new Dictionary<int, List<int>>();

        public int CardBoxOpenCount { get; private set; } = 0;

        private bool needSaveData = false;

        private string PlayerID;

        public bool IsFirstLogin { get; set; } = false;

        public FightResult LastFightResult { get; private set; } = FightResult.Undefine;

        public PlayerDataMgr()
        {

        }


        public void Init()
        {
            Data.Models.Instance.Init();

            PlayerID = string.Format("{0}_{1}", SystemInfo.deviceUniqueIdentifier, DB.guid);

            Debug.Log("PlayerID " + PlayerID);

            UpdateVersion();

            //UpdateDropCard();

            //for (int i = 0; i < 5; i++)
            //{
            //    DB.stageDropCards.Add(new RewardCard()
            //    {
            //        count = 1,
            //        quality = 1
            //    });
            //}

            DateTimeMgr.singleton.onTimeRefresh -= RefreshData;
            DateTimeMgr.singleton.onTimeRefresh += RefreshData;
        }

        void UpdateVersion()
        {
            if (Models.Instance.GetVersion() < Models.Version)
            {

            }

            Data.Models.Instance.UpdateVersion();
        }

        public void SyncServerData(string serverData)
        {
#if KEEPACCOUNT_DB
            if (string.IsNullOrEmpty(serverData))
            {
                Debug.Log("no server data");
                return;
            }
            try
            {
                LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(serverData);
                string dbStr;
                if (jsonData.TryGetStringVal("db", out dbStr))
                {
                    Models.Instance.SyncServerJson(dbStr);
                }
                else
                {
                    Debug.LogError("load from server error: db not find");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("load from server error:" + ex);
            }
#endif
        }

        public void SendDbToServer()
        {
#if KEEPACCOUNT_DB
            var dbStr = Models.Instance.GetSendToServerData();
            KeepAccountUtils.GetInstance().DoOfflineOperationByKey("db", dbStr);
#endif
        }

        public void Update()
        {
            if (needSaveData)
            {
                if (Data.Models.Instance != null)
                    Data.Models.Instance.SaveToFile(true);

                SendDbToServer();

                needSaveData = false;

                TipsMgr.singleton.UpdateRedTips();
            }

            //cal_currency_reg(CurrencyType.POWER);
        }

        //第一次进入游戏初始化默认玩家数据
        public void InitDefaultPlayerData()
        {
            Debug.Log("InitDefaultPlayerData ");

            IsFirstLogin = true;
            DB.guid = Guid.NewGuid();
            DB.createTime = DateTime.Today;
            DB.curStageID = 1;
            DB.maxUnlockStageID = 1;
            DB.chapterData.startStageID = 1;
            DB.curGuideID = 1;
            DB.curGuideStageIndex = 1;
            DB.level = 1;
            DB.exp = 0;
            //DB.useWeaponID = 1001;
            //AddBigCurrency(CurrencyType.GOLD, 5000);
            UnlockStage(DB.curStageID);
            //DB.gold = 150;
            //DB.refreshTime = DB.createTime.AddHours(TableMgr.singleton.ValueTable.refresh_data_time);
            //DB.refreshTime = DB.refreshTime.ToUniversalTime();
            DB.refreshTime = DateTime.MinValue;
            Debug.Log("refreshTime " + DB.refreshTime);

            AddCard(TableMgr.singleton.ValueTable.init_weapon_id);
            SetUseWeaponID(TableMgr.singleton.ValueTable.init_weapon_id);

            //var boxRes = TableMgr.singleton.BoxTable.GetItemByID(1);
            //if (boxRes != null)
            //{
            //    DB.boxDrawDic[boxRes.id] = boxRes.awardCount - TableMgr.singleton.ValueTable.box_init_award_count;
            //}
            //var cardStr = TableMgr.singleton.ValueTable.init_battle_cards.Split(',');
            //for (int i = 0; i < cardStr.Length; ++i)
            //{
            //    var cardID = int.Parse(cardStr[i]);
            //    AddCard(cardID, 0, false);
            //    AddBattleCard(cardID);
            //    SetCardNew(cardID);
            //}

            //AddCurrency(CurrencyType.POWER, 30);

            //UpdateBoxID(DB.curStageID);
            //ResetBoxPRD();

            //UpdateBoxID(DB.curStageID);

            //InitLabyrinthData();

#if SKIP_GUIDE
            foreach (GameModuleType module in Enum.GetValues(typeof(GameModuleType)))
            {
                OpenModule(module);
            }
            PlayerDataMgr.singleton.SetGuideStageFinish(4);
            Battles.Objects.HeroType[] heros = new Battles.Objects.HeroType[]
            {
                 Battles.Objects.HeroType.ThunderMaster,
                  Battles.Objects.HeroType.FireMaster,
                   Battles.Objects.HeroType.IceMaster,
                    //Battles.Objects.HeroType.Archer,
                     Battles.Objects.HeroType.Crossbow
            };
            for (int i = 0; i < heros.Length; i++)
            {
                PlayerDataMgr.singleton.AddCard((int)heros[i], 0, false);
                PlayerDataMgr.singleton.AddBattleCard((int)heros[i]);
            }

            DB.cardDatas[1].level = 2;

            AddBigCurrency(CurrencyType.GOLD, 5000);
#endif
            //////////////////////////
            ///TEST
#if GAME_DEBUG
            //var str = "1,2,3,7,8";
            //var cards = str.Split(',');
            //for (int i = 0; i < cards.Length; ++i)
            //{
            //    var cardID = int.Parse(cards[i]);
            //    AddCard(cardID);
            //    AddBattleCard(cardID);
            //    SetCardNew(cardID);
            //}
            AddBigCurrency(CurrencyType.GOLD, 90000000);
            AddCurrency(CurrencyType.DIAMOND, 10000000);
            AddCurrency(CurrencyType.KEY, 1000);
            foreach (GunCard_TableItem item in TableMgr.singleton.GunCardTable.getEnumerator())
            {
                AddCard(item.id);
                AddCardChip(item.id, 100);
            }

            ////foreach (Stage_TableItem item in TableMgr.singleton.StageTable.getEnumerator())
            ////{
            ////    UnlockStage(item.id);
            ////}

            //DB.curGuideStageIndex = 5;
#endif
        }

        public string GetPlayerInfo()
        {
            return string.Format("{0},{1}", PlayerID, DateTime.Today > DB.createTime ? 0 : 1);
        }

        public bool IsNewPlayer()
        {
            return DateTime.Today == DB.createTime;
        }

        public bool DeleteSaveData()
        {
            Data.Models.Instance.RemoveSave();
            return false;
        }


        public void NotifySaveData()
        {
            needSaveData = true;
        }
        /////////////////////////////////////////////////////////////////////
        public void OpenModule(GameModuleType module)
        {
            if (module == GameModuleType.Undefine)
                return;
            int moduleVal = (int)module;
            if (!DB.openModules.Contains(moduleVal))
            {
                DB.openModules.Add(moduleVal);
                NotifySaveData();
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ModuleOpen, module);
            }
        }

        public bool ModuleIsOpen(GameModuleType module)
        {
            int moduleVal = (int)module;
            return DB.openModules.Contains(moduleVal);
        }
        /////////////////////////////////////////////////////////////////////
        static int SafeAdd(int a, int b, int max)
        {
            Int64 _a = a;
            _a += b;
            if (_a > max)
            {
                _a = max;
            }
            return (int)_a;
        }

        static BigInteger SafeAdd(BigInteger a, BigInteger b, BigInteger max)
        {
            if (max <= 0)
                return a + b;
            var _a = a + b;
            if (_a > max)
            {
                _a = max;
            }
            return _a;
        }

        public int GetMaxCurrency(CurrencyType type)
        {
            var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)type);
            if (currency_res == null)
                return 0;

            int max;
            if (int.TryParse(currency_res.max, out max))
            {
                return max;
            }
            else
            {
                Debug.Log(type.ToString() + "货币上限不为int类型:" + currency_res.max);
                return 0;
            }
        }

        public BigInteger GetMaxBigCurrency(CurrencyType type)
        {
            var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)type);
            if (currency_res == null)
                return 0;

            BigInteger max;
            if (BigInteger.TryParse(currency_res.max, out max))
            {
                return max;
            }
            else
            {
                Debug.Log(type.ToString() + "货币上限不为BigInteger类型:" + currency_res.max);
                return 0;

            }
        }

        public bool AddCurrency(CurrencyType type, int value, bool broadcast = true)
        {
            if (value < 0)
                return false;

            var old = GetCurrency(type);
            var cur = SafeAdd(old, value, GetMaxCurrency(type));
            if (cur == old)
                return true;

            SetCurrency(type, cur, broadcast);
            return true;
        }

        public bool AddBigCurrency(CurrencyType type, BigInteger value, bool broadcast = true)
        {
            if (value < 0)
                return false;

            var old = GetBigCurrency(type);
            var max = GetMaxBigCurrency(type);
            BigInteger cur = SafeAdd(old, value, max);
            if (cur == old)
                return true;

            SetBigCurrency(type, cur, broadcast);

            if (type == CurrencyType.GOLD)
                CampTaskMgr.singleton.AddTaskData(TaskType.Get_Gold, value);

            return true;
        }

        public void SetCurrency(CurrencyType type, int _value, bool broadcast = true)
        {
            DB.currencys[type] = _value;

            OnCurrencyChange(type);
            if (broadcast)
                BroadcastCurrencyChange(type);
            NotifySaveData();
        }

        public void SetBigCurrency(CurrencyType type, BigInteger _value, bool broadcast = true)
        {
            DB.bigCurrencys[type] = _value;

            OnCurrencyChange(type);
            if (broadcast)
                BroadcastCurrencyChange(type);
            NotifySaveData();
        }

        public int GetCurrency(CurrencyType type)
        {
            int old;
            if (DB.currencys.TryGetValue(type, out old))
                return old;
            return 0;
        }

        public BigInteger GetBigCurrency(CurrencyType type)
        {
            BigInteger old;
            if (DB.bigCurrencys.TryGetValue(type, out old))
            {
                return old;
            }
            return 0;
        }

        public void CostCurrency(CurrencyType type, int value, bool broadcast = true)
        {
            if (value <= 0)
                return;

            var old = GetCurrency(type);
            int costCur = Math.Max(0, old - value);

            SetCurrency(type, costCur, broadcast);
        }

        public void CostBigCurrency(CurrencyType type, BigInteger value, bool broadcast = true)
        {
            if (value <= 0)
                return;

            var old = GetBigCurrency(type);
            var costCur = old - value;
            if (costCur < 0)
                costCur = 0;

            SetBigCurrency(type, costCur, broadcast);

            CampTaskMgr.singleton.AddTaskData(TaskType.Cost_Gold, value);
        }

        void set_last_currency_reg_time(CurrencyType type, DateTime time)
        {
            DB.currencyRegtimes[type] = time;

            NotifySaveData();
        }

        public void cal_currency_reg(CurrencyType type)
        {
            var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)type);
            if (currency_res == null)
                return;

            var max = GetMaxCurrency(type);
            if (currency_res.reg_time <= 0 || GetCurrency(type) >= max)
                return;

            DateTime reg_time = DateTime.Now;
            if (!DB.currencyRegtimes.TryGetValue(type, out reg_time))
                return;

            var dt = DateTime.Now - reg_time;
            if (dt.TotalSeconds <= 0) return;


            int val = get_reg_value(ref dt, currency_res.reg_time);
            if (val < 1) return;
            set_last_currency_reg_time(type, DateTime.Now - dt);

            if (GetCurrency(type) + val > max)
                SetCurrency(type, max);
            else
                AddCurrency(type, val);

        }

        public int get_reg_value(ref TimeSpan t, int reg_period)
        {
            if (reg_period == 0)
            {
                return 0;
            }
            if (t.TotalSeconds <= reg_period) return 0;
            int val = (int)t.TotalSeconds / reg_period;
            t = TimeSpan.FromSeconds(t.TotalSeconds % reg_period);

            return val;
        }

        void OnCurrencyChange(CurrencyType type)
        {
        }

        public void BroadcastCurrencyChange(CurrencyType type)
        {
            //UI.UIMgr.singleton.BoradCast(UIEventType.CURRENCY, type);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CurrencyChange, type);
        }

        public double GetCurrencyCountDown(CurrencyType type)
        {
            var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)type);
            if (currency_res == null)
                return 0.0f;

            DateTime reg_time = DateTime.Now;
            if (!DB.currencyRegtimes.TryGetValue(type, out reg_time))
                return 0.0f;

            var dt = DateTime.Now - reg_time;
            var countDownDt = new TimeSpan(0, 0, currency_res.reg_time) - dt;
            return countDownDt.TotalSeconds;
        }

        ////////////////////////////////////////////////////////////////////
        ///
        public bool AddCard(int cardID, int star = 0, bool report = true)
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return false;

            //var starRes = TableMgr.singleton.CardStarTable.GetItemByID(cardRes.quality);
            //if (starRes == null)
            //    return false;

            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
            {
                data = new GunCardData();
                data.cardID = cardID;
                data.count = 0;
                data.isNew = true;
                data.level = 1;// GetCardInitLv(cardRes.quality);
                data.star = GetCardInitStar(cardRes.rarity);
                data.fuseSkillID = cardRes.fuseSkill;
                data.campSkillID = cardRes.campGunSkill;

                DB.cardDatas[cardID] = data;

                //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, 0);
                //if (report)
                //{
                //    if (!GuideMgr.singleton.IsGuideStart())
                //        StatsMgr.singleton.ReportEvent(string.Format("unlock_new_card"),
                //            new Dictionary<string, string>()
                //            {
                //                ["card_id"] = cardID.ToString()
                //            });
                //}
            }
            else
            {
                data.count += 10;

                //if (data.star < TableMgr.singleton.ValueTable.card_max_star &&
                //    data.count >= starRes.starChipList[data.star])
                //{
                //    data.count -= starRes.starChipList[data.star];
                //    data.star++;

                //    //if (report)
                //    //{
                //    //    if (!GuideMgr.singleton.IsGuideStart())
                //    //    {
                //    //        StatsMgr.singleton.ReportEvent(string.Format("upgrade_card_stars"),
                //    //            new Dictionary<string, string>()
                //    //            {
                //    //                ["card_id"] = cardID.ToString()
                //    //            });
                //    //    }
                //    //}
                //}
                //else if (data.star >= TableMgr.singleton.ValueTable.card_max_star)
                //{
                //    //var stageRes = TableMgr.singleton.TowerTable.GetItemByID(GetMaxUnlockStageID());
                //    //if (stageRes != null)
                //    //{
                //    //    AddBigCurrency(CurrencyType.GOLD, data.count * stageRes.idleSpeed * 60);

                //    //}

                //    //data.count = 0;
                //}

                //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, cardID);
            }

            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            NotifySaveData();

            return true;
        }

        public bool AddCardChip(int cardID, int chipCount)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
            {
                return false;
            }

            data.count += chipCount;

            NotifySaveData();
            //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            return true;

        }

        public bool CostCardChip(int cardID, int chipCount)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
            {
                return false;
            }

            if (data.count < chipCount)
                return false;

            data.count -= chipCount;

            NotifySaveData();

            return true;
        }

        public GunCardData GetGunCardData(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return null;

            return data;
        }

        public bool AddFuseCard(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            if (DB.fusedCardList.Count >= TableMgr.singleton.ValueTable.fuse_card_max_count)
                return false;

            if (DB.fusedCardList.Contains(cardID))
                return false;

            DB.fusedCardList.Add(cardID);

            Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            CampTaskMgr.singleton.AddTaskData(TaskType.Fuse_Gun, 1);

            NotifySaveData();

            return true;
        }

        public bool RemoveFuseCard(int cardID)
        {
            if (!DB.fusedCardList.Contains(cardID))
                return false;

            DB.fusedCardList.Remove(cardID);

            Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            NotifySaveData();

            return true;
        }

        public List<int> GetFusedCardList()
        {
            return DB.fusedCardList;
        }

        public bool ReplaceFuseCard(int cardID, int replaceID)
        {
            var index = DB.fusedCardList.IndexOf(cardID);
            if (index < 0)
                return false;
            //Debug.Log("index " + index);
            DB.fusedCardList.Remove(cardID);
            DB.fusedCardList.Insert(index, replaceID);

            //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, 0);
            Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            NotifySaveData();

            return true;
        }

        public List<int> GetFuseSkills()
        {
            List<int> list = new List<int>();
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(DB.useWeaponID, out data))
                return list;

            list.Add(data.fuseSkillID);

            foreach (var card in DB.fusedCardList)
            {
                if (!DB.cardDatas.TryGetValue(card, out data))
                    continue;

                list.Add(data.fuseSkillID);
            }

            return list;
        }

        public int GetCampSkillID(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return 0;

            return data.campSkillID;
        }

        public float GetCampRewardFactorByCard(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return 1f;

            return data.GetCampRewardFactor();
        }

        public float GetCampRewardFactorByPlayer()
        {
            var playerLv = GetPlayerLevel();
            return 1 + (playerLv - 1) * TableMgr.singleton.ValueTable.player_levelup_production_k1;
        }

        public List<int> GetCardsByRarity(int rarity)
        {
            List<int> result = new List<int>();
            foreach (var card in DB.cardDatas.Values)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(card.cardID);
                if (res != null && res.rarity == rarity)
                {
                    result.Add(card.cardID);
                }
            }

            return result;
        }

        public List<int> GetCardsByType(int gunType)
        {
            List<int> result = new List<int>();
            foreach (var card in DB.cardDatas.Values)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(card.cardID);
                if (res != null && res.gunType == gunType)
                {
                    result.Add(card.cardID);
                }
            }
            return result;
        }

        public List<int> GetCardsByLevel(int level)
        {
            List<int> result = new List<int>();
            foreach (var card in DB.cardDatas.Values)
            {
                if (card.level >= level)
                {
                    result.Add(card.cardID);
                }
            }

            return result;
        }
        public GunCardData GetUseWeapon()
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(DB.useWeaponID, out data))
                return null;

            return data;
        }

        public string GetUseWeaponName()
        {
            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(DB.useWeaponID);
            if (cardRes == null)
                return null;

            return cardRes.prefab;
        }


        public int GetUseWeaponID()
        {
            return DB.useWeaponID;
        }

        public void SetUseWeaponID(int id)
        {            
            if (RemoveFuseCard(id))
            {
                AddFuseCard(DB.useWeaponID);
            }
            DB.useWeaponID = id;
            //int occupiedPointIndex;
            //if (CampsiteMgr.singleton.CheckCardIsOccupied(id, out occupiedPointIndex))
            //{
            //    CampsiteMgr.singleton.PointRemoveGun(occupiedPointIndex);
            //}

            Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            CampTaskMgr.singleton.AddTaskData(TaskType.Epic_Gun, 1);

            NotifySaveData();
        }

        public float GetUseWeaponAtk()
        {
            GunCardData mainData;
            if (!DB.cardDatas.TryGetValue(DB.useWeaponID, out mainData))
                return 0;

            var mainRes = TableMgr.singleton.GunCardTable.GetItemByID(mainData.cardID);
            if (mainRes == null)
                return 0;

            float dps = GetUseWeaponPower();

            return dps * TableMgr.singleton.ValueTable.fuse_card_atk_scale /
                        mainData.GetAtkSpeed() / mainRes.bulletParam;
        }

        public float GetUseWeaponPower()
        {
            GunCardData mainData;
            if (!DB.cardDatas.TryGetValue(DB.useWeaponID, out mainData))
                return 0;

            float dps = mainData.DPS();
            foreach (var card in DB.fusedCardList)
            {
                GunCardData data;
                if (!DB.cardDatas.TryGetValue(card, out data))
                    continue;

                dps += data.DPS();
            }

            return dps;
        }

        public bool CanCardLvUp(int cardID, bool checkCost = true)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return false;

            var starRes = TableMgr.singleton.CardStarTable.GetItemByID(data.star);
            if (starRes == null)
                return false;

            if (data.level >= starRes.maxLevel)
                return false;

            if (checkCost && GetBigCurrency(CurrencyType.GOLD) < GetCardLvUpCost(data.level + 1))
                return false;

            return true;
        }

        public bool CanCardStarUp(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return false;

            if (data.star >= GetCardMaxStar(cardRes.rarity))
                return false;

            var starRes = TableMgr.singleton.CardStarTable.GetItemByID(data.star + 1);
            if (starRes == null)
                return false;

            for (int i = 0; i < starRes.needCardTypeList.Length; ++i)
            {
                if (starRes.needCardTypeList[i] <= 0)
                    continue;

                if (starRes.needCardTypeList[i] == 1)
                {
                    if (GetCardCount(cardID) < starRes.needCardCountList[i])
                        return false;
                }
                else if (starRes.needCardTypeList[i] == 2)
                {
                    if (GetQualityCardCount(starRes.needCardRarityList[i], cardRes.gunType) < starRes.needCardCountList[i])
                        return false;
                }
            }

            return true;
        }

        public bool CanFuseCard()
        {
            if (DB.fusedCardList.Count >= TableMgr.singleton.ValueTable.fuse_card_max_count)
                return false;

            foreach (var card in DB.cardDatas.Values)
            {
                if (card.cardID == DB.useWeaponID)
                    continue;

                if (!DB.fusedCardList.Contains(card.cardID))
                    return true;
            }

            return false;

        }

        public void SetCardNew(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return;

            if (!data.isNew)
                return;

            data.isNew = false;

            //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            NotifySaveData();
        }

        public int GetCampID()
        {
            return 1;
        }

        public bool CardStarUp(int cardID, Dictionary<int, List<CardChipData>> chipDic)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return false;

            if (data.star >= GetCardMaxStar(cardRes.rarity))
                return false;

            var starRes = TableMgr.singleton.CardStarTable.GetItemByID(data.star + 1);
            if (starRes == null)
                return false;

            if (chipDic.Count < starRes.slotCount)
                return false;

            for (int i = 0; i < starRes.slotCount; ++i)
            {
                if (starRes.needCardTypeList[i] <= 0)
                    continue;

                List<CardChipData> list;
                if (!chipDic.TryGetValue(i, out list))
                    return false;

                var count = 0;
                for (int j = 0; j < list.Count; ++j)
                {

                    if (starRes.needCardTypeList[i] == 1)
                    {
                        count = list[j].count;
                    }
                    else if (starRes.needCardTypeList[i] == 2)
                    {
                        var res = TableMgr.singleton.GunCardTable.GetItemByID(list[j].cardID);
                        if (res == null)
                            return false;

                        if (res.rarity != starRes.needCardRarityList[i])
                            return false;

                        count += list[j].count;
                    }
                }

                if (count < starRes.needCardCountList[i])
                    return false;
            }

            foreach (var chipList in chipDic.Values)
            {
                foreach (var chip in chipList)
                {
                    CostCardCount(chip.cardID, chip.count);
                }
            }

            data.star++;

            NotifySaveData();

            //UI.UIMgr.singleton.BoradCast(UIEventType.CARD, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardDataChange, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);
            //AddGameData(GameData.WeeklyCardStarUp, 1);
            //if (cardRes.quality == (int)CardQualityType.RARE)
            //{
            //    AddGameData(GameData.WeeklyCardStarUpRare, 1);
            //}
            //else if (cardRes.quality == (int)CardQualityType.EPIC)
            //{
            //    AddGameData(GameData.WeeklyCardStarUpEpic, 1);
            //}
            //else if (cardRes.quality == (int)CardQualityType.LEGEND)
            //{
            //    AddGameData(GameData.WeeklyCardStarUpLegend, 1);
            //}
            //if (!GuideMgr.singleton.IsGuideStart())
            //{
            //    StatsMgr.singleton.ReportEvent(string.Format("upgrade_card"),
            //        new Dictionary<string, string>()
            //        {
            //            ["card_id"] = cardID.ToString()
            //        });
            //}

            return true;
        }

        void CostCardCount(int cardID, int count)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return;

            if (data.count < count)
                return;

            data.count -= count;

            NotifySaveData();
        }

        public bool CardLvUp(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (cardRes == null)
                return false;

            var starRes = TableMgr.singleton.CardStarTable.GetItemByID(data.star);
            if (starRes == null)
                return false;

            if (data.level >= starRes.maxLevel)
                return false;

            var cost = GetCardLvUpCost(data.level + 1);
            if (GetBigCurrency(CurrencyType.GOLD) < cost)
                return false;

            data.level++;
            CostBigCurrency(CurrencyType.GOLD, cost);

            var campSkill = GetCampSkillID(cardRes.campGunSkill, data.level);
            if (TableMgr.singleton.CampGunSkillTable.GetItemByID(campSkill) != null &&
                data.campSkillID != campSkill)
            {
                data.campSkillID = campSkill;
                Debug.Log("campSkill " + campSkill);

                var info = new SkillInfo();
                info.type = SkillType.Camp;
                info.skillID = campSkill;
                Global.gApp.gUiMgr.OpenPanel(Wndid.GunSkillLvUpUI, info);
            }

            var fuseSkill = GetFuseSkillID(cardRes.fuseSkill, data.level);
            if (TableMgr.singleton.FuseGunSkillTable.GetItemByID(fuseSkill) != null &&
                data.fuseSkillID != fuseSkill)
            {
                data.fuseSkillID = fuseSkill;

                var info = new SkillInfo();
                info.type = SkillType.Fuse;
                info.skillID = fuseSkill;
                Global.gApp.gUiMgr.OpenPanel(Wndid.GunSkillLvUpUI, info);
            }
            NotifySaveData();

            //AddGameData(GameData.DailyCardLvUp, 1);
            ////UI.UIMgr.singleton.BoradCast(UIEventType.CARD, cardID);

            ////if (!GuideMgr.singleton.IsGuideStart())
            ////{
            ////    StatsMgr.singleton.ReportEvent(string.Format("upgrade_card"),
            ////        new Dictionary<string, string>()
            ////        {
            ////            ["card_id"] = cardID.ToString()
            ////        });
            ////}

            //PlayerRightsMgr.singleton.AddExp(TableMgr.singleton.ValueTable.card_lvup_add_exp);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardDataChange, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            CampTaskMgr.singleton.AddTaskData(TaskType.LevelUp_Gun, 1, cardRes.gunType);
            CampTaskMgr.singleton.AddTaskData(TaskType.Lv10_Gun, 1);

            return true;
        }

        public BigInteger GetCardLvUpCost(int level)
        {
            return TableMgr.singleton.ValueTable.card_levelup_cost_k1 *
                    ((int)Math.Pow(level - 1, TableMgr.singleton.ValueTable.card_levelup_cost_k2) +
                    TableMgr.singleton.ValueTable.card_levelup_cost_k3 * (level - 1)) -
                    TableMgr.singleton.ValueTable.card_levelup_cost_k4;

        }

        public int GetFuseSkillID(int skillID, int cardLv)
        {
            if (cardLv < 45)
                return skillID;

            var add = (cardLv - 45) / 40 + 1;

            return skillID + add;
        }

        public int GetCampSkillID(int skillID, int cardLv)
        {
            return skillID + CalcCampSkillAdd(cardLv);
        }

        public int CalcCampSkillAdd(int cardLv)
        {
            if (cardLv < 25)
                return 0;

            var add = (cardLv - 25) / 40 + 1;

            return add;
        }

        public static int GetCardInitStar(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return 1;
                case CardQualityType.EPIC:
                    return 5;
                case CardQualityType.LEGEND:
                    return 14;
            }

            return 0;
        }

        public static int GetCardMaxStar(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return 2;
                case CardQualityType.EPIC:
                    return 8;
                case CardQualityType.LEGEND:
                    return 26;
            }

            return 0;
        }

        public static int GetCardMaxStarCount(int quality, int starRarity)
        {
            var count = 0;
            foreach(CardStar_TableItem item in TableMgr.singleton.CardStarTable.getEnumerator())
            {
                if (item.star <= 0)
                    continue;

                if (item.cardRarity == quality && item.starRarity == starRarity)
                    count++;
            }

            return count;
        }

        public int GetNewCardCount()
        {
            int count = 0;
            foreach (var card in DB.cardDatas)
            {
                if (card.Value.isNew)
                    count++;
            }

            return count;
        }

        public int GetLevelCardCount(int level)
        {
            int count = 0;
            foreach (var card in DB.cardDatas)
            {
                if (card.Value.level >= level)
                    count++;
            }

            return count;
        }

        public int GetLvUpCardCount()
        {
            int count = 0;
            foreach (var card in DB.cardDatas)
            {

                if (CanCardLvUp(card.Key))
                    count++;


            }

            return count;
        }

        public int GetQualityCardCount(int quality, int gunType)
        {
            int count = 0;
            foreach (var card in DB.cardDatas)
            {
                var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(card.Key);
                if (cardRes == null)
                    continue;

                if (cardRes.rarity != quality)
                    continue;

                if (cardRes.gunType != gunType)
                    continue;

                count += card.Value.count;
            }

            return count;
        }

        public int GetCollectCardCount()
        {
            return DB.cardDatas.Count;
        }

        public List<int> GetGunCardTypeList()
        {
            List<int> list = new List<int>();
            foreach(var card in DB.cardDatas)
            {
                var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(card.Key);
                if (cardRes == null)
                    continue;

                if (list.Contains(cardRes.gunType))
                    continue;

                list.Add(cardRes.gunType);
            }

            return list;
        }

        public bool CardLvReset(int cardID)
        {
            GunCardData data;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return false;

            if (data.level <= 1)
                return false;

            if (GetCurrency(CurrencyType.DIAMOND) < TableMgr.singleton.ValueTable.card_reset_cost)
                return false;

            CostCurrency(CurrencyType.DIAMOND, TableMgr.singleton.ValueTable.card_reset_cost);

            BigInteger gold = 0;
            for (int i = 2;i <= data.level;++i)
            {
                gold += GetCardLvUpCost(i);
            }

            data.level = 1;
            AddBigCurrency(CurrencyType.GOLD, gold);

            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardDataChange, cardID);
            EZ.Global.gApp.gMsgDispatcher.Broadcast(EZ.MsgIds.GunCardOpt);

            NotifySaveData();

            return true;
        }
        //public BigInteger GetCardChipToGold(int cardID, int count)
        //{
        //    var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
        //    if (cardRes == null)
        //        return 0;

        //    var stageRes = TableMgr.singleton.TowerTable.GetItemByID(GetMaxUnlockStageID());
        //    if (stageRes == null)
        //        return 0;

        //    var starRes = TableMgr.singleton.CardStarTable.GetItemByID(cardRes.quality);
        //    if (starRes == null)
        //        return 0;

        //    return count * stageRes.idleSpeed * 60 * starRes.chipToGold;

        //}
        /////////////////////////////////////////////////////////////////////
        public int GetCurStageID()
        {
            return DB.curStageID;
        }
        //需要实现
        public int GetMaxUnlockStageID()
        {
            return DB.maxUnlockStageID;
        }

        public Data.StageData GetStageData(int stage_id)
        {
            Data.StageData _Data = null;
            DB.stageDatas.TryGetValue(stage_id, out _Data);
            return _Data;
        }

        public int GetStagMaxWaves(int id)
        {
            var data = GetStageData(id);
            if (data == null)
                return 0;

            //Debug.Log("GetStagMaxWaves " + data.maxWaves);
            return data.maxWaves;
        }

        public int GetStageFinishWaves()
        {
            int waves = 0;
            foreach (var stage in DB.stageDatas)
            {
                waves += stage.Value.finishWaves;
            }

            return waves;
        }

        public int GetEnterCount(int id)
        {
            var data = GetStageData(id);
            if (data == null)
                return 0;

            return data.enterTimes;
        }

        public int GetTotalEnterCount()
        {
            int count = 0;
            foreach (var item in DB.stageDatas)
            {
                count += item.Value.enterTimes;
            }
            return count;
        }

        public int GetFinishStageCount()
        {
            int count = 0;
            foreach (var item in DB.stageDatas)
            {
                if (item.Value.stageID != 1 && item.Value.finish)
                    count++;
            }
            return count;
        }

        public bool IsFinishStage(int id)
        {
            var data = GetStageData(id);
            if (data == null)
                return false;

            return data.finish;
        }

        //public bool IsNewStage(int id)
        //{
        //    var data = GetStageData(id);
        //    if (data == null)
        //        return true;

        //    return false;
        //}

        //public void NewStageShowed(int id)
        //{
        //    if (!IsNewStage(id))
        //        return;

        //    var data = new Data.StageData();
        //    data.stageID = id;
        //    DB.stageDatas.Add(id, data);
        //    NotifySaveData();
        //}

        public void UnlockStage(int id)
        {
            var data = GetStageData(id);
            if (data == null)
                data = new Data.StageData();

            data.stageID = id;
            data.isUnlock = true;
            DB.stageDatas[id] = data;
            NotifySaveData();
        }

        public void SetStageParam(int id, bool isCamp)
        {
            var levelRes = TableMgr.singleton.LevelTable.GetItemByID(id);
            if (levelRes == null)
                return;

            StageAtkLevel = levelRes.atkLevel;
            StageHpParam = levelRes.hpParam;
            
            StageWaveFactor = levelRes.waveFactor;

            var data = GetStageData(id);
            if (data != null && data.finish)
            {
                StageGold = 0;
            }
            else
            {
                StageGold = levelRes.gold;
            }


            if (isCamp)
            {
                StageHpParam = StageHpParam * TableMgr.singleton.ValueTable.camp_combat;
            }
        }

        public bool EnterStage(int id)
        {
            SetStageParam(id, false);

            EnterStageID = id;
            DB.curStageID = EnterStageID;

            Data.StageData _Data;
            if (!DB.stageDatas.TryGetValue(id, out _Data))
            {
                _Data = new Data.StageData();
                _Data.stageID = id;
                DB.stageDatas.Add(id, _Data);
            }

            _Data.enterTimes++;

            NotifySaveData();
            LastFightResult = FightResult.Undefine;
            return true;
        }

        //public void FinishStage(bool isWin)
        //{
        //    if (EnterStageID <= 0)
        //        return;

        //    Data.StageData _Data;
        //    if (!DB.stageDatas.TryGetValue(EnterStageID, out _Data))
        //    {
        //        _Data = new Data.StageData();
        //        _Data.stageID = EnterStageID;
        //        DB.stageDatas.Add(EnterStageID, _Data);
        //    }

        //    DB.curStageID = EnterStageID;

        //    //var stage = TableMgr.singleton.TowerTable.GetItemByID(EnterStageID);
        //    //if (stage == null)
        //    //    return;

        //    if (!_Data.finish && isWin)//首通奖励
        //    {
        //        _Data.finish = true;
        //        var next = TableMgr.singleton.LevelTable.GetItemByID(EnterStageID + 1);
        //        if (next != null)
        //        {
        //            DB.curStageID = next.id;
        //            DB.maxUnlockStageID = next.id;
        //            UnlockStage(next.id);
        //        }
        //    }

        //    CampTaskMgr.singleton.AddTaskData(TaskType.Finish_Battle, 1);

        //    NotifySaveData();
        //}

        public void RequestFinishStage(bool isWin, int gold, Action<bool> callback)
        {
            var lvRes = TableMgr.singleton.LevelTable.GetItemByID(EnterStageID);
            if (lvRes == null)
                return;

            LastFightResult = isWin ? FightResult.Win : FightResult.Lose;

            if (!isWin)
            {
                callback?.Invoke(true);
                return;
            }

            var data = GetStageData(EnterStageID);
            if (data != null && data.finish)
            {
                callback?.Invoke(true);
                return;
            }

            List<GameRewardData> goodList = new List<GameRewardData>();
            goodList.Add(new GameRewardData()
            {
                type = (int)GoodsType.GOLD,
                count = gold,
            });

            if (lvRes.reward > 0)
            {
                goodList.Add(new GameRewardData()
                {
                    type = lvRes.reward,
                    count = lvRes.reward_count,
                    param = lvRes.reward_type,
                });
            }

            GameGoodsMgr.singleton.RequestAddGameGoods((success, real_rewards, tips) =>
            {
                if (success == GoodsRequestResult.Success)
                {
                    Data.StageData _Data;
                    if (!DB.stageDatas.TryGetValue(EnterStageID, out _Data))
                    {
                        _Data = new Data.StageData();
                        _Data.stageID = EnterStageID;
                        DB.stageDatas.Add(EnterStageID, _Data);
                    }

                    DB.curStageID = EnterStageID;

                    if (!_Data.finish)
                    {
                        _Data.finish = true;
                        var next = TableMgr.singleton.LevelTable.GetItemByID(EnterStageID + 1);
                        if (next != null)
                        {
                            if (next.id <= DB.chapterData.startStageID + 9)
                            {
                                DB.curStageID = next.id;
                                DB.maxUnlockStageID = next.id;
                                UnlockStage(next.id);
                            }
                        }

                        CampTaskMgr.singleton.AddTaskData(TaskType.Finish_Battle, 1);
                    }
           
                    NotifySaveData();

                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }

            },
            goodList);
        }

        public List<int> GetStageStarList(bool win, float lifeScale, int reviveCount)
        {
            var lvRes = TableMgr.singleton.LevelTable.GetItemByID(EnterStageID);
            if (lvRes == null)
                return null;

            var stageData = GetStageData(EnterStageID);
            if (stageData == null)
                return null;

            for (int i = 0;i < lvRes.starList.Length;++i)
            {
                if (stageData.starList.Contains(lvRes.starList[i]))
                {
                    continue;
                }

                bool finish = false;
                switch((LevelStarType)lvRes.starList[i])
                {
                    case LevelStarType.Level_Pass:
                        {
                            finish = win;
                        }
                        break;
                    case LevelStarType.Life_50:
                        {
                            if (lifeScale >= 0.5f)
                                finish = true;
                        }
                        break;
                    case LevelStarType.Life_100:
                        {
                            if (lifeScale >= 1.0f)
                                finish = true;
                        }
                        break;
                    case LevelStarType.Revive_0:
                        {
                            if (reviveCount <= 0)
                                finish = true;
                        }
                        break;
                    case LevelStarType.Epic_Gun:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.rarity >= (int)CardQualityType.EPIC)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Legend_Gun:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.rarity >= (int)CardQualityType.LEGEND)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Gun_Type_1:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.gunType == 1)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Gun_Type_2:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.gunType == 2)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Gun_Type_3:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.gunType == 3)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Gun_Type_4:
                        {
                            var gunData = GetUseWeapon();
                            var res = TableMgr.singleton.GunCardTable.GetItemByID(gunData.cardID);
                            if (res != null)
                            {
                                if (res.gunType == 4)
                                    finish = true;
                            }
                        }
                        break;
                    case LevelStarType.Gun_Level:
                        {
                            var gunData = GetUseWeapon();
                            if (gunData.level >= lvRes.cardLevel)
                                finish = true;                            
                        }
                        break;
                    default:
                        break;
                }

                if (finish)
                {
                    stageData.starList.Add(lvRes.starList[i]);
                }
            }

            NotifySaveData();

            return stageData.starList;
        }

        public List<int> GetDropCardsByQuality(int quality)
        {
            List<int> cardList;
            if (dropCardDic.TryGetValue(quality, out cardList))
            {
                return cardList;
            }
            return null;
        }

        public int GetCardLevel(int cardID)
        {
            Data.GunCardData data = null;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return 0;

            return data.level;
        }

        public int GetCardCount(int cardID)
        {
            Data.GunCardData data = null;
            if (!DB.cardDatas.TryGetValue(cardID, out data))
                return 0;

            return data.count;
        }
        ///////////////////////////////////////////////////////////////////

        private Dictionary<int, int> cardQualityDic = new Dictionary<int, int>();
        private Dictionary<int, List<int>> cardNewDic = new Dictionary<int, List<int>>();

        int GetCardByQuality(int quality)
        {
            List<int> dropList;
            if (!dropCardDic.TryGetValue(quality, out dropList))
                return 0;


            var index = BaseRandom.Next(0, dropList.Count);
            var cardID = dropList[index];
            var isNew = GetGunCardData(cardID) == null;

            if (isNew)//新卡数量限制
            {
                List<int> cardList;
                if (cardNewDic.TryGetValue(quality, out cardList))
                {
                    if (!cardList.Contains(cardID))
                    {
                        if (quality == (int)CardQualityType.RARE && cardList.Count >= 2)
                        {
                            Debug.Log("CardQualityType.RARE && count >= 2");
                            index = BaseRandom.Next(0, cardList.Count);
                            cardID = cardList[index];
                        }
                        else if (quality >= (int)CardQualityType.EPIC && cardList.Count >= 2)
                        {
                            Debug.Log("CardQualityType.EPIC && count >= 2");
                            index = BaseRandom.Next(0, cardList.Count);
                            cardID = cardList[index];
                        }
                        else
                        {
                            cardList.Add(cardID);
                        }
                    }

                }
                else
                {
                    cardNewDic[quality] = new List<int>();
                    cardNewDic[quality].Add(cardID);
                }

            }

            //cardQualityDic[quality].Add(cardID);
            //if (!cardDropList.Contains(cardID))
            //    cardDropList.Add(cardID);

            return cardID;
        }

        public int GetGuideKeyBoxCount()
        {
            switch (GetCurGuideStageIndex() - 1)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                    return 2;
                default:
                    return 0;
            }
        }

        public int GetCurBoxID()
        {
            return DB.curBoxID;
        }


        public void SetCurGuideID(int id)
        {
            DB.curGuideID = id;

            NotifySaveData();
        }

        public int GetCurGuideID()
        {
            return DB.curGuideID;
        }

        public void ClearCurGuideID()
        {
            DB.curGuideID = -1;

            NotifySaveData();
        }

        public bool IsAllGuideStageFinish()
        {
            return DB.curGuideStageIndex > 4;
        }

        public bool EnterGuideStage(int index)
        {
            DB.curGuideStageIndex = index;
            return true;
        }

        public int GetCurGuideStageIndex()
        {
            return DB.curGuideStageIndex;
        }

        public void SetGuideStageFinish(int index)
        {
            DB.curGuideStageIndex = index + 1;
            NotifySaveData();
        }

        ////////////////////////////////////////////////////////////
        public void RefreshData(DateTimeRefreshType refreshType)
        {
            switch (refreshType)
            {
                case DateTimeRefreshType.OneDay:
                    //RefreshGoldBox();
                    //RefreshDailyTasks();
                    break;
                case DateTimeRefreshType.TwoDay:
                    break;
                case DateTimeRefreshType.OneWeek:
                    //RefreshWeeklyTasks();
                    break;
            }
        }

        /////////////////////////////////////////////////////////////////////
        ///
        public void SetPlayerLevel(int level)
        {
            DB.level = level;

            NotifySaveData();
        }

        public int GetPlayerLevel()
        {
            return DB.level;
        }

        public int GetChapterStar()
        {
            int star = 0;
            for (int i = 0;i < 10;++i)
            {
                var data = GetStageData(DB.chapterData.startStageID + i);
                if (data != null)
                {
                    star += data.starList.Count;
                }
            }

            return star;
        }

        public bool NextChapter()
        {
            var maxID = DB.chapterData.startStageID + 9;
            if (GetMaxUnlockStageID() < maxID)
                return false;

            if (!IsFinishStage(maxID))
                return false;

            var next = TableMgr.singleton.LevelTable.GetItemByID(maxID + 1);
            if (next == null)
                return false;

            DB.curStageID = next.id;
            DB.maxUnlockStageID = next.id;
            UnlockStage(next.id);
            DB.chapterData.startStageID = next.id;
            DB.chapterData.starList.Clear();

            NotifySaveData();

            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ChapterChange);

            return true;
        }

        public void ReceiveChapterStarReward(int star, Action callBack)
        {
            if (DB.chapterData.starList.Contains(star))
                return;

            if (GetChapterStar() < star)
                return;

            var starRes = TableMgr.singleton.LevelStarRewardTable.GetItemByID(star);
            if (starRes == null)
                return;

            GameGoodsMgr.singleton.RequestAddGameGoods((rst, goods, tips) =>
            {
                if (rst != GoodsRequestResult.Success)
                    return;

                DB.chapterData.starList.Add(star);

                NotifySaveData();

                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.StarReward);
                if (callBack != null)
                    callBack();

            }, starRes.reward, starRes.reward_count, starRes.reward_type);
        }
    }
}
