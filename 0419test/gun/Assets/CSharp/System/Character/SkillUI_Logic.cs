using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class SkillUI
    {

        private int m_RandomMax = 0;
        private ItemDTO m_ComsumeItem;
        private RectTransform_Button_Image_Container m_FinaleRectTransform_Container;
        private int m_CurIndex = -1;
        private int m_RanTimes;

        private int m_CurTips = -1;

        private const int m_Mills = 100;

        List<SkillItem> m_RandomList = new List<SkillItem>();

        private int m_LevelSkillTimes = 0;
        private int[] m_LevelSecurity;

        private GameObject m_SelectEffect;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            m_LevelSkillTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.LEVEL_SKILL_TIMES).content);


            //设置各个技能
            m_RandomList.Clear();
            m_RandomMax = 0;
            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
            {
                SetSkillInfo(skillItem);
                if (Global.gApp.gSystemMgr.GetSkillMgr().CanLevelUp(skillItem))
                {
                    m_RandomMax += skillItem.weight;
                    m_RandomList.Add(skillItem);
                }
            }
            SetConsumeCoin();

            NewbieGuideButton[] newBieButtons = this.GetComponentsInChildren<NewbieGuideButton>();
            foreach (NewbieGuideButton newBieButton in newBieButtons)
            {
                newBieButton.OnStart();
            }

            OnTipsBtn(-1);

            base.ChangeLanguage();
        }
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            RegisterListeners();
            UpBtn.button.onClick.AddListener(UpdateBtn);
            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
            {
                RectTransform_Button_Image_Container skillItemRect = ReflectionUtil.GetValueByProperty<SkillUI, RectTransform_Button_Image_Container>("skill_item" + skillItem.location, this);
                skillItemRect.button.onClick.AddListener(() => { OnTipsBtn(skillItem.location); });
            }
            BgBtn.button.onClick.AddListener(() => { OnTipsBtn(-1); });
            string[] levelStrs = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.HP_SKILL_SECURITY_AT_THE_END).contents;
            m_LevelSecurity = new int[levelStrs.Length];
            for (int i = 0; i < levelStrs.Length; i++)
            {
                m_LevelSecurity[i] = int.Parse(levelStrs[i]);
            }
        }
        private void UpdateBtn()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.SKILL_UPDATE);
            OnTipsBtn(-1);

            int nextLevel = (Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() / m_LevelSkillTimes + 1) * m_LevelSkillTimes;
            int timesLimit = (Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel() / m_LevelSkillTimes) * m_LevelSkillTimes;
            if (timesLimit <= Global.gApp.gSystemMgr.GetSkillMgr().GetTimes())
            {
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3045, nextLevel.ToString());
                return;
            }

            if (m_ComsumeItem == null)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4177);
                return;
            }

            if (m_RandomList.Count == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3042);
                return;
            }
            
            if (!GameItemFactory.GetInstance().CanReduce(m_ComsumeItem))
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
                return;
            }

            UpBtn.button.enabled = false;
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            //UpBtn.button.interactable = false;

            int lastIndex = 0;

            //保底逻辑
            bool securiy = false;
            for (int i = 0; i < m_LevelSecurity.Length; i++)
            {
                bool tmp = Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() >= m_LevelSecurity[i] && Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExHp) < i + 1;
                if (tmp)
                {
                    securiy = tmp;
                    break;
                }
            }


            if (securiy)
            {
                for (int i = 0; i < m_RandomList.Count; i++)
                {
                    if (m_RandomList[i].id.Equals(GameConstVal.SExHp))
                    {
                        lastIndex = i;
                        break;
                    }
                }
            }
            else
            {
                int value = UnityEngine.Random.Range(0, m_RandomMax);
                int cur = 0;

                for (int i = 0; i < m_RandomList.Count; i++)
                {
                    if (value >= cur && value < cur + m_RandomList[i].weight)
                    {
                        lastIndex = i;
                        break;
                    }
                    cur += m_RandomList[i].weight;
                }
            }
            SkillItem finalConfig = m_RandomList[lastIndex];
            m_ComsumeItem.paramStr1 = Global.gApp.gSystemMgr.GetSkillMgr().GetTimes().ToString();
            m_ComsumeItem.paramStr2 = finalConfig.id;
            m_ComsumeItem.paramStr3 = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(finalConfig.id).ToString();
            GameItemFactory.GetInstance().ReduceItem(m_ComsumeItem);
            Global.gApp.gSystemMgr.GetSkillMgr().SetTimes();
            Global.gApp.gSystemMgr.GetSkillMgr().LevelUp(finalConfig.id);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
            if (m_RandomList.Count == 1)
            {
                gameObject.AddComponent<DelayCallBack>().SetAction(() => { RandomIndex(lastIndex, 1); }, 0.1f);
            }
            else
            {
                int m_MaxTime = 52;
                DelayCallBack callBack = gameObject.AddComponent<DelayCallBack>();
                callBack.SetAction(
                    () =>
                    {
                        RandomIndex(lastIndex, m_MaxTime);
                        if (m_RanTimes == 15)
                        {
                            callBack.ResteTime();
                            callBack.SetLiveOffset(0);
                        }
                        else if (m_RanTimes == 40)
                        {
                            callBack.SetLiveTime(0);
                            callBack.ResteTime();
                            callBack.SetLiveOffset(0.02f);
                        }
                    ;},
                    0.2f);
                callBack.SetCallTimes(m_MaxTime - 1);
                callBack.SetLiveOffset(-0.016f);
                RandomIndex(lastIndex, m_MaxTime);
            }

        }

        private void RandomIndex(int lastIndex, int m_CallTimes)
        {

            //Debug.Log(string.Format("m_CurIndex = {0} and m_RanTimes = {1} in mills {2}", m_CurIndex, m_RanTimes, DateTimeUtil.GetMills(DateTime.Now)));
            if (m_CurIndex != -1 || m_CallTimes == 1)
            {
                foreach (SkillItem itemConfig in Global.gApp.gGameData.SkillData.items)
                {
                    RectTransform_Button_Image_Container itemRect = ReflectionUtil.GetValueByProperty<SkillUI, RectTransform_Button_Image_Container>("skill_item" + itemConfig.location, this);
                    itemRect.rectTransform.GetChild(3).gameObject.SetActive(false);
                }
                if (m_RanTimes < m_CallTimes - 1)
                {
                    int newIndex = UnityEngine.Random.Range(0, m_RandomList.Count - 1);
                    if (newIndex >= m_CurIndex)
                    {
                        m_CurIndex = newIndex + 1;
                    }
                    else
                    {
                        m_CurIndex = newIndex;
                    }
                }
                else
                {
                    m_CurIndex = lastIndex;
                    SkillItem finalConfig = m_RandomList[m_CurIndex];
                    //Global.gApp.gSystemMgr.GetSkillMgr().SetTimes();
                    //Global.gApp.gSystemMgr.GetSkillMgr().LevelUp(finalConfig.id);
                    SetConsumeCoin();
                    SetSkillInfo(finalConfig);

                    m_RandomList.Clear();
                    m_RandomMax = 0;
                    foreach (SkillItem item in Global.gApp.gGameData.SkillData.items)
                    {
                        if (Global.gApp.gSystemMgr.GetSkillMgr().CanLevelUp(item))
                        {
                            m_RandomMax += item.weight;
                            m_RandomList.Add(item);
                        }
                    }

                    m_RanTimes = 0;
                    RectTransform_Button_Image_Container finalSkillItemRect = ReflectionUtil.GetValueByProperty<SkillUI, RectTransform_Button_Image_Container>("skill_item" + finalConfig.location, this);
                    finalSkillItemRect.rectTransform.GetChild(3).gameObject.SetActive(true);
                    m_FinaleRectTransform_Container = finalSkillItemRect;

                    //UpBtn.button.interactable = true;

                    if (finalConfig != null)
                    {
                        if (finalConfig.id == GameConstVal.SExHp)
                        {
                            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                            {
                                FreshShowUi(EffectConfig.UI_Skill_1);
                            }, 0.5f);
                        }
                        else
                        {
                            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
                            {
                                FreshShowUi(EffectConfig.UI_Skill_2);
                            }, 0.5f);

                        }
                    }

                    DelayCallBack dcb = gameObject.AddComponent<DelayCallBack>();
                    dcb.SetAction(() =>
                    {
                        Global.gApp.gUiMgr.OpenPanel(Wndid.SkillUpUI, finalConfig);

                        if (m_SelectEffect != null)
                        {
                            Destroy(m_SelectEffect);
                            m_SelectEffect = null;
                        }

                        Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
                    }, 1f);

                    UpBtn.button.enabled = true;
                    return;
                }

            }
            else
            {
                m_CurIndex = UnityEngine.Random.Range(0, m_RandomList.Count);
            }
            SkillItem skillItem = m_RandomList[m_CurIndex];
            RectTransform_Button_Image_Container skillItemRect = ReflectionUtil.GetValueByProperty<SkillUI, RectTransform_Button_Image_Container>("skill_item" + skillItem.location, this);
            skillItemRect.rectTransform.GetChild(3).gameObject.SetActive(true);

            m_RanTimes++;

        }

        public void FreshShowUi(string effectPath)
        {
            if (m_FinaleRectTransform_Container != null)
            {
                m_SelectEffect = Global.gApp.gResMgr.InstantiateObj(effectPath);
                m_SelectEffect.transform.SetParent(m_FinaleRectTransform_Container.gameObject.transform, false);
                //effect.AddComponent<DelayDestroy>().SetLiveTime(2);
                m_FinaleRectTransform_Container.rectTransform.GetChild(3).gameObject.SetActive(false);
            }
        }
        private void SetConsumeCoin()
        {
            m_ComsumeItem = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillUpdateItem();
            CmIcon.gameObject.SetActive(m_ComsumeItem != null);
            CmNum.gameObject.SetActive(m_ComsumeItem != null);
            Maxtxt.gameObject.SetActive(m_ComsumeItem == null);
            dotxt.gameObject.SetActive(m_ComsumeItem != null);
            if (m_ComsumeItem != null)
            {
                int itemId = m_ComsumeItem.itemId;
                CmIcon.gameObject.SetActive(true);
                CmIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, itemId));
                CmNum.text.text = UiTools.FormateMoney(m_ComsumeItem.num);
                CmNum.text.color = ColorUtil.GetTextColor(GameItemFactory.GetInstance().GetItem((int)m_ComsumeItem.itemId) < m_ComsumeItem.num,
                    Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.YELLOW_COLOR).content);
            }

        }

        private void SetSkillInfo(SkillItem skillItem)
        {
            RectTransform_Button_Image_Container skillItemRect = ReflectionUtil.GetValueByProperty<SkillUI, RectTransform_Button_Image_Container>("skill_item" + skillItem.location, this);
            skillItemRect.rectTransform.GetChild(1).GetComponent<Image>().sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(skillItem.icon);
            int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(skillItem.id);
            skillItemRect.rectTransform.GetChild(2).GetChild(1).GetComponent<Text>().text = skillLevel.ToString();
            skillItemRect.rectTransform.GetChild(5).GetComponent<Text>().text = Global.gApp.gGameData.GetTipsInCurLanguage(skillItem.name);


            skillItemRect.rectTransform.GetChild(4).gameObject.SetActive(skillLevel == 0);
            skillItemRect.rectTransform.GetChild(1).gameObject.SetActive(skillLevel > 0);
            skillItemRect.rectTransform.GetChild(2).gameObject.SetActive(skillLevel > 0);

            int[] ints = new int[] { 0, 1, 4 };
            if (skillLevel == 0)
            {
                foreach (int i in ints)
                {
                    Color color = skillItemRect.rectTransform.GetChild(i).GetComponent<Image>().color;
                    color.a = 0.5f;
                    skillItemRect.rectTransform.GetChild(i).GetComponent<Image>().color = color;
                }

            }
            else
            {
                foreach (int i in ints)
                {
                    Color color = skillItemRect.rectTransform.GetChild(i).GetComponent<Image>().color;
                    color.a = 1f;
                    skillItemRect.rectTransform.GetChild(i).GetComponent<Image>().color = color;
                }

            }

            int nextLevel = (Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() / m_LevelSkillTimes + 1) * m_LevelSkillTimes;
            int timesLimit = (Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel() / m_LevelSkillTimes) * m_LevelSkillTimes;

            UpBtn.gameObject.SetActive(timesLimit > Global.gApp.gSystemMgr.GetSkillMgr().GetTimes());
            TimesText.gameObject.SetActive(timesLimit <= Global.gApp.gSystemMgr.GetSkillMgr().GetTimes());
            TimesText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(3045), nextLevel);


            string tips = Global.gApp.gGameData.GetTipsInCurLanguage(skillItem.desc);
            string noLearnTips = Global.gApp.gGameData.GetTipsInCurLanguage(skillItem.desc_nolearn);
            string maxTips = Global.gApp.gGameData.GetTipsInCurLanguage(skillItem.max_desc);

            Skill_dataItem skillItemConfig = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            float[] curSkillParam = skillItemConfig == null ? null : ReflectionUtil.GetValueByProperty<Skill_dataItem, float[]>(skillItem.id, skillItemConfig);
            Skill_dataItem nextSkillItemConfig = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel + 1);
            float[] nextSkillParam = nextSkillItemConfig == null ? null : ReflectionUtil.GetValueByProperty<Skill_dataItem, float[]>(skillItem.id, nextSkillItemConfig);
            Transform tip = transform.Find("Tips").Find("m_tip" + skillItem.location);

            if (nextSkillParam == null || nextSkillParam.Length == 0)
            {
                tip.GetComponentInChildren<Text>().text = string.Format(maxTips, Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(skillItem, curSkillParam));
            }
            else if (skillLevel == 0)
            {
                tip.GetComponentInChildren<Text>().text = string.Format(noLearnTips, Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(skillItem, nextSkillParam));
            }
            else
            {
                tip.GetComponentInChildren<Text>().text = string.Format(tips, Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(skillItem, curSkillParam)
                    , Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(skillItem, nextSkillParam));
            }

            m_TotalTimes.text.text = Global.gApp.gSystemMgr.GetSkillMgr().GetTimes().ToString();

        }

        private void OnTipsBtn(int index)
        {

            if (m_CurTips >= 0 && m_CurTips <= 8)
            {
                Transform tip = transform.Find("Tips").Find("m_tip" + m_CurTips);
                tip.gameObject.SetActive(false);
            }
            m_CurTips = index;
            if (m_CurTips >= 0 && m_CurTips <= 8)
            {
                Transform tip = transform.Find("Tips").Find("m_tip" + m_CurTips);
                tip.gameObject.SetActive(true);
                //InfoCLogUtil.instance.SendClickLog(ClickEnum.SKILL_INFO);
            }
        }

        private void RegisterListeners()
        {
        }
        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}