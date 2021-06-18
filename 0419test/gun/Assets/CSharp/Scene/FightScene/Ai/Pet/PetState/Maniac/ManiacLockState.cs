using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ManiacLockState : PetLockMonsterState
    {
        ManiacAtkState m_AtkState;
        Fight m_Fight;
        [Tooltip(" 主角锁定怪物 之后  机器人强制锁定 怪物的 距离")]
        public float FoceLockMonsterDis = 2;
        private float m_LockSqr = 4;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Fight = playerGo.GetComponent<Player>().GetFight();
            m_LockSqr = FoceLockMonsterDis * FoceLockMonsterDis;
        }
        public void SetAtkState(ManiacAtkState atkState)
        {
            m_AtkState = atkState;
        }
        public override void StartState()
        {
            m_Pet.PlayAnim(GameConstVal.Run);
            base.StartState();
        }
        protected override void StartAutoPath()
        {
            CircleCollider2D circleCollider2D = m_LockMonsterGo.GetComponent<CircleCollider2D>();
            m_MonsterRadio = circleCollider2D.radius * m_LockMonsterGo.transform.localScale.x;
            m_LockRadioSqr = m_MonsterRadio + m_Pet.CircleRadio;
            if (m_AtkState.GetTriggerType() == PetAtkState.TriggerSkillType.First)
            {
                m_Pet.GetAutoPathComp().SetAutoPathEnable(true, m_LockRadioSqr + LockRadioOffset, LockMoveSpeed, m_LockMonsterTsf);
                m_LockRadioSqr = m_LockRadioSqr + 0.5f;
                m_LockRadioSqr = m_LockRadioSqr * m_LockRadioSqr;
            }
            else
            {
                m_Pet.GetAutoPathComp().SetAutoPathEnable(true, m_LockRadioSqr + 3, LockMoveSpeed, m_LockMonsterTsf);
                m_LockRadioSqr = m_LockRadioSqr + 1000;
                m_LockRadioSqr = m_LockRadioSqr * m_LockRadioSqr;
            }

        }
        protected override void SearchLockMonster()
        {
            if (m_AtkState.GetTriggerType() == PetAtkState.TriggerSkillType.First)
            {
                SearchNearestMonster();
            }
            else
            {
                Transform lockMonster = m_Fight.GetLockEnemyTsf();
                if(lockMonster != null)
                {
                    if (!m_Pet.InCameraView)
                    {
                        SetLockMonster(lockMonster.gameObject);
                    }
                    else
                    {
                        if((lockMonster.position - m_PlayerGo.transform.position).sqrMagnitude < m_LockSqr)
                        {
                            SetLockMonster(lockMonster.gameObject);
                        }
                        else
                        {
                            SerachRandomMonster();
                        }
                    }
                }
                else
                {
                    if (!m_Pet.InCameraView)
                    {
                        SearchNearestPlayerMonster();
                    }
                    else
                    {
                        SerachRandomMonster();
                    }
                }
            }
        }
    }
}