//using EZ.Data;
//using EZ.DataMgr;
//using EZ.Weapon;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//namespace EZ
//{
//    public partial class CharUI_skill
//    {

//        private SkillItem m_CurSkill;
//        private SkillItem m_LaskSkill;
//        private int m_CurLevel;
//        private Transform m_WeaponBip;

//        private int m_DefaultLocation = 5;
//        private static string m_White = "#FFFFFF";
//        private static string m_Green = "#B4D530";
//        private static string m_NameSelectGreen = "#4FD684"; 
//        private static string m_NameDefault = "#B8C3D9"; 
//        private static string m_Red = "#FE4648";

//        public override void Init<T>(string name, UIInfo info, T arg)
//        {
//            base.Init(name, info, arg);
//            InitNode();
//            RegisterListeners();

//        }

//        private void UIFresh()
//        {
//            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
//            {
//                RectTransform_Container skillUp = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Container>("SkillUp" + skillItem.location, this);
//                if (Global.gApp.gSystemMgr.GetSkillMgr().CanLevelUp(skillItem))
//                {
//                    skillUp.gameObject.SetActive(true);
//                } else
//                {
//                    skillUp.gameObject.SetActive(false);
//                }
//            }

//            //设置货币
//            LevelItem levelData = Global.gApp.gGameData.LevelData.Get(m_CurLevel);
//            float[] costValue;
//            if (levelData == null)
//            {
//                costValue = m_CurSkill.unlock_cost;
//            }
//            else
//            {
//                costValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id + "_cost", levelData);
//            }
//            if (GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1])
//            {
//                SkillUp.gameObject.SetActive(false);
//            } else
//            {
//                SkillUp.gameObject.SetActive(true);
//            }
//            CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1], null);

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


//            Tabright.button.onClick.AddListener(OnLevelBtn);
//            UpBtn.button.onClick.AddListener(OnUpdateBtn);

//            //设置各个技能
//            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
//            {
//                int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//                SetSkillInfo(curCharLevel, skillItem);
//            }

//            m_LaskSkill = m_CurSkill;
//            if (Global.gApp.gGameData.SkillLocationMapData.TryGetValue(m_DefaultLocation, out m_CurSkill))
//            {
//                ChangeInfo();
//                RectTransform_Image_Container curSkillBg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("skillbg" + m_DefaultLocation, this);
//                curSkillBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(m_CurSkill.bgs);
//                RectTransform_Text_Container skillNameTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skillnametxt" + m_DefaultLocation, this);
//                skillNameTxt.text.color = ColorUtil.GetColor(m_NameSelectGreen);
//            }

//            UIFresh();
//        }

//        //设置技能显示信息
//        private void SetSkillInfo(int curCharLevel, SkillItem skillItem)
//        {
//            RectTransform_Button_Image_Container skillIcon = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Button_Image_Container>("Skillicon" + skillItem.location, this);
//            RectTransform_Text_Container skillNameTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skillnametxt" + skillItem.location, this);
//            RectTransform_Text_Container openLvTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Openlvtxt" + skillItem.location, this);
//            RectTransform_Text_Button_Container closeTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Button_Container>("Closetxt" + skillItem.location, this);
//            RectTransform_Text_Container lvTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skilllvtxt" + skillItem.location, this);
//            m_CurLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(skillItem.id);
//            RectTransform_Image_Container skilllvbg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("Skilllvbg" + skillItem.location, this);


//            if (skillIcon != null)
//            {
                
//                if (Global.gApp.gSystemMgr.GetSkillMgr().GetSkillState(skillItem, curCharLevel) == WeaponStateConstVal.NONE)
//                {
//                    skillIcon.gameObject.SetActive(false);
//                    closeTxt.gameObject.SetActive(true);
//                    skilllvbg.gameObject.SetActive(false);
//                    openLvTxt.text.text = skillItem.level + " open";
//                    //锁定状态下，不显示名称
//                    skillNameTxt.text.text = null;
//                }
//                else
//                {
//                    skillIcon.gameObject.SetActive(true);
//                    closeTxt.gameObject.SetActive(false);
//                    lvTxt.gameObject.SetActive(true);
//                    skillIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(skillItem.icon);
//                    SetLvColor(lvTxt, skilllvbg);

