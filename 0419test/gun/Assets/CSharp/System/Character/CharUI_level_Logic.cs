//using EZ.Data;
//using EZ.DataMgr;
//using EZ.Weapon;
//using System.Collections.Generic;
//using UnityEngine;

//namespace EZ
//{
//    public partial class CharUI_level
//    {
//        private Transform m_WeaponBip;
//        private List<CharUI_level_ItemUI> m_ShowItemList = new List<CharUI_level_ItemUI>();

//        public override void Init<T>(string name, UIInfo info, T arg)
//        {
//            base.Init(name, info, arg);
//            InitNode();
//            RegisterListeners();

//            PageLeft.button.onClick.AddListener(OnLeft);
//            PageLeft.longPressEventTrigger.onLongPress.AddListener(OnLeft);
//            PageRight.button.onClick.AddListener(OnRight);
//            PageRight.longPressEventTrigger.onLongPress.AddListener(OnRight);

//            Tabright.button.onClick.AddListener(OnSkillBtn);
//        }

//        private void OnSkillBtn()
//        {
//            int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//            if (curCharLevel < Global.gApp.gGameData.MinSkillOpenLevel)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3077, Global.gApp.gGameData.MinSkillOpenLevel.ToString());
//                return;
//            }
//            OnClick();
//            Global.gApp.gUiMgr.ClosePanel(Wndid.CharacterLevelPanel);
//            Global.gApp.gUiMgr.OpenPanel(Wndid.CharacterSkillPanel);
//            Tableft_on.gameObject.SetActive(false);
//            Tabright_on.gameObject.SetActive(true);
//        }

//        //左滑
//        private void OnLeft()
//        {
//            Vector3 v3 = Content.rectTransform.localPosition;
//            v3.x += 20f;
//            Content.rectTransform.localPosition = v3;
//        }

//        //右滑
//        private void OnRight()
//        {
//            Vector3 v3 = Content.rectTransform.localPosition;
//            v3.x -= 20f;
//            Content.rectTransform.localPosition = v3;
//        }

        
//        private void InitNode()
//        {
//            //设置武器
//            m_WeaponBip = transform.Find("MainRoleNode/hero/weapon_bip");

//            string m_CurMainWeaponName = Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon();

//            GameObject weapon = Global.gApp.gResMgr.InstantiateObj("Prefabs/Weapon/" + m_CurMainWeaponName);
//            weapon.layer = LayerMask.NameToLayer("UI");
//            weapon.transform.SetParent(m_WeaponBip, false);
//            weapon.GetComponent<Gun>().enabled = false;

//            double curExp = GameItemFactory.GetInstance().GetItem(SpecialItemIdConstVal.EXP);
//            Global.gApp.gSystemMgr.GetBaseAttrMgr().ResetLevel(curExp);
//            int curLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//            ExpChanged(curExp, curLevel);

//            ShowScrollItem(ChargeShowTypeConstVal.EXP);

//        }

//        private void ShowScrollItem(int itemType)
//        {
//            ItemUI.gameObject.SetActive(false);
//            if (!Global.gApp.gGameData.ChargeTypeMapData.ContainsKey(itemType))
//            {
//                return;
//            }

//            for (int i = Global.gApp.gGameData.ChargeTypeMapData[itemType].Count - 1; i >= 0; i--)
//            {
//                ChargeItem itemConfig = Global.gApp.gGameData.ChargeTypeMapData[itemType][i];
//                CharUI_level_ItemUI itemUI = ItemUI.GetInstance();
//                itemUI.Init(itemConfig);
//                m_ShowItemList.Add(itemUI);

//            }
//        }


//        private void ExpChanged(double newExp, int newLevel)
//        {       

//            LevelTxt.text.text = "LV:" + newLevel.ToString();
//            float beginExp;
//            if (newLevel == 1)
//            {
//                beginExp = 0f;
//            }
//            else
//            {
//                LevelItem lastConfig = Global.gApp.gGameData.LevelData.Get(newLevel - 1);
//                beginExp = lastConfig.presentExp;
//            }
//            LevelItem curConfig = Global.gApp.gGameData.LevelData.Get(newLevel);
//            Exp.image.fillAmount = (float)(newExp - beginExp) / (curConfig.presentExp - beginExp);
//            ExpDetail.text.text = (newExp - beginExp).ToString() + "/" + (curConfig.presentExp - beginExp).ToString();

//            foreach (CharUI_level_ItemUI itemUI in m_ShowItemList)
//            {
//                itemUI.ResetColor();
//            }
//        }

//        private void ResetColor(double val)
//        {
//            foreach (CharUI_level_ItemUI itemUI in m_ShowItemList)
//            {
//                itemUI.ResetColor();
//            }
//        }

//        private void RegisterListeners()
//        {
//            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.DiamondChanged, ResetColor);
//            Global.gApp.gMsgDispatcher.AddListener<double>(MsgIds.GoldChanged, ResetColor);
//            Global.gApp.gMsgDispatcher.AddListener<double, int>(MsgIds.ExpChanged, ExpChanged);
//            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
//        }


//        private void UnRegisterListeners()
//        {
//            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.DiamondChanged, ResetColor);
//            Global.gApp.gMsgDispatcher.RemoveListener<double>(MsgIds.GoldChanged, ResetColor);
//            Global.gApp.gMsgDispatcher.RemoveListener<double, int>(MsgIds.ExpChanged, ExpChanged);
//            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
//        }

//        private void UIFresh()
//        {
//            foreach (CharUI_level_ItemUI itemUI in m_ShowItemList)
//            {
//                itemUI.UIFresh();
//            }
//        }

//        public override void Release()
//        {
//            UnRegisterListeners();
//            base.Release();
//        }
//    }
//}
