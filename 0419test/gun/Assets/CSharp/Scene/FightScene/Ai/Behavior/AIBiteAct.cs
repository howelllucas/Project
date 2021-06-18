using UnityEngine;

namespace EZ
{
    public class AIBiteAct : AiBase
    {

        [SerializeField] private float DtTime = 3.0f;
        [SerializeField] private float StartSpeed = 10.0f;
        [SerializeField] private float EndSpeed = 0.0f;
        [SerializeField] private float TriggerRange = 5.0f;
        private float m_EndTime = 1f;
        private float m_SqrTriggerRange = 25.0f;
        private Vector3 m_LockSpeed;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            m_SqrTriggerRange = TriggerRange * TriggerRange;
            base.Init(player, wave, monster);
        }
        void Update()
        {
            if (m_StartAct)
            {
                BitePlayer();
            }
            else
            {
                m_CurTime += GetActDtTime();
                if (m_CurTime >= DtTime)
                {
                    Vector3 speedVec = m_Player.transform.position - transform.position;
                    if (speedVec.sqrMagnitude < m_SqrTriggerRange && m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01);
                        m_StartAct = true;
                        transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
                        m_LockSpeed = speedVec.normalized;
                    }
                }
            }
        }

        private void BitePlayer()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if(BaseScene.GetDtTime() <= 0)
            {
                m_Monster.SetSpeed(Vector2.zero);
                return;
            }
            if(m_CurTime <= m_EndTime)
            {
                float rate = m_CurTime / m_EndTime;
                float newSpeed = Mathf.Lerp(StartSpeed,EndSpeed,rate);
                m_Monster.SetSpeed(m_LockSpeed * newSpeed);
            }
            else
            {
                m_Monster.SetSpeed(Vector2.zero);
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndFirstAct();
            }
        }
    }
}

