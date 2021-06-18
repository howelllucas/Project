
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{

    public partial class ConfirmUI
    {
        private Vector3 mFxPos = Vector3.zero;

        //充值表id
        private string m_ShopId;
        private int mEffectNum = 0;
        private double m_ItemNum;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListeners();
            ShopUI_ShopItemUI scrShopItem = arg as ShopUI_ShopItemUI;
            m_ShopId = scrShopItem.GetId();
            mEffectNum = scrShopItem.effct_num;
            m_ItemNum = scrShopItem.GetItemNum();
            mFxPos = scrShopItem.gameObject.transform.position;
            InitNode();

            base.ChangeLanguage();

        }
        

        private void InitNode()
        {
            ShopItem chargeItemConfig = Global.gApp.gGameData.ShopConfig.Get(m_ShopId);

            Itemicon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(chargeItemConfig.itemIcon);
            if (chargeItemConfig.consumeType == SpecialItemIdConstVal.REAL_MONEY)
            {
                Moneycostbtn.text.text = "$ " + chargeItemConfig.price.ToString();
                Moneyiconbtn.gameObject.SetActive(false);
            }
            else
            {
                Moneycostbtn.text.text = UiTools.FormateMoneyUP(chargeItemConfig.price);
                Moneyiconbtn.gameObject.SetActive(true);
                Moneyiconbtn.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, chargeItemConfig.consumeType));

            }

            Itemvaluetxt.text.text = UiTools.FormateMoneyUP(m_ItemNum);
            Itemvalueicon.gameObject.SetActive(true);
            Itemvalueicon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, chargeItemConfig.goodsType));


            ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get(chargeItemConfig.goodsType);
            Confirmtxt3.text.text = itemConfig.gamename;

            Btn1.button.onClick.AddListener(TouchClose);
            Btn2.button.onClick.AddListener(Buy);

        }

        private void RegisterListeners()
        {
        }
        

        //装备武器操作
        public void Buy()
        {
            ShopItem itemConfig = Global.gApp.gGameData.ShopConfig.Get(m_ShopId);
            //扣钱
            bool reduceResult = false;
            int behaviorType = 0;
            if (itemConfig.consumeType == SpecialItemIdConstVal.REAL_MONEY)
            {
                behaviorType = BehaviorTypeConstVal.OPT_CHARGE;
                //暂无充值sdk
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 2021);
                return;
            }
            else
            {
                behaviorType = BehaviorTypeConstVal.OPT_DIAMOND_EXCHANGE_COIN;
                ItemDTO reduceItemDTO = new ItemDTO(itemConfig.consumeType, itemConfig.price, behaviorType);
                reduceItemDTO.paramStr1 = m_ShopId;
                GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
                reduceResult = reduceItemDTO.result;
            }

            if (reduceResult) {
                //发货
                //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1f);
                ItemDTO addItemDTO = new ItemDTO((int)itemConfig.goodsType, m_ItemNum, behaviorType);
                addItemDTO.paramStr1 = m_ShopId;
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, itemConfig.goodsType, mEffectNum, mFxPos);
                GameItemFactory.GetInstance().AddItem(addItemDTO);

                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1002);
            }

            TouchClose();
        }


        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
