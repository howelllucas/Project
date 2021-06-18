using Game;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class GunStarUpShowUI
    {

        private GunCard_TableItem gunCardRes;
        private GunCardData cardData;
        private int maxCount = 0;
        private bool isLock = true;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            gunCardRes = arg as GunCard_TableItem;
            if (gunCardRes != null)
            {
                InitData();
            }

        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            CloseImg.button.onClick.AddListener(OnCloseClick);
            
        }

        void InitData()
        {
            cardData = PlayerDataMgr.singleton.GetGunCardData(gunCardRes.id);
            if (cardData == null)
                return;

            var curStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star - 1);
            if (curStarRes == null)
                return;

            GunName.text.text = LanguageMgr.GetText(gunCardRes.tid_name); ;

            //IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;

            StarTxt.text.text = (curStarRes.star).ToString();

            //StartCoroutine(ChangeValue(countTxt, data.toCount, 0));


            isLock = true;
            Invoke("LvUpData", 1.13f);
        }

        void LvUpData()
        {
            var curStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star);
            if (curStarRes == null)
                return;

            StarTxt.text.text = (curStarRes.star).ToString();

            isLock = false;
        }
        
        void OnCloseClick()
        {
            if (isLock)
                return;

            TouchClose();
        }
    }
}