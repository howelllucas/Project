using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using LitJson;
using EZ;

namespace Game
{
    public class PurchaseProductData
    {
        #region 表内数据
        public int id;
        public string productId;
        public int type;
        #endregion
        #region SDK数据
        public string price;
        public string price_currency_code;
        public string price_amount;
        #endregion
        #region 后台数据
        public int[] reward_list;
        public int[] reward_type_list;
        public int[] reward_count_list;
        #endregion
    }

    public class PurchaseMgr : Singleton<PurchaseMgr>
    {
        //在服务器添加的商品
#if SERVER_CHECK
        private HashSet<GoodsType> AddOnServerProductRewards = new HashSet<GoodsType>() { GoodsType.DIAMOND };
#else
        private HashSet<GoodsType> AddOnServerProductRewards = new HashSet<GoodsType>() { };
#endif
        private const string PLATFORM_TYPE = "0";
        private string URL { get { return ServerMgr.URL + "/globalpay/iosPay?protocolVer="; } }
        private bool isInit = false;
        //全部商品id
        private string AllProductIdJsonArray
        {
            get; set;
        }
        //非消耗品、订阅商品查询json
        private string NonConsumableProductStatusRequestJson
        {
            //格式(每组订阅商品的商品id写在一个数组中)
            //"[[\"bnb_remove_ads\"],[\"bnb_weekly\",\"bnb_monthly\",\"bnb_yearly\"]]"
            get; set;
        }
        //恢复购买请求json
        private string RestoreRequestJson
        {
            //{\"Auto-Renewable Subscriptions\":[[\"bnb_weekly\",\"bnb_monthly\",\"bnb_yearly\"]],\"Non-Consumable\":[\"bnb_remove_ads\"]}
            get; set;
        }
        //所有商品信息
        private Dictionary<string, PurchaseProductData> productDataDic = new Dictionary<string, PurchaseProductData>();
        private Dictionary<int, PurchaseProductData> productDataDic2 = new Dictionary<int, PurchaseProductData>();
        /// <summary>
        /// 必须在ServerMgr初始化之后初始化
        /// </summary>
        public void Init()
        {
            //注册回调
            SdkdsPurchaseUtils.productListReceivedEvent += HandleProductListReceivedEvent;
            SdkdsPurchaseUtils.requestNonConsumableProductStatusCompleteEvent += HandleRequestNonConsumableProductStatusCompleteEvent;
            SdkdsPurchaseUtils.purchaseCompleteEvent += HandlePurchaseCompleteEvent;
            SdkdsPurchaseUtils.restoreCompleteEvent += HandleRestoreCompleteEvent;

            InitProductDatas();
        }

        private void InitProductDatas()
        {
            productDataDic.Clear();
            productDataDic2.Clear();
            List<string> allProductIdArray = new List<string>();
            Dictionary<string, List<string>> subscriptionGroupDic = new Dictionary<string, List<string>>();
            List<string> nonConsumableIdArray = new List<string>();
            foreach (PurchaseProduct_TableItem item in TableMgr.singleton.PurchaseProductTable.getEnumerator())
            {
                //之后可能分平台赋值
                string productId = item.productId;
                PurchaseProductData data = new PurchaseProductData();
                data.id = item.id;
                data.productId = productId;
                data.type = item.type;
#if !SERVER_CHECK
                int rewardNum = item.rewardNum;
                data.reward_list = new int[rewardNum];
                data.reward_type_list = new int[rewardNum];
                data.reward_count_list = new int[rewardNum];
                for (int i = 0; i < rewardNum; i++)
                {
                    data.reward_list[i] = item.reward_list[i];
                    data.reward_type_list[i] = item.reward_type_list[i];
                    data.reward_count_list[i] = item.reward_count_list[i];
                }
#endif
                productDataDic[productId] = data;
                productDataDic2[data.id] = data;
                allProductIdArray.Add(productId);
                switch (item.type)
                {
                    case 2:
                        {
                            nonConsumableIdArray.Add(productId);
                        }
                        break;
                    case 3:
                        {
                            if (!subscriptionGroupDic.ContainsKey(item.subscriptionGroup))
                            {
                                subscriptionGroupDic.Add(item.subscriptionGroup, new List<string>());
                            }
                            subscriptionGroupDic[item.subscriptionGroup].Add(productId);
                        }
                        break;
                    default:
                        break;
                }
            }

            AllProductIdJsonArray = JsonMapper.ToJson(allProductIdArray);

            List<List<string>> NonConsumableProductStatusRequestArr = new List<List<string>>();
            Dictionary<string, object> RestoreRequestDic = new Dictionary<string, object>();
            if (nonConsumableIdArray.Count > 0)
            {
                NonConsumableProductStatusRequestArr.Add(nonConsumableIdArray);
                RestoreRequestDic["Non-Consumable"] = nonConsumableIdArray;
            }
            if (subscriptionGroupDic.Count > 0)
            {
                List<List<string>> subscriptionGroupItemArr = new List<List<string>>();
                foreach (var item in subscriptionGroupDic.Values)
                {
                    subscriptionGroupItemArr.Add(item);
                    NonConsumableProductStatusRequestArr.Add(item);
                }
                RestoreRequestDic["Auto-Renewable Subscriptions"] = subscriptionGroupItemArr;
            }
            if (NonConsumableProductStatusRequestArr.Count > 0)
                NonConsumableProductStatusRequestJson = JsonMapper.ToJson(NonConsumableProductStatusRequestArr);
            if (RestoreRequestDic.Count > 0)
                RestoreRequestJson = JsonMapper.ToJson(RestoreRequestDic);
        }

