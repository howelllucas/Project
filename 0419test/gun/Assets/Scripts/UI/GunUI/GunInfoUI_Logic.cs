using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;
using Game.Util;

namespace EZ
{
    public partial class GunInfoUI
    {
        GunCard_TableItem gunCardRes;
        GunCardData cardData;
        List<GameObject> starList = new List<GameObject>();
        List<GameObject> starFrameList = new List<GameObject>();

        private int occupiedByCampPoint = -1;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            gunCardRes = arg as GunCard_TableItem;
            if (gunCardRes == null)
                return;

            ShowCardInfo();

            if (gunCardRes.id == PlayerDataMgr.singleton.GetUseWeaponID())
            {
                RemoveBtn.gameObject.SetActive(false);
                ReplaceBtn.gameObject.SetActive(true);
            }
            else if (PlayerDataMgr.singleton.GetFusedCardList().Contains(gunCardRes.id))
            {
                RemoveBtn.gameObject.SetActive(true);
                ReplaceBtn.gameObject.SetActive(true);
            }
            else
            {
                RemoveBtn.gameObject.SetActive(false);
                ReplaceBtn.gameObject.SetActive(false);
            }

            if (CampsiteMgr.singleton.CheckCardIsOccupied(gunCardRes.id, out occupiedByCampPoint))
            {
                CampDataChangeBubble.CardCampDataChangeBubble.Init(gunCardRes.id, occupiedByCampPoint);
                OccupiedFlag.gameObject.SetActive(true);
            }
            else
            {
                occupiedByCampPoint = -1;
                OccupiedFlag.gameObject.SetActive(false);
            }
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            starList.Clear();
            starList.Add(Icon1.gameObject);
            starList.Add(Icon2.gameObject);
            starList.Add(Icon3.gameObject);
            starList.Add(Icon4.gameObject);
            starList.Add(Icon5.gameObject);

            starFrameList.Clear();
            starFrameList.Add(IconFrame1.gameObject);
            starFrameList.Add(IconFrame2.gameObject);
            starFrameList.Add(IconFrame3.gameObject);
            starFrameList.Add(IconFrame4.gameObject);
            starFrameList.Add(IconFrame5.gameObject);

            LvUpBtn.buttonEx.onClick.AddListener(LvUpCard);
            LvUpBtn.buttonEx.onLongPress.AddListener(LvUpCard);
            RemoveBtn.button.onClick.AddListener(RemoveCard);
            ReplaceBtn.button.onClick.AddListener(ReplaceCard);
            StarUpBtn.button.onClick.AddListener(StarUpCard);
            ResetBtn.button.onClick.AddListener(ResetCard);
            FuseSkillIcon.button.onClick.AddListener(ShowFuseSkill);
            CampSkillIcon.button.onClick.AddListener(ShowCampSkill);

            CloseBtn.button.onClick.AddListener(TouchClose);

            RegisterListeners();
        }

