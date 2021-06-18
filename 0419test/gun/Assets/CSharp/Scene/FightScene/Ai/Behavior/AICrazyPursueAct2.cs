using UnityEngine;

namespace EZ
{
    public class AICrazyPursueAct2 : AiBase
    {

        // Use this for initialization
        [SerializeField] private float Speed;
        [SerializeField] private float DtTime = 2;

        private float m_SkillTime = 1.4f;
        private float m_DelayTime = 0.4f;
        private Vector2 m_LockSpeed;
        private float m_Distance = 7f;

        float m_OriMass = 100;
        private Rigidbody2D m_RightBody2d;

        private void Awake()
        {
            m_RightBody2d = GetComponentInChildren<Rigidbody2D>();
            m_OriMass = m_RightBody2d.mass;
        }
        void Update()
        {

            if (BaseScene.GetDtTime() > 0)
            {

                if (m_StartAct)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    MoveToRole();
                }
                else
                {
                    m_CurTime += GetActDtTime();
                    if (m_CurTime >= DtTime)
                    {
                        if (m_Monster.TriggerFirstAct())
                        {
                            m_CurTime = 0;
                            m_Monster.PlayAnim(GameConstVal.Skill02);
                            m_StartAct = true;
                            Vector3 posStart = transform.position;
                            Vector3 targetPos = m_Player.transform.position;
                            Vector2 velocity2 = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
                            m_LockSpeed = velocity2.normalized * Speed;
                            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill02_3009);
                            effect.transform.SetParent(m_Monster.transform, false);
                            m_RightBody2d.mass = 999999999;
                            InstanceEffect();
                        }
                    }
                }
            }
            else if(m_StartAct)
            {
                m_Monster.SetAbsSpeed(Vector2.zero);
            }
        }
        private void MoveToRole()
        {
            if (m_CurTime < m_SkillTime)
            {
                if (m_Player)
                {
                    if (m_CurTime > m_DelayTime)
                    {
                        m_Monster.SetAbsSpeed(m_LockSpeed * BaseScene.TimeScale);
                    }
                    else
                    {
                        m_Monster.SetAbsSpeed(Vector2.zero);
                    }
                }
            }
            else
            {
                m_Monster.PlayAnim(GameConstVal.Run);
                m_CurTime = 0;
                m_StartAct = false;
                m_RightBody2d.mass = m_OriMass;
                m_Monster.EndFirstAct();
            }
        }
        private void InstanceEffect()
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Yujing_3001);
            effect.GetComponent<WarningEffect>().Init(m_SkillTime, m_Distance, transform);
        }
        public override void Death()
        {
            base.Death();
            m_RightBody2d.mass = m_OriMass;
        }
    }
}
