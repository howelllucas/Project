using DG.Tweening;
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class MainUi
    {
        public Transform PetNode; 
        public Transform SubWeaponNode; 
        private Transform m_WeaponBip;
        private GameObject m_ShowWeapon;
        private GameObject m_ShowPet;
        private GameObject m_ShowSubWeapon;
        private string m_CurMainWeaponName = string.Empty;
        private string m_PetName = string.Empty;
        private string m_SubWpnName = string.Empty;
        private ItemItem m_LDAGunCfg;
        private ItemItem m_NDCfg;
        private Vector3 m_PassIconMinScale;
        private Vector3 m_PassIconMaxScale;
        private int m_PassIconNumMinScale;
        private int m_PassIconNumMaxScale;
        private GameObject m_LvUpEffect;
        private float m_LvEffectDelay;
        private QuestItemDTO m_LevelDetailDTO;
        private Vector3 m_LDAIconInitPos;
        private string m_RemainStr;
        private int m_TimeGift1Tick;
        private int m_TimeGift2Tick;
        private bool m_FreshPet = false;


        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();
            Global.gApp.gSystemMgr.GetNpcMgr().FreshCampInfo();
            
            ChangeLanguage();
            gameObject.AddComponent<DelayCallBack>().SetAction(() => {
                Global.gApp.gSystemMgr.GetPassMgr().ShowTankUiState();
            }, 1.5f, false);
            m_FreshPet = true;
        }

        public override void ChangeLanguage()
        {
            base.ChangeLanguage();
            m_RemainStr = Global.gApp.gGameData.GetTipsInCurLanguage(4187);
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            InitBg();
            RegisterListeners();




            m_RemainStr = Global.gApp.gGameData.GetTipsInCurLanguage(4187);
            Btnstart.button.onClick.AddListener(StartGame);
            Btnstartbranch.button.onClick.AddListener(StartGame);
            Config.button.onClick.AddListener(OnClickSetting);
            GetMDT.button.onClick.AddListener(OnClickGetMDT);
            BtnRandompass.button.onClick.AddListener(OnClickEnterBranchPass);
            LDAbgbtn.button.onClick.AddListener(OnClickGetLDA);
            NDbgbtn.button.onClick.AddListener(OnClickNextDay);

            levelshow.button.onClick.AddListener(OnLevelDetail);

            SevenDay.button.onClick.AddListener(OnClickSevenDay);
            DebugBtn.button.onClick.AddListener(OnDebugBtn);

            OlcoinBtn.button.onClick.AddListener(OnClickOnlineCoin);

            FirstCharge.button.onClick.AddListener(OnClickFirstPurchase);

            TimeGift1.button.onClick.AddListener(OnTimeGift1);
            TimeGift2.button.onClick.AddListener(OnTimeGift2);

            m_LDAIconInitPos = new Vector3(LDAAwardIcon.rectTransform.localPosition.x, LDAAwardIcon.rectTransform.localPosition.y, LDAAwardIcon.rectTransform.localPosition.z);

            if (Global.gApp.gSystemMgr.GetMiscMgr().FirstMain == 0)
            {
                Global.gApp.gSystemMgr.GetMiscMgr().FirstMain = 1;
            }

            InitCountdownPurchaseUI();

            
        }

        private void InitBg()
        {
            PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassItem();
            if (passItem != null)
            {
                Bg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(passItem.mainUIbg);
                //if (passItem.mainUIbg.EndsWith("bg1"))
                //{
                //    UI_fire_1.gameObject.SetActive(true);
                //    //UI_fire_2.gameObject.SetActive(false);
                //} else
                //{
                //    UI_fire_1.gameObject.SetActive(false);
                //    //UI_fire_2.gameObject.SetActive(true);
                //}

                if (passItem.mainUIeffect != null && !passItem.mainUIeffect.Equals(GameConstVal.EmepyStr))
                {
                    GameObject effect = Global.gApp.gResMgr.InstantiateObj(passItem.mainUIeffect);
                    effect.transform.SetParent(Bgeffect.rectTransform, false);
                }
            }
            else
            {
                Bg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>("UI/bg/bg1");
            }
        }

        private void LateUpdate()
        {
            Vector3 pos = MainRoleNode.transform.position;
            Vector3 newPos = MainRoleAdapter.rectTransform.position;
            MainRoleNode.transform.position = new Vector3(newPos.x, newPos.y, pos.z);

            Vector3 pos1 = UI_fire_1.transform.position;
            Vector3 newPos1 = FireAdapter.rectTransform.position;
            UI_fire_1.transform.position = new Vector3(newPos1.x, newPos1.y, pos1.z);
            if (m_FreshPet)
            {
                m_FreshPet = false;
                FreshPetModel();
            }
        }

        private void OnClickSetting()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_SETTING);
            Global.gApp.gUiMgr.OpenPanel(Wndid.GameConfigPanel);
        }

        private void OpenShopUi()
        {
            OnClick();
            Global.gApp.gUiMgr.OpenPanel(Wndid.ShopPanel);
            Global.gApp.gUiMgr.ClosePanel(Wndid.MainPanel);
        }
        private void StartGame()
        {            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_START_GAME);            OnClick();
            int branchId = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassId();
            //string[] consumeItemStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GAME_CONSUME_ITEMS).contents;
           
            if (branchId > 0)
            {
                string[] consumeItemStr1 = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GAME_CONSUME_ITEMS).contents;
                PassBranchItem branckItem = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassItem();
                if (branckItem.isMust > 0)
                {
                    //bool result0 = GameItemFactory.GetInstance().ReduceItem(consumeItemStr1, BehaviorTypeConstVal.OPT_GAME_CONSUME);
                    //if (!result0)
                    //{
                    //    Global.gApp.gUiMgr.OpenPanel(Wndid.EnergyShowPanel);
                    //    return;
                    //}
                    int passId = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassId();
                    Global.gApp.gUiMgr.ClossAllPanel();
                    Global.gApp.gGameCtrl.ChangeToFightScene(passId);
                    return;
                }
                else
                {
                    Global.gApp.gUiMgr.OpenPanel(Wndid.ConfirmBranchPassUI);
                    return;
                }
            }
            
            string[] consumeItem;
            if(!Global.gApp.gSystemMgr.GetPassMgr().GetHasPassedMaxPass())
            {
                consumeItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GAME_CONSUME_ITEMS).contents;
            }
            else
            {
                consumeItem = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REPASS_ENERGY_TAKE).contents;
            }
            //bool result = GameItemFactory.GetInstance().ReduceItem(consumeItem, BehaviorTypeConstVal.OPT_GAME_CONSUME);
            //if (!result)
            //{
            //    //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_NO_ENERGY);
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.EnergyShowPanel);
            //    return;
            //}
            string inputText = InputFieldCmp.inputField.text;
            float selectPassId;
            if (float.TryParse(inputText, out selectPassId))
            {
                //Game.PlayerDataMgr.singleton.StageHpParam = selectPassId;
                //PassItem newPassItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItemById(selectPassId);
                //if (newPassItem != null)
                //{
                //    Global.gApp.gUiMgr.ClossAllPanel();
                //    Global.gApp.gGameCtrl.ChangeToFightScene(newPassItem.id);
                //    return;
                //}
            }
            PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassItem();
            if (passItem != null)
            {
                Global.gApp.gSystemMgr.GetPassMgr().EnterPass();
                Global.gApp.gUiMgr.ClossAllPanel();
                Global.gApp.gGameCtrl.ChangeToFightScene(passItem.id);
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, "Stay tuned ");
            }
        }

        private void OnClickOnlineCoin()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_ONLINE_REWARD);

            Global.gApp.gUiMgr.OpenPanel(Wndid.GetRewardUI);
            Global.gApp.gSystemMgr.GetMiscMgr().FreshOnLineTime();
            Olcoinbg.gameObject.SetActive(false);
        }

        private void OnClickFirstPurchase()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_FIRST_PURCHASE);

            Global.gApp.gUiMgr.OpenPanel(Wndid.FirstPurchaseUI);
            Global.gApp.gSystemMgr.GetMiscMgr().EveryDayFP = 1;
            fTip.gameObject.SetActive(Global.gApp.gSystemMgr.GetMiscMgr().EveryDayFP == 0);
        }
        private void OnClickGetMDT()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.NoMDTUI);
        }
        private void OnClickEnterBranchPass()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_SPECIAL_GAME);
            int passId = Global.gApp.gSystemMgr.GetPassMgr().EnterBranchPass();
            if (passId > 0)
            {
                Global.gApp.gUiMgr.ClossAllPanel();
                Global.gApp.gGameCtrl.ChangeToFightScene(passId);
            }
        }
        private void OnClickGetLDA()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_LEVEL_DETAIL_AWARD);
            if (m_LevelDetailDTO != null)
            {
                int times = 1;
                QuestItem questCfg = Global.gApp.gGameData.QuestData.Get(m_LevelDetailDTO.id);
                ItemDTO itemDTO = new ItemDTO(Convert.ToInt32(questCfg.award[0]), questCfg.award[1] * times, BehaviorTypeConstVal.OPT_LEVEL_DETAIL);
                itemDTO.param3 = m_LevelDetailDTO.id;
                CommonUI commonUi = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
                if (commonUi != null)
                {
                    //commonUi.UnLockById(itemDTO.itemId, false, false);
                }
                
                Global.gApp.gUiMgr.OpenPanel<ItemDTO>(Wndid.OpenBoxUI, itemDTO);


                //Global.gApp.gMsgDispatcher.Broadcast<bool>(MsgIds.HideGameGuideAD, false);
                //Global.gApp.gUiMgr.OpenPanel<QuestItemDTO>(Wndid.GetMoneyUI, levelDetailDTO);
            } else
            {
                Global.gApp.gUiMgr.OpenPanel<ItemItem>(Wndid.NextDayWeaponUi, m_LDAGunCfg);
            }

            //Global.gApp.gUiMgr.OpenPanel<ItemItem>(Wndid.NextDayWeaponUi, m_NextGunCfg);
        }

        private void OnClickNextDay()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_NEXT_DAY);
            Global.gApp.gUiMgr.OpenPanel<ItemItem>(Wndid.NextDayWeaponUi, m_NDCfg);
        }
        private void OnClickSevenDay()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_SEVEN_DAY);
            Global.gApp.gUiMgr.OpenPanel(Wndid.SevenDayPanel);
        }

        private void OnDebugBtn()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.DebugPanel);
        }

        public void FreshEnterBtn()
        {
            int branchId = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassId();
            Btnstart.gameObject.SetActive(branchId < 0);
            Btnstartbranch.gameObject.SetActive(branchId > 0);
            if(branchId < 0)
            {
                levelContent.gameObject.SetActive(true);
                BranchLevel.gameObject.SetActive(false);
            }
            else
            {

                levelContent.gameObject.SetActive(false);
                BranchLevel.gameObject.SetActive(true);
            }
        }
        private void InitNode()
        {
            FreshEnterBtn();
            GeneralConfigItem passMaxScaleCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PASS_ICON_MAX_SCALE);
            m_PassIconMaxScale = new Vector3(float.Parse(passMaxScaleCfg.contents[0]), float.Parse(passMaxScaleCfg.contents[1]), float.Parse(passMaxScaleCfg.contents[2]));
            GeneralConfigItem passMinScaleCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PASS_ICON_MIN_SCALE);
            m_PassIconMinScale = new Vector3(float.Parse(passMinScaleCfg.contents[0]), float.Parse(passMinScaleCfg.contents[1]), float.Parse(passMinScaleCfg.contents[2]));
            GeneralConfigItem passNumMinScaleCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PASS_ICON_NUM_MIN_SCALE);
            m_PassIconNumMinScale = int.Parse(passNumMinScaleCfg.content);
            GeneralConfigItem passNumMaxScaleCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PASS_ICON_NUM_MAX_SCALE);
            m_PassIconNumMaxScale = int.Parse(passNumMaxScaleCfg.content);

            m_LvEffectDelay = float.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.MAIN_UI_LV_EFFECT_DELAY_SEC).content);
            m_WeaponBip = MainRoleNode.transform.Find("hero/weapon_bip");

            {
                string curMainWeaponName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();
                Destroy(m_ShowWeapon);
                string newWpnName = curMainWeaponName;
                if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(curMainWeaponName) > 0)
                {
                    newWpnName += "_s";
                }
                GameObject weapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/MainUI/" + newWpnName);
                weapon.layer = LayerMask.NameToLayer("UI");
                weapon.transform.SetParent(m_WeaponBip, false);
                m_ShowWeapon = weapon;
                m_CurMainWeaponName = curMainWeaponName;
            }

            string subWpnName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon();
            if(subWpnName == null || subWpnName.Equals(string.Empty) || !subWpnName.Equals(m_SubWpnName))
            {
                if (m_ShowSubWeapon != null)
                {
                    Destroy(m_ShowSubWeapon);
                    m_ShowSubWeapon = null;
                }
            }
            if (subWpnName != null && !subWpnName.Equals(string.Empty) && !subWpnName.Equals(m_SubWpnName))
            {
                GameObject weapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/MainUI/Sub/" + subWpnName);
                weapon.layer = LayerMask.NameToLayer("UI");
                weapon.transform.SetParent(SubWeaponNode, false);
                m_ShowSubWeapon = weapon;
            }
            m_SubWpnName = subWpnName;
            double curExp = GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.EXP);
            BaseAttrMgr baseAttrMgr = Global.gApp.gSystemMgr.GetBaseAttrMgr();
            baseAttrMgr.ResetLevel(curExp);
            int curLevel = baseAttrMgr.GetLevel();


            int branchPassCount = Global.gApp.gSystemMgr.GetPassMgr().FreshAndGetBranchPassCount();
            BtnRandompass.gameObject.SetActive(branchPassCount > 0);
            newNum.text.text = branchPassCount.ToString();

            if (m_LvUpEffect != null)
            {
                Destroy(m_LvUpEffect);
                m_LvUpEffect = null;
            }
            if (baseAttrMgr.LvUp)
            {
                baseAttrMgr.LvUp = false;
                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.Big_LEVEL_UP);
                //GameObject Obj = Global.gApp.gResMgr.InstantiateObj("Prefabs/Effect/ui/UI_shengji_1");
                //Obj.transform.SetParent(transform, false);

                gameObject.AddComponent<DelayCallBack>().SetAction(()=>
                {
                    m_LvUpEffect = UiTools.GetEffect(effectItem.path, transform);
                }, m_LvEffectDelay);
            }

            
