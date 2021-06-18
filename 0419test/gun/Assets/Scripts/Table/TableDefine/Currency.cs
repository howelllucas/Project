using System;

namespace Game
{
    public class Currency_TableItem : TableItem
	{
        public int id;
        public string name;
        public string max;
        public int reg_time;
        public string icon;



        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "name", "name");
            fields.addField(this, "max", "max");
            fields.addField(this, "reg_time", "reg_time");
            fields.addField(this, "icon", "icon");

        }
	
    }
}
