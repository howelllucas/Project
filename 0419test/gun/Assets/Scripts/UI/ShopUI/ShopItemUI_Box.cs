using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public class ShopItemUI_Box : ShopItemUIBase
    {
        public Image costIcon;
        public Text costTxt;
        public GameObject redPoint;
        private CurrencyType costType;
        private BigInteger costPrice;
        private bool isClick;

        private void OnEnable()
        {
            Global.gApp.gMsgDispatcher.AddListener<CurrencyType>(MsgIds.CurrencyChange, OnCurrencyChange);
        }

        private void OnDisable()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<CurrencyType>(MsgIds.CurrencyChange, OnCurrencyChange);
        }

        public override void Init(Shop_TableItem res)
        {
            base.Init(res);
            RefreshBoxCost();
            RefreshUIData();
        }

        private void RefreshBoxCost()
        {
            if (res == null)
                return;
            var type = res.type;
            var boxId = res.type_param;

            switch (type)
            {
                case 2:
                    {
                        //单抽花费查询接口
                        var cost = DrawBoxMgr.singleton.GetBoxCost(boxId, 1);
                        costType = GameGoodsMgr.singleton.Goods2Currency(cost.type);
                        costPrice = cost.count;
                    }
                    break;
                case 3:
                    {
                        //十连花费查询接口
                        var cost = DrawBoxMgr.singleton.GetBoxCost(boxId, 10);
                        costType = GameGoodsMgr.singleton.Goods2Currency(cost.type);
                        costPrice = cost.count;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RefreshUIData()
        {
            costIcon.sprite = GameGoodsMgr.singleton.GetCurrencyIcon(costType);
            if (costType == CurrencyType.KEY)
            {
                costTxt.text = string.Format("{0}/{1}", PlayerDataMgr.singleton.GetCurrency(costType), costPrice);
                if (redPoint != null)
                    redPoint.gameObject.SetActive(true);
            }
            else
            {
                costTxt.text = costPrice.ToString();
                if (redPoint != null)
                    redPoint.gameObject.SetActive(false);
            }
        }

        private void OnCurrencyChange(CurrencyType type)
        {
            if (type == CurrencyType.KEY || type == CurrencyType.DIAMOND || type == CurrencyType.GOLD)
            {
                RefreshUIData();
            }
        }

        protected override void OnBuyBtnClick()
        {
            if (isClick)
                return;
            base.OnBuyBtnClick();
            if (res != null)
            {
                isClick = true;
                int boxId = res.type_param;
                switch (res.type)
                {
                    case 2:
                        DrawBoxMgr.singleton.OpenBox(boxId, 1, OnBuyComplete);
                        break;
                    case 3:
                        DrawBoxMgr.singleton.OpenBox(boxId, 10, OnBuyComplete);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnBuyComplete(GoodsRequestResult result, List<DrawCardData> list)
        {
            isClick = false;
            if (result == GoodsRequestResult.Success)
            {
                base.OnBuySuccess();
                RefreshBoxCost();
                RefreshUIData();
                Global.gApp.gUiMgr.OpenPanel(Wndid.BoxOpenUI);
                var openUI = Global.gApp.gUiMgr.GetPanelCompent<BoxOpenUI>(Wndid.BoxOpenUI);
                int boxId = res.type_param;
                openUI.ShowOpenBox(boxId, list);
                if (res.type == 2 && costType == CurrencyType.KEY)
                {
                    openUI.ShowOpenBtn(costType, (int)costPrice, OnBuyBtnClick);
                }
            }
            else if (result == GoodsRequestResult.DataFail_NotEnough)
            {
                if (costType == CurrencyType.DIAMOND)
                    Global.gApp.gUiMgr.OpenPanel(Wndid.NoDiamondUI);
            }
        }

    }
}