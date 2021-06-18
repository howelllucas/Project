
using EZ;
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public partial class TimeGiftUI1
    {
        private string m_IdStr;
        private string m_RemainStr;
        //private SdkdsPurchaseUtils.Product m_Product;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            Btn2.button.onClick.AddListener(OnGoTo);
            BtnC.button.onClick.AddListener(OnCloseBtn);
            RegisterListeners();
            base.ChangeLanguage();

            m_IdStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TIME_GIFT1_PRODUCT).content;
            string[] prms = m_IdStr.Split('.');
            ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(int.Parse(prms[2]));
            NextGunName.text.text = itemCfg.gamename;

            m_RemainStr = Global.gApp.gGameData.GetTipsInCurLanguage(4186);

            double mills = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
            if (mills < 0d)
            {
                mills = 0d;
            }
            DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
            TimeCount.text.text = GetTimtTip(mills);
            dcb.SetAction(() =>
            {
                double ms = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
                if (ms < 0d)
                {
                    ms = 0d;
                }
                TimeCount.text.text = GetTimtTip(ms);
            }, 1f);

            List<ItemDTO> itemDTOs = GameItemFactory.GetInstance().DealItems(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TIME_GIFT1).contents, BehaviorTypeConstVal.OPT_TIME_GIFT1);
            ItemIcon1.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemDTOs[0].itemId));
            ItemCount1.text.text = "X " + (int)itemDTOs[0].num;
            ItemIcon2.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemDTOs[1].itemId));
            ItemCount2.text.text = "X " + (int)itemDTOs[1].num;

            dcb.SetCallTimes(Convert.ToInt32(mills / DateTimeUtil.m_Sec_Mills) + 1);

#if (UNITY_EDITOR || DISBLE_PLATFORM)
#else
            //if (SdkdsPurchaseUtils.m_ProductList!= null && SdkdsPurchaseUtils.m_ProductList.Count > 0)
            //{
            //    foreach (SdkdsPurchaseUtils.Product product in SdkdsPurchaseUtils.m_ProductList) {
            //        if (SdkdsPurchaseUtils.IsSpecialPruchase(product, GeneralConfigConstVal.TIME_GIFT1_PRODUCT))
            //        {
            //            m_Product = product;
            //            break;
            //        }
            //    }
                
            //    moneyNum.text.text = m_Product.price;
            //}
#endif

        }

        private string GetTimtTip(double mls)
        {
            return string.Format(m_RemainStr, Global.gApp.gGameData.GetTimeInCurLanguage(mls));
        }

        private void OnGoTo()
        {
            ////InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_TIME_GIFT1_BUY);
            //if (m_Product != null)
            //{
            //    SdkdsPurchaseUtils.m_StartPos = ItemIcon1.rectTransform.position;
            //    SdkdsPurchaseUtils.Instance.buyProduct(ClientData.m_playerid, "0", m_Product.productId);
            //}
            //else
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "NO PURCHASE SDK");
            }
        }

        public override void TouchClose()
        {
            base.TouchClose();

        }
        private void OnCloseBtn()
        {
            OnClick();
            TouchClose();
                //Global.gApp.gUiMgr.ClosePanel(Wndid.LevelDetail);
        }

        private void UIFresh()
        {
            if (!Global.gApp.gSystemMgr.GetMiscMgr().NotHaveShowTimeGift(GeneralConfigConstVal.TIME_GIFT1_PRODUCT))
            {
                TouchClose();
            }
        }


        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
        }


        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
