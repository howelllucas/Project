using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Reward_TableItem : TableItem
    {
        public int id;
        public int reward;
        public int reward_type;
        public int reward_count;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "reward", "reward");
            fields.addField(this, "reward_type", "reward_type");
            fields.addField(this, "reward_count", "reward_count");
        }
    }
}