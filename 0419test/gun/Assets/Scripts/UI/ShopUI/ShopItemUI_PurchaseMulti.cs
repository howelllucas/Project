using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    //组合物品(礼包)内购
    public class ShopItemUI_PurchaseMulti : ShopItemUIBase
    {
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
        }
    }
}