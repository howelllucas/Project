using UnityEngine;
namespace EZ
{
    public class AiMoveSpeedBuff : AiBaseBuff
    {

        private string m_CurEffectPath = string.Empty;
        public AiMoveSpeedBuff(Monster monster, AiBuffMgr buffMgr, float duration, int buffId, double val,float dtTime)
        {
            Init(monster, buffMgr, duration, buffId, val, dtTime);
        }
        private void CreateEffect()
        {
            bool newReCreate = false;
            if (m_Val > 0 && m_CurEffectPath != EffectConfig.Skill01_2021_2)
            {
                m_CurEffectPath = EffectConfig.Skill01_2021_2;
                newReCreate = true;
            }
            else if(m_Val <= 0 && m_CurEffectPath != EffectConfig.ReduceSpeedEffect)
            {
                m_CurEffectPath = EffectConfig.ReduceSpeedEffect;
                //m_Monster.SetAbsAnimSpeed(0);
                //m_Monster.SetStaticByRigidProp();
                newReCreate = true;
            }
            if (newReCreate)
            {
                if (m_Effect != null)
                {
                    GameObject.Destroy(m_Effect);
                    m_Effect = null;
                }
                GameObject buffEffect = Global.gApp.gResMgr.InstantiateObj(m_CurEffectPath);
                if (m_Val > 0)
                {
                    buffEffect.transform.SetParent(m_Monster.GetHpNode(), true);
                    buffEffect.transform.position = m_Monster.GetHpNode().position;
                }
                else
                {
                    buffEffect.transform.SetParent(m_Monster.GetBodyNode(), false);
                    buffEffect.transform.position = m_Monster.GetBodyNode().position;
                }
                m_Effect = buffEffect;
            }
        }
        public override void Reload(float duration, double val, float dtTime)
        {
            if (val < 0 && m_Val < 0)
            {
                if(val > m_Val)
                {
                    return;
                }
            }
            base.Reload(duration, val, dtTime);
        }
        protected override void SetVal(double val)
        {
            base.SetVal(val);
            m_Monster.SetAnimSpeed((float)(1 + val));
            CreateEffect();
        }
        public override void Destroy()
        {
            if (m_Val <= -1)
            {
                m_Monster.ResetSpeed();
                m_Monster.ResetStaticByRigidProp();
            }
            m_Monster.SetAnimSpeed(1);
            base.Destroy();
        }
    }
}
