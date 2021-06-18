using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class SkillUp_UI
    {
        SkillItem m_ShowSkillItem;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_ShowSkillItem = arg as SkillItem;
            InitNode();

            base.ChangeLanguage();
        }
        private void InitNode()
        {
            int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(m_ShowSkillItem.id);
            m_Pre_Lv.text.text = (skillLevel - 1).ToString();
            m_Cur_Lv.text.text = (skillLevel).ToString();
            string tips = Global.gApp.gGameData.GetTipsInCurLanguage(m_ShowSkillItem.desc);
            string maxTips = Global.gApp.gGameData.GetTipsInCurLanguage(m_ShowSkillItem.max_desc);
            string title = Global.gApp.gGameData.GetTipsInCurLanguage(m_ShowSkillItem.name);
            Skill_dataItem skillItemConfig = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            float[] curSkillParam = skillItemConfig == null ? null : ReflectionUtil.GetValueByProperty<Skill_dataItem, float[]>(m_ShowSkillItem.id, skillItemConfig);
            Skill_dataItem nextSkillItemConfig = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel + 1);
            float[] nextSkillParam = nextSkillItemConfig == null ? null : ReflectionUtil.GetValueByProperty<Skill_dataItem, float[]>(m_ShowSkillItem.id, nextSkillItemConfig);

            //等级最大
            if (nextSkillParam == null || nextSkillParam.Length == 0)
            {
                m_tip.text.text = string.Format(maxTips, Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(m_ShowSkillItem, curSkillParam));
            } else 
            {
                m_tip.text.text = string.Format(tips, Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(m_ShowSkillItem, curSkillParam)
                    , Global.gApp.gSystemMgr.GetPassMgr().GetParamStr(m_ShowSkillItem, nextSkillParam));
            }
            
            m_titletxt.text.text = title;
            m_CloseBtn.button.onClick.AddListener(CloseCall);
            m_Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(m_ShowSkillItem.icon);
            m_Icon.image.SetNativeSize();
            m_AllNode.gameObject.SetActive(false);
            gameObject.AddComponent<DelayCallBack>().SetAction(DelayCall, 0.2f, true);
        }

        

        private void DelayCall()
        {
            m_AllNode.gameObject.SetActive(true);
        }
        private void CloseCall()
        {
            //InfoCLogUtil.instance.SendClickLog(ClickEnum.SKILL_CONFIRM);
            Global.gApp.gUiMgr.ClosePanel(Wndid.SkillUpUI);
        }
    }
}