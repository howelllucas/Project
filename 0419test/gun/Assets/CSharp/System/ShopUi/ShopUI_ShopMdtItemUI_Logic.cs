
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{

    public partial class ShopUI_ShopMdtItemUI
    {
        //充值表id
        private int m_ItemId = SpecialItemIdConstVal.MDT;
        private int m_ItemNum;
        private GameObject mEffect;
        private int m_Index;
        //private SdkdsPurchaseUtils.Product m_Product;

        public int effct_num { get; private set; }

        private void Awake()
        {
            RegisterListeners();
        }
        private void RegisterListeners()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnBuy);
        }


        public void UIFresh()
        {
           

        }

        //public void Init(SdkdsPurchaseUtils.Product product, int location)
        //{
        //    m_Index = location;
        //    ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get(m_ItemId);
        //    m_Product = product;
        //    m_ItemNum = int.Parse(product.productId.Substring(product.productId.LastIndexOf(".") + 1));
        //    effct_num = m_ItemNum;


        //    m_Valuetxt.text.text = UiTools.FormateMoneyUP(m_ItemNum);

        //    ConsumeIcon.gameObject.SetActive(false);
        //    CmIcon.gameObject.SetActive(true);
        //    CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ItemId));
        //    ItemIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.PURCHASE_ICON_RESOURCE_PATH, location));

        //    ConsumeValue.text.text = m_Product.price;

        //    Cmbglight.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>("UI/Icon/Shop/goldlight");

        //    EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.SHOP_MDT);
        //    GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, location), transform);
        //    if (mEffect != null) { GameObject.Destroy(mEffect); }
        //    mEffect = effect;
        //    gameObject.SetActive(true);
        //}


        private void OnBuy()
        {
            ////InfoCLogUtil.instance.SendClickLog(ClickEnum.SHOP_MDT_PURCHASE1 - 1 + m_Index);
            //SdkdsPurchaseUtils.m_StartPos = ItemIcon.rectTransform.position;
            //SdkdsPurchaseUtils.Instance.buyProduct(ClientData.m_playerid, "0", m_Product.productId);
        }

        public double GetItemNum()
        {
            return m_ItemNum;
        }
    }
}
