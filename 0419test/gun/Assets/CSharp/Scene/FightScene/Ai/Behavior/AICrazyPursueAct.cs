using UnityEngine;

namespace EZ
{
    public class AICrazyPursueAct : AiBase
    {

        // Use this for initialization
        [SerializeField] private float Speed;
        [SerializeField] private float DtTime = 2;

        private float m_SkillTime = 1.0f;
        private float m_DelayTime = 0.4f;
        private Vector2 m_LockSpeed;
        private float m_Distance = 3f;
        void Update()
        {

            if (BaseScene.GetDtTime() > 0)
            {

                if (m_StartAct)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    MoveToRole();
                }
                else
                {
                    m_CurTime += GetActDtTime();
                    if (m_CurTime >= DtTime)
                    {
                        if ( m_Monster.TriggerFirstAct())
                        {
                            m_CurTime = 0;
                            m_Monster.PlayAnim(GameConstVal.Skill01);
                            m_StartAct = true;
                            Vector3 posStart = transform.position;
                            Vector3 targetPos = m_Player.transform.position;
                            Vector2 velocity2 = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
                            m_LockSpeed = velocity2.normalized * Speed;
                            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill01_2004);
                            effect.transform.SetParent(m_Monster.transform, false);
                            InstanceEffect();
                        }
                    }
                }
            }
            else if(m_StartAct)
            {
                m_Monster.SetAbsSpeed(Vector2.zero);
            }
        }
        private void MoveToRole()
        {
            if (m_CurTime < m_SkillTime)
            {
                if (m_Player)
                {
                    if (m_CurTime > m_DelayTime)
                    {
                        m_Monster.SetAbsSpeed(m_LockSpeed * BaseScene.TimeScale);
                    }
                    else
                    {
                        m_Monster.SetAbsSpeed(Vector2.zero);
                    }
                }
            }
            else
            {
                m_Monster.PlayAnim(GameConstVal.Run);
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.EndFirstAct();
            }
        }
        private void InstanceEffect()
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Yujing_3001);
            effect.GetComponent<WarningEffect>().Init(m_SkillTime, m_Distance, transform);
        }
    }
}
