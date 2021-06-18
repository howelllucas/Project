
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{

    public partial class ShopUI_ShopItemUI
    {
        //充值表id
        private string m_ShopId;
        private double m_ItemNum;
        private GameObject mEffect;
        private int m_Index;
        public int effct_num { get; private set; }

        private void Awake()
        {
            RegisterListeners();
        }
        private void RegisterListeners()
        {
            BuyBtn.button.onClick.AddListener(OnBuy);
            gameObject.GetComponent<Button>().onClick.AddListener(OnBuy);
        }


        public void UIFresh()
        {
            ShopItem chargeItemConfig = Global.gApp.gGameData.ShopConfig.Get(m_ShopId);
            if (chargeItemConfig.consumeType != SpecialItemIdConstVal.REAL_MONEY)
            {
                ConsumeValue.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(chargeItemConfig.consumeType) < chargeItemConfig.price, null);
            }

        }

        public void Init(ShopItem itemConfig, int i)
        {
            m_Index = i;
            m_ShopId = itemConfig.id;
            effct_num = itemConfig.effectNum;

            Shop_dataItem shop_DataItem = Global.gApp.gGameData.ShopDataConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            if (shop_DataItem != null)
            {
                m_ItemNum = ReflectionUtil.GetValueByProperty<Shop_dataItem, float>("stageGet_" + (i + 1), shop_DataItem);
                m_Valuetxt.text.text = UiTools.FormateMoneyUP(m_ItemNum);
            } else
            {
                m_Valuetxt.text.text = GameConstVal.EmepyStr;
            }
            
            CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemConfig.goodsType));

            ItemIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemConfig.itemIcon);
            
            if (itemConfig.consumeType == SpecialItemIdConstVal.REAL_MONEY)
            {
                ConsumeValue.text.text = "$ " + itemConfig.price.ToString();
                ConsumeIcon.gameObject.SetActive(false);
            }
            else
            {
                ConsumeValue.text.text = "  " + UiTools.FormateMoneyUP(itemConfig.price);
                ConsumeIcon.gameObject.SetActive(true);
                ConsumeIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemConfig.consumeType));

                ConsumeValue.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemConfig.consumeType) < itemConfig.price, null);
            }
            
            Cmbglight.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemConfig.bgLight);

            EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.SHOP_GOLD);
            string location = System.Text.RegularExpressions.Regex.Replace(itemConfig.itemIcon, @"[^0-9]+", GameConstVal.EmepyStr);
            GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, location), transform);
            if (mEffect != null) { GameObject.Destroy(mEffect); }
            mEffect = effect;
            gameObject.SetActive(true);
        }


        private void OnBuy()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.SHOP_MDT_COIN1 + m_Index);
            ShopItem itemConfig = Global.gApp.gGameData.ShopConfig.Get(m_ShopId);

            //扣钱
            bool reduceResult = false;
            if (itemConfig.consumeType == SpecialItemIdConstVal.REAL_MONEY)
            {
                //暂无充值sdk
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 2021);
                return;
            }
            else
            { 
                double curCoin = GameItemFactory.GetInstance().GetItem(itemConfig.consumeType);
                if (curCoin < itemConfig.price || itemConfig.price < 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1002);
                    return;
                }
            }

            Global.gApp.gUiMgr.OpenPanel(Wndid.ConfirmPanel, this);
        }
        public string GetId()
        {
            return m_ShopId;
        }

        public double GetItemNum()
        {
            return m_ItemNum;
        }
    }
}
