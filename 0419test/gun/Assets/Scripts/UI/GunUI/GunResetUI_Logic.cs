using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;
using System;
using System.Numerics;

namespace EZ
{

    public partial class GunResetUI
    {
        private GunCard_TableItem optCardRes;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            optCardRes = arg as GunCard_TableItem;
            if (optCardRes == null)
                return;

            var cardData = PlayerDataMgr.singleton.GetGunCardData(optCardRes.id);
            if (cardData == null)
                return;

            ResetCost.text.text = TableMgr.singleton.ValueTable.card_reset_cost.ToString();

            BigInteger gold = 0;
            for (int i = 2; i <= cardData.level; ++i)
            {
                gold += PlayerDataMgr.singleton.GetCardLvUpCost(i);
            }

            GetGold.text.text = gold.ToSymbolString();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            CloseBtn.button.onClick.AddListener(TouchClose);
            OkBtn.button.onClick.AddListener(OnResetClick);
        }

        private void OnResetClick()
        {
            PlayerDataMgr.singleton.CardLvReset(optCardRes.id);
            TouchClose();
        }
    }
}