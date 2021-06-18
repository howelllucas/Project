using System;
using UnityEngine;
namespace EZ
{
    public class DelayCallBack : MonoBehaviour
    {

        float LiveTime = 1;
        private float m_CurTime = 0;
        private bool m_IgnoreSceneTimeScale = false;
        private Action m_Acton;
        private int m_CallTimes = 1;
        private int m_HasCallTimes = 0;
        private float m_TimeOffset = 0;
        private void Update()
        {
            if (!m_IgnoreSceneTimeScale)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime(); ;
            }
            else
            {
                m_CurTime = m_CurTime + Time.deltaTime;
            }
            if (m_CurTime >= LiveTime)
            {
                m_CurTime -= LiveTime;
                LiveTime = LiveTime + m_TimeOffset;
                if (m_Acton != null)
                {
                    m_Acton();
                }
                if(m_CallTimes == ++m_HasCallTimes)
                {
                    Destroy(this);
                }
            }
        }
        public void SetAction(Action action, float time,bool ignoreSceneTimeScale = false)
        {
            m_CurTime = 0;
            LiveTime = time;
            m_Acton = action;
            m_IgnoreSceneTimeScale = ignoreSceneTimeScale;
        }
        public void SetLiveOffset(float offset)
        {
            m_TimeOffset = offset;
        }
        public void ResteTime()
        {
            m_CurTime = 0;
        }
        public void SetLiveTime(float liveTime)
        {
            LiveTime = liveTime;
        }
        public void SetCallTimes(int callTimes)
        {
            m_CallTimes = callTimes;
        }
        public void SetIgnoreSceneTimeScale(bool arg)
        {
            m_IgnoreSceneTimeScale = arg;
        }
    }
}
