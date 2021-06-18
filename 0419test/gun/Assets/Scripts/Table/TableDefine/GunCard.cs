using UnityEngine;
using System.Collections;

namespace Game
{
    public class GunCard_TableItem : TableItem
    {
        public int id;
        public string tid_name;
        public string name;
        public string prefab;
        public string icon;
        public int rarity;
        public int gunType;
        public float atk;
        public float atkSpeed;
        public float bulletParam;
        public float productionBonus;
        public int campGunSkill;
        public int fuseSkill;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "prefab", "prefab");
            fields.addField(this, "name", "name");
            fields.addField(this, "icon", "icon");
            fields.addField(this, "rarity", "rarity");
            fields.addField(this, "gunType", "gunType");
            fields.addField(this, "atk", "atk");
            fields.addField(this, "atkSpeed", "atkSpeed");
            fields.addField(this, "bulletParam", "bulletParam");
            fields.addField(this, "productionBonus", "productionBonus");
            fields.addField(this, "campGunSkill", "campGunSkill");
            fields.addField(this, "fuseSkill", "fuseSkill");

        }
    }
}
