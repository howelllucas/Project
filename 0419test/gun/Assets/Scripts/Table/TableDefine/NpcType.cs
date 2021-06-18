using System;

namespace Game
{
    public class NpcType_TableItem : TableItem
    {

        public int id;
        public string npcModel;
        public float lifeScale;
        public float speedScale;
        public float modelScale;
        public int damage;
        public string lifeUI;
        public int haveDeadAnim;
        public int deadFallDown;
        public string deadEffectName;
        public string soundType;
        public string deadSoundName;
        public string deadAnimSoundName;
        public string beHitSoundName;
        public float rvoRadius;
        public float rvoPriority;
        public float rvoObstacleTimeHorizon;

        public int[] skillList = new int[2];
        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "npcModel", "npcModel");
            fields.addField(this, "lifeScale", "lifeScale");
            fields.addField(this, "speedScale", "speedScale");
            fields.addField(this, "modelScale", "modelScale");
            fields.addField(this, "damage", "damage");
            fields.addField(this, "lifeUI", "lifeUI");
            fields.addField(this, "haveDeadAnim", "haveDeadAnim");
            fields.addField(this, "deadFallDown", "deadFallDown");
            fields.addField(this, "deadEffectName", "deadEffectName");
            fields.addField(this, "soundType", "soundType");
            fields.addField(this, "deadSoundName", "deadSoundName");
            fields.addField(this, "deadAnimSoundName", "deadAnimSoundName");
            fields.addField(this, "beHitSoundName", "beHitSoundName");
            fields.addField(this, "rvoRadius", "rvoRadius");
            fields.addField(this, "rvoPriority", "rvoPriority");
            fields.addField(this, "rvoObstacleTimeHorizon", "rvoObstacleTimeHorizon");

            fields.addArrayFileds(this, "skillList",
                new string[]{
                    "skill_1",
                    "skill_2",


            });
        }

    }
}
