using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class QuickIdle_TableItem : TableItem
    {
        public int id;
        public int consume;
        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "consume", "consume");
        }
    }
}