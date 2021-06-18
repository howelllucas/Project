using System.Collections.Generic;

namespace Game
{
    public class TableMgr : Singleton<TableMgr>
    {

        public Table<Currency_TableItem> CurrencyTable = new Table<Currency_TableItem>();
        public Value_Table ValueTable = new Value_Table();
        public Table<LanguageUnit_TableItem> LanguageTable = new Table<LanguageUnit_TableItem>();

        public Table<Task_TableItem> CampTaskTable = new Table<Task_TableItem>();
        public Table<TaskType_TableItem> CampTaskTypeTable = new Table<TaskType_TableItem>();
        public Level_Table LevelTable = new Level_Table();
        public Table<CampSet_TableItem> CampSetTable = new Table<CampSet_TableItem>();
        public Table<LevelStarRate_TableItem> LevelStarRateTable = new Table<LevelStarRate_TableItem>();
        public Table<LevelStarReward_TableItem> LevelStarRewardTable = new Table<LevelStarReward_TableItem>();
        public Table<Npc_TableItem> NpcTable = new Table<Npc_TableItem>();
        public Dialogue_Table DialogueTable = new Dialogue_Table();

        public Table<GunCard_TableItem> GunCardTable = new Table<GunCard_TableItem>();
        public Table<CardStar_TableItem> CardStarTable = new Table<CardStar_TableItem>();
        public Table<FuseGunSkill_TableItem> FuseGunSkillTable = new Table<FuseGunSkill_TableItem>();
        public Table<CampGunSkill_TableItem> CampGunSkillTable = new Table<CampGunSkill_TableItem>();
        public Table<GunType_TableItem> GunTypeTable = new Table<GunType_TableItem>();

        public Table<CampBuilding_TableItem> CampBuildingTable = new Table<CampBuilding_TableItem>();
        public Table<CampBuildingCycle_TableItem> CampBuildingCycleTable = new Table<CampBuildingCycle_TableItem>();

        public Table<PurchaseProduct_TableItem> PurchaseProductTable = new Table<PurchaseProduct_TableItem>();

        public Shop_Table ShopTable = new Shop_Table();
        public Table<GameGoods_TableItem> GameGoodsTable = new Table<GameGoods_TableItem>();
        public Table<Box_TableItem> BoxTable = new Table<Box_TableItem>();

        public struct TableLoader
        {
            public string mFileName;
            public Game.TableLoader mTable;
            public bool mNeedReplaceEnter;

            public TableLoader(string fileName, Game.TableLoader table, bool needReplaceEnter = false)
            {
                mFileName = fileName;
                mTable = table;
                mNeedReplaceEnter = needReplaceEnter;
            }
        }

        public HashSet<Game.TableLoader> notInitTables = new HashSet<Game.TableLoader>();

        protected virtual string transePath(string path)
        {
            return path;
        }

        public bool LoadTables(IEnumerable<TableLoader> loaders)
        {
            bool bRet = true;
            var info = new CheckInfo();
            foreach (var _load in loaders)
            {
                bRet &= _load.mTable.Read(transePath(_load.mFileName), _load.mNeedReplaceEnter);
                _load.mTable.Check(info);
                notInitTables.Add(_load.mTable);
            }
            return bRet;
        }

        public void InitUnInitTables(TableInitType type)
        {
            foreach (var tab in notInitTables)
            {
                tab.Init(type);
            }
            notInitTables.Clear();
        }

        protected string tablePath = "Table/";

        public IEnumerable<TableLoader> CollectTables()
        {
            var loaders = new List<TableLoader>();

            loaders.Add(new TableLoader(tablePath + "Currency", CurrencyTable));
            loaders.Add(new TableLoader(tablePath + "ConstValues", ValueTable));
            loaders.Add(new TableLoader(tablePath + "Language", LanguageTable));

            loaders.Add(new TableLoader(tablePath + "gunCard", GunCardTable));
            loaders.Add(new TableLoader(tablePath + "gunType", GunTypeTable));
            loaders.Add(new TableLoader(tablePath + "cardStarAscend", CardStarTable));
            loaders.Add(new TableLoader(tablePath + "fuseGunSkill", FuseGunSkillTable));
            loaders.Add(new TableLoader(tablePath + "campGunSkill", CampGunSkillTable));
            loaders.Add(new TableLoader(tablePath + "campTask", CampTaskTable));
            loaders.Add(new TableLoader(tablePath + "campTaskType", CampTaskTypeTable));
            loaders.Add(new TableLoader(tablePath + "campSet", CampSetTable));
            loaders.Add(new TableLoader(tablePath + "campBuilding", CampBuildingTable));
            loaders.Add(new TableLoader(tablePath + "campBuildingCycle", CampBuildingCycleTable));
            loaders.Add(new TableLoader(tablePath + "level", LevelTable));
            loaders.Add(new TableLoader(tablePath + "levelStarRate", LevelStarRateTable));
            loaders.Add(new TableLoader(tablePath + "levelStarReward", LevelStarRewardTable));
            loaders.Add(new TableLoader(tablePath + "npc", NpcTable));
            loaders.Add(new TableLoader(tablePath + "dialogue", DialogueTable));

            loaders.Add(new TableLoader(tablePath + "PurchaseProduct", PurchaseProductTable));
            loaders.Add(new TableLoader(tablePath + "Shop", ShopTable));
            loaders.Add(new TableLoader(tablePath + "GameGoods", GameGoodsTable));
            loaders.Add(new TableLoader(tablePath + "box", BoxTable));

            return loaders;
        }
    }
}
