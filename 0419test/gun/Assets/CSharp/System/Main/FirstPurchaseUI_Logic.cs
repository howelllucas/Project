
using EZ;
using EZ.Data;
using EZ.Util;
using UnityEngine;
namespace EZ
{
    public partial class FirstPurchaseUI
    {
        //private SdkdsPurchaseUtils.Product m_Product;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            Btn2.button.onClick.AddListener(OnGoTo);
            BtnC.button.onClick.AddListener(TouchClose);

            string idStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.FIRST_PURCHASE_REWARD).contents[0];
            ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(int.Parse(idStr));
            NextGunName.text.text = itemCfg.gamename;
#if (UNITY_EDITOR || DISBLE_PLATFORM)
#else
            //if (SdkdsPurchaseUtils.m_ProductList!= null && SdkdsPurchaseUtils.m_ProductList.Count > 0)
            //{
            //    foreach (SdkdsPurchaseUtils.Product product in SdkdsPurchaseUtils.m_ProductList) {
            //        if (product.p_type.Equals(SdkdsPurchaseUtils.P_MDT))
            //        {
            //            m_Product = product;
            //            break;
            //        }
            //    }
            //    string[] prms = m_Product.productId.Split('.');
            //    int m_ItemNum = int.Parse(prms[prms.Length - 1]);
            //    int m_ItemId = int.Parse(prms[prms.Length - 2]);
            //    ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get(m_ItemId);

            //    CmIcon.gameObject.SetActive(true);
            //    CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ItemId));
            //    CmNum.text.text = UiTools.FormateMoneyUP(m_ItemNum);
            //    moneyNum.text.text = m_Product.price;
            //}
#endif



            base.ChangeLanguage();
            RegisterListeners();

        }

        private void OnGoTo()
        {
            //Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.CommonUIIndexChange4Param, 0, "m_PurchaseGun");
            //Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "NO PURCHASE SDK");
            ////InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_FIRST_PURCHASE_BUY);
            //if (m_Product != null)
            //{
            //    SdkdsPurchaseUtils.m_StartPos = CmIcon.rectTransform.position;
            //    SdkdsPurchaseUtils.Instance.buyProduct(ClientData.m_playerid, "0", m_Product.productId);
            //} else
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, "NO PURCHASE SDK");
            }
            
            //OnCloseBtn();
        }

        private void OnCloseBtn()
        {
            OnClick();
            TouchClose();
            //Global.gApp.gUiMgr.ClosePanel(Wndid.LevelDetail);
        }
        private void UIFresh()
        {
            if (Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase == 1)
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