        public void HideOptBtns()
        {
            OptBtns.gameObject.SetActive(false);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.GunCardOpt, ShowCardInfo);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.GunCardOpt, ShowCardInfo);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void ShowCardInfo()
        {
            NameTxt.text.text = LanguageMgr.GetText(gunCardRes.tid_name); ;
            RarityTxt.text.text = TableMgr.singleton.ValueTable.GetQualityName(gunCardRes.rarity);
            IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;
            Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(gunCardRes.rarity),
                                                typeof(Sprite)) as Sprite;

            var gunTypeRes = TableMgr.singleton.GunTypeTable.GetItemByID(gunCardRes.gunType);
            if (gunTypeRes == null)
                return;

            TypeTxt.text.text = LanguageMgr.GetText(gunTypeRes.tid_type);
            TypeIcon.image.sprite = Resources.Load(gunTypeRes.icon, typeof(Sprite)) as Sprite;

            cardData = PlayerDataMgr.singleton.GetGunCardData(gunCardRes.id);
            if (cardData == null)
                return;

            ChipCount.text.text = cardData.count.ToString();

            var gunStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star);
            if (gunStarRes == null)
                return;

            if (cardData.level >= gunStarRes.maxLevel)
            {
                LevelTxt.text.text = string.Format("Lv.{0}(Max)", cardData.level);
            }
            else
            {
                LevelTxt.text.text = string.Format("Lv.{0}/{1}", cardData.level, gunStarRes.maxLevel);
            }
            if (cardData.level <= 1)
            {
                ResetBtn.gameObject.SetActive(false);
            }
            else
            {
                ResetBtn.gameObject.SetActive(true);
            }

            for (int i = 0; i < starList.Count; ++i)
            {
                if (i < gunStarRes.star)
                {
                    starList[i].SetActive(true);
                }
                else
                {
                    starList[i].SetActive(false);
                }
            }

            var maxStar = PlayerDataMgr.GetCardMaxStarCount(gunCardRes.rarity, gunStarRes.starRarity);
            for (int i = 0; i < starFrameList.Count; ++i)
            {
                if (i < maxStar)
                {
                    starFrameList[i].SetActive(true);
                }
                else
                {
                    starFrameList[i].SetActive(false);
                }
            }

            if (cardData.star >= PlayerDataMgr.GetCardMaxStar(gunCardRes.rarity))
            {
                StarUpBtn.gameObject.SetActive(false);
            }
            else
            {
                StarUpBtn.gameObject.SetActive(true);
                if (PlayerDataMgr.singleton.CanCardStarUp(gunCardRes.id))
                {
                    RedPoint.gameObject.SetActive(true);
                }
                else
                {
                    RedPoint.gameObject.SetActive(false);
                }
            }

            CurAtk.text.text = ((int)cardData.GetAtk()).ToString();
            //NextAtk.text.text = cardData.GetAtk(cardData.level + 1, cardData.star).ToString();
            CurAtkSpeed.text.text = cardData.GetAtkSpeed().ToString();
            //NextAtkSpeed.text.text = cardData.GetAtkSpeed(cardData.level + 1, cardData.star).ToString();

            ProductionBonus.text.text = cardData.GetCampRewardFactor().ToString();
            FirePower.text.text = (cardData.DPS() / TableMgr.singleton.ValueTable.combat_capability).ToString("f0");

            var fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(cardData.fuseSkillID);
            if (fuseSkillRes == null)
                return;

            FuseSkillName.text.text = LanguageMgr.GetText(fuseSkillRes.tid_name);
            FuseSkillLevel.text.text = string.Format("Lv.{0}", fuseSkillRes.level);

            var campSkillRes = TableMgr.singleton.CampGunSkillTable.GetItemByID(cardData.campSkillID);
            if (campSkillRes == null)
                return;

            CampSkillName.text.text = LanguageMgr.GetText(campSkillRes.tid_name);
            CampSkillLevel.text.text = string.Format("Lv.{0}", campSkillRes.level);

            LvUpCost.text.text = PlayerDataMgr.singleton.GetCardLvUpCost(cardData.level + 1).ToSymbolString();
            var offset = -(PTUtil.CalculateLengthOfText(LvUpCost.text.text, LvUpCost.text)) / 2;
            if (offset < -80)
                offset = -80;
            GoldIcon.rectTransform.localPosition = new Vector3(offset, 0.0f, 0.0f);
        }


        private void LvUpCard()
        {
            if (!PlayerDataMgr.singleton.CardLvUp(gunCardRes.id))
                return;
            if (occupiedByCampPoint >= 0)
                CampDataChangeBubble.CardCampDataChangeBubble.Show();
        }

        private void RemoveCard()
        {
            if (PlayerDataMgr.singleton.RemoveFuseCard(gunCardRes.id))
            {
                ShowCardInfo();
                TouchClose();
            }
        }

        private void ReplaceCard()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.GunListUI, gunCardRes);
            TouchClose();
        }

        private void StarUpCard()
        {
            if (cardData.star >= PlayerDataMgr.GetCardMaxStar(gunCardRes.rarity))
                return;

            Global.gApp.gUiMgr.OpenPanel(Wndid.GunStarUpUI, gunCardRes);
            //TouchClose();
        }

        private void ResetCard()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.GunResetUI, gunCardRes);
        }

        private void ShowFuseSkill()
        {
            var skillInfo = new SkillInfo();
            skillInfo.type = SkillType.Fuse;
            skillInfo.skillID = cardData.fuseSkillID;

            Global.gApp.gUiMgr.OpenPanel(Wndid.GunSkillInfoUI, skillInfo);
        }

        private void ShowCampSkill()
        {
            var skillInfo = new SkillInfo();
            skillInfo.type = SkillType.Camp;
            skillInfo.skillID = cardData.campSkillID;

            Global.gApp.gUiMgr.OpenPanel(Wndid.GunSkillInfoUI, skillInfo);
        }
    }
}