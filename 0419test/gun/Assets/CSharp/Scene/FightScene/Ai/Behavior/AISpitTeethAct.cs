using UnityEngine;

namespace EZ
{
    public class AISpitTeethAct : AiBase
    {

        [SerializeField] private GameObject m_CobweblBullet;
        [SerializeField] private float DtTime = 3.0f;
        private float m_DelayTime = 1.0f;
        private float m_EndTime = 2f;
        private bool m_InstanceBullet = false;
        private Transform m_FirePoint;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            if (m_FirePoint == null)
            {
                m_FirePoint = monster.transform.Find(GameConstVal.FireNode);
            }
            m_InstanceBullet = false;
        }
        void Update()
        {

            if (m_StartAct)
            {
                ThrowTeethBullet();
            }
            else
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
                        m_InstanceBullet = true; 
                    }
                }
            }
        }

        private void ThrowTeethBullet()
        {
            m_Monster.SetSpeed(Vector2.zero);
            float dtTime = BaseScene.GetDtTime();
            if (dtTime == 0)
            {
                return;
            }
            m_CurTime = m_CurTime + dtTime;
            if (m_CurTime >= m_DelayTime && m_InstanceBullet)
            {
                m_InstanceBullet = false;
                GameObject bullet = Instantiate(m_CobweblBullet);
                bullet.GetComponent<CobweblBullet>().Init(1, m_FirePoint,0,0);
            }
            if(m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndFirstAct();
            }
        }

        public override void Death()
        {
            base.Death();
            m_InstanceBullet = false;
        }
    }
}

