using System;
using System.Collections.Generic;

namespace Game
{
    public class Dialogue_TableItem : TableItem
    {

        public int id;
        public string tid_text;
        public string dialogues;
        public int npc1;
        public int npc2;
        public int pos;
        public int group;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid_text", "tid_text");
            fields.addField(this, "dialogues", "dialogues");
            fields.addField(this, "npc1", "npc1");
            fields.addField(this, "npc2", "npc2");
            fields.addField(this, "pos", "pos");
            fields.addField(this, "group", "group");
        }

    }

    public class Dialogue_Table : Table<Dialogue_TableItem>
    {
        private Dictionary<int, List<Dialogue_TableItem>> groupDic = new Dictionary<int, List<Dialogue_TableItem>>();

        protected override bool FillData()
        {
            var result = base.FillData();
            if (result)
            {
                foreach (Dialogue_TableItem item in getEnumerator())
                {
                    int group = item.group;
                    if (!groupDic.ContainsKey(group))
                        groupDic.Add(group, new List<Dialogue_TableItem>());
                    groupDic[group].Add(item);
                }
            }

            return result;
        }

        public List<Dialogue_TableItem> GetDialogueByGroup(int group)
        {
            List<Dialogue_TableItem> list;
            if (groupDic.TryGetValue(group, out list))
                return list;
            return null;
        }

        public Dictionary<int, List<Dialogue_TableItem>> GetDialogueGroups()
        {
            return groupDic;
        }
    }
}
