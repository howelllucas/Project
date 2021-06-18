using System;

namespace Game
{
    public class CardLv_TableItem : TableItem
    {
        public enum Define { paramCount = 4, }

        public int lv;
        public int lvupGold;



        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "lv", "lv");
            fields.addField(this, "lvupGold", "lvupGold");

        }

    }
}
