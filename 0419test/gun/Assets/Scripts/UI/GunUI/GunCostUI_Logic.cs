using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace EZ
{

    public partial class GunCostUI
    {
        private List<CardChipInfo> cardChipList = new List<CardChipInfo>();

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            cardChipList = arg as List<CardChipInfo>;

            ShowCardChip();

        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            CloseBtn.button.onClick.AddListener(TouchClose);
            ExchangeBtn.button.onClick.AddListener(OnExchangeCard);
        }


        public override void Recycle()
        {
            base.Recycle();

        }

        private void ShowCardChip()
        {
            foreach(var info in cardChipList)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(info.cardData.cardID);
                if (res == null)
                    continue;

                if (info.addCount <= 0)
                    continue;

                var chipUI = GunChip.GetInstance();
                chipUI.Init(info.cardData.cardID);
                chipUI.SetCount(info.addCount, info.cardData.count);
                chipUI.transform.SetParent(WeaponContent.gameObject.transform);
                chipUI.transform.SetAsLastSibling();
                chipUI.gameObject.SetActive(true);
            }

        }

        private void OnExchangeCard()
        {
            foreach (var info in cardChipList)
            {
                if (info.addCount <= 0)
                    continue;

                if (!PlayerDataMgr.singleton.CostCardChip(info.cardData.cardID, info.addCount))
                    return;
            }

            var cardID = DrawBoxMgr.singleton.GetCardByQuality((int)CardQualityType.LEGEND);

            PlayerDataMgr.singleton.AddCard(cardID);

            cardChipList.Clear();

            TouchClose();
        }
        
    }
}