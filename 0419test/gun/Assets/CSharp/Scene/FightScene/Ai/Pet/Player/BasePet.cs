using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public abstract class BasePet : MonoBehaviour
    {
        protected Rigidbody2D m_Rigidbody2D;
        protected CircleCollider2D m_CircleCollider2D;
        protected Player m_Player;
        protected GameObject m_PlayerGo;
        protected Transform m_LockPosGo;
        protected PetBaseState m_CurState; 
        protected Animator m_Animator; 
        protected PetAutoPath m_AutoPath;
        protected int m_Guid = -1;

        public float CircleRadio
        {
            get; private set;
        }
        public bool InCameraView
        {
            get; private set;
        }
        protected Transform m_NameNode;
        public virtual void Init(GameObject playerGo,int guid)
        {
            m_Guid = guid;
            m_PlayerGo = playerGo;
            m_Player = playerGo.GetComponent<Player>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Animator = GetComponentInChildren<Animator>();
            m_AutoPath = gameObject.AddComponent<PetAutoPath>();
            m_LockPosGo = playerGo.transform.Find("PetNode");
            CircleRadio = m_CircleCollider2D.radius * transform.localScale.x;
            m_NameNode = transform.Find(GameConstVal.NameNode);
        }
        public virtual void SetSpeed(Vector2 speed)
        {
            m_Rigidbody2D.velocity = speed;
        }
        public virtual void ChangeToLockMonsterState()
        {
            EndCurState();
        }
        public virtual void ChangeToAtkState(Transform monsterTsf, Monster monster,float monsterRadio)
        {
            EndCurState();
        }
        public virtual void ChangeToPursueState()
        {
            EndCurState();
        }
        public virtual void ChangeToFlashState()
        {
            EndCurState();
        }
        public virtual bool CheckLockMonsterState()
        {
            return false;
        }
        public virtual bool CheckPursueState()
        {
            return false;
        }
        public virtual bool CheckFlashState()
        {
            return false;
        }
        public virtual bool CheckAtkState()
        {
            return false;
        }
        public PetAutoPath GetAutoPathComp()
        {
            return m_AutoPath;
        }
        public void ChangeState(PetBaseState state)
        {
            EndCurState();
            m_CurState = state;
            state.StartState();
        }
        public PetBaseState GetCurState()
        {
            return m_CurState;
        }
        public void EndCurState()
        {
            if (m_CurState)
            {
                m_CurState.EndState();
                m_CurState = null;
                SetSpeed(Vector2.zero);
            }
        }
        public virtual void PlayAnim(string name, int layer = -1, float normalizedTime = 0)
        {
            m_Animator.Play(name, layer, normalizedTime);
        }
        public void PlayAnimAbs(string name, int layer = -1, float normalizedTime = 0)
        {
            m_Animator.Play(name, layer, normalizedTime);
        }
        public void SetAnimSpeed(float speed)
        {
            m_Animator.speed = speed;
        }
        private void OnBecameInvisible()
        {
            InCameraView = false;
            m_CircleCollider2D.isTrigger = true;
        }
        public void SetColliderTriggerEnable(bool isTrigger)
        {
            m_CircleCollider2D.isTrigger = isTrigger;
        }
        private void OnBecameVisible()
        {
            InCameraView = true;
            m_CircleCollider2D.isTrigger = false; ;
        }
        public void OnDestroy()
        {
            Global.gApp.gMsgDispatcher.Broadcast<int, string, int, Transform>(MsgIds.AddPetName, m_Guid,string.Empty, -1, m_NameNode);
        }
    }
}
