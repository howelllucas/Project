using System;
using System.Collections.Generic;

namespace EZ
{
    public class TimerMgr
    {
        private int m_Guid = 0;
        private SafeMap<int,Timer> m_TimerMap;
        private Action<Timer, float> m_Action;
        public TimerMgr()
        {
            m_TimerMap = new SafeMap<int,Timer>();
            m_Action = TimerUpdate;
        }
        public void TimerUpdate(Timer timer,float dt)
        {
            timer.Update(dt);
        }
        public void UpdateToRecentOne(float dt)
        {
            if (dt > 0)
            {
                float recentTime = GetRencentTime();
                float newDtTime = Math.Max(dt, recentTime);
                m_TimerMap.Update(newDtTime, m_Action);
            }
        }
        private float GetRencentTime()
        {
            Dictionary<int, Timer> allTimer = m_TimerMap.GetAll();
            float m_MinTime = 99999999;
            foreach (KeyValuePair<int, Timer> kv in allTimer)
            {
                m_MinTime = Math.Min(kv.Value.GetLeftTime(), m_MinTime);
            }
            return m_MinTime;
        }
        public void Update(float dt)
        {
            if (dt > 0)
            {
                m_TimerMap.Update(dt, m_Action);
            }
        }
        public int AddTimer(float dtTime,int callTimes, Action<float, bool> callBack)
        {
            int guid = GetNewGuid();
            Timer timer = new Timer(this,guid,dtTime,callTimes, callBack);
            m_TimerMap.Add(guid, timer);
            return guid;
        }

        public void RemoveTimer(int guid)
        {
            m_TimerMap.Remove(guid);
        }
        public void SetTimerScale(int guid,float scale)
        {
            Timer timer = m_TimerMap.Get(guid);
            if(timer != null)
            {
                timer.SetTimeScale(scale);
            }
        }
        public Timer GetTimer(int guid)
        {
            return m_TimerMap.Get(guid);
        }
        private int GetNewGuid()
        {
            m_Guid++;
            return m_Guid;
        }
        public void Destroy()
        {
            m_TimerMap.OnDestroy();
        }
    }
}
