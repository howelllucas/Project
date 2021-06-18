using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using EZ.Weapon;
using UnityEngine.UI;

namespace EZ
{
    public partial class GunUI
    {
        private GunUI_GunCard mainGunCard;
        private List<GunUI_GunCard> showGunList = new List<GunUI_GunCard>();
        private List<GunUI_GunSkill> showSkillList = new List<GunUI_GunSkill>();
        private List<GameObject> fuesRootList = new List<GameObject>();
        private List<GameObject> fuesRedPointList = new List<GameObject>();
        private List<GameObject> skillRootList = new List<GameObject>();
        private List<GunCard_TableItem> cardResList = new List<GunCard_TableItem>();
        private Transform m_WeaponBip;
        private GameObject m_ShowWeapon;
        private GunCard_TableItem curMainWeaponRes = null;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            ShowGunCard();

            bool fuseOpen = PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunFuse);
            SubGunLockImg1.gameObject.SetActive(!fuseOpen);
            SubGunLockImg2.gameObject.SetActive(!fuseOpen);
            SubGunLockImg3.gameObject.SetActive(!fuseOpen);
            SubGunLockImg4.gameObject.SetActive(!fuseOpen);
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            cardResList.Clear();
            foreach (GunCard_TableItem item in TableMgr.singleton.GunCardTable.getEnumerator())
            {
                cardResList.Add(item);
            }



            fuesRootList.Add(SubGunBtn1.gameObject);
            fuesRootList.Add(SubGunBtn2.gameObject);
            fuesRootList.Add(SubGunBtn3.gameObject);
            fuesRootList.Add(SubGunBtn4.gameObject);

            fuesRedPointList.Add(SubGunRedPoint1.gameObject);
            fuesRedPointList.Add(SubGunRedPoint2.gameObject);
            fuesRedPointList.Add(SubGunRedPoint3.gameObject);
            fuesRedPointList.Add(SubGunRedPoint4.gameObject);


            skillRootList.Add(SubGunSkillBtn1.gameObject);
            skillRootList.Add(SubGunSkillBtn2.gameObject);
            skillRootList.Add(SubGunSkillBtn3.gameObject);
            skillRootList.Add(SubGunSkillBtn4.gameObject);

            SubGunBtn1.button.onClick.AddListener(OpenGunList);
            SubGunBtn2.button.onClick.AddListener(OpenGunList);
            SubGunBtn3.button.onClick.AddListener(OpenGunList);
            SubGunBtn4.button.onClick.AddListener(OpenGunList);

            ChipBtn.button.onClick.AddListener(OpenChipUI);

