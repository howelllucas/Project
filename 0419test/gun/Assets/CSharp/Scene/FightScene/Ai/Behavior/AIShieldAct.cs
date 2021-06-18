using UnityEngine;

namespace EZ
{
    public class AIShieldAct : AiBase
    {

        public GameObject m_ShieldGo;
        public GameObject m_ShieldGoEffect;
        [SerializeField] private float DtTime = 2;
        [SerializeField] private float m_ShieldTime = 2;

        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_ShieldGo.SetActive(false);
            m_ShieldGoEffect.SetActive(false);
        }
        private void Update()
        {
         
            if (!m_StartAct)
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        m_ShieldGo.SetActive(true);
                        m_ShieldGoEffect.SetActive(true);
                    }
                }
            }
            else
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_ShieldTime)
                {
                    m_CurTime = 0;
                    m_StartAct = false;
                    m_ShieldGo.SetActive(false);
                    m_ShieldGoEffect.SetActive(false);
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                }
            }
        }
        public override void Death()
        {
            base.Death();
            m_ShieldGo.SetActive(false);
            m_ShieldGoEffect.SetActive(false);
        }
    }
}
