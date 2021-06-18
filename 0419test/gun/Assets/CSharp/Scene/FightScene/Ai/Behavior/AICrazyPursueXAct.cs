using UnityEngine;

namespace EZ
{
    public class AICrazyPursueXAct : AiBase
    {

        // Use this for initialization
        [SerializeField] private int PursueTimes;
        [SerializeField] private float Speed;
        [SerializeField] private float DtTime = 2;

        private int m_PursueTimes;
        private float m_SkillTime = 1.0f;
        private float m_DelayTime = 0.4f;
        private Vector2 m_LockSpeed;
        private float m_Distance = 3f;

        private bool m_TriggerTag;
        public override void Init(GameObject player, Wave wave, Monster monster)
        {
            base.Init(player, wave, monster);
            m_PursueTimes = 0;
        }
        public void TriggerPlayer()
        {
            if (m_StartAct)
            {
                m_PursueTimes = PursueTimes - 1;
            }
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
                        if ( m_Monster.TriggerFirstAct())
                        {
                            m_CurTime = 0;
                            m_Monster.PlayAnim(GameConstVal.Skill01);
                            m_StartAct = true;
                            Vector3 posStart = transform.position;
                            Vector3 targetPos = m_Player.transform.position;
                            Vector2 velocity2 = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
                            m_LockSpeed = velocity2.normalized * Speed;
                            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Skill01_2004);
                            effect.transform.SetParent(m_Monster.transform, false);
                            InstanceEffect();
                        }
                    }
                }
            }
            else if (m_StartAct)
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
                m_PursueTimes++;
                if (m_PursueTimes < PursueTimes)
                {
                    m_CurTime = DtTime;
                }
                else
                {
                    m_PursueTimes = 0;
                }
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
            m_PursueTimes = 0;
            m_TriggerTag = false;
        }
    }
}
