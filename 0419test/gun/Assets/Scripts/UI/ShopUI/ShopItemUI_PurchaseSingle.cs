using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    //单一物品内购
    public class ShopItemUI_PurchaseSingle : ShopItemUIBase
    {
        public Image rewardIconImg;
        public Text rewardNumTxt;
        public Text purchasePriceTxt;
        private PurchaseProductData purchaseProductData;

        private void OnEnable()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.PurchaseDataRefresh, OnPurchaseDataRefresh);
        }

        private void OnDisable()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.PurchaseDataRefresh, OnPurchaseDataRefresh);
        }

        public override void Init(Shop_TableItem res)
        {
            base.Init(res);
            int purchaseId = res.type_param;
            if (!PurchaseMgr.singleton.GetProductInfo(purchaseId, out purchaseProductData))
            {
                gameObject.SetActive(false);
                return;
            }
            RefreshUIData();
        }

        private void RefreshUIData()
        {
            if (purchaseProductData == null)
                return;

            if (string.IsNullOrEmpty(purchaseProductData.price))
            {
                purchasePriceTxt.text = "default";
            }
            else
            {
                purchasePriceTxt.text = purchaseProductData.price;
            }

            if (purchaseProductData.reward_list == null || purchaseProductData.reward_list.Length <= 0)
            {
                rewardIconImg.gameObject.SetActive(false);
                rewardNumTxt.gameObject.SetActive(false);
            }
            else
            {
                rewardIconImg.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon((GoodsType)purchaseProductData.reward_list[0], purchaseProductData.reward_type_list[0]);
                rewardNumTxt.text = purchaseProductData.reward_count_list[0].ToString();

                rewardIconImg.gameObject.SetActive(true);
                rewardNumTxt.gameObject.SetActive(true);
            }
        }

        protected override void OnBuyBtnClick()
        {
            base.OnBuyBtnClick();
            if (purchaseProductData != null)
            {
                Global.gApp.gMsgDispatcher.AddListener<bool, string, List<GameGoodData>>(MsgIds.PurchaseSuccess, OnPurchaseSuccess);
                PurchaseMgr.singleton.BuyProduct(purchaseProductData.productId);
            }
        }

        private void OnPurchaseDataRefresh()
        {
            RefreshUIData();
        }

        private void OnPurchaseSuccess(bool isRemedy, string productId, List<GameGoodData> rewardList)
        {
            if (purchaseProductData == null || purchaseProductData.productId != productId)
                return;

            base.OnBuySuccess();
            Global.gApp.gMsgDispatcher.RemoveListener<bool, string, List<GameGoodData>>(MsgIds.PurchaseSuccess, OnPurchaseSuccess);
            //表现
            for (int i = 0; i < rewardList.Count; i++)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowRewardGetEffect, rewardList[i].type, rewardList[i].count.GetPropEffectValue(), transform.position);

            }
        }
    }
}