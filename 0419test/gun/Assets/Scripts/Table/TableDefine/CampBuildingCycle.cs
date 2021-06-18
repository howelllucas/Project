using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CampBuildingCycle_TableItem : TableItem
    {
        public int id;
        public double productionInitValue;
        public int billingTime;
        public double costInitValue;
        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "productionInitValue", "productionInitValue");
            fields.addField(this, "billingTime", "billingTime");
            fields.addField(this, "costInitValue", "costInitValue");
        }
    }
}