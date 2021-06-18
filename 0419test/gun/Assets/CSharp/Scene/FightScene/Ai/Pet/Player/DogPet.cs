using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class DogPet : BasePet
    {

        DogAtkState m_AtkState;
        DogFlashState m_FlashState; 
        DogLockState m_LockState;
        DogPursueState m_PursueState;
        private float m_OriMass;
        private string m_CurPetName = string.Empty;
        public Animator m_GunAnimator;
        public override void Init(GameObject playerGo, int guid)
        {
            base.Init(playerGo, guid);
            m_OriMass = m_Rigidbody2D.mass;
            m_AtkState = GetComponent<DogAtkState>();
            m_FlashState = GetComponent<DogFlashState>();
            m_LockState = GetComponent<DogLockState>();
            m_PursueState = GetComponent<DogPursueState>();
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
            ItemItem petItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WDog);
            Global.gApp.gMsgDispatcher.Broadcast<int, string, int, Transform>(MsgIds.AddPetName, m_Guid, petItem.gamename, 1, m_NameNode);
        }
        public override void ChangeToLockMonsterState()
        {
            base.ChangeToLockMonsterState();
            ChangeState(m_LockState);
        }
        public override void ChangeToAtkState(Transform monsterTsf,Monster monster,float monsterRadio)
        {
            base.ChangeToAtkState(monsterTsf, monster, monsterRadio);
            m_AtkState.SetLockMonster(monsterTsf,monster, monsterRadio);
            ChangeState(m_AtkState);
        }
        public override void PlayAnim(string name, int layer = -1, float normalizedTime = 0)
        {
            if (!m_CurPetName.Equals(name))
            {
                m_CurPetName = name;
                base.PlayAnim(name, layer, normalizedTime);
            }
        }
        public void PlayFireAnim()
        {
            m_GunAnimator.Play(GameConstVal.Fire, -1, 0);
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
