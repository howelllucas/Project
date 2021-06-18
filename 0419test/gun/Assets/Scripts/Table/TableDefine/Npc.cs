using System;

namespace Game
{
    public class Npc_TableItem : TableItem
    {

        public int id;
        public string tid_name;
        public string name;
        public string icon;



        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "name", "name");
            fields.addField(this, "icon", "icon");
        }

    }
}
