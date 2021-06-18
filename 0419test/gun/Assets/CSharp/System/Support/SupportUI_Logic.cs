
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
namespace EZ
{

    public partial class SupportUI
    {
        private List<SupportUI_SupportItemUI> m_ShowItemList = new List<SupportUI_SupportItemUI>();
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();
            RegisterListeners();

            MainTxtBtn.button.onClick.AddListener(OnMain);
            SubTxtBtn.button.onClick.AddListener(OnSub);

            base.ChangeLanguage();
        }

        private void InitNode()
        {
            GoldChanged(GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.GOLD));
            DiamondChanged(GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.DIAMOND));


            ShowScrollItem(ItemTypeConstVal.CARRIER);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.DiamondChanged, DiamondChanged);
            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.GoldChanged, GoldChanged);

        }
        
        private void OnMain()
        {
            OnClick();
            if (MainWp.gameObject.activeSelf)
            {
                return;
            }
            MainWp.gameObject.SetActive(true);
            SubWp.gameObject.SetActive(false);

            ClearShowList();
            ShowScrollItem(ItemTypeConstVal.CARRIER);
        }
        private void OnSub()
        {
            OnClick();
            if (SubWp.gameObject.activeSelf)
            {
                return;
            }
            MainWp.gameObject.SetActive(false);
            SubWp.gameObject.SetActive(true);

            ClearShowList();
            ShowScrollItem(ItemTypeConstVal.SPECIAL);
        }

        //刷新ShowScrollItem
        private void ShowScrollItem(int itemType)
        {
            SupportItemUI.gameObject.SetActive(false);
            if (!Global.gApp.gGameData.ItemTypeMapData.ContainsKey(itemType))
            {
                return;
            }

            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[itemType].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[itemType][i];
                SupportUI_SupportItemUI itemUI = SupportItemUI.GetInstance();
                itemUI.Init(itemConfig);
                m_ShowItemList.Add(itemUI);

            }
        }


        private void ClearShowList()
        {
            foreach (SupportUI_SupportItemUI obj in m_ShowItemList)
            {
                SupportItemUI.CacheInstance(obj);
            }
            m_ShowItemList.Clear();
        }


        private void GoldChanged(double val)
        {
            CoinTxt.text.text = UiTools.FormateMoneyUP(val);
        }
        private void DiamondChanged(double val)
        {
            MoneyTxt.text.text = UiTools.FormateMoneyUP(val);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.DiamondChanged, DiamondChanged);
            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.GoldChanged, GoldChanged);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
