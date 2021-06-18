using EZ.Data;
using UnityEngine;

namespace EZ
{

    public abstract class BaseBuff{

        protected Player m_Player;
        protected float m_Duration = 0;
        protected float m_CurTime = 0;
        protected BuffMgr m_BuffMgr;
        protected GameObject m_Particle;
        protected float m_Val = 0;
        protected float m_Val2 = 0;
        protected float m_Val3 = 0;
        private int m_BuffId;
        public virtual void Init(Player player,BuffMgr buffMgr,float duration,int buffId,float val,float va2 = 0,float val3 = 0)
        {
            m_Val = val;
            m_Val2 = va2;
            m_Val3 = val3;
            m_BuffId = buffId;
            m_BuffMgr = buffMgr;
            m_Player = player;

            m_Duration = duration;
            m_CurTime = 0;
        }

        public virtual void Init(Player player, BuffMgr buffMgr, float duration, int buffId)
        {
            Init(player, buffMgr, duration, buffId, 0);
        }
        protected void CreateParticle(GameObject prefab)
        {
            if(m_Particle != null)
            {
                Object.Destroy(m_Particle);
                m_Particle = null;
            }
            if (prefab != null)
            {
                m_Particle = Object.Instantiate(prefab);
                m_Particle.transform.SetParent(m_Player.EffectNode, false);
            }
        }
        public int GetBuffId()
        {
            return m_BuffId;
        }
        public virtual void Reload(float duration,float val,float val2 = 0 ,float val3 = 0)
        {
            m_Duration = duration;
            m_CurTime = 0;
        }

        public virtual void Update(float dt) {
            m_CurTime = m_CurTime + dt;
            if(m_CurTime >= m_Duration)
            {
                m_BuffMgr.RemoveBuff(this);
            }
        }
        
        public float GetVal3()
        {

            return m_Val3;
        }
        public float GetVal2()
        {
            return m_Val2;
        }
        public float GetVal()
        {
            return m_Val;
        }
        protected void SetVal(float val,float val2 = 0,float val3 = 0)
        {
            m_Val = Mathf.Max(val,m_Val);
            m_Val2 = val2;
            m_Val3 = val3;
        }
        protected void AddVal(float val,float val2 = 0,float val3 = 0)
        {
            m_Val += val;
            m_Val2 += val2;
            m_Val3 += val3;
        }
        // just clear something 
        public virtual void Destroy()
        {
            if(m_Particle != null)
            {
                Object.Destroy(m_Particle);
                m_Particle = null;
            }
        }
    }
}
