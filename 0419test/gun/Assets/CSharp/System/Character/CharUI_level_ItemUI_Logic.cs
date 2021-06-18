
//using EZ.Data;
//using EZ.DataMgr;
//using UnityEngine;
//namespace EZ
//{

//    public partial class CharUI_level_ItemUI
//    {
//        //充值表id
//        private string m_ShopId;
//        private static string m_DefaultColor = "#FFFFFF";
//        private static string m_LackMoneyColor = "#B4B4B4";
//        private static string m_DefaultMoneyTextColor = "#F8E082";
//        private static string m_LackMoneyTextColor = "#FE4648";

//        private void Awake()
//        {
//            RegisterListeners();
//        }
//        private void RegisterListeners()
//        {
//            Itembtn.button.onClick.AddListener(OnBuy);
//        }

//        public void UIFresh()
//        {
//            ChargeItem chargeItemConfig = Global.gApp.gGameData.ChargeData.Get(m_ShopId);
//            if (chargeItemConfig.consumeType != SpecialItemIdConstVal.REAL_MONEY)
//            {
//                CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(chargeItemConfig.consumeType) < chargeItemConfig.price, null);
//            }
//        }

//        public void Init(ChargeItem itemConfig)
//        {
//            m_ShopId = itemConfig.id;

//            TipsItem tipsItem = Global.gApp.gGameData.TipsData.Get(itemConfig.desc);
//            if (tipsItem != null)
//            {
//                ItemNameTxt.text.text = tipsItem.txtcontent;
//            }
//            else
//            {
//                ItemNameTxt.text.text = string.Format(TipsFormatConstValpublic.NO_TIPS_CONFIG_FOMAT, itemConfig.desc);
//            }

//            ItemExpTxt.text.text = "EXP +"+itemConfig.goodsNum.ToString();

//            ItemIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemConfig.itemIcon);

//            CmNum.text.text = itemConfig.price.ToString();
//            CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemConfig.consumeType));

//            CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemConfig.consumeType) < itemConfig.price, null);
//            gameObject.SetActive(true);

//            ResetColor();
//        }


//        private void OnBuy()
//        {
//            ChargeItem itemConfig = Global.gApp.gGameData.ChargeData.Get(m_ShopId);

//            //扣钱
//            bool reduceResult = false;
//            if (itemConfig.consumeType == SpecialItemIdConstVal.REAL_MONEY)
//            {
//                //暂无充值sdk
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 2021);
//                return;
//            }
//            else
//            {
//                ItemDTO reduceItemDTO = new ItemDTO(itemConfig.consumeType, itemConfig.price, BehaviorTypeConstVal.OPT_CHAR_LEVEL_UP);
//                reduceItemDTO.paramStr1 = m_ShopId;
//                GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
//                reduceResult = reduceItemDTO.result;
//            }

//            //发货
//            if (reduceResult)
//            {
//                ItemDTO addItemDTO = new ItemDTO((int)itemConfig.goodsType, itemConfig.goodsNum, BehaviorTypeConstVal.OPT_CHAR_LEVEL_UP);
//                addItemDTO.paramStr1 = m_ShopId;
//                int oldLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//                GameItemFactory.GetInstance().AddItem(addItemDTO);
//                int newLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//                if (oldLevel != newLevel)
//                {
//                    EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.Big_LEVEL_UP);
//                    GameObject effect = UiTools.GetEffect(effectItem.path, transform.parent, new Vector3(0, 0, 5));
//                    GameObject.Destroy(effect, 3f);
//                } else
//                {
//                    EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.SMALL_LEVEL_IP);
//                    GameObject effect = UiTools.GetEffect(effectItem.path, transform.parent, new Vector3(0, 0, 5));
//                    GameObject.Destroy(effect, 3f);
//                }

                
//            }
//            else
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
//                return;
//            }
//        }
//        public string GetId()
//        {
//            return m_ShopId;
//        }


//        public void ResetColor()
//        {
//            ChargeItem itemConfig = Global.gApp.gGameData.ChargeData.Get(m_ShopId);
//            if (itemConfig.consumeType != SpecialItemIdConstVal.REAL_MONEY)
//            {
//                double curVal = GameItemFactory.GetInstance().GetItem(itemConfig.consumeType);
//                if (curVal < itemConfig.price)
//                {
//                    Itembtn.image.color = ColorUtil.GetColor(m_LackMoneyColor);
//                    CmNum.text.color = ColorUtil.GetColor(m_LackMoneyTextColor);
//                    return;
//                }
//            }
//            Itembtn.image.color = ColorUtil.GetColor(m_DefaultColor);
//            CmNum.text.color = ColorUtil.GetColor(m_DefaultMoneyTextColor);
//        }
//    }
//}
