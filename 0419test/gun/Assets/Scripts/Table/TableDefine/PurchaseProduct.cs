using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PurchaseProduct_TableItem : TableItem
    {
        public enum Define { paramCount = 2, }
        public int id;
        public string productId;
        //1-消耗品
        //2-非消耗品
        //3-订阅商品
        public int type;
        //订阅商品分组 暂时用不到
        public string subscriptionGroup;

        public int rewardNum;
        public int[] reward_list = new int[(int)Define.paramCount];
        public int[] reward_type_list = new int[(int)Define.paramCount];
        public int[] reward_count_list = new int[(int)Define.paramCount];

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "productId", "productId");
            fields.addField(this, "type", "type");
            fields.addField(this, "subscriptionGroup", "subscriptionGroup");
            fields.addField(this, "rewardNum", "rewardNum");

            fields.addArrayFileds(this, "reward_list",
                new string[]{
                    "reward1",
                    "reward2",
                });


            fields.addArrayFileds(this, "reward_type_list",
                new string[]{
                    "reward_type1",
                    "reward_type2",
                });


            fields.addArrayFileds(this, "reward_count_list",
                new string[]{
                    "reward_count1",
                    "reward_count2",
                });
        }
    }
}