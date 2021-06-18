using UnityEngine;

namespace EZ
{
    public class AIHurlAct : AiBase
    {
        public GameObject RugbyBall;
        public Transform FirePoint;
        [SerializeField] private float DtTime = 2;
        [SerializeField] float m_DelayTime = 0.8f;
        [SerializeField] float m_AnimTime = 0.2f;

        private bool m_InAnimAct = false;

        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_InAnimAct = false;
            Vector3 speedVec = m_Player.transform.position - transform.position;
            speedVec = speedVec.normalized;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
        }

        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime = m_CurTime + dtTime;

            if (m_StartAct)
            {
                m_Monster.SetSpeed(Vector2.zero);
                if (dtTime == 0)
                {
                    return;
                }
                HurlToMainPlayer();
            }
            else if(!m_InAnimAct)
            {
                if (m_CurTime >= DtTime)
                {
                 
                    if (!m_StartAct)
                    {
                        if (m_Monster.TriggerFirstAct())
                        {
                            m_CurTime = 0;
                            m_Monster.SetSpeed(Vector2.zero);
                            m_Monster.PlayAnim(GameConstVal.Skill02);
                            m_StartAct = true;
                            m_InAnimAct = true;
                        }
                    }
                }
            }
            else
            {
                m_Monster.SetSpeed(Vector2.zero);
                if (m_CurTime >= m_AnimTime)
                {
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                }
            }
        }
        private void HurlToMainPlayer()
        {
            Vector3 speedVec = m_Player.transform.position - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject rugbyBall = Instantiate(RugbyBall);
                rugbyBall.GetComponent<RugbyBullet>().Init(1, FirePoint,m_Player.transform.position - FirePoint.position);
            }
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
        }
    }
}
