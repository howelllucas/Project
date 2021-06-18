using UnityEngine;
namespace EZ
{
    public class AiFiringSpeedBuff : AiBaseBuff
    {
        private float m_DamageTime;
        public AiFiringSpeedBuff(Monster monster, AiBuffMgr buffMgr, float duration, int buffId, double val,float dtTime)
        {
            Init(monster, buffMgr, duration, buffId, val, dtTime);
        }
        public override void Reload(float duration, double val,float dtTime)
        {
            base.Reload(duration, val,dtTime);
        }
        private void CreateEffect()
        {
            if (m_Effect == null)
            {
                GameObject buffEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.Firegun_Hit]);
                buffEffect.transform.SetParent(m_Monster.GetBodyNode(), false);
                buffEffect.transform.position = m_Monster.GetBodyNode().position;
                m_Effect = buffEffect;
            }
        }
        public override void Update(float dt)
        {
            m_DamageTime += dt;
            if (m_DamageTime >= m_DtTime)
            {
                m_Monster.OnDamage(m_Val);
                m_DamageTime -= m_DtTime; ;
            }
            base.Update(dt);
        }
        protected override void SetVal(double val)
        {
            base.SetVal(val);
            CreateEffect();
        }
        public override void Destroy()
        {
            base.Destroy();
        }
    }
}
