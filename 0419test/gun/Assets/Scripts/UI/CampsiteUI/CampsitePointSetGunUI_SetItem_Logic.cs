using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using System;

namespace EZ
{
    public partial class CampsitePointSetGunUI_SetItem
    {
        public event Action<int> onSelect;
        private int cardId;

        private void Awake()
        {
            SelectBtn.button.onClick.AddListener(OnSelectBtnClick);
        }

        public void Init(CampsitePointMgr targetPoint, int cardId)
        {
            this.cardId = cardId;
            CardData.CampsiteCardData.Init(targetPoint, cardId);
            int occupiedPointIndex;
            //bool hasOccupied = CampsiteMgr.singleton.CheckCardIsOccupied(cardId, out occupiedPointIndex);
            //OccupiedFlag.gameObject.SetActive(hasOccupied);
        }

        private void OnSelectBtnClick()
        {
            onSelect?.Invoke(cardId);
        }
    }
}