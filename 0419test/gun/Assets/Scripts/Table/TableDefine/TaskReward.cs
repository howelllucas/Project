using System;

namespace Game
{
    public class TaskReward_TableItem : TableItem
    {
        public enum Define { paramCount = 4, }

        public int id;

        public int[] rewardList = new int[(int)Define.paramCount];
        public int[] typeList = new int[(int)Define.paramCount];
        public int[] countList = new int[(int)Define.paramCount];


        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");

            fields.addArrayFileds(this, "rewardList",
                new string[]{
                    "reward1",
                    "reward2",
                    "reward3",
                    "reward4",

                });


            fields.addArrayFileds(this, "typeList",
                new string[]{
                    "reward_type1",
                    "reward_type2",
                    "reward_type3",
                    "reward_type4",

                });


            fields.addArrayFileds(this, "countList",
                new string[]{
                    "reward_count1",
                    "reward_count2",
                    "reward_count3",
                    "reward_count4",

                });
        }

    }
}
