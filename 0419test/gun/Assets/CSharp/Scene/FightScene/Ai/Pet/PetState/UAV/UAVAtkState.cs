using UnityEngine;

namespace EZ
{
    public class UAVAtkState : PetAtkState
    {
        UAVSkillAct m_UAVSkillAct;
        protected TriggerSkillType m_NeedTriggerSkill = TriggerSkillType.First;
        private bool m_NeedCheckNewState = false;
        Fight m_Fight;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Fight = playerGo.GetComponent<Player>().GetFight();
            m_UAVSkillAct = GetComponent<UAVSkillAct>();
            m_UAVSkillAct.SetPlayerGo(playerGo);
            m_UAVSkillAct.Init(pet, this);
        }
        public void Update()
        {
            if (m_EnterState)
            {
                //Transform lockTsf = m_Fight.GetLockEnemyTsf();
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
            m_UAVSkillAct.SetLockMonster(monsterTsf, lockMonster,radio);
        }
        public override bool TriggerFirstSkill()
        {
            if(!m_EnterState || m_UsingSkill)
            {
                return false;
            }
            m_CurSkillAct = m_UAVSkillAct;
            m_UAVSkillAct.StartSkill();
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
