using System;

namespace Game
{
    public class CampGunSkill_TableItem : TableItem
    {

        public int id;
        public int type;//1-产能提升 2-结算周期降低
        public string icon;
        public string name;
        public string tid_name;
        public string desc;
        public string tid_desc;
        public int level;
        public int campBuilding;
        public float value;


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "type", "type");
            fields.addField(this, "icon", "icon");
            fields.addField(this, "name", "name");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "desc", "desc");
            fields.addField(this, "tid_desc", "tid_desc");
            fields.addField(this, "level", "level");
            fields.addField(this, "campBuilding", "campBuilding");
            fields.addField(this, "value", "value");

        }

    }
}
