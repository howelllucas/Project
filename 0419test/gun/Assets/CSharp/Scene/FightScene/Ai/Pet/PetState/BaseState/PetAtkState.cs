using UnityEngine;
namespace EZ
{
    public abstract class PetAtkState : PetBaseState
    {
        public enum TriggerSkillType
        {
            First = 1,
            Second = 2,
        }
        protected PetBaseAct m_CurSkillAct;
        protected Transform m_LockMonsterTsf; 
        protected Monster m_LockMonster;
        protected bool m_UsingSkill = false;
        public virtual void SetLockMonster(Transform monsterTsf, Monster lockMonster, float monsterRadio)
        {
            m_LockMonsterTsf = null;
            m_LockMonster = null;
            if (monsterTsf != null)
            {
                m_LockMonsterTsf = monsterTsf;
                m_LockMonster = lockMonster;
            }
        }

        public virtual bool TriggerFirstSkill()
        {
            return false;
        }

        public virtual bool TriggerSecondSkill()
        {
            return false;
        }
        
        public virtual void EndSecondSkill()
        {
            EndSkill();
        }
        public void EndSkill()
        {
            m_UsingSkill = false;
            if (m_CurSkillAct != null)
            {
                m_CurSkillAct.EndSkill();
            }
            m_CurSkillAct = null;
            m_Pet.SetSpeed(Vector2.zero);
        }
        public virtual void EndFirstSkill()
        {
            EndSkill();
        }

        public override bool CheckState()
        {
            return true;
        }

        public override bool CheckCanEnterOtherState()
        {
            bool enterAtkState = m_Pet.CheckLockMonsterState();
            if (enterAtkState)
            {
                m_Pet.ChangeToLockMonsterState();
                return true;
            }
            bool enterFlashState = m_Pet.CheckFlashState();
            if (enterFlashState)
            {
                m_Pet.ChangeToFlashState();
                return true;
            }
            bool enterPursueState = m_Pet.CheckPursueState();
            if (enterPursueState)
            {
                m_Pet.ChangeToPursueState();
                return true;
            }
            return false;
        }
    }
}


