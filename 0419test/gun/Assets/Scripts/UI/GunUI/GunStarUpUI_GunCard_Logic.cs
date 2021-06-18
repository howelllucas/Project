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

    public partial class GunStarUpUI_GunCard
    {
        public Action<int,int,int> OnCardClick = null;

        private int index = 0;
        private int chipType = 0;
        private int chipRarity = 0;

        private void Awake()
        {
            InitNode();
        }
        public void Init(int idx, int type, int rarity)
        {
            index = idx;
            chipType = type;
            chipRarity = rarity;
        }

        private void InitNode()
        {
            IconBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            if (OnCardClick != null)
                OnCardClick(index, chipType, chipRarity);
        }
    }
}