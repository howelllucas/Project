using EZ;
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Game
{
    //游戏物品数据
    public class GameGoodData
    {
        public GoodsType type = GoodsType.NONE;   //类型
        public BigInteger count = 0;         //数量
        public int param = -1;        //参数（id）
    }

    public class GameRewardData
    {
        public int type = 0;            //类型
        public int count = 0;           //数量
        public int param = -1;          //参数（id）
    }

    public enum GoodsRequestType
    {
        Undefine,
        Local,
        Network,
    }

    public enum GoodsRequestResult
    {
        Undefine,
        Success,
        NetFail,
        DataFail_NotEnough,
    }

    public class GameGoodsAddResult
    {
        public GoodsRequestType requestType = GoodsRequestType.Undefine;
        public List<GameGoodData> realAddGoods = null;
        public HashSet<string> tips = null;
    }

    public class GameGoodsCostResult
    {
        public GoodsRequestType requestType = GoodsRequestType.Undefine;
        public GameGoodData costGoods = null;
        public bool canCost;
        public string tips;
    }

    public class GameGoodsMgr : Singleton<GameGoodsMgr>
    {
        #region 添加物品
        /// <summary>
        /// 添加物品请求回调
        /// </summary>
        /// <param name="result">请求结果</param>
        /// <param name="realAddGoods">实际添加的物品</param>
        /// <param name="tips">提示消息</param>
        public delegate void RequestAddGameGoodsCallback(GoodsRequestResult result, List<GameGoodData> realAddGoods, HashSet<string> tips);

        /// <summary>
        /// 请求添加游戏奖励
        /// </summary>
        public void RequestAddGameGoods(RequestAddGameGoodsCallback callback, List<GameRewardData> addGoods)
        {
            int[] reward_list = new int[addGoods.Count];
            int[] reward_type_list = new int[addGoods.Count];
            BigInteger[] reward_count_list = new BigInteger[addGoods.Count];
            for (int j = 0; j < addGoods.Count; j++)
            {

                reward_list[j] = (int)addGoods[j].type;
                reward_type_list[j] = addGoods[j].param;
                reward_count_list[j] = addGoods[j].count;

            }
            RequestAddGameGoods(callback, reward_list, reward_count_list, reward_type_list);
        }

        /// <summary>
        /// 请求添加游戏物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="add_goods_type"></param>
        /// <param name="add_goods_param"></param>
        /// <param name="add_goods_count"></param>
        public void RequestAddGameGoods(RequestAddGameGoodsCallback callback, int add_goods_type, BigInteger add_goods_count, int add_goods_param = 0)
        {
            RequestAddGameGoods(callback, new int[] { add_goods_type }, new BigInteger[] { add_goods_count }, new int[] { add_goods_param });
        }
        /// <summary>
        /// 请求添加游戏物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_counts"></param>
        /// <param name="add_goods_params"></param>
        public void RequestAddGameGoods(RequestAddGameGoodsCallback callback, int[] add_goods_types, int[] add_goods_counts, int[] add_goods_params)
        {
            RequestAddGameGoods(callback, add_goods_types, ConvertIntArrToBigIntegerArr(add_goods_counts), add_goods_params);
        }

        /// <summary>
        /// 请求添加游戏物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_params"></param>
        /// <param name="add_goods_counts"></param>
        public void RequestAddGameGoods(RequestAddGameGoodsCallback callback, int[] add_goods_types, BigInteger[] add_goods_counts, int[] add_goods_params)
        {
            List<GameGoodsAddResult> localAddResults = new List<GameGoodsAddResult>();
            List<GameGoodsAddResult> netAddResults = new List<GameGoodsAddResult>();
            for (int i = 0; i < add_goods_types.Length; i++)
            {
                GameGoodsAddResult addResult = CheckGoodsAddResult(add_goods_types[i], add_goods_counts[i], add_goods_params[i]);
                if (addResult == null || addResult.requestType == GoodsRequestType.Undefine)
                    continue;
                switch (addResult.requestType)
                {
                    case GoodsRequestType.Local:
                        localAddResults.Add(addResult);
                        break;
                    case GoodsRequestType.Network:
                        netAddResults.Add(addResult);
                        break;
                    default:
                        break;
                }
            }

            if (netAddResults.Count <= 0)
            {
                List<GameGoodData> realAddGoods = new List<GameGoodData>();
                HashSet<string> tips = new HashSet<string>();
                MergeGoodsAddResult(localAddResults, realAddGoods, tips);
                AddGameGoods(realAddGoods);
                callback?.Invoke(GoodsRequestResult.Success, realAddGoods, tips);
            }
            else
            {
                List<int> request_types = new List<int>();
                List<int> request_counts = new List<int>();
                List<int> request_params = new List<int>();
                for (int i = 0; i < netAddResults.Count; i++)
                {
                    for (int j = 0; j < netAddResults[i].realAddGoods.Count; j++)
                    {
                        var goods = netAddResults[i].realAddGoods[j];
                        request_types.Add((int)goods.type);
                        request_counts.Add((int)goods.count);//暂时使用强制转换 之后有大数字请求 需与服务器进行商讨确定
                        request_params.Add(goods.param);
                    }
                }
                //打开联网遮罩
                Global.gApp.gUiMgr.OpenPanel(Wndid.LoadingUI);
                ServerMgr.singleton.RequestAddDelResource(request_types.ToArray(), request_counts.ToArray(), request_params.ToArray(), (state) =>
                   {
                       //关闭联网遮罩
                       Global.gApp.gUiMgr.ClosePanel(Wndid.LoadingUI);
                       switch (state)
                       {
                           case ServerMgr.RequestCallbackState.Success:
                               {
                                   List<GameGoodData> realAddGoods = new List<GameGoodData>();
                                   HashSet<string> tips = new HashSet<string>();
                                   MergeGoodsAddResult(localAddResults, realAddGoods, tips);
                                   AddGameGoods(realAddGoods);//之后本地物品需要实际添加
                                   MergeGoodsAddResult(netAddResults, realAddGoods, tips);
                                   callback?.Invoke(GoodsRequestResult.Success, realAddGoods, tips);
                               }
                               break;
                           default:
                               {
                                   callback?.Invoke(GoodsRequestResult.NetFail, null, null);
                               }
                               break;
                       }

                   });

            }
        }

        private void MergeGoodsAddResult(List<GameGoodsAddResult> results, List<GameGoodData> realAddGoods, HashSet<string> tips)
        {
            for (int i = 0; i < results.Count; i++)
            {
                realAddGoods.AddRange(results[i].realAddGoods);
                if (results[i].tips != null)
                {
                    foreach (var tip in results[i].tips)
                    {
                        tips.Add(tip);
                    }
                }
            }
        }

        private GameGoodsAddResult CheckGoodsAddResult(int goods_type, BigInteger goods_count, int goods_param)
        {
            GameGoodsAddResult result = null;
            GoodsType goodsType = (GoodsType)goods_type;
            switch (goodsType)
            {
                case GoodsType.DIAMOND:
                    {
                        result = new GameGoodsAddResult();
#if SERVER_CHECK
                        result.requestType = GoodsRequestType.Network;
#else
                        result.requestType = GoodsRequestType.Local;
#endif
                        result.realAddGoods = new List<GameGoodData>()
                        {
                             new GameGoodData()
                             {
                                  type = (GoodsType)goods_type,
                                  count = goods_count,
                                  param = goods_param,
                             }
                        };
                    }
                    break;
                case GoodsType.GOLD:
                case GoodsType.KEY:
                case GoodsType.CAMPSITE_REWARD:
                    {
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>()
                        {
                             new GameGoodData()
                             {
                                  type = (GoodsType)goods_type,
                                  count = goods_count,
                                  param = goods_param,
                             }
                        };
                    }
                    break;
                case GoodsType.CARD:
                    {
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>();
                        result.tips = new HashSet<string>();
                        CheckCardRealAdd((int)goods_count, goods_param, result.realAddGoods, result.tips);
                    }
                    break;
                case GoodsType.CARD_CHIP:
                    {
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>();
                        result.tips = new HashSet<string>();
                        CheckCardClipRealAdd((int)goods_count, goods_param, result.realAddGoods, result.tips);
                    }
                    break;
                case GoodsType.RANDOM_CARD:
                    {
                        List<int> cardList = PlayerDataMgr.singleton.GetDropCardsByQuality(goods_param);
                        var index = BaseRandom.Next(0, cardList.Count);
                        var id = cardList[index];
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>();
                        result.tips = new HashSet<string>();
                        CheckCardRealAdd((int)goods_count, id, result.realAddGoods, result.tips);
                    }
                    break;
                case GoodsType.RANDOM_CARD_CHIP:
                    {
                        List<int> cardList = PlayerDataMgr.singleton.GetDropCardsByQuality(goods_param);
                        var index = BaseRandom.Next(0, cardList.Count);
                        var id = cardList[index];
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>();
                        result.tips = new HashSet<string>();
                        CheckCardClipRealAdd((int)goods_count, id, result.realAddGoods, result.tips);
                    }
                    break;
                case GoodsType.GOLD_MINUTE:
                    {
                        var count = goods_count * IdleRewardMgr.singleton.GetGoldPerMinute();
                        result = new GameGoodsAddResult();
                        result.requestType = GoodsRequestType.Local;
                        result.realAddGoods = new List<GameGoodData>()
                        {
                             new GameGoodData()
                             {
                                  type = GoodsType.GOLD,
                                  count = count,
                                  param = goods_param,
                             }
                        };
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        private void CheckCardRealAdd(int count, int cardId, List<GameGoodData> realAddGoods, HashSet<string> tips)
        {
            //判断卡片是否需要转换为碎片 并赋值转换后的卡片数量和碎片数量
            int cardCount = count;
            int clipCount = 0;

            if (cardCount > 0)
                realAddGoods.Add(new GameGoodData()
                {
                    type = GoodsType.CARD,
                    count = cardCount,
                    param = cardId,
                });

            if (clipCount > 0)
            {
                tips.Add("ShowPage_Card_Fragments");
                CheckCardClipRealAdd(clipCount, cardId, realAddGoods, tips);
            }

        }

        private void CheckCardClipRealAdd(int count, int cardId, List<GameGoodData> realAddGoods, HashSet<string> tips)
        {
            //判断碎片是否需要转换为金币 并赋值转换后的碎片数量及金币数量
            int clipCount = count;
            int goldCount = 0;

            if (clipCount > 0)
            {
                realAddGoods.Add(new GameGoodData()
                {
                    type = GoodsType.CARD_CHIP,
                    count = clipCount,
                    param = cardId,
                });
            }

            if (goldCount > 0)
            {
                realAddGoods.Add(new GameGoodData()
                {
                    type = GoodsType.GOLD,
                    count = goldCount,
                });
                tips.Add("ShowPage_Card_StarMax");
            }
        }

        private void AddGameGoods(List<GameGoodData> goodsList)
        {
            for (int i = 0; i < goodsList.Count; i++)
            {
                var goods = goodsList[i];
                AddGameGoods(goods.type, goods.count, goods.param);
            }
        }

        private bool AddGameGoods(GoodsType _type, BigInteger count, int param = -1)
        {
            if (count <= 0)
            {
                return false;
            }
            switch (_type)
            {
                case GoodsType.DIAMOND:
                    {
                        return PlayerDataMgr.singleton.AddCurrency(CurrencyType.DIAMOND, (int)count);
                    }
                case GoodsType.GOLD:
                    {
                        return PlayerDataMgr.singleton.AddBigCurrency(CurrencyType.GOLD, count);
                    }
                case GoodsType.KEY:
                    {
                        return PlayerDataMgr.singleton.AddCurrency(CurrencyType.KEY, (int)count);
                    }
                case GoodsType.CARD:
                    {
                        for (int i = 0; i < count; ++i)
                        {
                            PlayerDataMgr.singleton.AddCard(param);
                        }
                        return true;
                    }
                case GoodsType.CARD_CHIP:
                    {
                        PlayerDataMgr.singleton.AddCardChip(param, (int)count);
                        return true;
                    }
                case GoodsType.CAMPSITE_REWARD:
                    {
                        CampsiteMgr.singleton.AddReward((double)count);
                        return true;
                    }
                    //case GoodsType.PLAYER_EXP:
                    //    {
                    //        PlayerRightsMgr.singleton.AddExp(count);
                    //        return true;
                    //    }
            }
            return false;

        }
        /// <summary>
        /// 直接添加至本地
        /// </summary>
        /// <param name="goodsList"></param>
        public void AddLocalGameGoods(List<GameGoodData> goodsList)
        {
            List<GameGoodsAddResult> localAddResults = new List<GameGoodsAddResult>();
            for (int i = 0; i < goodsList.Count; i++)
            {
                GameGoodsAddResult addResult = CheckGoodsAddResult((int)goodsList[i].type, goodsList[i].count, goodsList[i].param);
                if (addResult == null || addResult.requestType == GoodsRequestType.Undefine)
                    continue;
                localAddResults.Add(addResult);
            }

            List<GameGoodData> realAddGoods = new List<GameGoodData>();
            HashSet<string> tips = new HashSet<string>();
            MergeGoodsAddResult(localAddResults, realAddGoods, tips);
            AddGameGoods(realAddGoods);
        }
        /// <summary>
        /// 单个物品获得效果展示
        /// </summary>
        /// <param name="goods"></param>
        /// <param name="sender"></param>
        public void PlayAddGameGoodsEffect(GameGoodData goods, GameObject sender)
        {
            GoodsType reward = goods.type;
            BigInteger reward_count = goods.count;
            int reward_type = goods.param;

            switch (reward)
            {
                case GoodsType.CARD:
                case GoodsType.CARD_CHIP:
                    {
                        //UIMgr.singleton.FindUIObject<ResourceUI>().PlayCollectCardAnim
                        //(reward, reward_type, reward_count, sender.transform.position);
                    }
                    break;
                case GoodsType.GOLD:
                case GoodsType.DIAMOND:
                case GoodsType.KEY:
                    {
                        int rewardEfCount = reward_count > int.MaxValue ? int.MaxValue : (int)reward_count;
                        Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, reward, reward_count, sender.transform.position);
                    }
                    break;

            }
        }
        /// <summary>
        /// 多物品获得效果展示
        /// </summary>
        /// <param name="goodsList"></param>
        /// <param name="tips"></param>
        public void PlayAddGameGoodsEffect(List<GameGoodData> goodsList, HashSet<string> tips)
        {
            //ShowRewardUI.InputData showData = new ShowRewardUI.InputData()
            //{
            //    rewardList = goodsList,
            //    tips = tips,
            //};
            //UIMgr.singleton.Open("ShowRewardUI", showData);
        }
        #endregion

        #region 消耗物品
        /// <summary>
        /// 消耗物品请求回调
        /// </summary>
        /// <param name="result">请求结果</param>
        /// <param name="failTips">提示消息</param>
        public delegate void RequestCostGameGoodsCallback(GoodsRequestResult result, string detail);

        /// <summary>
        /// 请求消耗物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_type"></param>
        /// <param name="cost_goods_count"></param>
        /// <param name="cost_goods_param"></param>
        public void RequestCostGameGoods(RequestCostGameGoodsCallback callback, int cost_goods_type, BigInteger cost_goods_count, int cost_goods_param = 0)
        {
            RequestCostGameGoods(callback, new int[] { cost_goods_type }, new BigInteger[] { cost_goods_count }, new int[] { cost_goods_param });
        }

        /// <summary>
        /// 请求消耗物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_types"></param>
        /// <param name="cost_goods_counts"></param>
        /// <param name="cost_goods_params"></param>
        public void RequestCostGameGoods(RequestCostGameGoodsCallback callback, int[] cost_goods_types, int[] cost_goods_counts, int[] cost_goods_params)
        {
            RequestCostGameGoods(callback, cost_goods_types, ConvertIntArrToBigIntegerArr(cost_goods_counts), cost_goods_params);
        }

        /// <summary>
        /// 请求消耗物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_types"></param>
        /// <param name="cost_goods_counts"></param>
        /// <param name="cost_goods_params"></param>
        public void RequestCostGameGoods(RequestCostGameGoodsCallback callback, int[] cost_goods_types, BigInteger[] cost_goods_counts, int[] cost_goods_params)
        {
            List<GameGoodsCostResult> localCostResults = new List<GameGoodsCostResult>();
            List<GameGoodsCostResult> netCostResults = new List<GameGoodsCostResult>();
            for (int i = 0; i < cost_goods_types.Length; i++)
            {
                var costResult = CheckGoodsCostResult(cost_goods_types[i], cost_goods_counts[i], cost_goods_params[i]);
                if (costResult == null || costResult.requestType == GoodsRequestType.Undefine)
                    continue;
                if (!costResult.canCost)
                {
                    callback?.Invoke(GoodsRequestResult.DataFail_NotEnough, ((int)costResult.costGoods.type).ToString());
                    return;
                }

                switch (costResult.requestType)
                {
                    case GoodsRequestType.Local:
                        localCostResults.Add(costResult);
                        break;
                    case GoodsRequestType.Network:
                        netCostResults.Add(costResult);
                        break;
                    default:
                        break;
                }
            }

            if (netCostResults.Count <= 0)
            {
                ApplyCostResults(localCostResults);
                callback?.Invoke(GoodsRequestResult.Success, null);
            }
            else
            {
                List<int> request_types = new List<int>();
                List<int> request_counts = new List<int>();
                List<int> request_params = new List<int>();
                for (int i = 0; i < netCostResults.Count; i++)
                {
                    var goods = netCostResults[i].costGoods;
                    request_types.Add((int)goods.type);
                    request_counts.Add(-(int)goods.count);//暂时使用强制转换 之后有大数字请求 需与服务器进行商讨确定
                    request_params.Add(goods.param);
                }
                //打开联网遮罩
                Global.gApp.gUiMgr.OpenPanel(Wndid.LoadingUI);
                ServerMgr.singleton.RequestAddDelResource(request_types.ToArray(), request_counts.ToArray(), request_params.ToArray(), (state) =>
                {
                    //关闭联网遮罩
                    Global.gApp.gUiMgr.ClosePanel(Wndid.LoadingUI);

                    switch (state)
                    {
                        case ServerMgr.RequestCallbackState.Success:
                            {
                                ApplyCostResults(localCostResults);
                                BroadcastCostResults(netCostResults);
                                callback?.Invoke(GoodsRequestResult.Success, null);
                            }
                            break;
                        case ServerMgr.RequestCallbackState.DataFail:
                            {
                                GoodsRequestResult failResult = GoodsRequestResult.Undefine;
                                string detail = null;
                                for (int i = 0; i < cost_goods_types.Length; i++)
                                {
                                    var costResult = CheckGoodsCostResult(cost_goods_types[i], cost_goods_counts[i], cost_goods_params[i]);
                                    if (costResult == null || costResult.requestType == GoodsRequestType.Undefine)
                                        continue;
                                    if (!costResult.canCost)
                                    {
                                        failResult = GoodsRequestResult.DataFail_NotEnough;
                                        detail = ((int)costResult.costGoods.type).ToString();
                                        break;
                                    }
                                }

                                callback?.Invoke(failResult, detail);
                            }
                            break;
                        case ServerMgr.RequestCallbackState.NetFail:
                            {
                                callback?.Invoke(GoodsRequestResult.NetFail, null);
                            }
                            break;
                        default:
                            {
                                callback?.Invoke(GoodsRequestResult.Undefine, null);
                            }
                            break;
                    }
                });
            }
        }

        private GameGoodsCostResult CheckGoodsCostResult(int goods_type, BigInteger goods_count, int goods_param)
        {
            if (goods_count <= 0)
                return null;

            GameGoodsCostResult result = null;
            switch ((GoodsType)goods_type)
            {
                case GoodsType.DIAMOND:
                    {
                        result = new GameGoodsCostResult();
#if SERVER_CHECK
                        result.requestType = GoodsRequestType.Network;
#else
                        result.requestType = GoodsRequestType.Local;
#endif
                        if (PlayerDataMgr.singleton.GetCurrency(CurrencyType.DIAMOND) >= goods_count)
                        {
                            result.canCost = true;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                        }
                        else
                        {
                            result.canCost = false;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                            result.tips = string.Format("{0} 不足", GetGameGoodsName((GoodsType)goods_type));
                        }
                    }
                    break;
                case GoodsType.GOLD:
                    {
                        result = new GameGoodsCostResult();
                        result.requestType = GoodsRequestType.Local;
                        if (PlayerDataMgr.singleton.GetBigCurrency(CurrencyType.GOLD) >= goods_count)
                        {
                            result.canCost = true;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                        }
                        else
                        {
                            result.canCost = false;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                            result.tips = string.Format("{0} 不足", GetGameGoodsName((GoodsType)goods_type));
                        }
                    }
                    break;
                case GoodsType.KEY:
                    {
                        result = new GameGoodsCostResult();
                        result.requestType = GoodsRequestType.Local;
                        CurrencyType currencyType = Goods2Currency((GoodsType)goods_type);
                        if (PlayerDataMgr.singleton.GetCurrency(currencyType) >= goods_count)
                        {
                            result.canCost = true;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                        }
                        else
                        {
                            result.canCost = false;
                            result.costGoods = new GameGoodData()
                            {
                                type = (GoodsType)goods_type,
                                count = goods_count,
                                param = goods_param,
                            };
                            result.tips = string.Format("{0} 不足", GetGameGoodsName((GoodsType)goods_type));
                        }
                    }
                    break;
                case GoodsType.CARD:
                    break;
                case GoodsType.CARD_CHIP:
                    break;
                default:
                    break;
            }
            return result;
        }

        private void ApplyCostResults(List<GameGoodsCostResult> costResultList)
        {
            for (int i = 0; i < costResultList.Count; i++)
            {
                var goods = costResultList[i].costGoods;
                CostGameGoods(goods.type, goods.count, goods.param);
            }
        }

        private void BroadcastCostResults(List<GameGoodsCostResult> costResultList)
        {
            for (int i = 0; i < costResultList.Count; i++)
            {
                var goods = costResultList[i].costGoods;
                switch (goods.type)
                {
                    case GoodsType.DIAMOND:
                    case GoodsType.GOLD:
                    case GoodsType.KEY:
                        {
                            CurrencyType currency = Goods2Currency(goods.type);
                            PlayerDataMgr.singleton.BroadcastCurrencyChange(currency);
                        }
                        break;
                    case GoodsType.CARD:
                        {
                            //UIMgr.singleton.BoradCast(UIEventType.CARD);
                        }
                        break;
                    case GoodsType.CARD_CHIP:
                        {
                            //UIMgr.singleton.BoradCast(UIEventType.CARD);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public bool CostGameGoods(GoodsType _type, BigInteger count, int param = -1)
        {
            if (count <= 0)
            {
                return false;
            }
            switch (_type)
            {
                case GoodsType.DIAMOND:
                    {
                        PlayerDataMgr.singleton.CostCurrency(CurrencyType.DIAMOND, (int)count);
                    }
                    break;
                case GoodsType.GOLD:
                    {
                        PlayerDataMgr.singleton.CostBigCurrency(CurrencyType.GOLD, count);
                    }
                    break;
                case GoodsType.KEY:
                    {
                        PlayerDataMgr.singleton.CostCurrency(CurrencyType.KEY, (int)count);
                    }
                    break;
                default:
                    return false;
            }
            return true;

        }

        #endregion

        #region 消耗&添加物品
        /// <summary>
        /// 消耗并添加物品回调
        /// </summary>
        /// <param name="result">请求结果</param>
        /// <param name="realAddGoods">成功时实际添加的物品信息</param>
        /// <param name="successTips">成功提示消息</param>
        /// <param name="failDetail">错误细节</param>
        public delegate void RequestCostAddGameGoodsCallback(GoodsRequestResult result, List<GameGoodData> realAddGoods, HashSet<string> successTips, string failDetail);

        /// <summary>
        /// 请求消耗物品以添加物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_type"></param>
        /// <param name="cost_goods_count"></param>
        /// <param name="cost_goods_param"></param>
        /// <param name="add_goods_type"></param>
        /// <param name="add_goods_count"></param>
        /// <param name="add_goods_param"></param>
        public void RequestCostAddGameGoods(RequestCostAddGameGoodsCallback callback,
            int cost_goods_type, BigInteger cost_goods_count, int cost_goods_param,
            int add_goods_type, BigInteger add_goods_count, int add_goods_param)
        {
            RequestCostAddGameGoods(callback, new int[] { cost_goods_type }, new BigInteger[] { cost_goods_count }, new int[] { cost_goods_param },
              new int[] { add_goods_type }, new BigInteger[] { add_goods_count }, new int[] { add_goods_param });
        }

        /// <summary>
        /// 请求消耗物品以添加物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_type"></param>
        /// <param name="cost_goods_count"></param>
        /// <param name="cost_goods_param"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_counts"></param>
        /// <param name="add_goods_params"></param>
        public void RequestCostAddGameGoods(RequestCostAddGameGoodsCallback callback,
            int cost_goods_type, BigInteger cost_goods_count, int cost_goods_param,
            int[] add_goods_types, int[] add_goods_counts, int[] add_goods_params)
        {
            RequestCostAddGameGoods(callback, cost_goods_type, cost_goods_count, cost_goods_param,
                add_goods_types, ConvertIntArrToBigIntegerArr(add_goods_counts), add_goods_params);
        }

        /// <summary>
        /// 请求消耗物品以添加物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_type"></param>
        /// <param name="cost_goods_count"></param>
        /// <param name="cost_goods_param"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_counts"></param>
        /// <param name="add_goods_params"></param>
        public void RequestCostAddGameGoods(RequestCostAddGameGoodsCallback callback,
            int cost_goods_type, BigInteger cost_goods_count, int cost_goods_param,
            int[] add_goods_types, BigInteger[] add_goods_counts, int[] add_goods_params)
        {
            RequestCostAddGameGoods(callback, new int[] { cost_goods_type }, new BigInteger[] { cost_goods_count }, new int[] { cost_goods_param },
                add_goods_types, add_goods_counts, add_goods_params);
        }
        /// <summary>
        /// 请求消耗物品以添加物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_types"></param>
        /// <param name="cost_goods_counts"></param>
        /// <param name="cost_goods_params"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_counts"></param>
        /// <param name="add_goods_params"></param>
        public void RequestCostAddGameGoods(RequestCostAddGameGoodsCallback callback,
            int[] cost_goods_types, int[] cost_goods_counts, int[] cost_goods_params,
            int[] add_goods_types, int[] add_goods_counts, int[] add_goods_params)
        {
            RequestCostAddGameGoods(callback, cost_goods_types, ConvertIntArrToBigIntegerArr(cost_goods_counts), cost_goods_params,
                add_goods_types, ConvertIntArrToBigIntegerArr(add_goods_counts), add_goods_params);
        }
        /// <summary>
        /// 请求消耗物品以添加物品
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cost_goods_types"></param>
        /// <param name="cost_goods_counts"></param>
        /// <param name="cost_goods_params"></param>
        /// <param name="add_goods_types"></param>
        /// <param name="add_goods_counts"></param>
        /// <param name="add_goods_params"></param>
        public void RequestCostAddGameGoods(RequestCostAddGameGoodsCallback callback,
            int[] cost_goods_types, BigInteger[] cost_goods_counts, int[] cost_goods_params,
            int[] add_goods_types, BigInteger[] add_goods_counts, int[] add_goods_params)
        {
            List<GameGoodsCostResult> localCostResults = new List<GameGoodsCostResult>();
            List<GameGoodsCostResult> netCostResults = new List<GameGoodsCostResult>();
            for (int i = 0; i < cost_goods_types.Length; i++)
            {
                var costResult = CheckGoodsCostResult(cost_goods_types[i], cost_goods_counts[i], cost_goods_params[i]);
                if (costResult == null || costResult.requestType == GoodsRequestType.Undefine)
                    continue;
                if (!costResult.canCost)
                {
                    callback?.Invoke(GoodsRequestResult.DataFail_NotEnough, null, null, ((int)costResult.costGoods.type).ToString());
                    return;
                }

                switch (costResult.requestType)
                {
                    case GoodsRequestType.Local:
                        localCostResults.Add(costResult);
                        break;
                    case GoodsRequestType.Network:
                        netCostResults.Add(costResult);
                        break;
                    default:
                        break;
                }
            }

            List<GameGoodsAddResult> localAddResults = new List<GameGoodsAddResult>();
            List<GameGoodsAddResult> netAddResults = new List<GameGoodsAddResult>();
            for (int i = 0; i < add_goods_types.Length; i++)
            {
                GameGoodsAddResult addResult = CheckGoodsAddResult(add_goods_types[i], add_goods_counts[i], add_goods_params[i]);
                if (addResult == null || addResult.requestType == GoodsRequestType.Undefine)
                    continue;
                switch (addResult.requestType)
                {
                    case GoodsRequestType.Local:
                        localAddResults.Add(addResult);
                        break;
                    case GoodsRequestType.Network:
                        netAddResults.Add(addResult);
                        break;
                    default:
                        break;
                }
            }

            if (netCostResults.Count <= 0 && netAddResults.Count <= 0)
            {
                List<GameGoodData> realAddGoods = new List<GameGoodData>();
                HashSet<string> tips = new HashSet<string>();
                MergeGoodsAddResult(localAddResults, realAddGoods, tips);
                AddGameGoods(realAddGoods);
                ApplyCostResults(localCostResults);
                callback?.Invoke(GoodsRequestResult.Success, realAddGoods, tips, null);
            }
            else
            {
                List<int> request_types = new List<int>();
                List<int> request_counts = new List<int>();
                List<int> request_params = new List<int>();
                for (int i = 0; i < netCostResults.Count; i++)
                {
                    var goods = netCostResults[i].costGoods;
                    request_types.Add((int)goods.type);
                    request_counts.Add(-(int)goods.count);//暂时使用强制转换 之后有大数字请求 需与服务器进行商讨确定
                    request_params.Add(goods.param);
                }
                for (int i = 0; i < netAddResults.Count; i++)
                {
                    for (int j = 0; j < netAddResults[i].realAddGoods.Count; j++)
                    {
                        var goods = netAddResults[i].realAddGoods[j];
                        request_types.Add((int)goods.type);
                        request_counts.Add((int)goods.count);//暂时使用强制转换 之后有大数字请求 需与服务器进行商讨确定
                        request_params.Add(goods.param);
                    }
                }
                //打开联网遮罩
                Global.gApp.gUiMgr.OpenPanel(Wndid.LoadingUI);
                ServerMgr.singleton.RequestAddDelResource(request_types.ToArray(), request_counts.ToArray(), request_params.ToArray(), (state) =>
                {
                    //关闭联网遮罩
                    Global.gApp.gUiMgr.ClosePanel(Wndid.LoadingUI);
                    switch (state)
                    {
                        case ServerMgr.RequestCallbackState.Success:
                            {
                                ApplyCostResults(localCostResults);
                                BroadcastCostResults(netCostResults);
                                List<GameGoodData> realAddGoods = new List<GameGoodData>();
                                HashSet<string> tips = new HashSet<string>();
                                MergeGoodsAddResult(localAddResults, realAddGoods, tips);
                                AddGameGoods(realAddGoods);//之后本地物品需要实际添加
                                MergeGoodsAddResult(netAddResults, realAddGoods, tips);
                                callback?.Invoke(GoodsRequestResult.Success, realAddGoods, tips, null);
                            }
                            break;
                        case ServerMgr.RequestCallbackState.DataFail:
                            {
                                GoodsRequestResult failResult = GoodsRequestResult.Undefine;
                                string failDetail = null;
                                for (int i = 0; i < cost_goods_types.Length; i++)
                                {
                                    var costResult = CheckGoodsCostResult(cost_goods_types[i], cost_goods_counts[i], cost_goods_params[i]);
                                    if (costResult == null || costResult.requestType == GoodsRequestType.Undefine)
                                        continue;
                                    if (!costResult.canCost)
                                    {
                                        failResult = GoodsRequestResult.DataFail_NotEnough;
                                        failDetail = ((int)costResult.costGoods.type).ToString();
                                        break;
                                    }
                                }

                                BroadcastCostResults(netCostResults);
                                callback?.Invoke(failResult, null, null, failDetail);
                            }
                            break;
                        case ServerMgr.RequestCallbackState.NetFail:
                            {
                                callback?.Invoke(GoodsRequestResult.NetFail, null, null, null);
                            }
                            break;
                        default:
                            {
                                callback?.Invoke(GoodsRequestResult.Undefine, null, null, null);
                            }
                            break;
                    }
                });
            }
        }

        #endregion

        public CurrencyType Goods2Currency(GoodsType type)
        {
            switch (type)
            {
                case GoodsType.DIAMOND:
                    return CurrencyType.DIAMOND;
                case GoodsType.GOLD:
                case GoodsType.GOLD_MINUTE:
                    return CurrencyType.GOLD;
                case GoodsType.KEY:
                    return CurrencyType.KEY;
            }
            return CurrencyType.Undefine;
        }

        public GoodsType Currency2Goods(CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.DIAMOND:
                    return GoodsType.DIAMOND;
                case CurrencyType.GOLD:
                    return GoodsType.GOLD;
                case CurrencyType.KEY:
                    return GoodsType.KEY;
                default:
                    break;
            }

            return GoodsType.NONE;
        }

        /// <summary>
        /// 获得游戏物品名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public string GetGameGoodsName(GoodsType _type, int value = -1)
        {
            switch (_type)
            {

                case GoodsType.DIAMOND:
                case GoodsType.GOLD:
                case GoodsType.KEY:
                    {
                        var currency = Goods2Currency(_type);
                        var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)currency);
                        if (currency_res != null)
                        {
                            return currency_res.name;
                        }
                    }
                    break;
                case GoodsType.CARD:
                    {
                        var card_res = TableMgr.singleton.GunCardTable.GetItemByID(value);
                        if (card_res != null)
                        {
                            return LanguageMgr.GetText(card_res.tid_name);
                        }
                    }
                    break;
            }
            return "";
        }

        public Sprite GetGameGoodsIcon(GoodsType _type, int value = -1)
        {
            switch (_type)
            {
                case GoodsType.DIAMOND:
                case GoodsType.GOLD:
                case GoodsType.GOLD_MINUTE:
                case GoodsType.KEY:
                    {
                        var currency = Goods2Currency(_type);
                        return GetCurrencyIcon(currency);
                    }
                    break;
                case GoodsType.CAMPSITE_REWARD:
                    {
                        return Global.gApp.gResMgr.LoadAssets<Sprite>("ResourcesSprites/Goods/ui_campreward");

                    }
                default:
                    break;
            }

            return null;
        }

        public Sprite GetCurrencyIcon(CurrencyType _type)
        {
            var currency_res = TableMgr.singleton.CurrencyTable.GetItemByID((int)_type);
            if (currency_res != null)
            {
                //之后考虑改为图集加载
                return Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format("ResourcesSprites/Goods/{0}", currency_res.icon));
                //return ResourceMgr.singleton.GetSprite(SpriteAtlasNames.ACom, currency_res.icon);
            }

            return null;
        }

        private BigInteger[] ConvertIntArrToBigIntegerArr(int[] arr)
        {
            if (arr == null)
                return null;
            BigInteger[] result = new BigInteger[arr.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = arr[i];
            }

            return result;
        }
    }
}