//                    TipsItem nameTipsItem = Global.gApp.gGameData.TipsData.Get(skillItem.name);
//                    if (nameTipsItem != null && skillNameTxt != null)
//                    {
//                        skillNameTxt.text.text = nameTipsItem.txtcontent;
//                    }

//                    //新武器
//                    DealNewWeapon(curCharLevel, skillItem);
//                }
//                skillIcon.button.onClick.AddListener(OnSkillBtn);
//            }

//            if (closeTxt != null)
//            {
//                closeTxt.button.onClick.AddListener(OnSkillBtn);
//            }
//        }

//        private void DealNewWeapon(int curCharLevel, SkillItem skillItem)
//        {
//            RectTransform_Image_Container curSkillBg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("skillbg" + skillItem.location, this);
//            RectTransform_Image_Container newSkill = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("Newskill" + skillItem.location, this);
//            RectTransform_Image_Container newIMG = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("NewIMG" + skillItem.location, this);
//            if (Global.gApp.gSystemMgr.GetSkillMgr().GetSkillState(skillItem, curCharLevel) == WeaponStateConstVal.NEW)
//            {
//                curSkillBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(skillItem.bgn);
//                newSkill.gameObject.SetActive(true);
//                newIMG.gameObject.SetActive(true);
//            }
//            else
//            {
//                curSkillBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(skillItem.bg);
//                newSkill.gameObject.SetActive(false);
//                newIMG.gameObject.SetActive(false);
//            }
//        }

//        //设置等级字体颜色
//        private void SetLvColor(RectTransform_Text_Container lvTxt, RectTransform_Image_Container skilllvbg)
//        {
//            lvTxt.text.text = m_CurLevel.ToString();
//            if (m_CurLevel == 0)
//            {
//                skilllvbg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(String.Format(CommonResourceConstVal.RESOURCE_SKILL_PATH, "lvbg1"));
//                //lvTxt.text.color = ColorUtil.GetColor(m_Red);
//            }
//            else
//            {
//                skilllvbg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(String.Format(CommonResourceConstVal.RESOURCE_SKILL_PATH, "lvbg2"));
//                //lvTxt.text.color = ColorUtil.GetColor(m_Green);
//            }
//        }

//        //选择技能
//        private void OnSkillBtn()
//        {
//            GameObject buttonObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
//            string location = System.Text.RegularExpressions.Regex.Replace(buttonObj.name, @"[^0-9]+", GameConstVal.EmepyStr);
//            if (location.Equals(m_CurSkill.location.ToString()))
//            {
//                return;
//            }
//            int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//            SkillItem model = Global.gApp.gGameData.SkillLocationMapData[int.Parse(location)];
//            if (curCharLevel < model.level)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3076, model.level.ToString());
//                return;
//            }
//            m_LaskSkill = m_CurSkill;
//            RectTransform_Image_Container lastSkillBg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("skillbg" + m_LaskSkill.location, this);
//            lastSkillBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(m_LaskSkill.bg);
//            RectTransform_Text_Container lastNameTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skillnametxt" + m_LaskSkill.location, this);
//            lastNameTxt.text.color = ColorUtil.GetColor(m_NameDefault);
//            if (Global.gApp.gGameData.SkillLocationMapData.TryGetValue(int.Parse(location), out m_CurSkill))
//            {
//                ChangeInfo();

//                if (Global.gApp.gSystemMgr.GetSkillMgr().GetSkillState(m_CurSkill.id) == WeaponStateConstVal.NEW)
//                {
//                    Global.gApp.gSystemMgr.GetSkillMgr().SetSkillState(m_CurSkill.id, WeaponStateConstVal.EXIST);
//                    DealNewWeapon(curCharLevel, m_CurSkill);
//                }

