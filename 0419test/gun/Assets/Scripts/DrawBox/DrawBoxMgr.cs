using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

namespace Game
{
    public class DrawCardData
    {
        public int cardID = 0;              //卡牌id
        public int fromCount = 0;           //起始数量
        public int toCount = 0;             //到达数量
        public bool isNew = true;           //是否新卡

        public void AddCard(int id)
        {
            var cardData = PlayerDataMgr.singleton.GetGunCardData(id);
            cardID = id;
            if (cardData != null)
            {
                isNew = false;
                fromCount = cardData.count;
                toCount = cardData.count + 10;
            }
            else
            {
                isNew = true;
            }
        }

        public int GetCount()
        {
            return toCount - fromCount;
        }

    }

    public class DrawBoxMgr : Singleton<DrawBoxMgr>
    {
        private Dictionary<int, List<int>> cardsDic = new Dictionary<int, List<int>>();
        private Dictionary<int, int> cardQualityDic = new Dictionary<int, int>();
        private List<int> lockCardList = new List<int>();

        public void Init()
        {
            foreach (GunCard_TableItem item in TableMgr.singleton.GunCardTable.getEnumerator())
            {
                List<int> cardList;
                if (!cardsDic.TryGetValue(item.rarity, out cardList))
                {
                    cardList = new List<int>();
                    cardsDic[item.rarity] = cardList;
                }

                cardsDic[item.rarity].Add(item.id);
            }

            var res = TableMgr.singleton.PurchaseProductTable.GetItemByID(7);
            if (res != null)//礼包中卡牌是否解锁
            {
                for (int i = 0; i < res.reward_list.Length; ++i)
                {
                    if (res.reward_list[i] == (int)GoodsType.CARD)
                    {
                        lockCardList.Add(res.reward_type_list[i]);
                    }
                }
            }
        }

        public GameGoodData GetBoxCost(int boxID, int times)
        {
            var boxRes = TableMgr.singleton.BoxTable.GetItemByID(boxID);
            if (boxRes == null)
                return null;

            var data = new GameGoodData();
            if (PlayerDataMgr.singleton.GetCurrency((CurrencyType)boxRes.key) >= times)
            {
                data.type = GoodsType.KEY;
                data.count = times;
            }
            else
            {
                if (times == 10)
                    data.count = TableMgr.singleton.ValueTable.open_box_ten_cost;
                else
                    data.count = times * boxRes.cost;

                if (boxRes.type == 1)
                {
                    data.type = GoodsType.DIAMOND;
                }
                else if (boxRes.type == 2)
                {
                    data.type = GoodsType.GOLD;
                }
            }

            return data;
        }

        public delegate void OpenBoxCallback(GoodsRequestResult result, List<DrawCardData> list);

        public void OpenBox(int id, int times, OpenBoxCallback callback)
        {
            var boxRes = TableMgr.singleton.BoxTable.GetItemByID(id);
            if (boxRes == null)
                return;

            var cardDataList = new List<DrawCardData>();
            int drawCount = 0;
            var cardList = DrawCard(id, times, out drawCount);
            List<int> typeList = new List<int>();
            List<int> countList = new List<int>();
            HashSet<int> curAddCard = new HashSet<int>();
            for (int i = 0; i < cardList.Count; ++i)
            {
                typeList.Add((int)GoodsType.CARD);
                countList.Add(1);

                var data = new DrawCardData();
                data.AddCard(cardList[i]);
                if (curAddCard.Contains(cardList[i]))
                    data.isNew = false;
                curAddCard.Add(cardList[i]);
                cardDataList.Add(data);
            }

            if (PlayerDataMgr.singleton.GetCurrency((CurrencyType)boxRes.key) >= times)
            {
                GameGoodsMgr.singleton.RequestCostAddGameGoods((rst, goods, tips, failDetail) =>
                {
                    if (rst == GoodsRequestResult.Success)
                    {
                        PlayerDataMgr.singleton.DB.boxDrawDic[id] = drawCount;

                        PlayerDataMgr.singleton.NotifySaveData();

                        CampTaskMgr.singleton.AddTaskData(TaskType.Open_Box, times);
                    }

                    callback?.Invoke(rst, cardDataList);

                }, (int)GameGoodsMgr.singleton.Currency2Goods((CurrencyType)boxRes.key), times, 0,
                    typeList.ToArray(), countList.ToArray(), cardList.ToArray());
            }
            else
            {
                //var cost = times * boxRes.cost;
                var costData = GetBoxCost(id, times);
                GameGoodsMgr.singleton.RequestCostAddGameGoods((rst, goods, tips, failDetail) =>
                {
                    if (rst == GoodsRequestResult.Success)
                    {
                        PlayerDataMgr.singleton.DB.boxDrawDic[id] = drawCount;

                        PlayerDataMgr.singleton.NotifySaveData();

                        CampTaskMgr.singleton.AddTaskData(TaskType.Open_Box, times);
                    }

                    callback?.Invoke(rst, cardDataList);

                }, (int)costData.type, costData.count, 0,
                    typeList.ToArray(), countList.ToArray(), cardList.ToArray());

            }
        }