        public void OnLoginComplete()
        {
            if (!isInit)
            {
                SdkdsPurchaseUtils.InitPurchaseSDK(URL, ServerMgr.singleton.UserID, PLATFORM_TYPE, AllProductIdJsonArray, ServerMgr.singleton.GameID.ToString(), ServerMgr.singleton.Token);
                isInit = true;
            }
            RefreshProductDatas();
        }

        /// <summary>
        /// 刷新商品数据
        /// </summary>
        public void RefreshProductDatas()
        {
#if SERVER_CHECK
            ServerMgr.singleton.RequestGetPurchaseConfig(HandleGetPurchaseConfig);
#endif
            RequestProductInfos();
            if (!string.IsNullOrEmpty(NonConsumableProductStatusRequestJson))
                SdkdsPurchaseUtils.Instance.requestNonConsumableProductStatusWithProductIDs(NonConsumableProductStatusRequestJson);
        }

        private void HandleGetPurchaseConfig(ServerMgr.RequestCallbackState state, JsonData jsonData)
        {
            if (jsonData == null || !jsonData.IsArray)
            {
                return;
            }
            for (int i = 0; i < jsonData.Count; i++)
            {
                var dataDic = jsonData[i];
                if (dataDic == null)
                    continue;
                try
                {
                    string productId;
                    string productIdKey = "productId";//之后可能分平台赋值
                    if (!dataDic.TryGetStringVal(productIdKey, out productId))
                        continue;

                    PurchaseProductData productData;
                    if (productDataDic.TryGetValue(productId, out productData))
                    {
                        int rewardNum;
                        if (!dataDic.TryGetIntVal("rewardNum", out rewardNum))
                            continue;
                        int[] reward_list = new int[rewardNum];
                        int[] reward_type_list = new int[rewardNum];
                        int[] reward_count_list = new int[rewardNum];
                        for (int j = 0; j < rewardNum; j++)
                        {
                            int pos = j + 1;
                            int reward;
                            int reward_type;
                            int reward_count;
                            if (dataDic.TryGetIntVal("reward" + pos, out reward)
                                && dataDic.TryGetIntVal("reward_type" + pos, out reward_type)
                                && dataDic.TryGetIntVal("reward_count" + pos, out reward_count)
                                )
                            {
                                reward_list[j] = reward;
                                reward_type_list[j] = reward_type;
                                reward_count_list[j] = reward_count;
                            }
                        }
                        productData.reward_list = reward_list;
                        productData.reward_type_list = reward_type_list;
                        productData.reward_count_list = reward_count_list;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.PurchaseDataRefresh);
        }

        /// <summary>
        /// 获取全部商品数据
        /// </summary>
        /// <returns></returns>
        public PurchaseProductData[] GetAllProductData()
        {
            return productDataDic.Values.ToArray();
        }
        /// <summary>
        /// 根据productId获取商品数据
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool GetProductInfo(string productId, out PurchaseProductData info)
        {
            return productDataDic.TryGetValue(productId, out info);
        }
        /// <summary>
        /// 根据id获取商品数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool GetProductInfo(int id, out PurchaseProductData info)
        {
            return productDataDic2.TryGetValue(id, out info);
        }
        /// <summary>
        /// 请求商品数据
        /// </summary>
        private void RequestProductInfos()
        {
            SdkdsPurchaseUtils.Instance.requestProducts(AllProductIdJsonArray);
        }
        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="productId">商品id</param>
        public void BuyProduct(string productId)
        {
            Debug.Log("json buy:" + productId);
            if (!InternetMgr.singleton.IsInternetConnect())
            {
                //网络错误提示
                return;
            }
            if (NeedRemedyTransaction())
            {
                //补单提示
                RemedyTransaction();
                return;
            }

            PurchaseProductData productData;
            if (GetProductInfo(productId, out productData) && productData.reward_list != null)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.LoadingUI);
                SdkdsPurchaseUtils.Instance.buyProduct(ServerMgr.singleton.UserID, PLATFORM_TYPE, productId);
#if UNITY_EDITOR
                GameGoodsMgr.singleton.RequestAddGameGoods((GoodsRequestResult success, List<GameGoodData> realAddGoods, HashSet<string> tips) =>
                {
                    Global.gApp.gUiMgr.ClosePanel(Wndid.LoadingUI);
                    if (success == GoodsRequestResult.Success)
                        Global.gApp.gMsgDispatcher.Broadcast(MsgIds.PurchaseSuccess, false, productId, realAddGoods);
                    else
                        DealPurchaseFail("207", productId);
                }
            , productData.reward_list, productData.reward_count_list, productData.reward_type_list);
#endif
            }
            else
            {
                //网络错误提示
            }
        }
        /// <summary>
        /// 恢复购买
        /// </summary>
        public void Restore()
        {
            if (!string.IsNullOrEmpty(RestoreRequestJson))
                SdkdsPurchaseUtils.Instance.restoreCompletedTransactions(RestoreRequestJson);
        }

