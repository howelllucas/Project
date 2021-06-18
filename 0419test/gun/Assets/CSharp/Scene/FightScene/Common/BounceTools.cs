using System;
using UnityEngine;
namespace EZ
{

    public class BounceTools : MonoBehaviour
    {
        public float G = 9.8f;
        public int MaxBounceTimes = 8;
        private Vector3 m_OriLocalPos;
        float m_ThreadHold;
        float m_StartSpeed = 0;
        float m_NextSpeed = 0;
        float m_CurTime = 0;
        float m_BounceTime = 0;
        float m_Damp;
        Action m_EndCall;
        float m_StartZ = 0;
        float m_Gsymble = 1;
        private void Awake()
        {
            m_OriLocalPos = transform.localPosition;
        }
        private void Update()
        {
            m_CurTime += BaseScene.GetDtTime();
            float newTime = Math.Min(m_CurTime, m_BounceTime);
            float newPos = m_StartSpeed * newTime + m_Gsymble * G * newTime * newTime / 2;
            float posZ = m_StartZ - newPos;
            Vector3 localPos = transform.localPosition;
            localPos.z = -Mathf.Abs(posZ);
            transform.localPosition = localPos;
            if (m_CurTime > m_BounceTime)
            {
                m_CurTime -= m_BounceTime;
                CalcNextSpeed();
            }
        }
        public void SetBounceInfo(float startSpeed,float damp,float threadHold,float startZ,Action endCall)
        {
            m_StartZ = Mathf.Abs(startZ);
            m_ThreadHold = threadHold;
            m_StartSpeed = startSpeed;
            m_Damp = damp;
            m_EndCall = endCall;
            m_BounceTime = ((Mathf.Sqrt(startSpeed * startSpeed + 2.0f * G * Mathf.Abs(m_StartZ))) - startSpeed) / G;
            m_NextSpeed = startSpeed + G * m_BounceTime;
            MaxBounceTimes--;
        }
        private void CalcNextSpeed()
        {
            m_Gsymble = -1;
            m_StartSpeed = m_NextSpeed * m_Damp;
            m_BounceTime = m_StartSpeed / G * 2;
            m_NextSpeed = m_StartSpeed;
            m_StartZ = 0;
            if (m_StartSpeed < m_ThreadHold || MaxBounceTimes <= 0)
            {
                m_EndCall();
                enabled = false;
            }
            MaxBounceTimes--;
        }
    }
}