        public List<int> DrawCard(int boxID, int times, out int drawCount)
        {
            if (!PlayerDataMgr.singleton.DB.boxDrawDic.TryGetValue(boxID, out drawCount))
            {
                drawCount = 0;
            }

            var boxRes = TableMgr.singleton.BoxTable.GetItemByID(boxID);
            if (boxRes == null)
                return null;

            Debug.Log("OpenBox " + boxID);
            Debug.Log("drawCount " + drawCount);

            var q = (int)CardQualityType.LEGEND;
            //cardQualityDic.Clear();
            List<int> cardQualityList = new List<int>();
            float allRate = 0.0f;
            for (int i = 0; i < boxRes.cardRateList.Length; ++i)//确定宝箱品质总概率
            {
                var quality = i + 1;
                if (!cardsDic.ContainsKey(quality))
                    continue;

                if (quality == q)
                {
                    //cardQualityDic[quality] = 0;
                    continue;
                }

                if (boxRes.cardRateList[i] > 0.0f)
                {
                    allRate += boxRes.cardRateList[i];
                    //cardQualityDic[quality] = 0;
                }
            }

            Debug.Log("allRate " + allRate);
            List<int> cardList = new List<int>();

            for (int i = 0; i < times; ++i)//随机宝箱各品质的数量
            {
                drawCount++;
                if (PlayerDataMgr.singleton.DB.boxDrawCount <
                    TableMgr.singleton.ValueTable.BoxAwardGunArr.Length)
                {
                    var cardID = TableMgr.singleton.ValueTable.BoxAwardGunArr[PlayerDataMgr.singleton.DB.boxDrawCount];
                    //cardID = GetTypeCardByQuality(quality, type);
                    cardList.Add(cardID);
                    PlayerDataMgr.singleton.DB.boxDrawCount++;

                    continue;
                }

                if (drawCount >= boxRes.awardCount)
                {//宝箱保底
                    //cardQualityDic[q] += 1;
                    cardQualityList.Add(q);
                    drawCount = 0;
                    continue;
                }

                bool b = PRD.Winning(drawCount, boxRes.cardRateList[q - 1]);
                if (b)
                {
                    //cardQualityDic[q] += 1;
                    cardQualityList.Add(q);
                    drawCount = 0;
                    continue;
                }

                var rate = BaseRandom.Next(0.0f, allRate);
                var all = 0.0f;
                for (int j = 0; j < boxRes.cardRateList.Length - 1; ++j)
                {
                    all += boxRes.cardRateList[j];
                    if (rate < all)
                    {
                        //cardQualityDic[j + 1] += 1;
                        cardQualityList.Add(j + 1);
                        break;
                    }

                }
            }


            for (int i = 0; i < cardQualityList.Count; ++i)
            {
                var quality = cardQualityList[i];
                //int count = 0;
                //if (!cardQualityDic.TryGetValue(quality, out count))
                //    continue;

                //Debug.LogFormat("quality{0} count{1}", quality, count);
                //for (int j = 0; j < count; ++j)
                //{
                var cardID = GetCardByQuality(quality);

                cardList.Add(cardID);
                //}

            }

            Debug.Log("drawCount :" + drawCount);


            return cardList;
        }

        public int GetCardByQuality(int quality)
        {
            List<int> dropList;
            if (!cardsDic.TryGetValue(quality, out dropList))
                return 0;

            var index = BaseRandom.Next(0, dropList.Count);
            var cardID = dropList[index];

            if (lockCardList.Contains(cardID))//卡牌是否解锁
            {
                Debug.Log("卡牌未解锁");
                return GetCardByQuality(quality);
            }

            return cardID;
        }

        public int GetTypeCardByQuality(int quality, int type)
        {
            List<int> dropList = new List<int>();
            foreach (GunCard_TableItem item in TableMgr.singleton.GunCardTable.getEnumerator())
            {
                if (item.gunType != type)
                    continue;

                if (item.rarity != quality)
                    continue;

                dropList.Add(item.id);
            }

            var index = BaseRandom.Next(0, dropList.Count);
            var cardID = dropList[index];

            Debug.Log(cardID + " GetTypeCardByQuality " + type);

            return cardID;
        }

        //获得宝箱必出传说卡次数
        public int GetBoxAwardCount(int boxID)
        {
            var boxRes = TableMgr.singleton.BoxTable.GetItemByID(boxID);
            if (boxRes == null)
                return 0;

            int count = 0;
            if (!PlayerDataMgr.singleton.DB.boxDrawDic.TryGetValue(boxID, out count))
            {
                return 0;
            }

            var list = PlayerDataMgr.singleton.GetCardsByRarity((int)CardQualityType.LEGEND);
            if (list.Count > 0)
                return 0;

            return boxRes.awardCount - count;
        }
    }
}