using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CampBuilding_TableItem : TableItem
    {
        public int id;
        public string tid_name;
        public string buildingName
        {
            get
            {
                return LanguageMgr.GetText(tid_name);
            }
        }
        public int gunType;
        public string prefab;
        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid_name", "tid_name");
            //fields.addField(this, "buildingName", "buildingName");
            fields.addField(this, "gunType", "gunType");
            fields.addField(this, "prefab", "prefab");
        }
    }
}