        /// <summary>
        /// 是否有需要补单的订单
        /// </summary>
        /// <returns></returns>
        public bool NeedRemedyTransaction()
        {
            return SdkdsPurchaseUtils.Instance.canRemedyTransaction();
        }

        /// <summary>
        /// 补单
        /// </summary>
        public void RemedyTransaction()
        {
            SdkdsPurchaseUtils.Instance.remedyTransaction();
        }

        //支付回调
        private void HandlePurchaseCompleteEvent(string strJson)
        {
            #region json格式
            /*
                消耗品和非消耗品
                {
                	"status": 100,
                	"orderId": "1000000478567975",
                	"originalTransactionId": "1000000478567975",
                	"productId": "bnb_ruby_100",
                	"isSandBox": 1,
                	"count": 0,
                "trialPeriod": false,
                	"inIntroOfferPeriod": false,
                	"userId": "375",
                	"message": "",
                	"expires": "0"
                }
                
                订阅商品
                {
                	"status": 100,
                	"orderId": "1000000478566079",
                	"originalTransactionId": "1000000478477973",
                	"productId": "bnb_weekly",
                	"isSandBox": 1,
                	"count": 0,
                	"userId": "375",
                	"message": "",
                	"expires": "1543315753000"
                }
            */
            #endregion
            var jsonData = JsonMapper.ToObject(strJson);

            string status;
            if (!jsonData.TryGetStringVal("status", out status))
                return;
            switch (status)
            {
                case "100"://验证成功
                    {
                        DealPurchaseSuccess(false, jsonData);
                    }
                    break;
                case "101"://补单
                    {
                        DealPurchaseSuccess(true, jsonData);
                    }
                    break;
                case "201"://网络错误
                case "202"://服务器错误
                case "203"://数据错误
                    {
                        string productId = null;
                        if (jsonData.TryGetStringVal("productId", out productId))
                        {
                            //自动补单or弹窗提示？
                            DealPurchaseFail(status, productId);
                        }
                    }
                    break;
                case "200"://支付失败
                case "204"://用户取消
                case "205"://支付失败，恢复账单
                case "206"://支付失败，重复账单
                case "207"://未知错误
                    {
                        string productId = null;
                        if (jsonData.TryGetStringVal("productId", out productId))
                        {
                            DealPurchaseFail(status, productId);
                        }
                    }
                    break;
                default:
                    break;
            }
            Global.gApp.gUiMgr.ClosePanel(Wndid.LoadingUI);
        }

