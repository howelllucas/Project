using EZ.Data;
using UnityEngine;

namespace EZ
{
    public abstract class AiBaseBuff
    {
        protected GameObject m_Effect;
        protected Monster m_Monster;
        protected float m_Duration = 0;
        protected float m_CurTime = 0;
        protected AiBuffMgr m_BuffMgr;
        protected double m_Val = 0;
        protected float m_DtTime = 0;
        private int m_BuffId;

        public virtual void Init(Monster monster, AiBuffMgr buffMgr,float duration,int buffId, double val,float dtTime = 0)
        {
            m_BuffId = buffId;
            m_BuffMgr = buffMgr;
            m_Monster = monster;
            m_Duration = duration;
            m_CurTime = 0;
            m_DtTime = dtTime;
            SetVal(val);
        }

        public virtual void Init(Monster player, AiBuffMgr buffMgr, float duration, int buffId)
        {
            Init(player, buffMgr, duration, buffId, 0);
        }

        public int GetBuffId()
        {
            return m_BuffId;
        }
        public virtual void Reload(float duration, double val,float dtTime)
        {
            m_Duration = duration;
            m_CurTime = 0;
            m_DtTime = dtTime;
            SetVal(val);
        }

        public virtual void Update(float dt) {
            m_CurTime = m_CurTime + dt;
            if(m_CurTime >= m_Duration)
            {
                m_BuffMgr.RemoveBuff(this);
            }
        }
        
        public double GetVal()
        {
            return m_Val;
        }
        protected virtual void SetVal(double val)
        {
            m_Val = val;
        }
        protected void AddVal(float val)
        {
            m_Val += val;
        }
        protected void DestroyEffect()
        {
            if (m_Effect != null)
            {
                GameObject.Destroy(m_Effect);
                m_Effect = null;
            }
        }
        // just clear something 
        public virtual void Destroy()
        {
            DestroyEffect();
        }
    }
}
