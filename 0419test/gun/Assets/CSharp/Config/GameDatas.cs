using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Data
{

    public class GameDatas
    {
        //public Level LevelData { get; private set; }
        public int m_MinNormalId = int.MaxValue;
        public int m_MaxNormalId = int.MinValue;
        public Pass PassData { get; private set; }
		public Wave WaveData { get; private set; }
		public Drop DropData { get; private set; }
		public Monster MosterData { get; private set; }
        public Scene SceneDate { get; private set; }
        //物品配置
        public Item ItemData { get; private set; }
        public Dictionary<int, List<ItemItem>> ItemTypeMapData { get; private set; }
        //提示文本
        public Tips TipsData{ get; private set; }
        //通用配置
        public GeneralConfig GeneralConfigData { get; private set; }
        //充值表 不用了换成shop
        //public Charge ChargeData { get; private set; }
        //public Dictionary<int, List<ChargeItem>> ChargeTypeMapData { get; private set; }
        //技能表
        public Skill SkillData { get; private set; }
        //最小技能开放等级
        //public int MinSkillOpenLevel;
        public Dictionary<int, SkillItem> SkillLocationMapData { get; private set; }
        //任务表
        public Quest QuestData { get; private set; }
        //任务条件类型配置
        public Dictionary<int, List<QuestItem>> ConditionTypeQuestMapData { get; private set; }
        //任务类型配置
        public Dictionary<int, List<QuestItem>> QuestTypeMapData { get; private set; }
        //特效
        public Effect EffectData { get; private set; }
        //新手引导
        public NewbieGuide NewbieGuideData { get; private set; }
        //商城表
        public Shop ShopConfig { get; private set; }
        public Shop_data ShopDataConfig { get; private set; }
        public Dictionary<int, List<ShopItem>> ShopTypeMapData { get; private set; }
        //英雄升级表
        public Hero_data HeroDataConfig { get; private set; }
        //枪升级表
        public Guns_data GunDataConfig { get; private set; }
        // gun pass 表
        public GunsPass_data GunPassDataConfig { get; private set; }
        //技能升级表
        public Skill_data SkillDataConfig { get; private set; }
        //关卡阶段
        public List<int> PassStep { get; private set; }
        //在线奖励
        public FallBox_data FallBoxDataConfig { get; private set; }
        //特殊关卡金币
        public PassSpecial_data PassSpecialCfg{ get; private set; }
        //枪的解锁类型排序
        public Dictionary<float, SortedList<int, ItemItem>> OpenOrderGun { get; private set; }
        public Dictionary<int, SortedList<int, ItemItem>> ShowOrderGun { get; private set; }
        //服务器升级表
        public GunsSub_data GunSubDataConfig { get; private set; }
        //商城开箱子
        public int m_ADBox = 20;
        public int m_MDTBox = 21;
        public Dictionary<int, List<int>> BoxDropRandomMap { get; private set; }
        public Dictionary<int, List<int>> BoxDropTypeMap { get; private set; }
        public Gold_params GoldParamsConfig { get; private set; }
        public CampHeart_params CampHeartParamsConfig { get; private set; }

        public Dictionary<string, Font> FontMap { get; private set; }
        private string m_TimeFormat = "0{0}";

        //CampNpc表
        public CampNpc CampNpcConfig { get; private set; }

        public List<CampNpcItem> NotFreshNpcList { get; private set; }
        //CampTasks表
        public CampTasks CampTasksConfig { get; private set; }
        public Dictionary<int, List<CampTasksItem>> CampTaskConditonMap { get; private set; }
        public Dictionary<int, List<CampTasksItem>> CampTasksKindMap { get; private set; }
        public int[] NpcMax;
        public int[] NpcShowMax;
        public int[] NpcTaskMax;

        public CampShop CampShopConfig { get; private set; }
        public CampRecycle CampRecycleConfig { get; private set; }
		public PassBranch PassBranchConfig { get; private set; }		
        //对话表
        public Dialogue DialogueConfig { get; private set; }
        //营地步骤表
        public CampStep CampStepConfig { get; private set; }
        //营地buff表
        public CampBuff CampBuffConfig { get; private set; }
        //营地buff表
        public CampBuff_data CampBuffDataConfig { get; private set; }
        // 营地徽章
        public CampBadge CampBadgeConfig { get; private set; }
        public GameDatas()
        {
            //LevelData = Resources.Load<Level>("Config/Level");
			PassData = Resources.Load<Pass>("Config/Pass");

            WaveData = Resources.Load<Wave>("Config/Wave");
			DropData = Resources.Load<Drop>("Config/Drop");
            ItemData = Resources.Load<Item>("Config/Item");
            MosterData = Resources.Load<Monster>("Config/Monster");

            LoadItemTypeMap();
            TipsData = Resources.Load<Tips>("Config/Tips");
            GeneralConfigData = Resources.Load<GeneralConfig>("Config/GeneralConfig");
            //ChargeData = Resources.Load<Charge>("Config/Charge");
            ShopConfig = Resources.Load<Shop>("Config/Shop");
            ShopDataConfig = Resources.Load<Shop_data>("Config/Shop_data");
            LoadChargeTypeMap();
            SkillData = Resources.Load<Skill>("Config/Skill");
            LoadSkillLocationMap();
            QuestData = Resources.Load<Quest>("Config/Quest");
            LoadQuestMapData();
            EffectData = Resources.Load<Effect>("Config/Effect");
            SceneDate = Resources.Load<Scene>("Config/Scene");
            NewbieGuideData = Resources.Load<NewbieGuide>("Config/NewbieGuide");
            HeroDataConfig = Resources.Load<Hero_data>("Config/Hero_data");
            GunDataConfig = Resources.Load<Guns_data>("Config/Guns_data");
            GunPassDataConfig = Resources.Load<GunsPass_data>("Config/GunsPass_data");
            SkillDataConfig = Resources.Load<Skill_data>("Config/Skill_data");
            PassSpecialCfg = Resources.Load<PassSpecial_data>("Config/PassSpecial_data");
            LoadPassId();
            LoadPassStep();
            FallBoxDataConfig = Resources.Load<FallBox_data>("Config/FallBox_data");
            LoadOrderGun();
            GunSubDataConfig = Resources.Load<GunsSub_data>("Config/GunsSub_data");
            LoadBoxDropRandomMap();
            GoldParamsConfig = Resources.Load<Gold_params>("Config/Gold_params");
            CampHeartParamsConfig = Resources.Load<CampHeart_params>("Config/CampHeart_params");
            LoadFont();
            CampNpcConfig = Resources.Load<CampNpc>("Config/CampNpc");
            CampTasksConfig = Resources.Load<CampTasks>("Config/CampTasks");
            LoadNotFreshNpcList();
            LoadCampTasksKindMap();
            LoadCampTaskConditonMap();
            LoadCampMax();
            CampShopConfig = Resources.Load<CampShop>("Config/CampShop");
            CampRecycleConfig = Resources.Load<CampRecycle>("Config/CampRecycle");
			PassBranchConfig = Resources.Load<PassBranch>("Config/PassBranch");
            DialogueConfig = Resources.Load<Dialogue>("Config/Dialogue");
            CampStepConfig = Resources.Load<CampStep>("Config/CampStep");
            CampBuffConfig = Resources.Load<CampBuff>("Config/CampBuff");
            CampBuffDataConfig = Resources.Load<CampBuff_data>("Config/CampBuff_data");
            CampBadgeConfig = Resources.Load<CampBadge>("Config/CampBadge");
        }

        private void LoadCampTaskConditonMap()
        {
            CampTaskConditonMap = new Dictionary<int, List<CampTasksItem>>();
            foreach (CampTasksItem item in CampTasksConfig.items)
            {
                List<CampTasksItem> list;
                if (!CampTaskConditonMap.TryGetValue((int)item.taskCondition[0], out list))
                {
                    list = new List<CampTasksItem>();
                    CampTaskConditonMap[(int)item.taskCondition[0]] = list;
                }
                list.Add(item);
            }
        }

        private void LoadNotFreshNpcList()
        {
            NotFreshNpcList = new List<CampNpcItem>();
            foreach (CampNpcItem i in CampNpcConfig.items)
            {
                if (i.notFresh == 1)
                {
                    NotFreshNpcList.Add(i);
                }
            }
        }

        private void LoadCampTasksKindMap()
        {
            CampTasksKindMap = new Dictionary<int, List<CampTasksItem>>();
            foreach (CampTasksItem ctiCfg in CampTasksConfig.items)
            {
                List<CampTasksItem> list; 
                if (!CampTasksKindMap.TryGetValue(ctiCfg.kind, out list))
                {
                    list = new List<CampTasksItem>();
                    CampTasksKindMap[ctiCfg.kind] = list;
                }
                list.Add(ctiCfg);
            }
        }

        private void LoadCampMax()
        {
            string[] npcMaxCfg = GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_NUM).contents;
            NpcMax = new int[npcMaxCfg.Length];
            for (int i = 0; i < npcMaxCfg.Length; i++)
            {
                NpcMax[i] = int.Parse(npcMaxCfg[i]);
            }
            string[] npcShowMaxCfg = GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_SHOW_NUM).contents;
            NpcShowMax = new int[npcShowMaxCfg.Length];
            for (int i = 0; i < npcShowMaxCfg.Length; i++)
            {
                NpcShowMax[i] = int.Parse(npcShowMaxCfg[i]);
            }
            string[] npcTaskMaxCfg = GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_TASK_NUM).contents;
            NpcTaskMax = new int[npcTaskMaxCfg.Length];
            for (int i = 0; i < npcTaskMaxCfg.Length; i++)
            {
                NpcTaskMax[i] = int.Parse(npcTaskMaxCfg[i]);
            }
        }

        public int GetCampLevel(int npcNum)
        {
            //特殊处理 一级 0-50   二级 50-100 三级 100-200 三级 200+
            int maxLv = NpcMax.Length / 2;
            for (int i = 0; i < NpcMax.Length - 1; i+=2)
            {
                if (i == 0 && npcNum < NpcMax[i])
                {
                    return 1;
                }
                if (NpcMax[i] <= npcNum && npcNum < NpcMax[i + 1])
                {
                    return Mathf.Min(i/2 + 1,maxLv);
                }
            }
            return maxLv;
        }

        public Font GetFont(string lan)
        {
            Font f;
            if (!FontMap.TryGetValue(lan, out f))
            {
                f = FontMap["en"];
            }
            return f;
        }

        private void LoadFont()
        {
            FontMap = new Dictionary<string, Font>();
            GeneralConfigItem cfg = GeneralConfigData.Get(GeneralConfigConstVal.LANGUAGE_LIST);
            for (int i = 0; i < cfg.contents.Length; i += 3)
            {
                Font font = Resources.Load<Font>(string.Format(CommonResourceConstVal.FONT_PATH, cfg.contents[i + 2]));
                FontMap[cfg.contents[i + 1]] = font;
            }
        }

        private void LoadOrderGun()
        {
            OpenOrderGun = new Dictionary<float, SortedList<int, ItemItem>>();
            ShowOrderGun = new Dictionary<int, SortedList<int, ItemItem>>();
            int[] types = new int[] { ItemTypeConstVal.BASE_MAIN_WEAPON, ItemTypeConstVal.SUB_WEAPON, ItemTypeConstVal.PET };
            foreach (int type in types)
            {
                List<ItemItem> allGun = ItemTypeMapData[type];
                foreach (ItemItem gun in allGun)
                {
                    SortedList<int, ItemItem> showList;
                    if (!ShowOrderGun.TryGetValue(gun.showtype, out showList))
                    {
                        showList = new SortedList<int, ItemItem>();
                        ShowOrderGun[gun.showtype] = showList;
                    }
                    showList.Add(gun.showorder, gun);

                    if (gun.opencondition[0] != FilterTypeConstVal.SUM_LOGIN_DAY)
                    {
                        continue;
                    }
                    SortedList<int, ItemItem> sl;
                    if (!OpenOrderGun.TryGetValue(gun.opencondition[0], out sl))
                    {
                        sl = new SortedList<int, ItemItem>();
                        OpenOrderGun.Add(gun.opencondition[0], sl);
                    }
                    sl.Add((int)gun.opencondition[1] * 100 + (100 - type), gun);

                }
            }
            //foreach (ItemItem gun in OpenOrderGun[FilterTypeConstVal.SUM_LOGIN_DAY].Values)
            //{
            //    Debug.Log(gun.gamename + " = " + gun.opencondition[1]);
            //}
        }

        //加载以condition 为key的任务map
        private void LoadQuestMapData()
        {
            foreach (QuestItem item in QuestData.items)
            {
                if (ConditionTypeQuestMapData == null)
                {
                    ConditionTypeQuestMapData = new Dictionary<int, List<QuestItem>>();
                }
                if (!ConditionTypeQuestMapData.ContainsKey((int)item.condition[0]))
                {
                    List<QuestItem> conditionList = new List<QuestItem>();
                    ConditionTypeQuestMapData.Add((int)item.condition[0], conditionList);
                }
                ConditionTypeQuestMapData[(int)item.condition[0]].Add(item);


                if (QuestTypeMapData == null)
                {
                    QuestTypeMapData = new Dictionary<int, List<QuestItem>>();
                }
                if (!QuestTypeMapData.ContainsKey(item.type))
                {
                    List<QuestItem> typeList = new List<QuestItem>();
                    QuestTypeMapData.Add(item.type, typeList);
                }
                QuestTypeMapData[item.type].Add(item);
            }
        }

        //加载以显示位置为key的技能
        private void LoadSkillLocationMap()
        {
            foreach (SkillItem item in SkillData.items)
            {
                if (SkillLocationMapData == null)
                {
                    SkillLocationMapData = new Dictionary<int, SkillItem>();
                }

                if (!SkillLocationMapData.ContainsKey(item.location))
                {
                    SkillLocationMapData.Add(item.location, item);
                }

                //if (MinSkillOpenLevel == 0 || MinSkillOpenLevel > item.level)
                //{
                //    MinSkillOpenLevel = item.level;
                //}

            }
        }

        //加载出以物品type为key的map
        private void LoadItemTypeMap()
        {
            foreach (ItemItem itemItem in ItemData.items)
            {
                if (ItemTypeMapData == null)
                {
                    ItemTypeMapData = new Dictionary<int, List<ItemItem>>();
                }
                if (!ItemTypeMapData.ContainsKey(itemItem.showtype))
                {
                    List<ItemItem> list = new List<ItemItem>();
                    list.Add(itemItem);
                    ItemTypeMapData.Add(itemItem.showtype, list);
                }
                else
                {
                    ItemTypeMapData[itemItem.showtype].Add(itemItem);
                }
            }

            foreach (List<ItemItem> list in ItemTypeMapData.Values)
            {
                list.Sort((x, y) => x.showorder - y.showorder);
            }
        }

        //开箱子随机组
        private void LoadBoxDropRandomMap()
        {
            BoxDropRandomMap = new Dictionary<int, List<int>>();
            BoxDropTypeMap = new Dictionary<int, List<int>>();
            BoxDropTypeMap.Add(m_ADBox, new List<int>());
            BoxDropTypeMap.Add(m_MDTBox, new List<int>());
            foreach (DropItem dropItem in DropData.items)
            {
                int dropType = dropItem.id / 10000;
                if (dropType == m_ADBox || dropType == m_MDTBox)
                {
                    List<int> list = new List<int>();
                    for (int i = 0; i < dropItem.prop.Length; i++)
                    {
                        list.Add(Convert.ToInt32(dropItem.prop[i]));
                    }
                    BoxDropRandomMap.Add(dropItem.id, list);

                    BoxDropTypeMap[dropType].Add(dropItem.id);
                }
            }
        }

        //通过名字获取物品
        public SkillItem GetSkillDataByName(string name)
        {
            foreach (SkillItem itemItem in Global.gApp.gGameData.SkillData.items)
            {
                if (itemItem.id.Equals(name))
                {
                    return itemItem;
                }
            }
            return null;
        }

        //通过名字获取物品
        public ItemItem GetItemDataByName(string name)
        {
            foreach (ItemItem itemItem in Global.gApp.gGameData.ItemData.items)
            {
                if (itemItem.name.Equals(name))
                {
                    return itemItem;
                }
            }
            return null;
        }

        //加载出以商品type为key的map
        private void LoadChargeTypeMap()
        {
            foreach (ShopItem item in ShopConfig.items)
            {
                if (ShopTypeMapData == null)
                {
                    ShopTypeMapData = new Dictionary<int, List<ShopItem>>();
                }

                List<ShopItem> list = null;
                if (!ShopTypeMapData.TryGetValue(item.type, out list))
                {
                    list = new List<ShopItem>();
                    list.Add(item);
                    ShopTypeMapData.Add(item.type, list);
                } else
                {
                    ShopTypeMapData[item.type].Add(item);
                }
            }

        }

        private void LoadPassId()
        {
            GeneralConfigItem initPassIdConfig = GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            int initId = int.Parse(initPassIdConfig.content);
            foreach (PassItem itemConfig in PassData.items)
            {
                switch (itemConfig.sceneType)
                {
                    case (int)SceneType.NormalScene:
                        if (itemConfig.id / initId == 1)
                        {
                            if (itemConfig.id < m_MinNormalId)
                            {
                                m_MinNormalId = itemConfig.id;
                            }
                            if (itemConfig.id > m_MaxNormalId)
                            {
                                m_MaxNormalId = itemConfig.id;
                            }
                        }
                        break;
                }
                
            }
        }

        private void LoadPassStep()
        {
            PassStep = new List<int>();
            PassItem[] passItems = PassData.items;
            int lastSceneID = -1; 
            for (int i = 0; i < passItems.Length; i ++)
            {
                if (passItems[i].id >= m_MinNormalId && passItems[i].id <= m_MaxNormalId && passItems[i].sceneID != lastSceneID)
                {
                    if (lastSceneID != -1)
                    {
                        PassStep.Add(passItems[i - 1].id);
                    }
                    lastSceneID = passItems[i].sceneID;
                }
            }
            if (PassStep.Count > 0 && PassStep[PassStep.Count - 1] != m_MaxNormalId)
            {
                PassStep.Add(m_MaxNormalId);
            }
        }


        public string GetTipsInCurLanguage(int tipsId)
        {
            TipsItem tCfg = TipsData.Get(tipsId);
            if (tCfg != null)
            {
                string language = Global.gApp.gSystemMgr.GetMiscMgr().Language;
                if (language == null || language.Equals(GameConstVal.EmepyStr))
                {
                    language = UiTools.GetLanguage();
                }
                string lTxt = ReflectionUtil.GetValueByProperty<TipsItem, string>(language, tCfg);
                if (lTxt == null || lTxt.Equals(GameConstVal.EmepyStr))
                {
                    //Debug.Log("tipsId = " + tipsId + "don't have language = " + language);
                    return tCfg.txtcontent;
                }
                else
                {
                    return lTxt;
                }
            } else
            {
                //Debug.Log("tipsId = " + tipsId + "don't exist");
            }
            return GameConstVal.EmepyStr;
        }


        public string GetTimeInCurLanguage(double mills)
        {
            int[] result = DateTimeUtil.GetTimeString(mills);
            if (mills > DateTimeUtil.m_Day_Mills)
            {
                string tip = GetTipsInCurLanguage(4194);
                return string.Format(tip, result[0] + 1);
            } else
            {
                string tip = GetTipsInCurLanguage(4188);
                string h = result[1] < 10 ? string.Format(m_TimeFormat, result[1]) : result[1].ToString();
                string m = result[2] < 10 ? string.Format(m_TimeFormat, result[2]) : result[2].ToString();
                string s = result[3] < 10 ? string.Format(m_TimeFormat, result[3]) : result[3].ToString();
                return string.Format(tip, h, m, s);
            }
        }

        public CampBuffItem GetCampBuffById(string id)
        {
            foreach (CampBuffItem i in CampBuffConfig.items)
            {
                if (i.id.Equals(id))
                {
                    return i;
                }
            }
            return null;
        }
    }
}
