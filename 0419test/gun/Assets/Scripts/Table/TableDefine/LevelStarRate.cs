using System;

namespace Game
{
    public class LevelStarRate_TableItem : TableItem
	{
        public int id;
        public string tid_name;



        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid_name", "tid_name");

        }
	
    }
}
