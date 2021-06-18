using UnityEngine;

namespace EZ
{
    public class AISprayVenomAct : AiBase
    {
        public GameObject BulletPrefab;
        public GameObject FireEffect;
        public Transform FirePoint;
        [SerializeField] private float DtTime = 2;
        [SerializeField] float m_DelayTime = 2f;
        [SerializeField] float m_AnimTime = 1f;

        private bool m_InAnimAct = false;
        float m_ResetAnimTime = 2.1f;
        private bool m_ResetAnim = true;

        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_InAnimAct = false;
            m_ResetAnim = true;
            Vector3 speedVec = m_Player.transform.position - transform.position;
            speedVec = speedVec.normalized;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
        }

        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (m_StartAct)
            {
                m_Monster.SetSpeed(Vector2.zero);
                if (dtTime == 0)
                {
                    return;
                }
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
                        m_ResetAnim = true;
                    }
                }
            }
            else if(m_InAnimAct)
            {
                m_CurTime = m_CurTime + dtTime;
                m_Monster.SetSpeed(Vector2.zero);
                if (m_ResetAnim && m_CurTime > m_ResetAnimTime)
                {
                    m_ResetAnim = false;
                    m_Monster.PlayAnim(GameConstVal.Run);
                }
                if (m_CurTime >= m_AnimTime)
                {
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndFirstAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                    m_ResetAnim = false;
                }
            }
        }
        private void FireToMainPlayer()
        {
            if (m_CurTime >= m_DelayTime)
            {
                m_StartAct = false;
                GameObject sprayVenomBullet = Instantiate(BulletPrefab);
                sprayVenomBullet.GetComponent<SprayVenomBullet>().Init(1, FirePoint,0,0,0);
                if(FireEffect != null)
                {
                    GameObject fireEffect = Instantiate(FireEffect);
                    fireEffect.transform.position = FirePoint.position;
                    fireEffect.transform.right = FirePoint.right;
                }
            }
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
            m_ResetAnim = true;
        }
    }
}
