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
    public partial class GunListUI_GunCard
    {
        public Action<int> OnCardClick = null;

        private GunCard_TableItem gunCardRes;
        List<GameObject> starList = new List<GameObject>();


        private void Awake()
        {
            InitNode();

            starList.Clear();
            starList.Add(Icon1.gameObject);
            starList.Add(Icon2.gameObject);
            starList.Add(Icon3.gameObject);
            starList.Add(Icon4.gameObject);
            starList.Add(Icon5.gameObject);
        }
        public void Init(GunCard_TableItem res)
        {
            gunCardRes = res;
            if (gunCardRes == null)
                return;

            NameTxt.text.text = LanguageMgr.GetText(gunCardRes.tid_name);
            Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(gunCardRes.icon);
            Frame.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(TableMgr.singleton.ValueTable.GetCardQualityFrame(gunCardRes.rarity));
            EffectImg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(TableMgr.singleton.ValueTable.GetCardQualityEffectImg(gunCardRes.rarity));

            var cardData = PlayerDataMgr.singleton.GetGunCardData(gunCardRes.id);
            if (cardData == null)
                return;

            var gunTypeRes = TableMgr.singleton.GunTypeTable.GetItemByID(gunCardRes.gunType);
            if (gunTypeRes == null)
                return;

            //TypeTxt.text.text = gunTypeRes.gunType;
            TypeIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(gunTypeRes.icon);

            LevelTxt.text.text = string.Format("Lv.{0}", cardData.level);

            var gunStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star);
            if (gunStarRes == null)
                return;

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

            CurAtk.text.text = (cardData.DPS() / TableMgr.singleton.ValueTable.combat_capability).ToString("f0");

            var fuseSkillRes = TableMgr.singleton.FuseGunSkillTable.GetItemByID(cardData.fuseSkillID);
            if (fuseSkillRes == null)
                return;

            FuseSkillName.text.text = LanguageMgr.GetText(fuseSkillRes.tid_name);
            FuseSkillLevel.text.text = string.Format("Lv.{0}", fuseSkillRes.level);

            if (gunCardRes.id == PlayerDataMgr.singleton.GetUseWeaponID())
            {
                EquipTxt.gameObject.SetActive(true);
                FuseTxt.gameObject.SetActive(false);
            }
            else if (PlayerDataMgr.singleton.DB.fusedCardList.Contains(gunCardRes.id))
            {
                EquipTxt.gameObject.SetActive(false);
                FuseTxt.gameObject.SetActive(true);
            }
            else
            {
                EquipTxt.gameObject.SetActive(false);
                FuseTxt.gameObject.SetActive(false);
            }
        }

        private void InitNode()
        {
            IconBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            if (OnCardClick != null)
                OnCardClick(gunCardRes.id);
        }
    }
}