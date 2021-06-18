

using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public enum AiBuffType
    {
        None = 0,
        FireBuff = 1,
        MoveSpeed = 2,
    }
    public class AiBuffMgr
    {
        private Monster m_Monster;
        private SafeMap<int, AiBaseBuff> m_BuffMap;
        private Action<AiBaseBuff, float> m_Action;

        public AiBuffMgr(Monster monster)
        {
            m_Monster = monster;
            m_BuffMap = new SafeMap<int, AiBaseBuff>();
            m_Action = UpdateImp;
        }
        private void UpdateImp(AiBaseBuff buff,float dt)
        {
            if (!m_Monster.InDeath)
            {
                buff.Update(dt);
            }
        }

        public void Update(float dt)
        {
            m_BuffMap.Update(dt, m_Action);
        }
        public void AddBuff(AiBuffType buffType,float effectTime, double val,float dtTime = 0)
        {
            AiBaseBuff buff;
            int buffId = (int)buffType;
            if (m_BuffMap.TryGetValue(buffId, out buff))
            {
                buff.Reload(effectTime, val, dtTime);
            }
            else
            {
                if (buffType == AiBuffType.MoveSpeed)
                {
                    buff = new AiMoveSpeedBuff(m_Monster, this, effectTime, buffId, val, dtTime);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == AiBuffType.FireBuff)
                {
                    buff = new AiFiringSpeedBuff(m_Monster, this, effectTime, buffId, val, dtTime);
                    m_BuffMap.Add(buffId, buff);
                }
            }
        }
        public bool CheckHasBuff(AiBuffType buffType)
        {
            return CheckHasBuff((int)buffType);
        }

        public bool CheckHasBuff(int buffId)
        {
            return m_BuffMap.Get(buffId) != null;
        }

        public AiBaseBuff GetBuff(AiBuffType buffType)
        {
            return GetBuff((int)buffType);
        }
        public AiBaseBuff GetBuff(int buffId)
        {
            AiBaseBuff buff = null;
            m_BuffMap.TryGetValue(buffId, out buff);
            return buff;
        }

        public void RemoveBuff(AiBuffType buffType)
        {
            RemoveBuff((int)buffType);
        }
        public void RemoveBuff(int buffId)
        {
            AiBaseBuff buff;
            if(m_BuffMap.TryGetValue(buffId,out buff))
            {
                RemoveBuff(buff);
            }
        }
        public void RemoveBuff(AiBaseBuff buff)
        {
            buff.Destroy();
            m_BuffMap.Remove(buff.GetBuffId());
        }
        //=================================
      
        public float GetIncMoveSpeed()
        {
            double val = 0;
            AiBaseBuff buff;
            if (m_BuffMap.TryGetValue((int)AiBuffType.MoveSpeed, out buff))
            {
                val += buff.GetVal();
            }
            return (float)val;
        }
        public void ClearBuff()
        {
            Dictionary<int, AiBaseBuff> allBuff = m_BuffMap.GetAll();
            foreach (KeyValuePair<int, AiBaseBuff> buffKv in  allBuff)
            {
                buffKv.Value.Destroy();
            }
            m_BuffMap.Clear();
        }
    }
}
