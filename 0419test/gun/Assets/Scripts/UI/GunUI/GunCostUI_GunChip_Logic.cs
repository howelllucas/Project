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

    public partial class GunCostUI_GunChip
    {
        GunCard_TableItem gunCardRes;

        private void Awake()
        {

        }
        public void Init(int id)
        {
            gunCardRes = TableMgr.singleton.GunCardTable.GetItemByID(id);
            if (gunCardRes == null)
                return;

            TipsTitle.text.text = LanguageMgr.GetText(gunCardRes.tid_name);
            IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;
            Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(gunCardRes.rarity),
                                                typeof(Sprite)) as Sprite;
        }


        public void SetCount(int cost, int all)
        {
            ChipCount.text.text = string.Format("{0}/{1}", cost, all);
        }
      
    }
}