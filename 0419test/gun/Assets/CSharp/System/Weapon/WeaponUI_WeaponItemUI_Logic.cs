using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using UnityEngine;

namespace EZ
{
    public partial class WeaponUI_WeaponItemUI
    {
        private ItemItem m_ItemConfig;
        private WeaponNode m_Parent;
        public Transform m_IconParentNode;
        //武器名
        private string m_WeaponName;
        //消耗道具
        private int m_ConsumItemId;

        private GameObject mEffect;
        private string m_CM_COLOR = "#FDE47EFF";


        private void Awake()
        {
            InitNode();
        }

        public void UIFresh()
        {
            if (m_ItemConfig == null)
            {
                return;
            }
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon().Equals(m_ItemConfig.name))
            {
                Equip.gameObject.SetActive(true);
                EquipBg.gameObject.SetActive(true);
            }
            else
            {
                Equip.gameObject.SetActive(false);
                EquipBg.gameObject.SetActive(false);
            }
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_ItemConfig) == 0)
            {
                WeaponIcon.image.sprite = Resources.Load(m_ItemConfig.image_grow, typeof(Sprite)) as Sprite;
            }
            else
            {
                WeaponIcon.image.sprite = Resources.Load(m_ItemConfig.image_grow + "_s", typeof(Sprite)) as Sprite;
            }
            WeaponNameTxt.text.text = m_ItemConfig.gamename;

            gameObject.SetActive(true);
            UnlockBtn.gameObject.SetActive(false);
            if (!Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig))
            {

                if (m_ItemConfig.opencondition[0] == FilterTypeConstVal.CUR_ITEM_NUM)
                {
                    Mask1.gameObject.SetActive(true);
                    MaskTxt.gameObject.SetActive(false);
                    MaskTxt.text.text = FilterFactory.GetInstance().GetUnfinishTips(m_ItemConfig.opencondition);
                    UnlockBtn.gameObject.SetActive(true);
                    u_CmNum.text.text = UiTools.FormateMoney(m_ItemConfig.opencondition[2]);
                    u_CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)m_ItemConfig.opencondition[1]) < m_ItemConfig.opencondition[2], null);
                    u_CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ItemConfig.opencondition[1]));
                }
                else
                {
                    Mask1.gameObject.SetActive(true);
                    MaskTxt.gameObject.SetActive(true);
                    MaskTxt.text.text = FilterFactory.GetInstance().GetUnfinishTips(m_ItemConfig.opencondition);
                }


                Equip.gameObject.SetActive(false);
                EquipBg.gameObject.SetActive(false);
                WeaponNameTxt.gameObject.SetActive(false);
                WeaponLvTxt.gameObject.SetActive(false);
                Param1NameTxt.gameObject.SetActive(false);
                UpBtn.gameObject.SetActive(false);

                WeaponIcon.image.color = ColorUtil.GetColor(ColorUtil.m_BlackColor);
            }
            else
            {
                Mask1.gameObject.SetActive(false);

                WeaponNameTxt.gameObject.SetActive(true);
                WeaponLvTxt.gameObject.SetActive(true);
                Param1NameTxt.gameObject.SetActive(true);
                UpBtn.gameObject.SetActive(true);

                WeaponIcon.image.color = ColorUtil.GetColor(ColorUtil.m_DeaultColor);

                //新获得的武器显示特效
                if (Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name) == WeaponStateConstVal.NEW)
                {
                    NewWeapon.gameObject.SetActive(true);
                    NewIMG.gameObject.SetActive(true);
                }

            }


            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(m_ItemConfig.name);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_ItemConfig.name + "_cost", weaponLevelData);
            if ((int)costValue[0] != SpecialItemIdConstVal.REAL_MONEY)
            {
                CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1], m_CM_COLOR);
            }

            //可升级提示
            if (Global.gApp.gSystemMgr.GetWeaponMgr().CanUpdateWeapon(m_ItemConfig))
            {
                WeaponUp.gameObject.SetActive(true);
            }
            else
            {
                WeaponUp.gameObject.SetActive(false);
            }

            Guns_dataItem nextLevelCfg = Global.gApp.gGameData.GunDataConfig.Get(gunLevel + 1);
            double[] weaponParams = null;
            if (nextLevelCfg != null)
                weaponParams = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_ItemConfig.name, nextLevelCfg);
            bool max = weaponParams == null || weaponParams.Length == 0;
            CmIcon.gameObject.SetActive(!max);
            CmNum.gameObject.SetActive(!max);
            dotxt.gameObject.SetActive(!max);
            MaxTxt.gameObject.SetActive(max);

            if (m_ItemConfig.opencondition[0] == FilterTypeConstVal.WEAPON_UNLOCK)
            {
                u_CmNum.text.text = UiTools.FormateMoney(m_ItemConfig.opencondition[2]);
                u_CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)m_ItemConfig.opencondition[1]) < m_ItemConfig.opencondition[2], null);
                u_CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ItemConfig.opencondition[1]));

            }
        }
        public void Init(ItemItem itemConfig, int showOrder, WeaponNode parent)
        {
            gameObject.SetActive(true);
            m_Parent = parent;
            AskBg.gameObject.SetActive(false);
            AskIcon.gameObject.SetActive(false);
            AskTxt.gameObject.SetActive(false);
            Item.gameObject.SetActive(true);
            m_ItemConfig = itemConfig;
            transform.SetSiblingIndex(1 * (showOrder * 10000 + m_ItemConfig.showorder));

            GeneralConfigItem gCfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ASK_WEAPON_OPEN_PASS);
            if (gCfg != null)
            {
                int id = int.Parse(gCfg.contents[0]);
                int openPass = int.Parse(gCfg.contents[1]);
                if (m_ItemConfig.id == id && Global.gApp.gSystemMgr.GetPassMgr().GetPassSerial() <= openPass)
                {
                    Item.gameObject.SetActive(false);
                    AskBg.gameObject.SetActive(true);
                    AskIcon.gameObject.SetActive(true);
                    AskTxt.gameObject.SetActive(true);
                    AskTxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(3106);
                    return;
                }

            }

            UIFresh();
            Deal4Level(m_ItemConfig);
            NewbieGuideButton[] newBieButtons = this.GetComponentsInChildren<NewbieGuideButton>();
            foreach (NewbieGuideButton newBieButton in newBieButtons)
            {
                newBieButton.Param = m_WeaponName;
                newBieButton.OnStart();
            }

        }

        private void Deal4Level(ItemItem itemConfig)
        {
            m_ItemConfig = itemConfig;
            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(itemConfig.name);
            int gunShowLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponShowLevel(itemConfig.name);
            WeaponLvTxt.text.text = "lv: " + gunShowLevel;
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);


            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name + "_cost", weaponLevelData);
            CmNum.text.text = UiTools.FormateMoney(costValue[1]);
            CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1], m_CM_COLOR);
            CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, costValue[0]));
            m_ConsumItemId = (int)costValue[0];
            double[] paramValue;
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_ItemConfig) == 0)
            {
                paramValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name, weaponLevelData);
                double showVal = itemConfig.showParam / itemConfig.dtime * paramValue[0];
                Param1Txt.text.text = UiTools.FormateMoney(showVal);
            }
            else
            {
                paramValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name + "_super", weaponLevelData);
                double showVal = itemConfig.showParam / itemConfig.dtime * paramValue[0];
                Param1Txt.text.text = UiTools.FormateMoney(showVal);
            }
            if (paramValue.Length <= 1)
            {
                Param2Txt.text.text = "None";
            }
            else
            {
                Param2Txt.text.text = paramValue[1].ToString();
            }
            m_WeaponName = itemConfig.name;
            int m_QualityLevel = itemConfig.qualevel;
            GeneralConfigItem colorConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.QUALITY_COLOR);
            WeaponNameTxt.text.color = ColorUtil.GetColor(colorConfig.contents[m_QualityLevel]);
            WeaponBg.image.sprite = Resources.Load(string.Format(CommonResourceConstVal.WEAPON_BG_PATH, m_QualityLevel), typeof(Sprite)) as Sprite;

            if (m_QualityLevel > 0)
            {
                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.QUALITY);
                GameObject effect = UiTools.GetEffect(string.Format(effectItem.path, m_QualityLevel), transform);
                if (mEffect != null) { GameObject.Destroy(mEffect); }
                mEffect = effect;
                mEffect.transform.SetParent(WeaponIcon.rectTransform, true);
                if (m_IconParentNode == null)
                {
                    m_IconParentNode = WeaponIcon.gameObject.transform.parent;
                    WeaponIcon.gameObject.GetComponent<WeaponUI_Item_Follow>().SetFollowNode(m_IconParentNode);
                    WeaponIcon.gameObject.transform.SetParent(m_Parent.GetViewPoint2().transform, true);
                }
            }

            //if (m_QualityLevel < itemConfig.qualevel.Length - 1 && gunLevel - itemConfig.qualevel[m_QualityLevel] <= itemConfig.qualevel[m_QualityLevel + 1] - itemConfig.qualevel[m_QualityLevel])
            //{
            //    LvPro.image.fillAmount = (float)(gunLevel - itemConfig.qualevel[m_QualityLevel]) / (itemConfig.qualevel[m_QualityLevel + 1] - itemConfig.qualevel[m_QualityLevel]);
            //}
            //else if (m_QualityLevel >= itemConfig.qualevel.Length - 1)
            //{
            //    LvPro.image.fillAmount = 0;
            //}
            //else
            //{
            //    LvPro.image.fillAmount = 1;
            //}
        }

        private void InitNode()
        {
            EquipBtn.button.onClick.AddListener(OnEquip);
            UpBtn.button.onClick.AddListener(OnLevelUp);
            UnlockBtn.button.onClick.AddListener(OnUnlock);
            WeaponIcon.button.image.raycastTarget = false;
            //WeaponIcon.button.onClick.AddListener(OnOpenSuperGunUI);
        }

        private void OnOpenSuperGunUI()
        {
            //if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_ItemConfig) == 0)
            //{
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.SuperGunUI,this);
            //}
            //else
            //{
            //    OnEquip();
            //}
        }
        public ItemItem GetItemConfig()
        {
            return m_ItemConfig;
        }
        private void OnUnlock()
        {
            int state = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponOpenState(m_ItemConfig.name);
            int filterType = Convert.ToInt32(m_ItemConfig.opencondition[0]);
            if (filterType == FilterTypeConstVal.CUR_ITEM_NUM)
            {
                ItemDTO reduceItemDTO = new ItemDTO(Convert.ToInt32(m_ItemConfig.opencondition[1]), m_ItemConfig.opencondition[2], BehaviorTypeConstVal.OPT_WEAPON_LEVEL_UP);

                if (state == WeaponStateConstVal.NONE && FilterFactory.GetInstance().Filter(m_ItemConfig.opencondition))
                {
                    bool reduceResult = false;
                    reduceItemDTO.paramStr1 = m_WeaponName;
                    GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
                    reduceResult = reduceItemDTO.result;

                    if (!reduceResult)
                    {
                        ItemItem reduceItemCfg = Global.gApp.gGameData.ItemData.Get(reduceItemDTO.itemId);
                        Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 1008, Global.gApp.gGameData.GetTipsInCurLanguage(reduceItemCfg.sourceLanguage));
                        return;
                    }

                    state = WeaponStateConstVal.NEW;
                    Global.gApp.gSystemMgr.GetWeaponMgr().SetWeaponOpenState(m_ItemConfig.name, state);



                    UIFresh();
                    OnEquip();
                }
                else
                {
                    ItemItem reduceItemCfg = Global.gApp.gGameData.ItemData.Get(reduceItemDTO.itemId);
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 1008, Global.gApp.gGameData.GetTipsInCurLanguage(reduceItemCfg.sourceLanguage));
                    return;
                }
            }
        }

        private void OnEquip()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_EXCHANGE);
            if (m_WeaponName.Equals(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon()))
            {
                return;
            }
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(m_WeaponName);
            if (itemConfig == null)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1003);
                return;
            }
            if (itemConfig.showtype != ItemTypeConstVal.BASE_MAIN_WEAPON)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1001);
                return;
            }
            if (Mask1.gameObject.activeSelf)
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, MaskTxt.text.text);
                return;
            }

            Global.gApp.gAudioSource.PlayOneShot("equip_weapon",true);

            m_Parent.Equip(m_WeaponName);
        }

        private void OnLevelUp()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.WEAPON_UPDATE);
            OnEquip();

            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(m_WeaponName);
            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(itemConfig.name);
            int roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            int errorCode = Global.gApp.gSystemMgr.GetWeaponMgr().CanLevelUp(m_WeaponName);
            if (errorCode > 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, errorCode);
                return;
            }
            Guns_dataItem levelItem = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            Guns_dataItem nextLevelItem = Global.gApp.gGameData.GunDataConfig.Get(gunLevel + 1);
            double[] nextValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_WeaponName, nextLevelItem);
            if (nextValue == null || nextValue.Length == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1004);
                return;
            }

            //是否成功扣钱
            bool reduceResult = false;
            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_WeaponName + "_cost", levelItem);
            ItemDTO reduceItemDTO = new ItemDTO(m_ConsumItemId, Convert.ToSingle(costValue[1]), BehaviorTypeConstVal.OPT_WEAPON_LEVEL_UP);
            reduceItemDTO.paramStr1 = m_WeaponName;
            reduceItemDTO.paramStr2 = gunLevel.ToString();
            GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
            reduceResult = reduceItemDTO.result;

            if (!reduceResult)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
                return;
            }

            bool levelUpResult = Global.gApp.gSystemMgr.GetWeaponMgr().LevelUp(m_WeaponName);
            if (levelUpResult)
            {
                Deal4Level(itemConfig);

                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.WEAPON_LEVEL_IP);
                GameObject effect = UiTools.GetEffect(effectItem.path, transform);
                GameObject.Destroy(effect, 3f);
            }
            else
            {
                ItemDTO addItemDTO = new ItemDTO(m_ConsumItemId, Convert.ToSingle(costValue[1]), BehaviorTypeConstVal.OPT_WEAPON_LEVEL_UP);
                addItemDTO.paramStr1 = m_WeaponName;
                addItemDTO.paramStr2 = "MakeUp4Fail";
                GameItemFactory.GetInstance().AddItem(addItemDTO);
            }

            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }
        public void Recycle()
        {
            if (m_IconParentNode != null)
            {
                WeaponIcon.gameObject.GetComponent<WeaponUI_Item_Follow>().SetFollowNode(null);
                WeaponIcon.gameObject.transform.SetParent(m_IconParentNode, true);
                m_IconParentNode = null;
            }
        }
        public string GetWeaponName()
        {
            return m_WeaponName;
        }
    }
}