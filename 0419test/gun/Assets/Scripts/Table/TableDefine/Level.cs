using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class Level_TableItem : TableItem
    {
        public enum Define { paramCount = 3, }

        public int id;
        public float hpParam;
        public int atkLevel;
        public int idleSpeed;
        public int gold;
        public int passID;
        public int bossLevel;
        public int cardLevel;
        public int reward;
        public int reward_type;
        public int reward_count;
        public float waveFactor;
        public int[] starList = new int[(int)Define.paramCount];

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "hpParam", "hpParam");
            fields.addField(this, "atkLevel", "atkLevel");
            fields.addField(this, "idleSpeed", "idleSpeed");
            fields.addField(this, "gold", "gold");
            fields.addField(this, "passID", "passID");
            fields.addField(this, "bossLevel", "bossLevel");
            fields.addField(this, "cardLevel", "cardLevel");
            fields.addField(this, "reward", "reward");
            fields.addField(this, "reward_type", "reward_type");
            fields.addField(this, "reward_count", "reward_count");
            fields.addField(this, "waveFactor", "waveFactor");

            fields.addArrayFileds(this, "starList",
                new string[]{
                    "star1",
                    "star2",
                    "star3",
                });
        }
    }

    public class Level_Table : Table<Level_TableItem>
    {
        private Dictionary<int, List<Level_TableItem>> levelDic = new Dictionary<int, List<Level_TableItem>>();

        protected override bool FillData()
        {
            var result = base.FillData();
            if (result)
            {
                foreach (Level_TableItem item in getEnumerator())
                {
                    int level = item.cardLevel;
                    if (!levelDic.ContainsKey(level))
                        levelDic.Add(level, new List<Level_TableItem>());
                    levelDic[level].Add(item);
                }
            }

            return result;
        }

        public int GetCampUnlockStage(int autoLevel)
        {
            if (autoLevel <= 0)
                return 0;

            List<Level_TableItem> list;
            if (!levelDic.TryGetValue(autoLevel, out list))
                return GetCampUnlockStage(autoLevel - 1);

            var level = list[BaseRandom.Next(0, list.Count)];

            return level.id;
        }
    }
}
