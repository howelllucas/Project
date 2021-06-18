using System;
using UnityEngine;

namespace EZ
{
    public class Timer
    {
        private int m_RepeatForeverTimes = 0;

        private TimerMgr m_TimerMgr;
        private Action<float, bool> m_CallBack;
        private float m_DtTime;
        private float m_CurTime;
        private float m_scale;

        private int m_CallTimes;
        private int m_CurCallTimes;
        private int m_Guid;

        private bool m_IsEnd;

        public Timer(TimerMgr timerMgr,int guid,float dtTime, int callTimes, Action<float,bool> callBack)
        {
            m_TimerMgr = timerMgr;
            m_Guid = guid;
            m_DtTime = dtTime;
            m_CallTimes = callTimes;
            m_CurCallTimes = 0;
            m_CallBack = callBack;
            m_CurTime = 0;
            m_scale = 1;

            m_IsEnd = false;

        }
        public float GetLeftTime()
        {
            return m_DtTime - m_CurTime;
        }
        public void Update(float dt)
        {
            float curTimer = m_CurTime + dt * m_scale;
            if(curTimer >= m_DtTime)
            {
                curTimer = curTimer - m_DtTime;
                m_CurCallTimes++;
                CheckEnd();
                m_CallBack(dt, m_IsEnd);
            }
            m_CurTime = curTimer;
            //if(m_DtTime > 0 && m_CurTime >= m_DtTime && !m_IsEnd)
            //{
            //    Update(0);
            //}
        }
        public void CheckEnd()
        {
           if(m_CallTimes != m_RepeatForeverTimes && m_CurCallTimes >= m_CallTimes)
            {
                RemoveSelf();
            } 
        }
        public void SetTimeScale(float scale = 1)
        {
            m_scale = scale;
        }
        private void RemoveSelf()
        {
            m_TimerMgr.RemoveTimer(m_Guid);
            m_IsEnd = true;
        }
    }
}
