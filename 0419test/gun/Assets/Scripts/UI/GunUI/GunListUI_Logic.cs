using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;

namespace EZ
{
    public enum GunListOpt
    {
        ReplaceUseGun = 1,
        ReplaceFuseGun = 2,
        AddFuseGun = 3,
    }

    public partial class GunListUI
    {
        private GunCard_TableItem optCardRes;
        private List<GunCardData> cardDataList = new List<GunCardData>();
        private List<GunListUI_GunCard> showGunList = new List<GunListUI_GunCard>();
        private GunListOpt opt;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            optCardRes = arg as GunCard_TableItem;
            if (optCardRes == null)
            {
                opt = GunListOpt.AddFuseGun;
            }
            else if (optCardRes.id == PlayerDataMgr.singleton.GetUseWeaponID())
            {
                opt = GunListOpt.ReplaceUseGun;
            }
            else if (PlayerDataMgr.singleton.GetFusedCardList().Contains(optCardRes.id))
            {
                opt = GunListOpt.ReplaceFuseGun;
            }
            Debug.Log("opt " + opt);


            ShowGunCard();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            cardDataList.Clear();
            foreach (var data in PlayerDataMgr.singleton.DB.cardDatas)
            {
                cardDataList.Add(data.Value);
            }

            SortCards();

            CloseBtn.button.onClick.AddListener(TouchClose);
            GotoBtn.button.onClick.AddListener(OnGotoClick);
        }


        public override void Recycle()
        {
            base.Recycle();

        }

        private void ShowGunCard()
        {

            ClearShowList();

            foreach (var item in cardDataList)
            {
                if (PlayerDataMgr.singleton.GetUseWeaponID() == item.cardID)
                    continue;

                if (opt == GunListOpt.ReplaceFuseGun || opt == GunListOpt.AddFuseGun)
                {
                    if (PlayerDataMgr.singleton.DB.fusedCardList.Contains(item.cardID))
                        continue;
                }

                var res = TableMgr.singleton.GunCardTable.GetItemByID(item.cardID);
                if (res == null)
                    continue;

                var itemUI = GunCard.GetInstance();
                itemUI.Init(res);
                itemUI.transform.SetParent(WeaponContent.gameObject.transform);
                itemUI.OnCardClick += OnCardClick;
                itemUI.transform.SetAsLastSibling();
                showGunList.Add(itemUI);

            }

            if (showGunList.Count <= 0)
            {
                GotoBtn.gameObject.SetActive(true);
            }
            else
            {
                GotoBtn.gameObject.SetActive(false);
            }

            if (opt == GunListOpt.ReplaceUseGun || opt == GunListOpt.ReplaceFuseGun)
            {
                var itemUI = GunCard.GunCard;
                itemUI.Init(optCardRes);
                //itemUI.transform.SetParent(WeaponContent.gameObject.transform);
                //itemUI.OnCardClick += OnCardClick;
                //showGunList.Add(itemUI);
                GunCard.gameObject.SetActive(true);
                EmptyTxt.gameObject.SetActive(false);
                CurImage.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(TableMgr.singleton.ValueTable.GetCardQualityEquipFrame(optCardRes.rarity));
                CurImage.gameObject.SetActive(true);
            }
            else
            {
                GunCard.gameObject.SetActive(false);
                EmptyTxt.gameObject.SetActive(true);
                CurImage.gameObject.SetActive(false);
            }
        }

        private void ClearShowList()
        {
            foreach (var obj in showGunList)
            {
                GunCard.CacheInstance(obj);
            }
            showGunList.Clear();

        }

        void SortCards()
        {
            cardDataList.Sort((x, y) =>
            {
                if (x.DPS() < y.DPS())
                    return 1;
                else if (x.DPS() > y.DPS())
                    return -1;
                return (y.cardID).CompareTo((x.cardID));
            });
        }
        private void OnCardClick(int cardID)
        {
            switch (opt)
            {
                case GunListOpt.ReplaceUseGun:
                    {
                        PlayerDataMgr.singleton.SetUseWeaponID(cardID);
                        TouchClose();
                    }
                    break;
                case GunListOpt.ReplaceFuseGun:
                    {
                        PlayerDataMgr.singleton.ReplaceFuseCard(optCardRes.id, cardID);
                        TouchClose();
                    }
                    break;
                case GunListOpt.AddFuseGun:
                    {
                        PlayerDataMgr.singleton.AddFuseCard(cardID);
                        TouchClose();
                    }
                    break;
            }
        }

        void OnGotoClick()
        {
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CommonUIIndexChange4Param, 3, "box");
            TouchClose();
        }
    }
}