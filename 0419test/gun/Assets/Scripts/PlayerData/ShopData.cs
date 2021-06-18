using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    public class ShopDataKey
    {
        public static readonly string NUMOFBUY_DIC = "numOfBuyDic";
    }

    public class ShopData : ModelDataBase
    {
        private Dictionary<int, int> numOfBuyDic = new Dictionary<int, int>();

        public int GetBuyNum(int shopId)
        {
            if (numOfBuyDic.ContainsKey(shopId))
                return numOfBuyDic[shopId];

            return 0;
        }

        public void AddBuyNum(int shopId)
        {
            if (numOfBuyDic.ContainsKey(shopId))
                numOfBuyDic[shopId]++;
            else
                numOfBuyDic[shopId] = 1;
        }

        public void ClearShopNum(int shopId)
        {
            if (numOfBuyDic.ContainsKey(shopId))
                numOfBuyDic.Remove(shopId);
        }

        public override JsonData GetJsonData()
        {
            JsonData ret = new JsonData();
            ret.SetJsonType(JsonType.Object);

            if (numOfBuyDic.Count > 0)
            {
                JsonData numOfBuyJson = new JsonData();
                foreach (var item in numOfBuyDic)
                {
                    numOfBuyJson[item.Key.ToString()] = item.Value;
                }

                ret[ShopDataKey.NUMOFBUY_DIC] = numOfBuyJson;
            }


            return ret;
        }

        public override bool InitWithJson(JsonData data)
        {
            foreach (var key in data.Keys)
            {
                if (key == ShopDataKey.NUMOFBUY_DIC)
                {
                    numOfBuyDic.Clear();
                    var numOfBuyJson = data[key];
                    foreach (var idStr in numOfBuyJson.Keys)
                    {
                        int shopId;
                        if (int.TryParse(idStr, out shopId))
                        {
                            numOfBuyDic[shopId] = (int)numOfBuyJson[idStr];
                        }
                    }
                }
            }

            return true;
        }
    }
}