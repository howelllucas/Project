using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using UnityEngine;

namespace EZ
{
    public partial class WeaponUI_PetItemUI
    {
        private ItemItem m_ItemConfig;
        private PetNode m_Parent;
        //武器名
        private string m_WeaponName;
        public Transform m_IconParentNode;
        private ItemDTO m_ReduceItemDTO;

        private void Awake()
        {
            InitNode();
        }
        private void InitNode()
        {
            PetEquipBtn.button.onClick.AddListener(OnEquip);
            BtnUnlock.button.onClick.AddListener(OnUpdateLevel);
            BtnUpgrade.button.onClick.AddListener(OnUpdateLevel);
        }
        public void UIFresh()
        {
            if (m_ItemConfig == null)
            {
                return;
            }
            int lv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(m_ItemConfig.name);
            int state = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name);
            int filterType = Convert.ToInt32(m_ItemConfig.opencondition[0]);
            GunsSub_dataItem lvCfg = Global.gApp.gGameData.GunSubDataConfig.Get(lv);
            //SubLvtxt
            //passProgress
            //BtnUnlock
            //BtnUpgrade
            //Unlocktxt
            //Moneyiconbtn
            //Moneycostbtn
            //m_Lock
            //SubWeaponIcon

            bottom.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.SUB_BOTTOM, m_ItemConfig.qualevel));
            if (Effect.rectTransform.childCount == 0)
            {
                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.SUB_EFFECT);
                GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, m_ItemConfig.qualevel), Effect.rectTransform);
            }
            PetWeaponBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.SUB_BG, m_ItemConfig.qualevel));

            if (filterType != FilterTypeConstVal.CUR_ITEM_NUM)
            {
                //if (state == WeaponStateConstVal.NONE && FilterFactory.GetInstance().Filter(m_ItemConfig.opencondition))
                //{
                //    state = WeaponStateConstVal.NEW;
                //    Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(m_ItemConfig.name, state);
                //}
                //应该显示解锁
                bool unLock = state == WeaponStateConstVal.NEW || state == WeaponStateConstVal.EXIST;
                PetLvtxt.gameObject.SetActive(unLock);
                progress_bg.gameObject.SetActive(false);
                BtnUnlock.gameObject.SetActive(false);
                BtnUpgrade.gameObject.SetActive(unLock);
                Unlocktxt.gameObject.SetActive(!unLock);
                Moneyiconbtn.gameObject.SetActive(unLock);
                Moneycostbtn.gameObject.SetActive(unLock);
                m_Lock.gameObject.SetActive(!unLock);
                PetWeaponIcon.gameObject.SetActive(true);
                PetWeaponIcon.image.color = unLock ? ColorUtil.GetColor(ColorUtil.m_DeaultColor) : ColorUtil.GetColor(ColorUtil.m_WhiteColor);
                AtkMsg.gameObject.SetActive(unLock);
            }
            else
            {
                //应该显示解锁
                bool unLock = state == WeaponStateConstVal.NEW || state == WeaponStateConstVal.EXIST;
                PetLvtxt.gameObject.SetActive(unLock);
                progress_bg.gameObject.SetActive(false);
                BtnUnlock.gameObject.SetActive(!unLock);
                BtnUpgrade.gameObject.SetActive(unLock);
                Unlocktxt.gameObject.SetActive(false);
                Moneyiconbtn.gameObject.SetActive(true);
                Moneycostbtn.gameObject.SetActive(true);
                m_Lock.gameObject.SetActive(!unLock);
                PetWeaponIcon.gameObject.SetActive(true);
                PetWeaponIcon.image.color = unLock ? ColorUtil.GetColor(ColorUtil.m_DeaultColor) : ColorUtil.GetColor(ColorUtil.m_WhiteColor);
                AtkMsg.gameObject.SetActive(unLock);
            }

            Unlocktxt.text.text = FilterFactory.GetInstance().GetMiddleUnfinishTips(m_ItemConfig.opencondition);
            PetLvtxt.text.text = "LV:" + lv;
            if (lvCfg != null)
            {
                if (filterType == FilterTypeConstVal.CUR_ITEM_NUM && state == WeaponStateConstVal.NONE)
                {
                    int itemId = Convert.ToInt32(m_ItemConfig.opencondition[1]);
                    double unlockItemNum = Convert.ToDouble(m_ItemConfig.opencondition[2]);
                    m_ReduceItemDTO = new ItemDTO(itemId, unlockItemNum, BehaviorTypeConstVal.OPT_WEAPON_UNLOCK);
                    double itemCount = GameItemFactory.GetInstance().GetItem(itemId);
                    passProgress.image.fillAmount = (float)(itemCount / unlockItemNum);
                    Moneyiconbtn.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId));
                    Moneycostbtn.text.text = UiTools.FormateMoney(unlockItemNum);
                    Moneycostbtn.text.color = ColorUtil.GetTextColor(itemCount < unlockItemNum, null);
                }
                else
                {
                    double[] costValue = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_cost_" + m_ItemConfig.qualevel, lvCfg);
                    int itemId = Convert.ToInt32(costValue[0]);
                    double unlockItemNum = Convert.ToDouble(costValue[1]);
                    m_ReduceItemDTO = new ItemDTO(itemId, unlockItemNum, BehaviorTypeConstVal.OPT_WEAPON_LEVEL_UP);
                    double itemCount = GameItemFactory.GetInstance().GetItem(itemId);
                    passProgress.image.fillAmount = (float)(itemCount / unlockItemNum);
                    Moneyiconbtn.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId));
                    Moneycostbtn.text.text = UiTools.FormateMoney(unlockItemNum);
                    Moneycostbtn.text.color = ColorUtil.GetTextColor(itemCount < unlockItemNum, null);
                }
                double[] prm = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + m_ItemConfig.qualevel, lvCfg);
                Atk.text.text = string.Format("{0}%", (prm[0] * 100).ToString());
                if (Global.gApp.gGameData.GunSubDataConfig.Get(lv + 1) == null)
                {
                    MaxLevel(lv);
                }
            }
            else
            {
                MaxLevel(lv);
            }


            PetUp.gameObject.SetActive(Global.gApp.gSystemMgr.GetWeaponMgr().CanUpdateWeapon(m_ItemConfig));
            NewPet.gameObject.SetActive(state == WeaponStateConstVal.NEW);
            NewIMG_pet.gameObject.SetActive(state == WeaponStateConstVal.NEW);
        }

        private void MaxLevel(int lv)
        {
            PetLvtxt.text.text = "MAX LV:" + lv;
            BtnUnlock.gameObject.SetActive(false);
            BtnUpgrade.gameObject.SetActive(false);
            Unlocktxt.gameObject.SetActive(false);
            Moneyiconbtn.gameObject.SetActive(false);
            Moneycostbtn.gameObject.SetActive(false);
        }

        public void Init(ItemItem itemConfig, int showOrder, PetNode parent)
        {
            m_ItemConfig = itemConfig;
            m_WeaponName = itemConfig.name;
            gameObject.SetActive(true);
            AskIcon.gameObject.SetActive(false);
            Item.gameObject.SetActive(true);
            m_Parent = parent;

            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet() != null &&
                Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet().Equals(itemConfig.name))
            {
                m_Pet.gameObject.SetActive(true);
                Mask.gameObject.SetActive(true);

            }
            else
            {
                Mask.gameObject.SetActive(false);
                m_Pet.gameObject.SetActive(false);
            }
            transform.SetSiblingIndex(1 * (showOrder * 10000 + itemConfig.showorder));

            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(itemConfig))
            {
                m_Lock.gameObject.SetActive(false);
                m_PetWeaponIcon.gameObject.SetActive(true);
            }
            else
            {
                m_Lock.gameObject.SetActive(true);
                Mask.gameObject.SetActive(false);
                m_Pet.gameObject.SetActive(false);
                m_PetWeaponIcon.gameObject.SetActive(false);

            }

            if (m_IconParentNode == null)
            {
                m_IconParentNode = m_UpNode.gameObject.transform.parent;
                m_UpNode.gameObject.GetComponent<WeaponUI_Item_Follow>().SetFollowNode(m_IconParentNode);
                m_UpNode.gameObject.transform.SetParent(m_Parent.GetViewPoint2().transform, true);
            }


            m_PetWeaponIcon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;
            //if (itemConfig.id == 10005)
            //{
            //    m_PetWeaponIcon.rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            //}
            m_PetWeaponNameTxt.text.text = itemConfig.gamename;
            m_WeaponName = itemConfig.name;


            UIFresh();

            NewbieGuideButton[] newBieButtons = this.GetComponentsInChildren<NewbieGuideButton>();
            foreach (NewbieGuideButton newBieButton in newBieButtons)
            {
                newBieButton.Param = m_WeaponName;
                newBieButton.OnStart();
            }

        }

        private void OnEquip()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_PET_SELECT);
            if (m_WeaponName.Equals(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet()))
            {
                Mask.gameObject.SetActive(false);
                Pet.gameObject.SetActive(false);
                Global.gApp.gSystemMgr.GetWeaponMgr().SetCurPet(GameConstVal.EmepyStr);
                if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name) == WeaponStateConstVal.NEW)
                {
                    Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(m_ItemConfig.name, WeaponStateConstVal.EXIST);
                    UIFresh();
                }
                return;
            }
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(m_WeaponName);
            if (itemConfig == null)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1003);
                return;
            }
            if (itemConfig.showtype != ItemTypeConstVal.PET)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1001);
                return;
            }
            if (!Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig))
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3091);
                return;
            }
            m_Parent.Equip(m_WeaponName);
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name) == WeaponStateConstVal.NEW)
            {
                Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(m_ItemConfig.name, WeaponStateConstVal.EXIST);
                UIFresh();
            }
            
        }

        private void OnUpdateLevel()
        {

            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(m_ItemConfig.name);
            int roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            int state = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name);
            int filterType = Convert.ToInt32(m_ItemConfig.opencondition[0]);
            bool unlockNow = false;
            if (filterType == FilterTypeConstVal.CUR_ITEM_NUM)
            {
                if (state == WeaponStateConstVal.NONE && FilterFactory.GetInstance().Filter(m_ItemConfig.opencondition))
                {
                    unlockNow = true;
                    state = WeaponStateConstVal.NEW;
                    Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(m_ItemConfig.name, state);
                }
            }
            int errorCode = Global.gApp.gSystemMgr.GetWeaponMgr().CanLevelUpSub(m_WeaponName);
            if (errorCode > 0)
            {
                if (errorCode == 1008)
                {
                    ItemItem reduceCfg = Global.gApp.gGameData.ItemData.Get(m_ReduceItemDTO.itemId);
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, errorCode, Global.gApp.gGameData.GetTipsInCurLanguage(reduceCfg.sourceLanguage));
                }
                else
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, errorCode);
                }
                return;
            }
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(gunLevel);
            GunsSub_dataItem nextLevelItem = Global.gApp.gGameData.GunSubDataConfig.Get(gunLevel + 1);
            double[] nextValue = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_cost_" + m_ItemConfig.qualevel, nextLevelItem);
            if (nextValue == null || nextValue.Length == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1004);
                return;
            }

            //是否成功扣钱
            bool reduceResult = false;
            m_ReduceItemDTO.paramStr1 = m_WeaponName;
            m_ReduceItemDTO.paramStr2 = gunLevel.ToString();
            GameItemFactory.GetInstance().ReduceItem(m_ReduceItemDTO);
            reduceResult = m_ReduceItemDTO.result;

            if (!reduceResult)
            {
                ItemItem reduceItemCfg = Global.gApp.gGameData.ItemData.Get(m_ReduceItemDTO.itemId);
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 1008, Global.gApp.gGameData.GetTipsInCurLanguage(reduceItemCfg.sourceLanguage));
                return;
            }

            bool levelUpResult = false;
            if (unlockNow)
            {
                levelUpResult = true;

                //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_PET_UNLOCK);
            }
            else
            {
                levelUpResult = Global.gApp.gSystemMgr.GetWeaponMgr().LevelUp(m_WeaponName);

                //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_PET_UPDATE);
            }

            if (levelUpResult)
            {
                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.WEAPON_LEVEL_IP);
                GameObject effect = UiTools.GetEffect(effectItem.path, transform);
                effect.transform.GetChild(0).localPosition = new Vector3(0f, 0f, 0f);
                GameObject.Destroy(effect, 3f);
            }
            else
            {
                ItemDTO addItemDTO = new ItemDTO(m_ReduceItemDTO.itemId, m_ReduceItemDTO.num, BehaviorTypeConstVal.OPT_WEAPON_LEVEL_UP);
                addItemDTO.paramStr1 = m_WeaponName;
                addItemDTO.paramStr2 = "MakeUp4Fail";
                GameItemFactory.GetInstance().AddItem(addItemDTO);
            }

            //OnEquip();
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        public void Recycle()
        {
            if (m_IconParentNode != null)
            {
                m_UpNode.gameObject.GetComponent<WeaponUI_Item_Follow>().SetFollowNode(null);
                m_UpNode.gameObject.transform.SetParent(m_IconParentNode, true);
                m_IconParentNode = null;
            }

            for (int i = Effect.rectTransform.childCount - 1; i >= 0; --i)
            {
                var child = Effect.rectTransform.GetChild(i).gameObject;
                Destroy(child);
            }
        }
        public string GetWeaponName()
        {
            return m_WeaponName;
        }
    }
}