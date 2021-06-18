using DG.Tweening;
using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;

namespace EZ
{
    public partial class WeaponRaiseUI_WeaponItemUI
    {
        private ItemItem m_ItemConfig;
        private WeaponRaiseNode m_Parent;
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

        public void UIFresh(bool fromRaiseUpSucess = false)
        {
            if (m_ItemConfig == null)
            {
                return;
            }
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_ItemConfig) == 0)
            {
                WeaponIcon.image.sprite = Resources.Load(m_ItemConfig.image_grow, typeof(Sprite)) as Sprite;
                UpBtn.gameObject.SetActive(true);
                m_RaiseIcon.gameObject.SetActive(false);
            }
            else
            {
                WeaponIcon.image.sprite = Resources.Load(m_ItemConfig.image_grow + "_s", typeof(Sprite)) as Sprite;
                m_UpBtn.gameObject.SetActive(false);
                m_RaiseIcon.gameObject.SetActive(true);
                if(fromRaiseUpSucess)
                {
                    DOTweenAnimation[] dOTweenAnimations = m_RaiseIcon.gameObject.GetComponents<DOTweenAnimation>();
                    foreach(DOTweenAnimation dOTweenAnimation in dOTweenAnimations)
                    {
                        dOTweenAnimation.DORestart();
                    }
                }
            }
            WeaponNameTxt.text.text = m_ItemConfig.gamename;
            
            gameObject.SetActive(true);

            WeaponNameTxt.gameObject.SetActive(true);
            WeaponLvTxt.gameObject.SetActive(true);
            Param1NameTxt.gameObject.SetActive(true);
            WeaponIcon.image.color = ColorUtil.GetColor(ColorUtil.m_DeaultColor);

            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(m_ItemConfig.name);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double[] costValue = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_ItemConfig.name + "_cost", weaponLevelData);


            int itemId = Convert.ToInt32(m_ItemConfig.supercondition[1]);
            double unlockItemNum = Convert.ToDouble(m_ItemConfig.supercondition[2]);
            CmNum1.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemId) < unlockItemNum, m_CM_COLOR);
            CmNum1.text.text = unlockItemNum.ToString();

            double itemCount = GameItemFactory.GetInstance().GetItem(itemId);
            string iconPath = string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId);
            CmIcon1.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(iconPath);



            int itemId2 = Convert.ToInt32(m_ItemConfig.supercondition[3]);
            double unlockItemNum2 = Convert.ToDouble(m_ItemConfig.supercondition[4]);
            CmNum2.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem(itemId2) < unlockItemNum2, m_CM_COLOR);
            CmNum2.text.text = unlockItemNum2.ToString();

            double itemCount2 = GameItemFactory.GetInstance().GetItem(itemId2);
            string iconPath2 = string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId2);
            CmIcon2.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(iconPath2);

            Deal4Level(m_ItemConfig);
        }
        public void Init(ItemItem itemConfig, int showOrder, WeaponRaiseNode parent)
        {
            gameObject.SetActive(true);
            m_Parent = parent;
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
                    return;
                }
                
            }

            UIFresh();
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
        }

        private void InitNode()
        {
            UpBtn.button.onClick.AddListener(OnOpenSuperGunUI);
        }

        private void OnOpenSuperGunUI()
        {
            if (Global.gApp.gSystemMgr.GetWeaponMgr().GetQualityLv(m_ItemConfig) == 0)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.SuperGunUI,this);
            }
        }
        public ItemItem GetItemConfig()
        {
            return m_ItemConfig;
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