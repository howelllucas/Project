using UnityEngine;

namespace EZ
{
    public class AIThrowHookAct : AiBase
    {

        // Use this for initialization
        [SerializeField] private float Speed;
        [SerializeField] private float DtTime = 2;

        private float m_SkillTime = 3;
        private float m_PauseStartTime = 1.3f;
        private float m_PauseEndTime = 1.8f;
        private float m_DelayTime = 0.2f;

        private float m_BackTime = 2f;

        private bool m_StartBackStep1 = false; // 回退动作
        private bool m_StartBackStep2 = false;// 定住
        private bool m_HookPlayer = false;// 定住

        private float m_FaceTime = 0.7f;

        private Vector2 m_LockSpeed;
        private AIHookPosition m_HookPos;

        private void Awake()
        {
            m_HookPos = GetComponentInChildren<AIHookPosition>();
        }
        public override void Init(GameObject player,Wave wave,Monster monster)
        {
            base.Init(player,wave,monster);
            m_StartBackStep1 = false;
            m_StartBackStep2 = false;
            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
        }
        public void StartBackAct(bool hookPlayer = false)
        {
            if (!m_StartBackStep1)
            {
                if (hookPlayer && m_HookPos.CanUseSkill())
                {
                    m_Player.GetComponent<Player>().LockMove(0.5f);
                }
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 0f);
                m_BackTime = m_CurTime;
                m_CurTime = -0.5f;
                m_StartBackStep1 = true;
                m_StartBackStep2 = true;
                m_HookPlayer = hookPlayer;
            }
        }
        private void StartBackActImp()
        {
            if (m_CurTime > 0 && m_StartBackStep2)
            {
                m_StartBackStep2 = false;
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, -1);
                if (m_HookPlayer)
                {
                    m_Player.GetComponent<Player>().StartBackActToPos(m_HookPos.transform.position, m_BackTime - 0.35f);
                }
            }
            if (m_CurTime >= m_BackTime)
            {
                m_Monster.PlayAnim(GameConstVal.Run);
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.EndFirstAct();
                m_StartBackStep1 = false;
            }
            else if(m_CurTime < 0)
            {
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 0f);
            }
        }

        void Update()
        {
            if (BaseScene.GetDtTime() > 0)
            {

                if (m_StartBackStep1)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    StartBackActImp(); 
                }
                else if (m_StartAct)
                {
                    m_CurTime = m_CurTime + BaseScene.GetDtTime();
                    CheckEnd();
                }
                else
                {
                    m_CurTime += GetActDtTime();
                    if (m_CurTime >= DtTime)
                    {
                        if (m_Monster.TriggerFirstAct())
                        {
                            m_CurTime = 0;
                            m_Monster.PlayAnim(GameConstVal.Skill01, -1, 0);
                            m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
                            m_StartAct = true;
                            m_StartBackStep1 = false;
                            m_StartBackStep2 = false;
                            Vector3 posStart = transform.position;
                            Vector3 targetPos = m_Player.transform.position;
                            Vector2 velocity2 = new Vector2(targetPos.x - posStart.x, targetPos.y - posStart.y);
                            m_LockSpeed = velocity2.normalized * Speed;
                            m_Monster.SetSpeed(Vector2.zero);
                        }
                    }
                }
            }
            else if(m_StartAct)
            {
                m_Monster.SetSpeed(Vector2.zero);
            }
        }
        private void CheckEnd()
        {
            if(m_CurTime < m_FaceTime)
            {
                Vector3 faceVec = m_Player.transform.position - transform.position;
                transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(faceVec, Vector3.up));
            }
            if(m_CurTime > m_PauseStartTime && m_CurTime < m_PauseEndTime) {
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 0.0f);
            }
            else
            {
                m_Monster.SetAnimFloat(GameConstVal.SpeedSymbolStr, 1.0f);
            }
            if (m_CurTime > m_SkillTime)
            {
                m_Monster.PlayAnim(GameConstVal.Run);
                m_CurTime = 0;
                m_StartAct = false;
                m_Monster.EndFirstAct();
            }
        }
        public override void Death()
        {
            base.Death();
            m_StartBackStep1 = false;
        }
    }
}
