using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;
using System;

namespace EZ
{

    public partial class GunChipUI_GunChip
    {
        public Action<int,int> OnCardClick = null;

        private int cardID = 0;
        private int cardCount = 0;
        private int addCount = 0;
        GunCard_TableItem gunCardRes;

        private void Awake()
        {
            InitNode();
        }
        public void Init(int id, int count)
        {
            cardID = id;
            cardCount = count;

            SetCount(0);

            gunCardRes = TableMgr.singleton.GunCardTable.GetItemByID(cardID);
            if (gunCardRes == null)
                return;

            TipsTitle.text.text = LanguageMgr.GetText(gunCardRes.tid_name);
            IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;
            Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(gunCardRes.rarity),
                                                typeof(Sprite)) as Sprite;
        }

        private void InitNode()
        {
            IconBtn.button.onClick.AddListener(OnIconClick);
            SubBtn.button.onClick.AddListener(OnSubClick);
        }

        public void SetCount(int count)
        {
            addCount = count;
            ChipCount.text.text = string.Format("{0}/{1}", count, cardCount);
            if (count > 0)
                SubBtn.gameObject.SetActive(true);
            else
                SubBtn.gameObject.SetActive(false);
        }

        private void OnIconClick()
        {
            if (addCount >= cardCount)
                return;

            if (OnCardClick != null)
            {
                OnCardClick(cardID, 10);
            }

        }

        private void OnSubClick()
        {
            if (addCount <= 0)
                return;

            if (OnCardClick != null)
            {
                OnCardClick(cardID, -10);
            }
                
        }
    }
}