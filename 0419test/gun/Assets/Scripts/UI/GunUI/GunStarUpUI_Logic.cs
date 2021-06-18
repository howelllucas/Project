using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;

namespace EZ
{
    public class CardChipData
    {
        public int cardID;
        public int count;
    }

    public enum StarUpChipType
    {
        SameName = 1,
        SameType = 2,
    }


    public partial class GunStarUpUI
    {
        private GunCard_TableItem gunCardRes;
        private CardStar_TableItem upStarRes;
        private List<GameObject> curStarList = new List<GameObject>();
        private List<GameObject> upStarList = new List<GameObject>();
        private List<GunStarUpUI_GunCard> gunChipList = new List<GunStarUpUI_GunCard>();
        private List<GunStarUpUI_ChooseChip> chooseChipList = new List<GunStarUpUI_ChooseChip>();
        private Dictionary<int, List<CardChipData>> chipDic = new Dictionary<int, List<CardChipData>>();
        private int chooseIndex;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            gunCardRes = arg as GunCard_TableItem;
            if (gunCardRes != null)
            {
                ShowCardInfo();
            }

       
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            curStarList.Clear();
            curStarList.Add(CurStar1.gameObject);
            curStarList.Add(CurStar2.gameObject);
            curStarList.Add(CurStar3.gameObject);
            curStarList.Add(CurStar4.gameObject);
            curStarList.Add(CurStar5.gameObject);

            upStarList.Clear();
            upStarList.Add(UpStar1.gameObject);
            upStarList.Add(UpStar2.gameObject);
            upStarList.Add(UpStar3.gameObject);
            upStarList.Add(UpStar4.gameObject);
            upStarList.Add(UpStar5.gameObject);

            StarUpBtn.button.onClick.AddListener(StarUpCard);
            CloseBtn.button.onClick.AddListener(TouchClose);
            CloseChooseBtn.button.onClick.AddListener(CloseChoose);
        }


        public override void Recycle()
        {
            base.Recycle();

        }

        private void ShowCardInfo()
        {
            ClearShowList();
            //chipDic.Clear();

            //NameTxt.text.text = LanguageMgr.GetText(gunCardRes.tid_name);
            gunChipList.Clear();
            StarUpBtn.button.interactable = false;

            var cardData = PlayerDataMgr.singleton.GetGunCardData(gunCardRes.id);
            if (cardData == null)
                return;

            var curStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star);
            if (curStarRes == null)
                return;

            upStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star + 1);
            if (upStarRes == null)
                return;

            for(int i = 0;i < upStarRes.slotCount;++i)
            {
                List<CardChipData> chipList;
                if (!chipDic.TryGetValue(i, out chipList))
                {
                    chipList = new List<CardChipData>();
                    chipDic[i] = chipList;
                }

                var itemUI = GunCard.GetInstance();
                itemUI.Init(i, gunCardRes.gunType, upStarRes.needCardRarityList[i]);
                itemUI.transform.SetParent(ChipRoot.gameObject.transform);
                itemUI.OnCardClick = null;
                itemUI.gameObject.SetActive(true);
                
                gunChipList.Add(itemUI);

                var count = 0;
                if ((StarUpChipType)upStarRes.needCardTypeList[i] == StarUpChipType.SameName)
                {
                    
                    if (cardData.count < upStarRes.needCardCountList[i])
                        count = cardData.count;
                    else
                    {
                        count = upStarRes.needCardCountList[i];
                        StarUpBtn.button.interactable = true;
                    }
                    itemUI.IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;
                    //itemUI.IconBtn.image.enabled = true;

                    var data = new CardChipData();
                    data.cardID = cardData.cardID;
                    data.count = count;
                    chipList.Add(data);
                }
                else
                {
                    //for (int j = 0;j < chipList.Count;++j)
                    //{
                    //    count += chipList[j].count;
                    //}
                    //itemUI.IconBtn.image.enabled = false;

                    itemUI.OnCardClick = OpenChipRoot;
                }

                itemUI.Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(upStarRes.needCardRarityList[i]),
                                                    typeof(Sprite)) as Sprite;

                itemUI.ChipCount.text.text = string.Format("{0}/{1}", count, upStarRes.needCardCountList[i]);
            }


            for (int i = 0;i < curStarList.Count;++i)
            {
                if (i < curStarRes.star)
                {
                    curStarList[i].SetActive(true);
                }
                else
                {
                    curStarList[i].SetActive(false);
                }
            }

            for (int i = 0; i < upStarList.Count; ++i)
            {
                if (i < upStarRes.star)
                {
                    upStarList[i].SetActive(true);
                }
                else
                {
                    upStarList[i].SetActive(false);
                }
            }

            CurAtk.text.text = cardData.GetAtk().ToString("f0");
            NextAtk.text.text = cardData.GetAtk(cardData.level, cardData.star + 1).ToString("f0");
            CurDPS.text.text = (cardData.DPS() / TableMgr.singleton.ValueTable.combat_capability).ToString("f0");
            NextDPS.text.text = (cardData.DPS(cardData.level, cardData.star + 1) /
                                TableMgr.singleton.ValueTable.combat_capability).ToString("f0");
            CurLevel.text.text = curStarRes.maxLevel.ToString();
            NextLevel.text.text = upStarRes.maxLevel.ToString();
            //CurAtkSpeed.text.text = cardData.GetAtkSpeed().ToString();
            //NextAtkSpeed.text.text = cardData.GetAtkSpeed().ToString();

            var fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(cardData.fuseSkillID);
            if (fuseSkillRes == null)
                return;

            SkillName.text.text = LanguageMgr.GetText(fuseSkillRes.tid_name);
            SkillLevel.text.text = string.Format("Lv.{0}", fuseSkillRes.level);

        }

        private void ClearShowList()
        {
            foreach (var obj in gunChipList)
            {
                GunCard.CacheInstance(obj);
            }
            gunChipList.Clear();
        }
        private void OpenChipRoot(int idx, int type, int rarity)
        {
            chooseIndex = idx;
            foreach (var obj in chooseChipList)
            {
                ChooseChip.CacheInstance(obj);
            }
            chooseChipList.Clear();

            List<CardChipData> chipList;
            if (!chipDic.TryGetValue(chooseIndex, out chipList))
            {
                chipList = new List<CardChipData>();
                chipDic[chooseIndex] = chipList;
            }

            foreach (var card in PlayerDataMgr.singleton.DB.cardDatas)
            {
                var res = TableMgr.singleton.GunCardTable.GetItemByID(card.Key);
                if (res == null)
                    continue;

                if (card.Value.count <= 0)
                    continue;

                if (res.gunType == type && res.rarity == rarity)
                {
                    var count = card.Value.count;
                    var chipUI = ChooseChip.GetInstance();
                    chipUI.Init(card.Key, count);
                    chipUI.transform.SetParent(WeaponContent.gameObject.transform);
                    chipUI.gameObject.SetActive(true);
                    chipUI.OnCardClick = AddCradChip;

                    chooseChipList.Add(chipUI);

                    for (int i = 0; i < chipList.Count; ++i)
                    {
                        if (chipList[i].cardID == card.Key)
                        {
                            chipUI.SetCount(chipList[i].count);
                            break;
                        }
                    }
                }
            }
            CardsRoot.gameObject.SetActive(true);
        }

        private void AddCradChip(int id, int count)
        {
            List<CardChipData> chipList;
            if (!chipDic.TryGetValue(chooseIndex, out chipList))
            {
                chipList = new List<CardChipData>();
                chipDic[chooseIndex] = chipList;
            }

            var allCount = 0;
            CardChipData data = null;
            for (int i = 0; i < chipList.Count; ++i)
            {
                allCount += chipList[i].count;
                if (chipList[i].cardID == id)
                    data = chipList[i];
            }

            if (allCount >= upStarRes.needCardCountList[chooseIndex] && count > 0)
            {
                return;
            }
            else if (allCount <= 0 && count < 0)
            {
                return;
            }

            //if (count > upStarRes.needCardCountList[chooseIndex] - allCount)
            //    count = upStarRes.needCardCountList[chooseIndex] - allCount;

            if (data == null)
            {
                data = new CardChipData();
                data.cardID = id;
                data.count = count;
                chipList.Add(data);
            }
            else
            {
                data.count += count;
            }


            allCount += count;
            if (allCount >= upStarRes.needCardCountList[chooseIndex])
            {
                StarUpBtn.button.interactable = true;
            }
            else
            {
                StarUpBtn.button.interactable = false;
            }

            var itemUI = gunChipList[chooseIndex];
            itemUI.ChipCount.text.text = string.Format("{0}/{1}", allCount, upStarRes.needCardCountList[chooseIndex]);

            for (int i = 0; i < chooseChipList.Count; ++i)
            {
                if (chooseChipList[i].cardID == id)
                {
                    chooseChipList[i].SetCount(data.count);
                    break;
                }
            }

            //ShowCardInfo();
        }

        private void CloseChoose()
        {
            CardsRoot.gameObject.SetActive(false);
        }

        private void StarUpCard()
        {
            if (PlayerDataMgr.singleton.CardStarUp(gunCardRes.id, chipDic))
            {
                chipDic.Clear();
                ShowCardInfo();

                Global.gApp.gUiMgr.OpenPanel(Wndid.GunStarUpShowUI, gunCardRes);
                TouchClose();
            }

            
        }
        
    }
}