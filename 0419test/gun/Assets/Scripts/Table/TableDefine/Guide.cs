using System;

namespace Game
{
    public class Guide_TableItem : TableItem
	{

        public int ID;
        public float DELAY;
        public int PAUSE;
        public float PASS;
        public int CLICK;
        public int SCENE;
        public int CANVAS;
        public string FRAME;
        public string UI;
        public string TEXT;
        public string EFFECT;
        public string TARGET;
        public int DOTIMES;
        public int SAVE;
        public int NEXT;
        public string AF;


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "ID", "ID");
            fields.addField(this, "DELAY", "DELAY");
            fields.addField(this, "PAUSE", "PAUSE");
            fields.addField(this, "PASS", "PASS");
            fields.addField(this, "CLICK", "CLICK");
            fields.addField(this, "SCENE", "SCENE");
            fields.addField(this, "CANVAS", "CANVAS");
            fields.addField(this, "FRAME", "FRAME");
            fields.addField(this, "UI", "UI");
            fields.addField(this, "TEXT", "TEXT");
            fields.addField(this, "EFFECT", "EFFECT");
            fields.addField(this, "TARGET", "TARGET");
            fields.addField(this, "DOTIMES", "DOTIMES");
            fields.addField(this, "SAVE", "SAVE");
            fields.addField(this, "NEXT", "NEXT");
            fields.addField(this, "AF", "AF");
        }
	
    }
}
