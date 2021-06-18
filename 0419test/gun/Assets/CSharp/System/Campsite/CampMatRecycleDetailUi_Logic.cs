using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{
    public partial class CampMatRecycleDetailUi
    {
        CampMatBag_MatItemUI m_MatItemUI;
        CampRecycleItem m_CampRecycleItem;
        private double m_MaxCount;
        private double m_RealMaxCount;
        private double m_CurSellCount;
        double m_GoldParam;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_MatItemUI = arg as CampMatBag_MatItemUI;
            m_CampRecycleItem = m_MatItemUI.GetRecycleItem(); ;
            Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            m_GoldParam = gpiCfg.coinParams;
    
            double count = GameItemFactory.GetInstance().GetItem(m_CampRecycleItem.id);
            m_RealMaxCount = count;
            m_MaxCount = (double)((int)(m_RealMaxCount + 0.5d));
            m_CurSellCount = m_MaxCount;
            InitNode();
            base.ChangeLanguage();
        }
        private void InitNode()
        {
            ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(m_CampRecycleItem.id);
            MatName.text.text = itemItem.name;
            MatIcon.image.sprite = Resources.Load(itemItem.image_grow, typeof(Sprite)) as Sprite;
           
            MatPrice.text.text = UiTools.FormateMoneyUP(m_CampRecycleItem.price * m_GoldParam);

            MatDes.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(m_CampRecycleItem.dec); ;

            FreshSellInfo();
            Reduce10.button.onClick.AddListener(ReduceTen);
            Reduce1.button.onClick.AddListener(ReduceOne);
            Add1.button.onClick.AddListener(AddOne);
            Max.button.onClick.AddListener(OnMax);
            ConfirmBtn.button.onClick.AddListener(OnConfirm);

            CloseBtn.button.onClick.AddListener(TouchClose);
        }
        public override void TouchClose()
        {
            m_MatItemUI.FreshUi();
            base.TouchClose();
        }
        private void OnConfirm()
        {
            if (m_CurSellCount > 0)
            {
                double realSellCount = m_CurSellCount;
                if (realSellCount >= m_MaxCount)
                {
                    realSellCount = m_RealMaxCount;
                }
                ItemDTO reduceItemDTO = new ItemDTO(m_CampRecycleItem.id, realSellCount, BehaviorTypeConstVal.OPT_EXCHANGE_REDUCE_ITEM);
                GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);

                double addGoldCount = m_CurSellCount * m_CampRecycleItem.price * m_GoldParam;
                ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, addGoldCount, BehaviorTypeConstVal.OPT_EXCHANGE_ADD_COIN);
                GameItemFactory.GetInstance().AddItem(addItemDTO);
                Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.GOLD, 20, m_ConfirmBtn.rectTransform.position);
                m_MaxCount -= m_CurSellCount;
                m_RealMaxCount -= m_CurSellCount;
                if(m_RealMaxCount < 0)
                {
                    m_RealMaxCount = 0;
                }
                m_MaxCount = (double)((int)(m_RealMaxCount + 0.5d));
                m_CurSellCount = m_MaxCount;
                FreshSellInfo();
                TouchClose();
            }
        }
        private void FreshSellInfo()
        {
            m_CountText.text.text = m_CurSellCount + "/" + m_MaxCount;
            m_GoldCount.text.text = UiTools.FormateMoneyUP(m_CurSellCount * m_CampRecycleItem.price * m_GoldParam);  
        }
        private void OnMax()
        {
            m_CurSellCount = m_MaxCount;
            FreshSellInfo();
        }
        private void AddOne()
        {
            m_CurSellCount++;
            if(m_CurSellCount > m_MaxCount)
            {
                m_CurSellCount = m_MaxCount;
            }
            FreshSellInfo();
        }
        private void ReduceTen()
        {
            m_CurSellCount -= 10;
            if (m_CurSellCount < 0)
            {
                m_CurSellCount = 0;
            }
            FreshSellInfo();
        }
        private void ReduceOne()
        {
            m_CurSellCount--;
            if (m_CurSellCount < 0)
            {
                m_CurSellCount = 0;
            }
            FreshSellInfo();
        }
    }
}
