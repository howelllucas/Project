using UnityEngine;
namespace EZ
{
    public class UAVLockState : PetLockMonsterState
    {
        Fight m_Fight;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_Fight = playerGo.GetComponent<Player>().GetFight();
        }
        public override void StartState()
        {
            base.StartState();
            m_LockRadioSqr = LockRadio * LockRadio;
        }
    }
}
