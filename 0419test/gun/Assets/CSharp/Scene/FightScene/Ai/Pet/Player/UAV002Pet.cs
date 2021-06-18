using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class UAV002Pet : BasePet
    {

        UAV002AtkState m_AtkState;
        UAV002FlashState m_FlashState;
        UAV002LockState m_LockState;
        UAV002PursueState m_PursueState;
        private float m_OriMass;
        public override void Init(GameObject playerGo, int guid)
        {
            base.Init(playerGo, guid);
            m_OriMass = m_Rigidbody2D.mass;
            m_AtkState = GetComponent<UAV002AtkState>();
            m_FlashState = GetComponent<UAV002FlashState>();
            m_LockState = GetComponent<UAV002LockState>();
            m_PursueState = GetComponent<UAV002PursueState>();
            m_FlashState.Init(playerGo, this);
            m_PursueState.Init(playerGo, this);
            m_LockState.Init(playerGo, this);
            m_AtkState.Init(playerGo, this);
            m_AutoPath.SetAutoInfoTypeOne();
            ChangeToFlashState();
            InitName();
        }
        public void InitName()
        {
            ItemItem petItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WUAV002);
            Global.gApp.gMsgDispatcher.Broadcast<int, string, int, Transform>(MsgIds.AddPetName, m_Guid, petItem.gamename, 1, m_NameNode);
        }
        public override void ChangeToLockMonsterState()
        {
            base.ChangeToLockMonsterState();
            ChangeState(m_LockState);
        }
        public override void ChangeToAtkState(Transform monsterTsf, Monster monster, float monsterRadio)
        {
            base.ChangeToAtkState(monsterTsf, monster, monsterRadio);
            m_AtkState.SetLockMonster(monsterTsf, monster, monsterRadio);
            ChangeState(m_AtkState);
        }
        public override void ChangeToPursueState()
        {
            base.ChangeToPursueState();
            ChangeState(m_PursueState);
        }
        public override void ChangeToFlashState()
        {
            base.ChangeToFlashState();
            ChangeState(m_FlashState);
        }

        public override bool CheckLockMonsterState()
        {
            return m_LockState.CheckState();
        }
        public override bool CheckPursueState()
        {
            return m_PursueState.CheckState();
        }
        public override bool CheckFlashState()
        {
            return m_FlashState.CheckState();
        }
        public override bool CheckAtkState()
        {
            return true;
        }
    }
}
