using UnityEngine;

namespace EZ
{
    public class AIStretchHandAct : AiBase
    {

        [SerializeField] private GameObject m_StretchHandBullet;
        [SerializeField] private float DtTime = 3.0f;
        private float m_DelayTime = 0.5f;
        private float m_EndTime = 2;
        private bool m_InstanceBullet = false;
        private Transform m_FirePoint;
        private Transform m_Effect;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            if (m_FirePoint == null)
            {
                m_FirePoint = monster.transform.Find(GameConstVal.FireNode);
                m_Effect = m_FirePoint.Find("Effect");
            }
            m_Effect.gameObject.SetActive(false);
            m_InstanceBullet = false;
        }
        void Update()
        {
            if (m_StartAct)
            {
                StretchHand();
            }
            else
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerFirstAct())
                    {
                        m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        m_InstanceBullet = true;
                        m_Effect.gameObject.SetActive(true);
                        Vector3 vec = m_Player.transform.position - transform.position;
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
        private void StretchHand()
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
                GameObject bullet = Instantiate(m_StretchHandBullet);
                StretchHandBullet bulletCom = bullet.GetComponent<StretchHandBullet>();
                bulletCom.transform.SetParent(m_FirePoint, false);
                bulletCom.Init(1, m_FirePoint,0,0);
                bulletCom.SetCollisionMapAct(PlayBackAnim);
            }
            if(m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_Effect.gameObject.SetActive(false);
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndFirstAct();
            }
        }

        public override void Death()
        {

            base.Death();
            m_InstanceBullet = false;
            m_Effect.gameObject.SetActive(false);
        }
    }
}

