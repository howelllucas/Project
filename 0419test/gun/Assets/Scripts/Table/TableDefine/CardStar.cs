using System;

namespace Game
{
    public class CardStar_TableItem : TableItem
    {
        public enum Define { paramCount = 3, }

        public int id;
        public int cardRarity;
        public int star;
        public int starRarity;
        public int maxLevel;
        public int slotCount;

        public int[] needCardTypeList = new int[(int)Define.paramCount];
        public int[] needCardRarityList = new int[(int)Define.paramCount];
        public int[] needCardCountList = new int[(int)Define.paramCount];

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "cardRarity", "cardRarity");
            fields.addField(this, "star", "star");
            fields.addField(this, "starRarity", "starRarity");
            fields.addField(this, "maxLevel", "maxLevel");
            fields.addField(this, "slotCount", "slotCount");


            fields.addArrayFileds(this, "needCardTypeList",
                new string[]{
                    "needCard1Type",
                    "needCard2Type",
                    "needCard3Type",
                });

            fields.addArrayFileds(this, "needCardRarityList",
                new string[]{
                    "needCard1Rarity",
                    "needCard2Rarity",
                    "needCard3Rarity",
                });

            fields.addArrayFileds(this, "needCardCountList",
                new string[]{
                    "needCard1Count",
                    "needCard2Count",
                    "needCard3Count",
                });

        }

    }
}
