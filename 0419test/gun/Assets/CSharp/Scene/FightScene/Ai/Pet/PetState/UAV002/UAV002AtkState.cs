using UnityEngine;

namespace EZ
{
    public class UAV002AtkState : PetAtkState
    {
        UAV002SkillAct m_UAV002SkillAct;
        protected TriggerSkillType m_NeedTriggerSkill = TriggerSkillType.First;
        private bool m_NeedCheckNewState = false;
        Fight m_Fight;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Fight = playerGo.GetComponent<Player>().GetFight();
            m_UAV002SkillAct = GetComponent<UAV002SkillAct>();
            m_UAV002SkillAct.SetPlayerGo(playerGo);
            m_UAV002SkillAct.Init(pet, this);
        }
        public void Update()
        {
            if (m_EnterState)
            {
                if (m_LockMonsterTsf != null && !m_LockMonster.InDeath && !m_NeedCheckNewState)
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

        public override void SetLockMonster(Transform monsterTsf, Monster lockMonster,float radio)
        {
            m_NeedCheckNewState = false;
            base.SetLockMonster(monsterTsf, lockMonster, radio);
            m_UAV002SkillAct.SetLockMonster(monsterTsf, lockMonster,radio);
        }
        public override bool TriggerFirstSkill()
        {
            if(!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            m_CurSkillAct = m_UAV002SkillAct;
            m_UAV002SkillAct.StartSkill();
            return true;
        }


        public override void EndFirstSkill()
        {
            m_NeedTriggerSkill = TriggerSkillType.First;
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
            TriggerFirstSkill();
        }
    }
}
