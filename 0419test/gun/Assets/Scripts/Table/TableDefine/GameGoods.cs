using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameGoods_TableItem : TableItem
    {
        public int type;
        public string propEffect;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "type", "type");
            fields.addField(this, "propEffect", "propEffect");
        }
    }
}