//#if (UNITY_EDITOR)
            InputFieldCmp.gameObject.SetActive(true);
//#endif
//#if (!UNITY_EDITOR)
//            InputFieldCmp.gameObject.SetActive(false);
//#endif

            UIFresh();
            FlushOnlineCoinState();

            if (Global.gApp.gSystemMgr.GetMiscMgr().Dialogues.Count > 0)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, Global.gApp.gSystemMgr.GetMiscMgr().Dialogues[0].ToString());
                Global.gApp.gSystemMgr.GetMiscMgr().RemoveDialogue(0);
            }
        }
        private void FreshPetModel()
        {
            string petName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet();
            if (petName == null || petName.Equals(string.Empty) || !petName.Equals(m_PetName))
            {
                if (m_ShowPet != null)
                {
                    Destroy(m_ShowPet);
                    m_ShowPet = null;
                }
            }
            if (petName != null && !petName.Equals(string.Empty) && !petName.Equals(m_PetName))
            {
                GameObject petGo = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/MainUI/pet/" + petName);
                petGo.layer = LayerMask.NameToLayer("UI");
                petGo.transform.SetParent(PetNode, false);
                m_ShowPet = petGo;
            }
            m_PetName = petName;
            if (m_ShowPet != null)
            {
                m_ShowPet.GetComponentInChildren<Animator>().Play(GameConstVal.IdleUi, -1, 0);
            }
        }
        private void FlushOnlineCoinState()
        {
            bool hasReward = Global.gApp.gSystemMgr.GetMiscMgr().CheckHasOnlineReward();
            Olcoinbg.gameObject.SetActive(hasReward);
        }
        private void InitPassInfo()
        {
            //----------------pass info
            int passId1 = 0;
            int passId2 = 0;
            int passId3 = 0;

            int userStep = Global.gApp.gSystemMgr.GetPassMgr().GetUserStep();
            PassItem passItemCfg = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassItem();
            string bgIndex = System.Text.RegularExpressions.Regex.Replace(passItemCfg.mainUIbg, @"[^0-9]+", "");
            SceneImg.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.RESOURCE_LEVEL_DETAIL_ICON, bgIndex), typeof(Sprite)) as Sprite;
            int curPassId = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();
            int minPassId = Global.gApp.gGameData.m_MinNormalId;
            int maxPassId = Global.gApp.gGameData.m_MaxNormalId;
            if (curPassId == minPassId)
            {
                passId1 = curPassId;
                passId2 = curPassId + 1;
                passId3 = curPassId + 2;

                pass1.rectTransform.localScale = m_PassIconMaxScale;
                pass2.rectTransform.localScale = m_PassIconMinScale;
                pass3.rectTransform.localScale = m_PassIconMinScale;
            } else if (curPassId == maxPassId)
            {
                passId3 = curPassId;
                passId2 = curPassId - 1;
                passId1 = curPassId - 2;

                pass1.rectTransform.localScale = m_PassIconMinScale;
                pass2.rectTransform.localScale = m_PassIconMinScale;
                pass3.rectTransform.localScale = m_PassIconMaxScale;
            } else
            {
                passId1 = curPassId - 1;
                passId2 = curPassId;
                passId3 = curPassId + 1;

                pass1.rectTransform.localScale = m_PassIconMinScale;
                pass2.rectTransform.localScale = m_PassIconMaxScale;
                pass3.rectTransform.localScale = m_PassIconMinScale;
                if(curPassId % 100000 >= 100)
                {
                    pass2.gameObject.GetComponentInChildren<Text>().fontSize = 40;
                }
            }

            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            int initId = Convert.ToInt32(initPassIdConfig.content);

            SetPassIcon(pass1, passId1, curPassId, initId);
            SetPassIcon(pass2, passId2, curPassId, initId);
            SetPassIcon(pass3, passId3, curPassId, initId);

            List<QuestItem> configs = Global.gApp.gGameData.QuestTypeMapData[QuestConst.TYPE_LEVEL_DETAIL];
            int progressMax = maxPassId;
            int progressMin = minPassId;
            int progressNext = 0;
            QuestItem boxQuestItem = null;
            for (int i = 0; i < configs.Count; i ++)
            {
                QuestItem itemCfg = configs[i];
                int questPassId = (int)itemCfg.condition[1] + initId;
                if (progressNext == 0 && questPassId >= curPassId)
                {
                    progressNext = questPassId;
                    boxQuestItem = itemCfg;
                    break;
                }
            }
            for (int i = 0; i < Global.gApp.gGameData.PassStep.Count; i ++)
            {
                progressMax = Global.gApp.gGameData.PassStep[i];
                if (curPassId >= progressMin && curPassId <= progressMax)
                {
                    break;
                } else
                {
                    progressMin = progressMax + 1;
                }
            }
            passProgress.image.fillAmount = (float)(curPassId - progressMin) / (progressMax - progressMin);
            float rewardLocation = (float)(progressNext - progressMin) / (progressMax - progressMin);
            rewardBox.rectTransform.localPosition = new Vector3(passProgress.rectTransform.localPosition.x + (rewardLocation - 0.5f) * passProgress.rectTransform.sizeDelta.x, rewardBox.rectTransform.localPosition.y, rewardBox.rectTransform.localPosition.z);
            
            if (boxQuestItem == null)
            {
                rewardBox.gameObject.SetActive(false);
                Reward.gameObject.SetActive(false);
            } else
            {
                boxIcon.image.sprite = Resources.Load(boxQuestItem.awardIcon, typeof(Sprite)) as Sprite;
                //2019 10 31 陈冬要求加入临时修改
                rewardBox.gameObject.SetActive(true);
                //rewardBox.gameObject.SetActive(progressNext > curPassId);
                Reward.image.sprite = Resources.Load(boxQuestItem.awardIcon, typeof(Sprite)) as Sprite;
                //2019 10 31 陈冬要求加入临时修改
                Reward.gameObject.SetActive(false);
                //Reward.gameObject.SetActive(progressNext == curPassId);

                ItemItem awardItem = Global.gApp.gGameData.ItemData.Get((int)boxQuestItem.award[0]);
                if (awardItem != null)
                {
                    if (ItemTypeConstVal.isWeapon(awardItem.showtype))
                    {
                        boxIcon.rectTransform.sizeDelta = new Vector2(105, 105);
                        boxIcon.rectTransform.localPosition = new Vector3(boxIcon.rectTransform.localPosition.x, -51, boxIcon.rectTransform.localPosition.z);
                    } else
                    {
                        boxIcon.rectTransform.sizeDelta = new Vector2(70, 70);
                        boxIcon.rectTransform.localPosition = new Vector3(boxIcon.rectTransform.localPosition.x, -53, boxIcon.rectTransform.localPosition.z);
                    }
                }
            }
            //2019 10 31 陈冬要求加入临时修改
            SceneImg.gameObject.SetActive(true);
            //SceneImg.gameObject.SetActive(progressNext != curPassId);

            //等级显示
            //level.text.text = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel().ToString();
            //Hero_dataItem levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            //int initExp = 0;
            //if (levelConfig.level > 1)
            //{
            //    initExp = Global.gApp.gGameData.HeroDataConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel() - 1).expRequire;
            //}
            //exp.image.fillAmount = (float)(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetExp() - initExp) / (levelConfig.expRequire - initExp);
        }

        private void SetPassIcon(RectTransform_Image_Container passUI, int passId, int curPassId, int initId)
        {
            

            PassItem passItem = Global.gApp.gGameData.PassData.Get(passId);
            bool isBoss = passItem.bossLevel > 0;
            passUI.rectTransform.GetChild(0).gameObject.SetActive(!isBoss && passId < curPassId);
            passUI.rectTransform.GetChild(1).gameObject.SetActive(!isBoss && passId == curPassId);
            passUI.rectTransform.GetChild(2).gameObject.SetActive(!isBoss && passId > curPassId);

            passUI.rectTransform.GetChild(3).gameObject.SetActive(isBoss && passId < curPassId);
            passUI.rectTransform.GetChild(4).gameObject.SetActive(isBoss && passId == curPassId);
            passUI.rectTransform.GetChild(5).gameObject.SetActive(isBoss && passId > curPassId);

            passUI.rectTransform.GetChild(6).GetComponent<Text>().text = (passId % initId).ToString();
        }

        private void OnLevelDetail()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.MAIN_LEVEL_DETAIL);
            OnClick();
            //Global.gApp.gUiMgr.ClosePanel(Wndid.CommonPanel);
            //Global.gApp.gUiMgr.ClosePanel(Wndid.MainPanel);
            Global.gApp.gUiMgr.OpenPanel(Wndid.LevelDetail);
        }

        //初始化倒计时充值
        private void InitCountdownPurchaseUI()
        {
            double mills = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
            bool openTimeGift1 = Global.gApp.gSystemMgr.GetMiscMgr().IsShowTimeGift(mills, GeneralConfigConstVal.TIME_GIFT1_PRODUCT);
            if (openTimeGift1)
            {
                GeneralConfigItem timeGiftPassCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TIME_GIFT_SHOW_PASS);

                if (Global.gApp.gSystemMgr.GetMiscMgr().ShowTimeGiftToday == 0 && Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() > int.Parse(timeGiftPassCfg.content))
                {
                    OnTimeGift1();

                    Global.gApp.gSystemMgr.GetMiscMgr().ShowTimeGiftToday = 1;

                }
                
            }
           
        }

        private void OnTimeGift1()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_TIME_GIFT1);
            Global.gApp.gUiMgr.OpenPanel(Wndid.TimeGiftUI1);
        }

        private void OnTimeGift2()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.GAME_TIME_GIFT2);
            Global.gApp.gUiMgr.OpenPanel(Wndid.TimeGiftUI2);
        }

        private void UIFresh()
        {

            GetMDT.gameObject.GetComponent<Animator>().enabled = (Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTDataGift != 1);
            GetMDTLight.gameObject.SetActive(Global.gApp.gSystemMgr.GetMiscMgr().VideoMDTDataGift != 1);

            InitPassInfo();

            Global.gApp.gSystemMgr.GetQuestMgr().CheckLoginWeaponAward();

            int questId = Global.gApp.gSystemMgr.GetQuestMgr().GetShouldReceiveId4SevenDay();
            SevenDayBg.gameObject.SetActive(questId > 0);

            m_LevelDetailDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetLevelDetailQuest();
            ItemItem canReceiveItem = null;
            if (m_LevelDetailDTO != null)
            {
                canReceiveItem = Global.gApp.gGameData.ItemData.Get((int)Global.gApp.gGameData.QuestData.Get(m_LevelDetailDTO.id).award[0]);
            }
            QuestItemDTO nextLevelDetailDTO = Global.gApp.gSystemMgr.GetQuestMgr().GetNextLevelDetailQuest();
            if (nextLevelDetailDTO != null)
            {
                m_LDAGunCfg = Global.gApp.gGameData.ItemData.Get((int)Global.gApp.gGameData.QuestData.Get(nextLevelDetailDTO.id).award[0]);
            }

            m_NDCfg = Global.gApp.gSystemMgr.GetPassMgr().GetNextGun(FilterTypeConstVal.SUM_LOGIN_DAY);

            //可领取红点
            LDARedPoint.gameObject.SetActive(m_LevelDetailDTO != null && canReceiveItem != null);
            if (m_LevelDetailDTO != null && canReceiveItem != null)
            {
                InitNextUI(m_LevelDetailDTO, canReceiveItem);
                //可以领取
                LDAGunName.gameObject.SetActive(false);
                LDAAwardIcon.gameObject.GetComponent<DOTweenAnimation>().DORestart();
            }
            else if (m_LDAGunCfg != null)
            {
                LDAAwardIcon.gameObject.GetComponent<DOTweenAnimation>().DOPause();
                LDAAwardIcon.rectTransform.localPosition = m_LDAIconInitPos;
                InitNextUI(nextLevelDetailDTO, m_LDAGunCfg);
                LDAGunName.gameObject.SetActive(true);
            }
            else
            {
                LDAAwardIcon.gameObject.GetComponent<DOTweenAnimation>().DOPause();
                LDAAwardIcon.rectTransform.localPosition = m_LDAIconInitPos;
                LDAGunName.gameObject.SetActive(true);
                LevelDetailAward.gameObject.SetActive(false);
            }

            if (m_NDCfg != null)
            {
                bool isWeapon = ItemTypeConstVal.isWeapon(m_NDCfg.showtype);
                GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
                //NextGunName.text.color = ColorUtil.GetColor(colorConfig.contents[itemConfig.qualevel]);
                NDGunName.text.text = FilterFactory.GetInstance().GetTinyUnfinishTips(m_NDCfg.opencondition);
                NDGunIcon.gameObject.SetActive(m_NDCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON);
                NDGunIcon.image.sprite = Resources.Load(m_NDCfg.image_grow, typeof(Sprite)) as Sprite;
                NDSubWepIcon.gameObject.SetActive(m_NDCfg.showtype == ItemTypeConstVal.SUB_WEAPON);
                NDSubWepIcon.image.sprite = Resources.Load(m_NDCfg.image_grow, typeof(Sprite)) as Sprite;
                NDPetIcon.gameObject.SetActive(m_NDCfg.showtype == ItemTypeConstVal.PET);
                NDPetIcon.image.sprite = Resources.Load(m_NDCfg.image_grow, typeof(Sprite)) as Sprite;
                NDAwardIcon.gameObject.SetActive(false);
                if (isWeapon)
                {
                    if (m_NDCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                    {
                        NDGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, m_NDCfg.qualevel), typeof(Sprite)) as Sprite;
                    }
                    else
                    {
                        NDGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                    }
                    NDGunEffect.image.enabled = false;
                    if (NDGunEffect.rectTransform.childCount == 0)
                    {
                        EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                        string effectName = m_NDCfg.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? m_NDCfg.qualevel.ToString() : "common";
                        GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, effectName), NDGunEffect.rectTransform);

                        effect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);

                        ParticleSystem[] particleRenderers = effect.GetComponentsInChildren<ParticleSystem>();
                        foreach (ParticleSystem ps in particleRenderers)
                        {
                            ps.GetComponent<Renderer>().sortingOrder = 39;
                        }
                    }
                }
                else
                {
                    NDGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                }
                
            } else
            {
                NextDay.gameObject.SetActive(false);
            }

            FirstCharge.gameObject.SetActive(Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase == 0);
            fTip.gameObject.SetActive(Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase == 0 && Global.gApp.gSystemMgr.GetMiscMgr().EveryDayFP == 0);

            double mls = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
            bool showTimeGift1 = Global.gApp.gSystemMgr.GetMiscMgr().IsShowTimeGift(mls, GeneralConfigConstVal.TIME_GIFT1_PRODUCT);
            bool showTimeGift2 = Global.gApp.gSystemMgr.GetMiscMgr().IsShowTimeGift(mls, GeneralConfigConstVal.TIME_GIFT2_PRODUCT);
            TimeGift1.gameObject.SetActive(showTimeGift1);
            TimeGift2.gameObject.SetActive(showTimeGift2);
            if (showTimeGift1 && m_TimeGift1Tick == 0)
            {
                m_TimeGift1Tick = 1;
                DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
                
                TimeTip1.text.text = GetTimtTip(mls);
                dcb.SetAction(() =>
                {
                    double ms = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
                    
                    if (ms < 0d)
                    {
                        ms = 0d;
                    }
                    TimeTip1.text.text = GetTimtTip(ms);
                }, 1f);
                
                dcb.SetCallTimes(Convert.ToInt32(mls / DateTimeUtil.m_Sec_Mills) + 1);
            }
            if (showTimeGift2 && m_TimeGift2Tick == 0)
            {
                m_TimeGift2Tick = 1;
                DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
                
                TimeTip2.text.text = GetTimtTip(mls);
                dcb.SetAction(() =>
                {
                    double ms = DateTimeUtil.m_Day_Mills * 7 - DateTimeUtil.GetMills(DateTime.Now) + Global.gApp.gSystemMgr.GetMiscMgr().TimeGiftStartTime;
                    
                    if (ms < 0d)
                    {
                        ms = 0d;
                    }
                    TimeTip2.text.text = GetTimtTip(ms);
                }, 1f);
                
                dcb.SetCallTimes(Convert.ToInt32(mls / DateTimeUtil.m_Sec_Mills) + 1);
            }

