using System;

namespace Game
{
    public class Box_TableItem : TableItem
    {
        public enum Define { paramCount = 3, }

        public int id;
        public int type;
        public int cost;
        public int openLimit;
        public int awardCount;
        public int key;
        public string cards;

        public float[] cardRateList = new float[(int)Define.paramCount];
        


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "type", "type");
            fields.addField(this, "cost", "cost");
            fields.addField(this, "openLimit", "openLimit");
            fields.addField(this, "awardCount", "awardCount");
            fields.addField(this, "key", "key");
            fields.addField(this, "cards", "cards");

            fields.addArrayFileds(this, "cardRateList",
                new string[]{
                    "cardRate1",
                    "cardRate2",
                    "cardRate3",

                });
        }

    }
}
