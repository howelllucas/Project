using UnityEngine;

namespace EZ
{
    public class AIJumpOnlAct : AiBase
    {
        private float DtTime = 5;
        float m_AnimTime = 2f;
        float m_FlyTime = 1.2f;
        float m_DelayTime = 0.2f;

        [SerializeField] private GameObject m_Bullet;
        private bool m_InstanceBullet = false;
        private bool m_InAnimAct;
        private Vector2 m_LockSpeed;
        private Vector2 m_LockPos;
        private Vector2 m_CurPosition;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_InAnimAct = false;
            m_InstanceBullet = false;
        }

        void Update()
        {

            float dtTime = BaseScene.GetDtTime();
            m_CurTime = m_CurTime + dtTime;

            if (m_StartAct)
            {
                if (dtTime == 0)
                {
                    m_Monster.SetSpeed(Vector2.zero);
                    return;
                }
                JumpToMainPlayer();
            }
            else if (!m_InAnimAct)
            {
                if (!m_StartAct)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    if (m_CurTime >= DtTime && m_Monster.TriggerSecondAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Skill01, -1, 0);
                        m_StartAct = true;
                        m_InAnimAct = true;
                        Vector3 speedVec = m_Player.transform.position - transform.position;
                        transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(speedVec, Vector3.up));
                        m_LockSpeed = speedVec.normalized * 3;
                        m_LockPos = m_Player.transform.position;
                        m_CurPosition = transform.position;
                        GetComponent<Collider2D>().enabled = false;
                        GetComponent<Rigidbody2D>().simulated = false;
                        m_InstanceBullet = false;
                        InitLockEffect();
                    }
                }
            }
            else
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_AnimTime)
                {
                    InstanceBullet();
                    GetComponent<Collider2D>().enabled = true;
                    GetComponent<Rigidbody2D>().simulated = true;
                    m_Monster.PlayAnim(GameConstVal.Run);
                    m_Monster.EndSecondAct();
                    m_CurTime = 0;
                    m_InAnimAct = false;
                    m_StartAct = false;
                }
            }
        }
        private void JumpToMainPlayer()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime <= m_FlyTime)
            {
                if (m_CurTime >= m_DelayTime)
                {
                    float rata = (m_CurTime - m_DelayTime) / (m_FlyTime - m_DelayTime);
                    transform.position = m_CurPosition * (1 - rata) + rata * m_LockPos;
                }
            }
            else
            {
                transform.position = m_LockPos;
                m_StartAct = false;
            }
        }

        private void InstanceBullet()
        {
            if (!m_InstanceBullet)
            {
                m_InstanceBullet = true;
                GameObject bullet = Instantiate(m_Bullet);
                bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                bullet.transform.right = m_LockPos - m_CurPosition;
                bullet.GetComponent<HitGroundBullet>().Init(m_LockPos);
            }
        }

        private void InitLockEffect()
        {
            GameObject lockEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.HarmRangeEffect);
            lockEffect.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, -0.2f);
            m_LockPos = m_Player.transform.position;
            lockEffect.GetComponent<DelayDestroy>().SetLiveTime(m_AnimTime);
        }
        public override void Death()
        {
            base.Death();
            m_InAnimAct = false;
            m_StartAct = false;
        }
    }
}
