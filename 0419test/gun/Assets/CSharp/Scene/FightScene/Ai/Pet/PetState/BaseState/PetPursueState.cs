using System;
using UnityEngine;
namespace EZ
{
    public abstract class PetPursueState :PetBaseState 
    {
        [SerializeField] protected float PursueSpeed;
        [SerializeField] protected float AtkCheckTime;
        protected bool m_ReachPlace = false;
        Transform m_LockNode;
        protected Action<bool> m_PursureCallBack;
        BornNode m_LockCompt;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_LockNode = playerGo.transform.Find("ModelNode/PetNode");
            m_LockCompt = m_LockNode.GetComponent<BornNode>();
            m_PursureCallBack = PursueCallBack;
        }
        protected virtual void Update()
        {
            if (m_EnterState)
            {
                float dtTime = BaseScene.GetDtTime();
                m_CurTime += dtTime;
                if(m_CurTime >= AtkCheckTime)
                {
                    m_CurTime = m_CurTime - AtkCheckTime;
                    if (CheckCanEnterOtherState())
                    {
                        return;
                    } 
                }
                //if (dtTime > 0)
                //{
                //    Vector3 vector = (m_LockNode.position - transform.position);
                //    if(vector.sqrMagnitude > m_CurSqrRadio)
                //    {
                //        m_CurSqrRadio = m_SqrMinRadio;
                //        m_Pet.SetSpeed(vector.normalized * PursueSpeed);
                //        float angleZ = EZMath.SignedAngleBetween(m_LockNode.transform.position - transform.position, Vector3.up);
                //        transform.localEulerAngles = new Vector3(0, 0, angleZ);
                //    }
                //    else
                //    {
                //        float angleZ = EZMath.SignedAngleBetween(m_PlayerGo.transform.position - transform.position, Vector3.up);
                //        transform.localEulerAngles = new Vector3(0, 0, angleZ);

                //        m_CurSqrRadio = m_SqrMaxRadio;
                //        m_Pet.SetSpeed(Vector3.zero);
                //    }
                //}
                //else
                //{
                //    m_Pet.SetSpeed(Vector3.zero);
                //}
            }
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
            return false;
        }
        protected virtual void PursueCallBack(bool repachplace)
        {
            m_ReachPlace = repachplace;
        }
        public override void StartState()
        {
            m_ReachPlace = false;
            m_CurTime = 0;
            m_EnterState = true;
            m_Pet.GetAutoPathComp().SetAutoPathEnable(true,3, PursueSpeed,m_PlayerGo.transform, m_PursureCallBack);
        }

        public override void EndState()
        {
            m_Pet.GetAutoPathComp().SetAutoPathEnable(false,0.5f);
            m_EnterState = false;
        }

        public override bool CheckState()
        {
            //if (!m_Pet.InCameraView)
            //{
            //    return false;
            //}
            return true;
        }
    }
}