#if (UNITY_EDITOR || DISBLE_PLATFORM)
#else
            //if (SdkdsPurchaseUtils.m_ProductList == null || SdkdsPurchaseUtils.m_ProductList.Count == 0)
            //{
            //    FirstCharge.gameObject.SetActive(false);
            //    TimeGift1.gameObject.SetActive(false);
            //    TimeGift2.gameObject.SetActive(false);
            //}
#endif
        }

        private string GetTimtTip(double mls)
        {
            if (mls < DateTimeUtil.m_Day_Mills)
            {
                return Global.gApp.gGameData.GetTimeInCurLanguage(mls);
            } else
            {
                return string.Format(m_RemainStr, Global.gApp.gGameData.GetTimeInCurLanguage(mls));
            }
        }

        
        private void InitNextUI(QuestItemDTO nextLevelDetailDTO, ItemItem itemConfig)
        {
            bool isWeapon = ItemTypeConstVal.isWeapon(itemConfig.showtype);
            //NextGunName.gameObject.SetActive(isWeapon);
            LDAGunIcon.gameObject.SetActive(itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON);
            LDASubWpIcon.gameObject.SetActive(itemConfig.showtype == ItemTypeConstVal.SUB_WEAPON);
            LDAPetIcon.gameObject.SetActive(itemConfig.showtype == ItemTypeConstVal.PET);
            LDAAwardIcon.gameObject.SetActive(!isWeapon);
            List<GameObject> toDeleteList = new List<GameObject>();

            LDAGunEffect.image.enabled = false;
            if (isWeapon)
            {
                GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
                //NextGunName.text.color = ColorUtil.GetColor(colorConfig.contents[itemConfig.qualevel]);
                LDAGunName.text.text = FilterFactory.GetInstance().GetTinyUnfinishTips(itemConfig.opencondition);
                
                LDAGunIcon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;
               
                LDASubWpIcon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;
               
                LDAPetIcon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;

                if (itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                {
                    LDAGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, itemConfig.qualevel), typeof(Sprite)) as Sprite;
                } else
                {
                    LDAGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
                }
                
                if (LDAGunEffect.rectTransform.childCount == 0)
                {
                    EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                    string effectName = itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? itemConfig.qualevel.ToString() : "common";
                    GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, effectName), LDAGunEffect.rectTransform);

                    effect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                }

            }
            else
            {
                QuestItem questCfg = Global.gApp.gGameData.QuestData.Get(nextLevelDetailDTO.id);
                LDAGunName.text.text = FilterFactory.GetInstance().GetTinyUnfinishTips(questCfg.condition);
                LDAAwardIcon.image.sprite = Resources.Load(questCfg.awardIcon, typeof(Sprite)) as Sprite;
                LDAGunDown.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.MAIN_UI_WEAPON_DOWN_PATH, 2), typeof(Sprite)) as Sprite;
            }
        }

        private void ExpChanged(double newExp, int newLevel)
        {
            level.text.text = newLevel.ToString();
            float beginExp;
            if (newLevel == 1)
            {
                beginExp = 0f;
            }
            else
            {
                Hero_dataItem lastConfig = Global.gApp.gGameData.HeroDataConfig.Get(newLevel - 1);
                beginExp = lastConfig.expRequire;
            }
            Hero_dataItem curConfig = Global.gApp.gGameData.HeroDataConfig.Get(newLevel);
            exp.image.fillAmount = (float)(newExp - beginExp) / (curConfig.expRequire - beginExp);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<double, int>(MsgIds.ExpChanged, ExpChanged);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
        }

        private void UnRegisterListeners()
        {

            Global.gApp.gMsgDispatcher.RemoveListener<double, int>(MsgIds.ExpChanged, ExpChanged);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
