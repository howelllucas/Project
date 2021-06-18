using EZ.Data;
using System;
using UnityEngine;

namespace EZ.DataMgr
{
    public class BaseAttrMgr : BaseDataMgr<AttrDTO>
    {

        public bool LvUp
        {
            get;
            set;
        }

        public BaseAttrMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("attr");
            if (m_Data == null)
            {
                m_Data = new AttrDTO();
                SaveData();
            }
        }

        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }

        public void AfterInit()
        {
            FreshEnergy();
        }
        public int GetNextFreshTime()
        {
            GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_LIMIT);
            int limit = int.Parse(limitConfig.content);
            if (m_Data.energy >= limit)
            {
                return -1000;
            }
            double now = DateTimeUtil.GetMills(DateTime.Now);
            GeneralConfigItem freshConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_FRESH_SECONDS);
            GeneralConfigItem increaseTimeConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENEGRY_FRESH_PARAMS);
            int energyFreshParams = int.Parse(increaseTimeConfig.content) * 1000;
            int freshMills = int.Parse(freshConfig.content) * 1000;
            float curFreshTime = freshMills + energyFreshParams * m_Data.energy;
            double dtTime = (now - m_Data.lastEnergyTime);
            if(dtTime > curFreshTime)
            {
                FreshEnergy();
                return GetNextFreshTime();
            }
            else
            {
                return (int)(curFreshTime - dtTime) / 1000;
            }
        }
        //能量刷新
        public void FreshEnergy()
        {
            GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_LIMIT);
            int limit = int.Parse(limitConfig.content);
            double now = DateTimeUtil.GetMills(DateTime.Now);

            if (m_Data.energy >= limit)
            {
                m_Data.lastEnergyTime = now;
                SaveData();
                return;
            }
            GeneralConfigItem freshConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_FRESH_SECONDS);
            GeneralConfigItem increaseTimeConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENEGRY_FRESH_PARAMS);

            int energyFreshParams = int.Parse(increaseTimeConfig.content) * 1000;
            int freshMills = int.Parse(freshConfig.content) * 1000;
            double dtTime = (now - m_Data.lastEnergyTime);
            double newLastEnergyTime = now;
            int curEnergy = m_Data.energy;

            for (int i = curEnergy + 1;i <= limit; i++)
            {
                float curFreshTime = freshMills + energyFreshParams * curEnergy;
                if(dtTime >= curFreshTime)
                {
                    dtTime -= curFreshTime;
                    curEnergy++;
                }
                else
                {
                    newLastEnergyTime = now - dtTime;
                    break;
                }
            }
            bool changed = false;
            if (curEnergy > limit)
            {
                if(m_Data.energy != limit)
                {
                    changed = true;
                }
                m_Data.energy = limit;
                m_Data.lastEnergyTime = now;
            }
            else
            {
                if (m_Data.energy != curEnergy)
                {
                    changed = true;
                }
                m_Data.energy = curEnergy;
                m_Data.lastEnergyTime = newLastEnergyTime;
            }

            if (changed)
            {
                Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.EnergyChanged, m_Data.energy);
            }
            SaveData();
        }
        public double GetHeart()
        {
            return m_Data.redHeart;
        }
        public void SetHeart(double heartCount)
        {
            m_Data.redHeart = heartCount;
        }
        public double GetDiamond()
        {
            return m_Data.diamond;
        }
        public void SetDiamond(double diamond)
        {
            m_Data.diamond = diamond;
        }
        public double GetGold()
        {
            return m_Data.gold;
        }
        public void SetGold(double gold)
        {
            m_Data.gold = gold;
        }
        public double GetExp()
        {
            return m_Data.exp;
        }
        public void SetExp(double exp)
        {
            m_Data.exp = exp;
        }
        public int GetLevel(){
            return m_Data.level;
        }
        public double GetEnergy()
        {
            return m_Data.energy;
        }
        public void SetEnergy(double energy)
        {
            m_Data.energy = (int)energy;
        }
        public double GetMDT()
        {
            return m_Data.MDT;
        }
        public void SetMDT(double MDT)
        {
            m_Data.MDT = MDT;
        }
        public double GetLastEnergyTime()
        {
            return m_Data.lastEnergyTime;
        }
        public void SetLastEnergyTime(double lastEnergyTime)
        {
            m_Data.lastEnergyTime = lastEnergyTime;
        }

        public void ResetLevel(double curExp)
        {
            int curLevel = m_Data.level;
            Hero_dataItem levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(curLevel);
            if (levelConfig == null)
            {
                levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(Global.gApp.gGameData.HeroDataConfig.items.Length);
            }
            //等级发生变化
            if (curExp >= levelConfig.expRequire)
            {
                curLevel++;
                levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(curLevel);
                while (curLevel <= Global.gApp.gGameData.HeroDataConfig.items.Length && curExp >= levelConfig.expRequire)
                {
                    curLevel++;
                    levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(curLevel);
                }
                if (curLevel == Global.gApp.gGameData.HeroDataConfig.items.Length + 1)
                {
                    curLevel = Global.gApp.gGameData.HeroDataConfig.items.Length;
                }
            }
            else
            {
                while (curLevel > 1 && Global.gApp.gGameData.HeroDataConfig.Get(curLevel - 1).expRequire > curExp)
                {
                    curLevel--;
                    levelConfig = Global.gApp.gGameData.HeroDataConfig.Get(curLevel);
                }
                if (curLevel == 0)
                {
                    curLevel = 1;
                }

            }

            m_Data.exp = curExp;
            if(m_Data.level < curLevel)
            {
                LvUp = true;
            }
            m_Data.level = curLevel;
            SaveData();
            Global.gApp.gSystemMgr.GetQuestMgr().QuestChange(FilterTypeConstVal.CHAR_LEVEL, 0f);
            Global.gApp.gMsgDispatcher.Broadcast<double, int>(MsgIds.ExpChanged, curExp, curLevel);
        }
    }
}

