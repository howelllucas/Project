using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Shop_TableItem : TableItem
    {
        public int id;
        public int type;//
        public int type_param;
        public int limit_count;
        public int group;
        public string prefab;
        public string bgIcon;
        public string bgEf;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "type", "type");
            fields.addField(this, "type_param", "type_param");
            fields.addField(this, "limit_count", "limit_count");
            fields.addField(this, "group", "group");
            fields.addField(this, "prefab", "prefab");
            fields.addField(this, "bgIcon", "bgIcon");
            fields.addField(this, "bgEf", "bgEf");
        }
    }

    public class Shop_Table : Table<Shop_TableItem>
    {
        private Dictionary<int, List<Shop_TableItem>> groupDic = new Dictionary<int, List<Shop_TableItem>>();

        protected override bool FillData()
        {
            var result = base.FillData();
            if (result)
            {
                foreach (Shop_TableItem item in getEnumerator())
                {
                    int group = item.group;
                    if (!groupDic.ContainsKey(group))
                        groupDic.Add(group, new List<Shop_TableItem>());
                    groupDic[group].Add(item);
                }
            }

            return result;
        }

        public List<Shop_TableItem> GetShopItemsByGroup(int group)
        {
            List<Shop_TableItem> list;
            if (groupDic.TryGetValue(group, out list))
                return list;
            return null;
        }
    }
}