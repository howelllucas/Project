using System;

namespace Game
{
    public class GunType_TableItem : TableItem
    {
        public enum Define { paramCount = 4, }

        public int id;
        public string gunType;
        public string tid_type;
        public string icon;


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "gunType", "gunType");
            fields.addField(this, "tid_type", "tid_type");
            fields.addField(this, "icon", "icon");

        }

    }
}
