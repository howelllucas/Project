using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class DeadthLockState : PetLockMonsterState
    {
        DeadthAtkState m_AtkState;
        Fight m_Fight;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Fight = playerGo.GetComponent<Player>().GetFight();
        }
        public void SetAtkState(DeadthAtkState atkState)
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
            m_Pet.GetAutoPathComp().SetAutoPathEnable(true, m_LockRadioSqr, LockMoveSpeed, m_LockMonsterTsf);
            m_LockRadioSqr += LockRadioOffset;
            m_LockRadioSqr = m_LockRadioSqr * m_LockRadioSqr;
        }
    }
}