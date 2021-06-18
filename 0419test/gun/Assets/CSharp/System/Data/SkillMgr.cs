using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.DataMgr
{
    public class SkillMgr : BaseDataMgr<SkillDTO>
    {
        
        public SkillMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("skill");
            if (m_Data == null)
            {
                m_Data = new SkillDTO();
            }
        }

        public void AfterInit()
        {
            //初始化默认技能状态
            string defaultSkill = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.DEFAULT_SKILL).content;
            //初始化技能
            int state = GetSkillState(Global.gApp.gGameData.SkillData.Get(defaultSkill), Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            if (state == WeaponStateConstVal.NEW)
            {
                SetSkillState(defaultSkill, WeaponStateConstVal.EXIST);
            }
        }

        public ItemDTO GetSkillUpdateItem()
        {
            GeneralConfigItem skillUpdateConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.SKILL_UPDATE);
            Skill_dataItem skillItemConfig = Global.gApp.gGameData.SkillDataConfig.Get(Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() + 1);
            if (skillItemConfig == null)
            {
                return null;
            }
            int itemId = SpecialItemIdConstVal.GOLD;
            double cum = skillItemConfig.coinCost;
            return new ItemDTO(itemId, cum, BehaviorTypeConstVal.OPT_SKILL_UPDATE);
        }

        //武器等级
        public int GetSkillLevel(string skillName)
        {
            SkillItemDTO itemDTO;
            if (!m_Data.itemMap.TryGetValue(skillName, out itemDTO))
            {
                itemDTO = new SkillItemDTO(skillName, 0);
                m_Data.itemMap.Add(skillName, itemDTO);
                SaveData();
            }
            return itemDTO.level;
        }
        public void SetSkillLevel(string skillName, int level)
        {
            int oriLevel = GetSkillLevel(skillName);
            if (oriLevel != level)
            {
                m_Data.itemMap[skillName].level = level;
                SaveData();
            }
        }

        //状态
        public int GetSkillState(string skillName)
        {
            SkillItemDTO itemDTO;
            if (!m_Data.itemMap.TryGetValue(skillName, out itemDTO))
            {
                itemDTO = new SkillItemDTO(skillName, 0);
                m_Data.itemMap.Add(skillName, itemDTO);
                SaveData();
            }
            return itemDTO.state;
        }
        //初始化技能时，检查武器状态是否
        public int GetSkillState(SkillItem itemConfig, int curCharLevel)
        {
            int state = GetSkillState(itemConfig.id);
            if (state == WeaponStateConstVal.NONE )//&& curCharLevel >= itemConfig.level)
            {
                state = WeaponStateConstVal.NEW;
                SetSkillState(itemConfig.id, state);
            }
            return state;
        }
        public void SetSkillState(string skillName, int state)
        {
            int oriState = GetSkillState(skillName);
            if (oriState != state)
            {
                m_Data.itemMap[skillName].state = state;
                SaveData();
            }
        }

        public bool CanLevelUp(SkillItem skillItem)
        {
            ItemDTO reduceDTO = GetSkillUpdateItem();
            if (reduceDTO == null)
            {
                return false;
            }
            int level = GetSkillLevel(skillItem.id);
            SkillItem skillConfig = Global.gApp.gGameData.SkillData.Get(skillItem.id);

            Skill_dataItem levelData = Global.gApp.gGameData.SkillDataConfig.Get(level + 1);
            float[] param = null;
            try
            {
                param = ReflectionUtil.GetValueByProperty<Skill_dataItem, float[]>(skillItem.id, levelData);
            }
            catch (Exception e)
            {
                Debug.LogError(skillItem.id + "在等级表中不存在");
            }

            return skillConfig.weight > 0 && param != null && param.Length > 0 && GameItemFactory.GetInstance().GetItem(reduceDTO.itemId) >= reduceDTO.num;

        }

        //武器升级
        public bool LevelUp(string skillName)
        {
            int oriLevel = GetSkillLevel(skillName);
            if (oriLevel == Global.gApp.gGameData.SkillDataConfig.items.Length)
            {
                return false;
            }
            SetSkillLevel(skillName, ++oriLevel);
            //SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_skill);
            return true;
        }

        public Dictionary<string, int> GetSkillLvMap()
        {
            Dictionary<string, int> skillLvMap = new Dictionary<string, int>();
            foreach (SkillItemDTO itemDTO in m_Data.itemMap.Values)
            {
                if (itemDTO.state > WeaponStateConstVal.NONE)
                {
                    skillLvMap[itemDTO.id] = itemDTO.level;
                }
            }
            return skillLvMap;
        }

        public int GetTimes()
        {
            return m_Data.times;
        }

        public void SetTimes()
        {
            m_Data.times += 1;
            SaveData();
        }

        public float GetHitTimeSkillParam()
        {
            //设置攻速技能
            int skillLevel = GetSkillLevel(GameConstVal.SExHitTime);
            Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            float skillParam = (skillLevelData == null) ? 1f : skillLevelData.skill_exhittime[0];
            return skillParam;
        }

        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }
    }
}
