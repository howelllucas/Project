using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TaskType_TableItem : TableItem
    {
        public int type;
        public string name;
        public string tid_name;
        public string desc;
        public string tid_desc;
        public int progress;
        public int cd;    


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "type", "type");
            fields.addField(this, "name", "name");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "tid_desc", "tid_desc");
            fields.addField(this, "desc", "desc");
            fields.addField(this, "progress", "progress");
            fields.addField(this, "cd", "cd");


        }
    }

}

