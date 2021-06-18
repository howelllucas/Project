namespace Game
{
    public class LanguageUnit_TableItem : TableItem
    {
        public string TID;
        public string CN;
        public string TW;
        public string EN;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "TID", "TID");
            fields.addField(this, "CN", "CN");
            fields.addField(this, "TW", "TW");
            fields.addField(this, "EN", "EN");
        }
    }

}