        private void DealPurchaseSuccess(bool isRemedy, JsonData jsonData)
        {
            string productId = null;
            if (!jsonData.TryGetStringVal("productId", out productId))
            { }
            string expires = null;//订阅商品有效期时间戳
            if (!jsonData.TryGetStringVal("expires", out expires))
            { }
            int count = 0;
            if (!jsonData.TryGetIntVal("count", out count))
            { }
            string message = null;
            if (!jsonData.TryGetStringVal("message", out message))
            { }
            int isSandBox = 0;
            if (!jsonData.TryGetIntVal("isSandBox", out isSandBox))
            { }
            string userId = null;
            if (!jsonData.TryGetStringVal("userId", out userId))
            { }

            //ServerMgr.singleton.RequestGetResource(null);
            //暂未考虑GetResources请求失败情况
            //若考虑 还需考虑弹窗与请求顺序问题
            PurchaseProductData productData;
            if (!string.IsNullOrEmpty(productId) && productDataDic.TryGetValue(productId, out productData))
            {
                List<GameGoodData> rewardList = new List<GameGoodData>();
                List<GameGoodData> addLocalList = new List<GameGoodData>();
                for (int i = 0; i < productData.reward_list.Length; i++)
                {
                    GoodsType goodsType = (GoodsType)productData.reward_list[i];
                    if (!AddOnServerProductRewards.Contains(goodsType))
                    {
                        addLocalList.Add(new GameGoodData()
                        {
                            type = goodsType,
                            count = productData.reward_count_list[i],
                            param = productData.reward_type_list[i]
                        });
                    }
                    rewardList.Add(new GameGoodData()
                    {
                        type = goodsType,
                        count = productData.reward_count_list[i],
                        param = productData.reward_type_list[i]
                    });
                }

                GameGoodsMgr.singleton.AddLocalGameGoods(addLocalList);

                #region  表现
                PlayerDataMgr.singleton.BroadcastCurrencyChange(CurrencyType.DIAMOND);
                //UI.ShowRewardUI.InputData showData = new UI.ShowRewardUI.InputData()
                //{
                //    rewardList = new List<GameGoodData>(),
                //    tips = new HashSet<string>(),
                //};
                if (isRemedy)
                {
                    //ShowBufferMgr.singleton.AddUIShowBuffer(CacheUITriggerScene.Main, 1, "ShowRewardUI", showData);

                }
                else
                {
                    //UI.UIMgr.singleton.Open("ShowRewardUI", showData);

                }
                #endregion

                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.PurchaseSuccess, isRemedy, productId, rewardList);
            }
        }