            RegisterListeners();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.GunCardOpt, OnGunDataChange);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.GunCardOpt, OnGunDataChange);
        }

        private void ShowGunCard()
        {

            ClearShowList();
            SortCards();

            foreach (var point in fuesRedPointList)
            {
                point.gameObject.SetActive(true);
            }


            curMainWeaponRes = TableMgr.singleton.GunCardTable.GetItemByID(PlayerDataMgr.singleton.GetUseWeaponID());
            if (curMainWeaponRes != null)
            {
                var itemUI = GunCard.GetInstance();
                itemUI.Init(curMainWeaponRes);
                itemUI.transform.SetParent(MainGunBtn.gameObject.transform);
                itemUI.transform.localPosition = Vector3.zero;
                itemUI.transform.localScale = Vector3.one;
                itemUI.gameObject.SetActive(true);
                showGunList.Add(itemUI);
            }

            var cardData = PlayerDataMgr.singleton.GetGunCardData(curMainWeaponRes.id);
            if (cardData != null)
            {

                var skillUI = GunSkill.GetInstance();
                skillUI.InitFuseSkill(cardData.fuseSkillID);
                skillUI.transform.SetParent(MainGunSkillBtn.gameObject.transform);
                skillUI.transform.localPosition = Vector3.zero;
                skillUI.gameObject.SetActive(true);
                showSkillList.Add(skillUI);

                FirePower.text.text = (PlayerDataMgr.singleton.GetUseWeaponPower() /
                                TableMgr.singleton.ValueTable.combat_capability).ToString("f0");
            }

            for (int i = 0; i < PlayerDataMgr.singleton.DB.fusedCardList.Count; i++)
            {
                var cardRes = TableMgr.singleton.GunCardTable.GetItemByID(PlayerDataMgr.singleton.DB.fusedCardList[i]);
                if (cardRes == null)
                {
                    continue;
                }

                var itemUI = GunCard.GetInstance();
                itemUI.Init(cardRes);
                itemUI.transform.SetParent(fuesRootList[i].transform);
                itemUI.transform.localPosition = Vector3.zero;
                itemUI.gameObject.SetActive(true);
                showGunList.Add(itemUI);
                fuesRedPointList[i].gameObject.SetActive(false);

                cardData = PlayerDataMgr.singleton.GetGunCardData(cardRes.id);
                if (cardData == null)
                    continue;

                var skillUI = GunSkill.GetInstance();
                skillUI.InitFuseSkill(cardData.fuseSkillID);
                skillUI.transform.SetParent(skillRootList[i].transform);
                skillUI.transform.localPosition = Vector3.zero;
                skillUI.gameObject.SetActive(true);
                showSkillList.Add(skillUI);

            }

            foreach (var item in cardResList)
            {

                if (PlayerDataMgr.singleton.DB.fusedCardList.Contains(item.id) || item.id == curMainWeaponRes.id)
                {
                    //obj = ResourceMgr.singleton.AddGameInstanceAsSubObject("UI/HeroCard", chooseRoot);
                    continue;
                }
                else
                {
                    if (PlayerDataMgr.singleton.GetGunCardData(item.id) == null)
                    {
                        var itemUI = GunCard.GetInstance();
                        itemUI.Init(item);
                        itemUI.transform.SetParent(LockCards.gameObject.transform);
                        itemUI.transform.localPosition = Vector3.zero;
                        itemUI.gameObject.SetActive(true);
                        showGunList.Add(itemUI);
                    }
                    else
                    {
                        var itemUI = GunCard.GetInstance();
                        itemUI.Init(item);
                        itemUI.transform.SetParent(HeroCards.gameObject.transform);
                        itemUI.transform.localPosition = Vector3.zero;
                        itemUI.gameObject.SetActive(true);
                        showGunList.Add(itemUI);
                    }
                }

            }

            GunCount.text.text = string.Format("{0}/{1}", PlayerDataMgr.singleton.GetCollectCardCount(),
                                                            TableMgr.singleton.GunCardTable.ItemCount);

            GunCard.gameObject.SetActive(false);

            foreach (var child in ScrollView.gameObject.GetComponentsInChildren<ContentSizeFitter>(true))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());
            }

            InitRole();
        }

        private void ClearShowList()
        {
            foreach (var obj in showGunList)
            {
                GunCard.CacheInstance(obj);
            }
            showGunList.Clear();

            foreach (var obj in showSkillList)
            {
                GunSkill.CacheInstance(obj);
            }
            showSkillList.Clear();

        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void LateUpdate()
        {
            Vector3 pos = MainRoleNode.transform.position;
            Vector3 newPos = MainRoleAdapter.rectTransform.position;
            MainRoleNode.transform.position = new Vector3(newPos.x, newPos.y, pos.z);

        }

        private void InitRole()
        {

            m_WeaponBip = MainRoleNode.transform.Find("hero/weapon_bip");
            {
                //string curMainWeaponName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();
                Destroy(m_ShowWeapon);
                //string newWpnName = curMainWeaponName;
                //if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(curMainWeaponName) > 0)
                //{
                //    newWpnName += "_s";
                //}
                GameObject weapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/WeaponNew/MainUI/" + curMainWeaponRes.prefab);
                //weapon.layer = LayerMask.NameToLayer("UI");
                weapon.transform.SetParent(m_WeaponBip, false);
                m_ShowWeapon = weapon;
            }

        }

        void SortCards()
        {
            cardResList.Sort((x, y) =>
            {
                var cardX = PlayerDataMgr.singleton.GetGunCardData(x.id);
                var cardY = PlayerDataMgr.singleton.GetGunCardData(y.id);

                if (x.rarity < y.rarity)
                    return 1;
                else if (x.rarity > y.rarity)
                    return -1;

                if (cardX == null && cardY != null)
                    return 1;
                else if (cardX != null && cardY == null)
                    return -1;
                else if (cardX != null && cardY != null)
                {
                    if (cardX.star < cardY.star)
                        return 1;
                    else if (cardX.star > cardY.star)
                        return -1;

                    if (cardX.level < cardY.level)
                        return 1;
                    else if (cardX.level > cardY.level)
                        return -1;
                }

                return (y.id).CompareTo((x.id));
            });
        }

        private void OpenGunList()
        {
            if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.GunFuse))
                Global.gApp.gUiMgr.OpenPanel(Wndid.GunListUI);
        }

        private void OpenChipUI()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.GunChipUI);
            //DrawBoxMgr.singleton.OpenBox(1, 10, null);
        }

        private void OnGunDataChange()
        {
            ShowGunCard();
        }
    }
}