using EZ.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class Robot001Pet : BasePet
    {

        Robot001AtkState m_AtkState;
        Robot001FlashState m_FlashState; 
        Robot001LockState m_LockState;
        Robot001PursueState m_PursueState;
        private float m_OriMass;
        public override void Init(GameObject playerGo, int guid)
        {
            base.Init(playerGo, guid);
            m_OriMass = m_Rigidbody2D.mass;
            m_AtkState = GetComponent<Robot001AtkState>();
            m_FlashState = GetComponent<Robot001FlashState>();
            m_LockState = GetComponent<Robot001LockState>();
            m_PursueState = GetComponent<Robot001PursueState>();
            m_FlashState.Init(playerGo, this);
            m_PursueState.Init(playerGo, this);
            m_LockState.Init(playerGo, this);
            m_AtkState.Init(playerGo, this);
            m_AtkState.SetLockState(m_LockState);
            transform.position = playerGo.transform.position;
            GameObject appearEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[EffectConfig.Robot_Appear]);
            appearEffect.transform.position = playerGo.transform.position;
            ChangeToPursueState();
            InitName();
        }
        public void InitName()
        {
            ItemItem petItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WRobot);
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
        //public void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
        //    {
        //        if (m_CurState == m_LockState)
        //        {
        //            m_Rigidbody2D.mass = 0;
        //        }
        //        else
        //        {
        //            m_Rigidbody2D.mass = m_OriMass;
        //        }
        //    }
        //}
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    m_Rigidbody2D.mass = m_OriMass;
        //}
    }
}