//                RectTransform_Image_Container curSkillBg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("skillbg" + m_CurSkill.location, this);
//                curSkillBg.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(m_CurSkill.bgs);
//                RectTransform_Text_Container skillNameTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skillnametxt" + m_CurSkill.location, this);
//                skillNameTxt.text.color = ColorUtil.GetColor(m_NameSelectGreen);

//            }

//            UIFresh();
//        }

//        //改变介绍信息
//        private void ChangeInfo()
//        {
//            int curCharLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
//            if (curCharLevel < m_CurSkill.level)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3018, (m_CurSkill.level - curCharLevel).ToString());
//                m_CurSkill = m_LaskSkill;
//                return;
//            }

//            TipsItem nameTipsItem = Global.gApp.gGameData.TipsData.Get(m_CurSkill.name);
//            if (nameTipsItem != null)
//            {
//                Skill_S_Name.text.text = nameTipsItem.txtcontent;
//            }
//            else
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3017);
//            }
//            m_CurLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(m_CurSkill.id);

//            RectTransform_Text_Container lvTxt = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Text_Container>("Skilllvtxt" + m_CurSkill.location, this);
//            RectTransform_Image_Container skilllvbg = ReflectionUtil.GetValueByProperty<CharUI_skill, RectTransform_Image_Container>("Skilllvbg" + m_CurSkill.location, this);

//            SetLvColor(lvTxt, skilllvbg);

//            //设置货币
//            LevelItem levelData = Global.gApp.gGameData.LevelData.Get(m_CurLevel);
//            float[] costValue;
//            if (levelData == null)
//            {
//                costValue = m_CurSkill.unlock_cost;
//                Skill_S_Level.text.text = "LV : --";
//            }
//            else
//            {
//                costValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id + "_cost", levelData);
//                Skill_S_Level.text.text = "LV : " + m_CurLevel;
//            }

//            CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, costValue[0]));
//            CmNum.text.text = UiTools.FormateMoney(costValue[1]);
//            CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1], null);

//            TipsItem descTipsItem = Global.gApp.gGameData.TipsData.Get(m_CurSkill.desc);
//            if (descTipsItem != null)
//            {
//                LevelItem nextWeaponLevelData = Global.gApp.gGameData.LevelData.Get(m_CurLevel + 1);
//                float[] nextValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id, nextWeaponLevelData);
//                if (nextValue == null || nextValue.Length == 0)
//                {
//                    descTipsItem = Global.gApp.gGameData.TipsData.Get(1004);
//                    Skill_S_Des.text.text = descTipsItem.txtcontent;
//                }
//                else
//                {
//                    float[] initValue = m_CurSkill.init_param;
//                    float[] curValue = m_CurSkill.init_param;
//                    if (levelData != null)
//                    {
//                        curValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id, levelData);
//                    }
//                    if (m_CurSkill.percentage == 1)
//                    {
//                        if (levelData == null)
//                        {
//                            TipsItem noLearnTips = Global.gApp.gGameData.TipsData.Get(m_CurSkill.desc_nolearn);
//                            Skill_S_Des.text.text = string.Format(noLearnTips.txtcontent, (nextValue[0] - initValue[0]).ToString());
//                        } else
//                        {
//                            Skill_S_Des.text.text = string.Format(descTipsItem.txtcontent, (curValue[0] - initValue[0]).ToString()
//                            , (nextValue[0] - initValue[0]).ToString());
//                        }
                            
//                    }
//                    else
//                    {
//                        if (levelData == null)
//                        {
//                            TipsItem noLearnTips = Global.gApp.gGameData.TipsData.Get(m_CurSkill.desc_nolearn);
//                            Skill_S_Des.text.text = string.Format(noLearnTips.txtcontent, Math.Round((nextValue[0] - initValue[0]) * m_CurSkill.percentage, 0) + "%");
//                        }
//                        else
//                        {
//                            Skill_S_Des.text.text = string.Format(descTipsItem.txtcontent, Math.Round((curValue[0] - initValue[0]) * m_CurSkill.percentage, 0) + "%"
//                            , Math.Round((nextValue[0] - initValue[0]) * m_CurSkill.percentage, 0) + "%");
//                        }
                        
