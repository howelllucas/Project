using System;
using UnityEngine;

namespace EZ
{
    public class PetLockDisTools
    {

        Transform m_OwnnerTsf;
        Transform m_LockTransform;
        Rigidbody2D m_OwnnerRigid;

        float m_FollowSpeed = 10;
        private float m_MaxRadio = 1;
        private float m_MinRadio = 0.5f;
        private float m_BreakDis = 8f;

        private float m_SqrMaxRadio = 3;
        private float m_SqrMinRadio = 1;
        private float m_CurSqrRadio = 1;
        Vector3 m_Offset = Vector3.zero;
        private float m_SqrBreakDis = 9;

        private Action m_BreakCallBack;
        BasePet m_Pet;
        private bool m_FollowState = true;
        private bool m_RotateState = true;
        public float MaxRadio
        {
            get{return m_MaxRadio;}
            set
            {
                m_MaxRadio = value;
                m_SqrMaxRadio = m_MaxRadio * m_MaxRadio;
            }
        }

        public float MinRadio
        {
            get{return m_MinRadio;}
            set
            {
                m_MinRadio = value;
                m_SqrMinRadio = m_MinRadio * m_MinRadio;
                m_CurSqrRadio = m_SqrMaxRadio;
            }
        }

        public float BreakDis
        {
            get{return m_BreakDis;}

            set
            {
                m_BreakDis = value;
                m_SqrBreakDis = m_BreakDis * m_BreakDis;
            }
        }

        public PetLockDisTools(Transform ownnerTsf,BasePet pet)
        {
            m_OwnnerTsf = ownnerTsf;
            m_OwnnerRigid = ownnerTsf.GetComponent<Rigidbody2D>();

            m_SqrMaxRadio = MaxRadio * MaxRadio;
            m_SqrMinRadio = MinRadio * MinRadio;

            m_CurSqrRadio = m_SqrMaxRadio;
            m_Pet = pet;
        }
        public void ResetCurRadio()
        {
            m_CurSqrRadio = m_SqrMaxRadio;
        }
        public void Update()
        {
            float angleZ1 = m_LockTransform.localEulerAngles.z * Mathf.Deg2Rad;
            float cosVal = (float)Math.Cos(angleZ1);
            float sinVal = (float)Math.Sin(angleZ1);
            Vector3 newOffset = new Vector3(cosVal * m_Offset.x - sinVal * m_Offset.y, sinVal * m_Offset.x + cosVal * m_Offset.y, 0);
            Vector3 vector = (m_LockTransform.position + newOffset - m_OwnnerTsf.position);
            float sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > m_SqrBreakDis)
            {
                if (m_BreakCallBack != null)
                {
                    m_BreakCallBack();
                    return;
                }
            }
            if (m_RotateState)
            {
                float angleZ = EZMath.SignedAngleBetween(vector, Vector3.up);
                m_OwnnerTsf.localEulerAngles = new Vector3(0, 0, angleZ);
            }
            if (m_FollowState)
            {
                if (sqrMagnitude > m_SqrMaxRadio)
                {
                    m_Pet.SetSpeed(vector.normalized * m_FollowSpeed);
                }
                else if (sqrMagnitude < m_SqrMinRadio)
                {
                    m_Pet.SetSpeed(-vector.normalized * m_FollowSpeed);
                }
                else
                {
                    m_Pet.SetSpeed(Vector3.zero);
                }
            }
        }
        public void SetRotateState(bool rotateState)
        {
            m_RotateState = rotateState;
        }
        public void SetFollowState(bool followState)
        {
            m_FollowState = followState;
        }
        public void SetFollowSpeed(float followSpeed)
        {
            m_FollowSpeed = followSpeed;
        }
        public void SetBreakCall(Action acton)
        {
            m_BreakCallBack = acton;
        }
        public void SetLockTsf(Transform lockTsf)
        {
            m_LockTransform = lockTsf;
            if(lockTsf != null)
            {
                m_Offset = lockTsf.GetComponent<CircleCollider2D>().offset * lockTsf.localScale.x;
            }
            else
            {
                m_Offset = Vector3.zero;
            }
        }
    }
}
