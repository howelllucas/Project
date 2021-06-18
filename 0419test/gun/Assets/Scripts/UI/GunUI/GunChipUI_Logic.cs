using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace EZ
{
    public class CardChipInfo
    {
        public GunChipUI_GunChip cardItem;
        public GunCardData cardData;
        public int addCount;
    }

    public partial class GunChipUI
    {
        //GunCard_TableItem gunCardRes;
        //private Dictionary<int, GunChipUI_GunChip> gunChipDic = new Dictionary<int, GunChipUI_GunChip>();
        //private Dictionary<int, int> chipDic = new Dictionary<int, int>();
        private List<CardChipInfo> cardChipList = new List<CardChipInfo>();
        private int chipProgress = 0;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            ShowCardChip();

            passProgress.image.fillAmount = 0;
            ChipCount.text.text = string.Format("{0}/{1}", 0, 100);

            ExchangeBtn.gameObject.SetActive(false);
            GotoBtn.gameObject.SetActive(true);
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            CloseBtn.button.onClick.AddListener(TouchClose);
            ChooseBtn.button.onClick.AddListener(OnChooseChip);
            ExchangeBtn.button.onClick.AddListener(OnExchangeCard);
            GotoBtn.button.onClick.AddListener(OnGoTo);
        }


        public override void Recycle()
        {
            base.Recycle();

        }

        private void ShowCardChip()
        {
            //foreach (var obj in gunChipDic.Values)
            //{
            //    GunChip.CacheInstance(obj);
            //}
            cardChipList.Clear();

            for (var q = 1; q < (int)CardQualityType.LEGEND; ++q)
            {
                foreach (var card in PlayerDataMgr.singleton.DB.cardDatas)
                {
                    if (card.Value.count <= 0)
                        continue;

                    var res = TableMgr.singleton.GunCardTable.GetItemByID(card.Key);
                    if (res == null)
                        continue;

                    if (res.rarity != q)
                        continue;

                    var info = new CardChipInfo();
                    info.cardData = card.Value;
                    info.addCount = 0;

                    cardChipList.Add(info);

                }
            }
            //CardsRoot.gameObject.SetActive(true);


            foreach(var info in cardChipList)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(info.cardData.cardID);
                if (res == null)
                    continue;

                var chipUI = GunChip.GetInstance();
                chipUI.Init(info.cardData.cardID, info.cardData.count);
                chipUI.transform.SetParent(WeaponContent.gameObject.transform);
                chipUI.transform.SetAsLastSibling();
                chipUI.gameObject.SetActive(true);
                chipUI.OnCardClick = AddCradChip;

                chipUI.IconBtn.image.sprite = Resources.Load(res.icon, typeof(Sprite)) as Sprite;
                chipUI.Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(res.rarity),
                                                    typeof(Sprite)) as Sprite;

                info.cardItem = chipUI;
            }
            

            UpdateChipProgress();
        }

        private void AddCradChip(int id, int count)
        {
            if (chipProgress >= 100 && count > 0)
                return;

            if (chipProgress <= 0 && count < 0)
                return;

            if (count == 0)
                return;

            for (int i = 0; i < cardChipList.Count; ++i)
            {
                if (cardChipList[i].cardData.cardID == id)
                {
                    cardChipList[i].addCount += count;
                    cardChipList[i].cardItem.SetCount(cardChipList[i].addCount);
                    break;
                }
            }

            UpdateChipProgress();

        }

        private void UpdateChipProgress()
        {
            chipProgress = 0;
            foreach (var info in cardChipList)
            {
                if (info.addCount <= 0)
                    continue;

                var res = TableMgr.singleton.GunCardTable.GetItemByID(info.cardData.cardID);
                if (res == null)
                    continue;

                if (res.rarity == (int)CardQualityType.RARE)
                {
                    chipProgress += info.addCount / 10;
                }
                else if (res.rarity == (int)CardQualityType.EPIC)
                {
                    chipProgress += info.addCount;
                }
            }

            passProgress.image.fillAmount = (float)chipProgress / 100;
            ChipCount.text.text = string.Format("{0}/{1}", chipProgress, 100);

            if (chipProgress >= 100)
            {
                ExchangeBtn.gameObject.SetActive(true);
                GotoBtn.gameObject.SetActive(false);
            }
            else
            {
                ExchangeBtn.gameObject.SetActive(false);
                GotoBtn.gameObject.SetActive(true);
            }
        }

        private void OnExchangeCard()
        {
            if (chipProgress < 100)
                return;

            Global.gApp.gUiMgr.OpenPanel(Wndid.GunCostUI, cardChipList);

            TouchClose();
        }
        
        void OnChooseChip()
        {
            var needChip = 100;
            foreach (var info in cardChipList)
            {
                info.addCount = 0;
                info.cardItem.SetCount(info.addCount);
            }

            var rareCount = 0;
            var epicCount = 0;
            foreach (var info in cardChipList)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(info.cardData.cardID);
                if (res == null)
                    continue;

                
                if (res.rarity == (int)CardQualityType.RARE)
                {
                    rareCount += info.cardData.count;
                }
                else
                {
                    epicCount += info.cardData.count;
                }
            }

            if (rareCount / 10 + epicCount < 100)
            {
                foreach (var info in cardChipList)
                {
                    info.addCount = info.cardData.count;
                    info.cardItem.SetCount(info.addCount);
                }
            }
            else
            {

                rareCount = rareCount / 100 * 100;

                foreach (var info in cardChipList)
                {
                    var res = TableMgr.singleton.GunCardTable.GetItemByID(info.cardData.cardID);
                    if (res == null)
                        continue;

                    var addCount = 0;
                    if (res.rarity == (int)CardQualityType.RARE)
                    {
                        if (rareCount <= 0)
                            continue;

                        addCount = info.cardData.count / 10;
                        if (addCount * 10 > rareCount)
                            addCount = rareCount / 10;

                        rareCount -= addCount * 10;
                    }
                    else
                    {
                        addCount = info.cardData.count;
                        epicCount -= addCount;
                    }

                    if (addCount > needChip)
                        addCount = needChip;

                    if (res.rarity == (int)CardQualityType.RARE)
                    {
                        info.addCount = addCount * 10;
                    }
                    else
                    {
                        info.addCount = addCount;
                    }

                    info.cardItem.SetCount(info.addCount);

                    needChip -= addCount;
                    if (needChip <= 0)
                        break;
                }
            }

            UpdateChipProgress();
        }

        private void OnGoTo()
        {
            Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.CommonUIIndexChange, 3);
            TouchClose();
        }
    }
}