//                    }

//                }
//            }
//            else
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3017);
//            }
//            Skill_S_Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(m_CurSkill.icon);
//        }

//        //点击升级按钮
//        private void OnUpdateBtn()
//        {

//            if (m_CurLevel == Global.gApp.gGameData.LevelData.items.Length)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1004);
//                return;
//            }
//            LevelItem levelItem = Global.gApp.gGameData.LevelData.Get(m_CurLevel);
//            LevelItem nextLevelItem = Global.gApp.gGameData.LevelData.Get(m_CurLevel + 1);
//            float[] nextValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id, nextLevelItem);
//            if (nextValue == null || nextValue.Length == 0)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1004);
//                return;
//            }

//            bool reduceResult = false;

//            LevelItem levelData = Global.gApp.gGameData.LevelData.Get(m_CurLevel);
//            float[] costValue;
//            if (levelData == null)
//            {
//                costValue = m_CurSkill.unlock_cost;
//            }
//            else
//            {
//                costValue = ReflectionUtil.GetValueByProperty<LevelItem, float[]>(m_CurSkill.id + "_cost", levelData);
//            }

//            ItemDTO reduceItemDTO = new ItemDTO((int)costValue[0], Convert.ToSingle(costValue[1]), BehaviorTypeConstVal.OPT_SKILL_LEVEL_UP);
//            reduceItemDTO.paramStr1 = m_CurSkill.id;
//            GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
//            reduceResult = reduceItemDTO.result;

//            if (!reduceResult)
//            {
//                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
//                return;
//            }
            
//            bool levelUpResult = Global.gApp.gSystemMgr.GetSkillMgr().LevelUp(m_CurSkill.id);
//            if (levelUpResult)
//            {
//                EffectItem effectItem = Global.gApp.gGameData.EffectData.Get(EffectConstVal.SMALL_LEVEL_IP);
//                GameObject effect = UiTools.GetEffect(effectItem.path, transform, new Vector3(0, 0, 5));
//                GameObject.Destroy(effect, 3f);
//                ChangeInfo();
//            }
//            else
//            {
//                ItemDTO addItemDTO = new ItemDTO((int)costValue[0], Convert.ToSingle(CmNum.text.text), BehaviorTypeConstVal.OPT_SKILL_LEVEL_UP);
//                reduceItemDTO.paramStr1 = m_CurSkill.id;
//                reduceItemDTO.paramStr2 = "MakeUp4Fail";
//                GameItemFactory.GetInstance().AddItem(addItemDTO);
//            }

//            UIFresh();

//        }

//        //点击等级页签
//        private void OnLevelBtn()
//        {
//            OnClick();

//            Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3078);
//            return;

//            Global.gApp.gUiMgr.ClosePanel(Wndid.CharacterSkillPanel);
//            Global.gApp.gUiMgr.OpenPanel(Wndid.CharacterLevelPanel);
//        }

//        private void ExpChanged(double newExp, int newLevel)
//        {
//            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
//            {
//                SetSkillInfo(newLevel, skillItem);
//            }
//        }

//        private void RegisterListeners()
//        {
//            Global.gApp.gMsgDispatcher.AddListener<double, int>(MsgIds.ExpChanged, ExpChanged);
//            Global.gApp.gMsgDispatcher.AddListener(MsgIds.UIFresh, UIFresh);
//        }


//        private void UnRegisterListeners()
//        {
//            Global.gApp.gMsgDispatcher.RemoveListener<double, int>(MsgIds.ExpChanged, ExpChanged);
//            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.UIFresh, UIFresh);
//        }

//        public override void Release()
//        {
//            UnRegisterListeners();
//            base.Release();
//        }
//    }
//}