        private void DealPurchaseFail(string status, string productId)
        {
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.PurchaseFail, status, productId);
        }
        //查询商品信息回调
        private void HandleProductListReceivedEvent(string strJson)
        {
            #region json格式
            /*
             [
               	{
               		"productId":"diamonds_60",
               		"type":"inapp",
               		"price":"￦1,200",
               		"price_amount_micros":1200000000,
               		"price_currency_code":"KRW",
               		"price_amount":1200,
               		"title":"60颗钻石 (弓箭手大作战)",
               		"description":"购买60颗钻石"
               	},
               	{
               		"productId":"diamonds_300",
               		"type":"inapp",
               		"price":"￦5,500",
               		"price_amount_micros":5500000000,
               		"price_currency_code":"KRW",
                    "price_amount":5500,
               		"title":"300颗钻石 (弓箭手大作战)",
               		"description":"购买300颗钻石"
               	}
              ]	
             */
            #endregion

            var jsonData = JsonMapper.ToObject(strJson);
            if (jsonData == null || !jsonData.IsArray)
                return;
            for (int i = 0; i < jsonData.Count; i++)
            {
                var dataDic = jsonData[i];
                if (dataDic == null)
                    continue;

                string productId = null;
                if (!dataDic.TryGetStringVal("productId", out productId))
                    continue;

                PurchaseProductData productData;
                if (productDataDic.TryGetValue(productId, out productData))
                {
                    dataDic.TryGetStringVal("price", out productData.price);
                    dataDic.TryGetStringVal("price_currency_code", out productData.price_currency_code);
                    dataDic.TryGetStringVal("price_amount", out productData.price_amount);
                }
            }

            //广播刷新
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.PurchaseDataRefresh);

        }
        //恢复购买回调
        private void HandleRestoreCompleteEvent(string strJson)
        {
            #region json格式
            /*
                         {
            	"status": "300",
            	"result": [{
            			"orderId": "1000000528737965",
            			"originalTransactionId": "1000000528732228",
            			"inIntroOfferPeriod": false,
            			"productId": "bnb_weekly",
            			"subscriptionsUserIds": [
            
            			],
            			"trialPeriod": false,
            			"expires": "1558093903000"
            		},
            		{
            			"orderId": "1000000534287281",
            			"originalTransactionId": "1000000534281297",
            			"productId": "bnb_aim",
            			"subscriptionsUserIds": [
            
            			],
            			"inIntroOfferPeriod": false,
            			"trialPeriod": false,
            			"expires": "0"
            		},
            		{
            			"orderId": "1000000534287284",
            			"originalTransactionId": "1000000534270817",
            			"productId": "bnb_remove_ads",
            			"subscriptionsUserIds": [
            
            			],
            			"inIntroOfferPeriod": false,
            			"trialPeriod": false,
            			"expires": "0"
            		}
            	],
            	"message": "",
            	"userId": "375",
            	"isSandBox": 1
            }
            */
            #endregion
            var jsonData = JsonMapper.ToObject(strJson);

            string status;
            if (!jsonData.TryGetStringVal("status", out status))
                return;
            switch (status)
            {
                case "300"://请求成功
                    {
                        string message = null;
                        if (!jsonData.TryGetStringVal("message", out message))
                        { }
                        int isSandBox = 0;
                        if (!jsonData.TryGetIntVal("isSandBox", out isSandBox))
                        { }
                        string userId = null;
                        if (!jsonData.TryGetStringVal("userId", out userId))
                        { }
                        JsonData resultJson;
                        if (jsonData.TryGetJsonData("result", out resultJson))
                        {
                            if (resultJson != null && resultJson.IsArray)
                            {
                                for (int i = 0; i < resultJson.Count; i++)
                                {
                                    RefreshNonConsumableProductStatus(resultJson[i], isSandBox, userId, message);
                                }
                            }
                        }
                    }
                    break;
                case "301"://请求失败
                    break;
                case "302"://网络错误
                    break;
                case "303"://服务器错误
                    break;
                case "304"://数据错误
                    break;
                default:
                    break;
            }
        }
        //查询非消耗品、订阅商品信息回调
        private void HandleRequestNonConsumableProductStatusCompleteEvent(string strJson)
        {
            #region json格式
            /*
             {
             	"status": "400",
             	"result": [{
             			"orderId": "1000000534281297",
             			"originalTransactionId": "1000000534281297",
             			"inIntroOfferPeriod": false,
             			"productId": "bnb_aim",
             			"subscriptionsUserIds": [
             
             
                         ],
             			"trialPeriod": false,
             			"expires": "0"
             		},
             		{
             			"orderId": "1000000534270817",
             			"originalTransactionId": "1000000534270817",
             			"inIntroOfferPeriod": false,
             			"productId": "bnb_remove_ads",
             			"subscriptionsUserIds": [
             
             
                         ],
             			"trialPeriod": false,
             			"expires": "0"
             		},
             		{
             			"orderId": "1000000528737965",
             			"originalTransactionId": "1000000528732228",
             			"inIntroOfferPeriod": false,
             			"productId": "bnb_weekly",
             			"subscriptionsUserIds": [
             
             
                         ],
             			"trialPeriod": false,
             			"expires": "1558093903000"
             		}
             	],
             	"message": "",
             	"isSandBox": 1,
             	"userId": "375"
             }
            */
            #endregion
            var jsonData = JsonMapper.ToObject(strJson);

            string status;
            if (!jsonData.TryGetStringVal("status", out status))
                return;
            switch (status)
            {
                case "400"://请求成功
                    {
                        string message = null;
                        if (!jsonData.TryGetStringVal("message", out message))
                        { }
                        int isSandBox = 0;
                        if (!jsonData.TryGetIntVal("isSandBox", out isSandBox))
                        { }
                        string userId = null;
                        if (!jsonData.TryGetStringVal("userId", out userId))
                        { }
                        JsonData resultJson;
                        if (jsonData.TryGetJsonData("result", out resultJson))
                        {
                            if (resultJson != null && resultJson.IsArray)
                            {
                                for (int i = 0; i < resultJson.Count; i++)
                                {
                                    RefreshNonConsumableProductStatus(resultJson[i], isSandBox, userId, message);
                                }
                            }
                        }
                    }
                    break;
                case "401"://请求失败
                    break;
                case "402"://网络错误
                    break;
                case "403"://服务器错误
                    break;
                case "404"://数据错误
                    break;
                default:
                    break;
            }

        }
        //刷新非消耗品、订阅商品状态
        private void RefreshNonConsumableProductStatus(JsonData jsonData, int isSandBox, string userId, string message)
        {
            if (jsonData == null)
                return;
            string productId = null;
            if (!jsonData.TryGetStringVal("productId", out productId))
            {
                return;
            }
            string expires = null;//订阅商品有效期时间戳
            if (!jsonData.TryGetStringVal("expires", out expires))
            { }
        }
    }
}