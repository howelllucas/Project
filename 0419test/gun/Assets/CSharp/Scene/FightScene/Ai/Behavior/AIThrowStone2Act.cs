using UnityEngine;

namespace EZ
{
    public class AIThrowStone2Act : AiBase
    {
        public GameObject RugbyBall;
        [SerializeField]private GameObject m_HandStone;
        [SerializeField] private float DtTime = 2;
        [SerializeField] float m_DelayTime = 0.9f;
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
            if (dtTime == 0)
            {
                m_Monster.SetSpeed(Vector2.zero);
                return;
            }

            if (m_StartAct)
            {
                m_Monster.SetSpeed(Vector2.zero);
                m_CurTime = m_CurTime + dtTime;
                Vector3 speedVec = m_Player.transform.position - transform.position;
                transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
                HurlToMainPlayer();
            }
            else if(!m_InAnimAct)
            {
                m_CurTime += GetActDtTime();
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
                m_CurTime = m_CurTime + dtTime;
                m_Monster.SetSpeed(Vector2.zero);
                if (m_CurTime >= m_AnimTime)
                {
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_HandStone.SetActive(true);
                    m_Monster.EndFirstAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                }
            }
        }
        private void HurlToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject rugbyBall = Instantiate(RugbyBall);
                rugbyBall.GetComponent<RugbyBullet>().Init(1, m_HandStone.transform, m_Player.transform.position - m_HandStone.transform.position);
                rugbyBall.transform.localScale = transform.localScale;
                m_HandStone.SetActive(false);
            }
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
        }
    }
}
