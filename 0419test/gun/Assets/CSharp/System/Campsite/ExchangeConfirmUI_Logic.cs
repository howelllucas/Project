using EZ.Data;
using UnityEngine;

namespace EZ
{
    public partial class ExchangeConfirmUI
    {
        WeaponRaiseUI_ExchangeMatItemUI m_ItemUI;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            m_ItemUI = arg as WeaponRaiseUI_ExchangeMatItemUI;
            base.Init(name, info, arg);
            InitNode();
            base.ChangeLanguage();
        }
        private void InitNode()
        {
            CampShopItem campShopItem = m_ItemUI.GetCampItem();
            CloseBtn.button.onClick.AddListener(TouchClose);
            ConfirmBtn.button.onClick.AddListener(OnConfirmExchange);
            double addNum = campShopItem.propNum;
            if (campShopItem.propId == SpecialItemIdConstVal.GOLD)
            {
                Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                addNum *= gpiCfg.coinParams;
            }
            ExchangeCount.text.text = "x" + UiTools.FormateMoneyUP(addNum);
            ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(campShopItem.propId);
            MatIcon.image.sprite = Resources.Load(itemItem.image_grow, typeof(Sprite)) as Sprite;
            MatName.text.text = itemItem.gamename;
            HeartCount.text.text = campShopItem.heartNum.ToString();
        }
        private void OnConfirmExchange()
        {
            m_ItemUI.ExchangeHeartImp();
            TouchClose();
        } 
    }
}
