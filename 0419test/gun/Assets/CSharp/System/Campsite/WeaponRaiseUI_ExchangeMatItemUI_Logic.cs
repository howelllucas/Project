using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;

namespace EZ
{
    public partial class WeaponRaiseUI_ExchangeMatItemUI
    {
        private CampShopItem m_CampShopItem;
        private MatExchangeNode m_Parent;

        private void Awake()
        {
            InitNode();
        }

        public void Init(CampShopItem itemConfig, MatExchangeNode parent)
        {
            m_CampShopItem = itemConfig;
            gameObject.SetActive(true);
            m_Parent = parent;
            UIFresh();
        }
        private void InitNode()
        {
            MatItemBtn.button.onClick.AddListener(OnExChangeHeart);
        }
        public void UIFresh()
        {
            ItemItem itemItem = Global.gApp.gGameData.ItemData.Get(m_CampShopItem.propId);

            //武器需要根据当前情况处理
            if (itemItem != null && GameItemFactory.GetInstance().GetItem(m_CampShopItem.propId) > 0)
            {
                Global.gApp.gSystemMgr.GetNpcMgr().CampShopTimesMap[m_CampShopItem.id.ToString()] = 1;
                Global.gApp.gSystemMgr.GetNpcMgr().SaveData();
            }

            MatIcon.image.sprite = Resources.Load(itemItem.image_grow, typeof(Sprite)) as Sprite;
            double addNum = m_CampShopItem.propNum;
            if (m_CampShopItem.propId == SpecialItemIdConstVal.GOLD)
            {
                Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                addNum *= gpiCfg.coinParams;
            }
            m_ExchangeCount.text.text = "X " + UiTools.FormateMoneyUP(addNum); 
            HeartCount.text.text = m_CampShopItem.heartNum.ToString();
            HeartCount.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.RED_HEART) < m_CampShopItem.heartNum, "#FDE47EFF");

            bool limit = Global.gApp.gSystemMgr.GetNpcMgr().CampShopTimesMap[m_CampShopItem.id.ToString()] >= m_CampShopItem.limitButTimes;
            ExchangeNode.gameObject.SetActive(!limit);
            SellOut.gameObject.SetActive(limit);
            MatIcon.image.color = limit ? ColorUtil.GetColor(ColorUtil.m_BlackColor) : ColorUtil.GetColor(ColorUtil.m_DeaultColor);

            MatName.text.text = itemItem.gamename;
            ExchangeCount.gameObject.SetActive(false);
        }

        public CampShopItem GetCampItem()
        {
            return m_CampShopItem;
        }
        private void OnExChangeHeart()
        {
            bool limit = Global.gApp.gSystemMgr.GetNpcMgr().CampShopTimesMap[m_CampShopItem.id.ToString()] >= m_CampShopItem.limitButTimes;

            if (limit)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4255);
                return;
            }
            bool enough = GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.RED_HEART) >= m_CampShopItem.heartNum;
            if (!enough)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4262);
                return;
            }
            Global.gApp.gUiMgr.OpenPanel(Wndid.ExchangeConfirmUI,this);
       }
        public void ExchangeHeartImp()
        {
            bool limit = Global.gApp.gSystemMgr.GetNpcMgr().CampShopTimesMap[m_CampShopItem.id.ToString()] >= m_CampShopItem.limitButTimes;
            
            if (limit)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4255);
                return;
            }
            bool enough = GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.RED_HEART) >= m_CampShopItem.heartNum;
            if (!enough)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4262);
                return;
            }

            ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get(SpecialItemIdConstVal.RED_HEART);
            ItemDTO reduceItemDTO = new ItemDTO(SpecialItemIdConstVal.RED_HEART, m_CampShopItem.heartNum, BehaviorTypeConstVal.OPT_EXCHANGE_REDUCE_HEART);
            reduceItemDTO.paramStr1 = m_CampShopItem.propId.ToString();
            GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);

            ItemItem addItemConfig = Global.gApp.gGameData.ItemData.Get(m_CampShopItem.propId);
            double addNum = m_CampShopItem.propNum;
            if (m_CampShopItem.propId == SpecialItemIdConstVal.GOLD)
            {
                Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                addNum *= gpiCfg.coinParams;
            }
            if (ItemTypeConstVal.isWeapon(addItemConfig.showtype))
            {
                Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(addItemConfig.name, WeaponStateConstVal.NEW);
            }

            ItemDTO addItemDTO = new ItemDTO(m_CampShopItem.propId, addNum, BehaviorTypeConstVal.OPT_EXCHANGE_ADD_ITEM);
            GameItemFactory.GetInstance().AddItem(addItemDTO);
            Global.gApp.gSystemMgr.GetNpcMgr().CampShopTimesMap[m_CampShopItem.id.ToString()]++;
            Global.gApp.gSystemMgr.GetNpcMgr().SaveData();
            m_Parent.FresAllItem();

            Global.gApp.gUiMgr.OpenPanel(Wndid.ExchangeSuccessUI, this);
        }

        public void Recycle()
        {
        }
    }
}