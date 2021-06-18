using UnityEngine;

namespace EZ
{
    public class Robot001AtkState : PetAtkState
    {
        Robot001Skill1Act m_Robot001Skill1Act;
        Robot001Skill2Act m_Robot002Skill1Act;
        protected TriggerSkillType m_NeedTriggerSkill = TriggerSkillType.First;
        private bool m_NeedCheckNewState = false;
        private Robot001LockState m_LockState;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Robot001Skill1Act = GetComponent<Robot001Skill1Act>();
            m_Robot002Skill1Act = GetComponent<Robot001Skill2Act>();
            m_Robot001Skill1Act.Init(pet, this);
            m_Robot002Skill1Act.Init(pet, this);
        }
        public void SetLockState(Robot001LockState lockState)
        {
            m_LockState = lockState;
        }
        public void Update()
        {
            if (m_EnterState)
            {
                if (m_LockMonsterTsf != null)
                {
                    if (m_CurSkillAct != null)
                    {
                        if (m_CurSkillAct == m_Robot002Skill1Act && m_LockMonster.InDeath)
                        {
                            Monster lockMonster = m_LockState.SearchNearestMonster();
                            if (lockMonster != null)
                            {
                                CircleCollider2D circleCollider2D = lockMonster.GetComponent<CircleCollider2D>();
                                float radio = circleCollider2D.radius * lockMonster.transform.localScale.x;
                                SetLockMonster(lockMonster.transform, lockMonster, radio);
                                m_Robot002Skill1Act.ResetInfo();
                            }
                        }
                        m_CurSkillAct.MUpdate();
                    }
                    else
                    {
                        EndSkill();
                        CheckCanEnterOtherState();
                    }
                }
                else
                {
                    m_NeedCheckNewState = true;
                    m_Pet.SetSpeed(Vector2.zero);
                    EndSkill();
                    CheckCanEnterOtherState();
                }
            }
        }
        public override void SetLockMonster(Transform monsterTsf, Monster lockMonster,float radio)
        {
            m_NeedCheckNewState = false;
            base.SetLockMonster(monsterTsf, lockMonster, radio);
            m_Robot001Skill1Act.SetLockMonster(monsterTsf, lockMonster,radio);
            m_Robot002Skill1Act.SetLockMonster(monsterTsf, lockMonster,radio);
        }
        public override bool TriggerFirstSkill()
        {
            if(!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            if (m_LockMonster != null && !m_LockMonster.InDeath)
            {
                m_UsingSkill = true;
                m_CurSkillAct = m_Robot001Skill1Act;
                m_Robot001Skill1Act.StartSkill();
                return true;
            }
            return true;
        }

        public override bool TriggerSecondSkill()
        {
            if (!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            if (m_LockMonster != null && !m_LockMonster.InDeath)
            {
                m_UsingSkill = true;
                m_CurSkillAct = m_Robot002Skill1Act;
                m_Robot002Skill1Act.StartSkill();
            }
            return true;
        }

        public override void EndSecondSkill()
        {
            m_NeedTriggerSkill = TriggerSkillType.First;
            base.EndSecondSkill();
        }
        public override void EndFirstSkill()
        {
            m_NeedTriggerSkill = TriggerSkillType.Second;
            base.EndFirstSkill();
        }
        
        public override void EndState()
        {
            m_EnterState = false;
            EndSkill();
        }

        public override void StartState()
        {
            m_EnterState = true;

            if (m_NeedTriggerSkill == TriggerSkillType.First)
            {
                TriggerFirstSkill();
            }
            else
            {
                TriggerSecondSkill();
            }
        }
    }
}
