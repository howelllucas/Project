using UnityEngine;

namespace EZ
{
    public class DeadthAtkState : PetAtkState
    {
        DeadthSkill1Act m_DeadthSkill1Act;
        DeadthSkill2Act m_DeadthSkill2Act;
        private DeadthLockState m_LockState;
        protected TriggerSkillType m_NeedTriggerSkill = TriggerSkillType.First;
        private bool m_NeedCheckNewState = false;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_DeadthSkill1Act = GetComponent<DeadthSkill1Act>();
            m_DeadthSkill2Act = GetComponent<DeadthSkill2Act>();
            m_DeadthSkill1Act.Init(pet, this);
            m_DeadthSkill2Act.Init(pet, this);
        }
        public void SetLockState(DeadthLockState lockState)
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
            m_DeadthSkill1Act.SetLockMonster(monsterTsf, lockMonster, radio);
            m_DeadthSkill2Act.SetLockMonster(monsterTsf, lockMonster, radio);
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
                m_CurSkillAct = m_DeadthSkill1Act;
                m_DeadthSkill1Act.StartSkill();
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
            if (m_LockMonster != null && !m_LockMonster.InDeath)
            {
                m_UsingSkill = true;
                m_CurSkillAct = m_DeadthSkill2Act;
                m_DeadthSkill2Act.StartSkill();
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
            if (m_NeedTriggerSkill == TriggerSkillType.First && m_DeadthSkill1Act.CanTriggerSkill())
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
