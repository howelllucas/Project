using UnityEngine;

namespace EZ
{
    public class AIDesertEagleAct : AiBase
    {
        public GameObject m_BulletPrefab;
        public GameObject m_FireEffectPrefab;
        public Transform FirePoint;
        [SerializeField] private float DtTime = 2;
        [SerializeField] float m_DelayTime = 2f;
        [SerializeField] float m_AnimTime = 1f;

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
                FireToMainPlayer();

                Vector3 speedVec = m_Player.transform.position - transform.position;
                transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
            }
            else if(!m_InAnimAct)
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
                        m_InAnimAct = true;
                    }
                }
            }
            else
            {
                m_CurTime = m_CurTime + dtTime;
                if (m_CurTime >= m_AnimTime)
                {
                    m_Monster.SetSpeed(Vector2.zero);
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                }
            }
        }
        private void FireToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject rugbyBall = Instantiate(m_BulletPrefab);
                rugbyBall.GetComponent<DesertEagleBullet>().Init(1, FirePoint,0,0,0);
                GameObject fireEffect = Instantiate(m_FireEffectPrefab);
                fireEffect.transform.SetParent(FirePoint, false);
                DelayDestroy delayDestroy = fireEffect.AddComponent<DelayDestroy>();
                delayDestroy.SetLiveTime(0.2f);
            }
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
        }
    }
}
