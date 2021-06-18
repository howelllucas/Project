using System;

namespace Game
{
    public class Chapter_TableItem : TableItem
    {

        public int id;
        public string tid;
        public string name;
        public string icon;
        public int unlockStage;
        public int chestOpenLimit;
        public int box;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid", "tid");
            fields.addField(this, "name", "name");
            fields.addField(this, "icon", "icon");
            fields.addField(this, "unlockStage", "unlockStage");
            fields.addField(this, "chestOpenLimit", "chestOpenLimit");
            fields.addField(this, "box", "box");

        }

    }
}
