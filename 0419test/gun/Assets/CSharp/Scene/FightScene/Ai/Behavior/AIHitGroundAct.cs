using UnityEngine;

namespace EZ
{
    public class AIHitGroundAct : AiBase
    {

        [SerializeField] private float DtTime = 2;
        [SerializeField] private float IdleTime = 1.0f;
        [SerializeField] private GameObject HitGroundBullet;
        [SerializeField] private Transform FirePoint;
        private float m_DelayTime = 0.5f;
        private float m_EndTime = 2.0f;
        private bool m_InstanceBullet = false;
        private bool m_EndedIdle = false;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_EndedIdle = false;
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
                m_Monster.SetSpeed(Vector2.zero);
                if (!m_EndedIdle && m_CurTime > IdleTime)
                {
                    m_EndedIdle = true;
                    m_CurTime = 0;
                    m_Monster.PlayAnim(GameConstVal.Skill01);
                }
                if (m_EndedIdle)
                {
                    HitGround();
                }
            }
            else
            {
                if (m_CurTime >= DtTime)
                {
                    if (m_Monster.TriggerFirstAct())
                    {
                        m_CurTime = 0;
                        m_Monster.SetSpeed(Vector2.zero);
                        m_Monster.PlayAnim(GameConstVal.Idle);
                        m_StartAct = true;
                        m_InstanceBullet = false;
                        m_EndedIdle = false;
                        AddLockEffect();
                    }
                }
            }
        }
        private void HitGround()
        {
            if (m_CurTime >= m_DelayTime)
            {
                InstanceBullet();
            }
            if(m_CurTime >= m_EndTime)
            {
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.PlayAnim(GameConstVal.Run);
                m_Monster.EndFirstAct();
            }
        }
        private void InstanceBullet()
        {
            if (!m_InstanceBullet)
            {
                m_InstanceBullet = true;
                GameObject bullet = Instantiate(HitGroundBullet);
                bullet.GetComponent<HitGroundBullet>().Init(FirePoint.position);
            }
        }

        private void AddLockEffect()
        {
            GameObject lockEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.HarmRangeEffect);
            lockEffect.transform.localScale = new Vector3(1.96f, 1.96f, 1.0f);
            lockEffect.transform.position = FirePoint.position;
            lockEffect.GetComponent<DelayDestroy>().SetLiveTime(1.5f);
        }

        public override void Death()
        {
            base.Death();
            m_EndedIdle = false;
            m_InstanceBullet = false;
        }
    }
}

