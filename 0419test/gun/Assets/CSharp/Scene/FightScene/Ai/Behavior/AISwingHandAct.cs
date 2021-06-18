using UnityEngine;

namespace EZ
{
    public class AISwingHandAct : AiBase
    {

        [SerializeField] private GameObject m_SwingHandBullet;
        [SerializeField] private float DtTime = 3.0f;
        private float m_PauseTime = 0.5f;
        private float m_DelayTime = 1.1f;
        private float m_RealEndTime = 2.2f;
        private float m_EndTime = 2f;
        private bool m_InstanceBullet = false;
        private Transform m_FirePoint;
        private Transform m_Effect;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_EndTime = m_RealEndTime + m_PauseTime;
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            if (m_FirePoint == null)
            {
                m_FirePoint = monster.transform.Find(GameConstVal.FireNode2);
                m_Effect = m_FirePoint.Find(GameConstVal.EffectNode);
            }
            m_Effect.gameObject.SetActive(false);
            m_InstanceBullet = false;
        }
        void Update()
        {
            if (m_StartAct)
            {
                SwingHand();
            }
            else
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerSecondAct())
                    {
                        m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill02);
                        m_StartAct = true;
                        m_InstanceBullet = true;
                        m_Effect.gameObject.SetActive(true);
                        m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 0.0f);

                        Vector3 vec  = m_Player.transform.position - transform.position;
                        transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(vec, Vector3.up));
                    }
                }
            }
        }

        private void PlayBackAnim()
        {
            m_CurTime = m_EndTime - m_CurTime;
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, -1.0f);
        }
        private void SwingHand()
        {
            float dtTime = BaseScene.GetDtTime();
            m_Monster.SetSpeed(Vector2.zero);
            if (dtTime == 0)
            {
                return;
            }
            m_CurTime = m_CurTime + dtTime;
            if (m_CurTime >= m_PauseTime && m_InstanceBullet)
            {
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1f);
            }
            if (m_CurTime >= m_DelayTime && m_InstanceBullet)
            {
                m_InstanceBullet = false;
                GameObject bullet = Instantiate(m_SwingHandBullet);
                SwingHandBullet bulletCom = bullet.GetComponent<SwingHandBullet>();
                bulletCom.transform.SetParent(m_FirePoint, false);
                bulletCom.Init(1, m_FirePoint, 0, 0);
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            }
            if(m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Effect.gameObject.SetActive(false);
                m_Monster.EndSecondAct();
            }
        }

        public override void Death()
        {
            base.Death();
            m_Effect.gameObject.SetActive(false);
            m_InstanceBullet = false;
        }
    }
}

