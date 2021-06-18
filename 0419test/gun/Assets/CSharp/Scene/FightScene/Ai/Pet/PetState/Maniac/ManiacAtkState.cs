using UnityEngine;

namespace EZ
{
    public class ManiacAtkState : PetAtkState
    {
        ManiacSkill1Act m_ManiacSkill1Act;
        ManiacSkill2Act m_ManiacSkill2Act;
        private ManiacLockState m_LockState;
        protected TriggerSkillType m_NeedTriggerSkill = TriggerSkillType.First;
        private bool m_NeedCheckNewState = false;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_ManiacSkill1Act = GetComponent<ManiacSkill1Act>();
            m_ManiacSkill2Act = GetComponent<ManiacSkill2Act>();
            m_ManiacSkill1Act.Init(pet, this);
            m_ManiacSkill2Act.Init(pet, this);
        }
        public void SetLockState(ManiacLockState lockState)
        {
            m_LockState = lockState;
        }
        public void Update()
        {
            if (m_EnterState)
            {
                if (m_LockMonsterTsf != null && !m_NeedCheckNewState)
                {
                    if (m_CurSkillAct != null)
                    {
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

        public override void SetLockMonster(Transform monsterTsf, Monster lockMonster, float radio)
        {
            m_NeedCheckNewState = false;
            base.SetLockMonster(monsterTsf, lockMonster, radio);
            m_ManiacSkill1Act.SetLockMonster(monsterTsf, lockMonster, radio);
            m_ManiacSkill2Act.SetLockMonster(monsterTsf, lockMonster, radio);
        }
        public override bool TriggerFirstSkill()
        {
            if (!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            if (m_LockMonster != null && !m_LockMonster.InDeath)
            {
                m_UsingSkill = true;
                m_CurSkillAct = m_ManiacSkill1Act;
                m_ManiacSkill1Act.StartSkill();
                return true;
            }
            return false;
        }

        public override bool TriggerSecondSkill()
        {
            if (!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            Monster lockMonster = m_LockState.GetLockMonster();
            if (lockMonster != null)
            {
                CircleCollider2D circleCollider2D = lockMonster.GetComponent<CircleCollider2D>();
                float radio = circleCollider2D.radius * lockMonster.transform.localScale.x;
                SetLockMonster(lockMonster.transform, lockMonster, radio);
            }
            if (m_LockMonster != null && !m_LockMonster.InDeath)
            {
                m_UsingSkill = true;
                m_CurSkillAct = m_ManiacSkill2Act;
                m_ManiacSkill2Act.StartSkill();
                return true;
            }
            return false;
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

        public TriggerSkillType GetTriggerType()
        {
            if (m_NeedTriggerSkill == TriggerSkillType.First && m_ManiacSkill1Act.CanTriggerSkill())
            {
                return TriggerSkillType.First;
            }
            else
            {
                return TriggerSkillType.Second;
            }
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
