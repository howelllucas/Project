using System;

namespace Game
{
    public class FuseGunSkill_TableItem : TableItem
    {

        public int id;
        public string icon;
        public string name;
        public string tid_name;
        public string desc;
        public string tid_desc;
        public int level;
        public float value;


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "icon", "icon");
            fields.addField(this, "name", "name");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "desc", "desc");
            fields.addField(this, "tid_desc", "tid_desc");
            fields.addField(this, "level", "level");
            fields.addField(this, "value", "value");

        }

    }
}
