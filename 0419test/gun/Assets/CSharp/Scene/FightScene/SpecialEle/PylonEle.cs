using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class PylonEle : MonoBehaviour
    {
        [Tooltip("激活需要的时间")]
        public float m_ActiveTime = 10;
        [Tooltip("激光墙的存活时间")]
        public float m_WallLiveTime = 10;
        PylonState m_PylonState = PylonState.None;
        private float m_CurTime = 0;
        private int m_Index = 0;
        private int m_TotalStep = 6;
        private float m_CountCountStepTime = 0.1f;
        private float m_ActiveStepTime = 0.1f;
        private int m_CurStep = 0;
        private PylonEleCtrol m_PylonEleCtrol;
        public void Init(PylonEleCtrol pylonEleCtrol,int index)
        {
            m_Index = index;
            m_CountCountStepTime = m_ActiveTime / m_TotalStep;
            m_ActiveStepTime = m_WallLiveTime / m_TotalStep;
            m_PylonEleCtrol = pylonEleCtrol;
            gameObject.SetActive(false);
            SetPylonState(PylonState.Frozen);
        }
        public void DelayInit()
        {
            gameObject.SetActive(true);
        }
        private void Update()
        {
            if (m_PylonState == PylonState.CountDown)
            {
                m_CurTime += BaseScene.GetDtTime();
                int step = (int)(m_CurTime / m_CountCountStepTime);
             
                if (m_CurTime > m_ActiveTime)
                {
                    m_PylonEleCtrol.ActiveWall(this);
                    m_CurTime = 0;
                    step = m_TotalStep;
                }
                if (step > m_CurStep)
                {
                    m_CurStep = step;
                    m_PylonEleCtrol.AddChargingStepEffect(this, step);
                }
            }
            else if (m_PylonState == PylonState.Active)
            {
                m_CurTime += BaseScene.GetDtTime();
                int step = Mathf.CeilToInt(((m_WallLiveTime - m_CurTime) / m_ActiveStepTime));
                if (m_CurTime > m_WallLiveTime)
                {
                    m_PylonEleCtrol.DestroyWall(this);
                    m_CurTime = 0;
                    step = 0;
                }
                if (step < m_CurStep)
                {
                    m_PylonEleCtrol.RemoveChargingStepEffect(m_CurStep);
                    m_CurStep = step;
                }
            }
        }
        public int GetIndex()
        {
            return m_Index;
        }
        public void SetPylonActive()
        {
            m_CurTime = 0;
            SetPylonState(PylonState.Active);
        }
        public void SetPylonState(PylonState pylonState)
        {
            m_PylonState = pylonState;
            if(pylonState == PylonState.Active)
            {
                m_PylonEleCtrol.SetChargFullVisible(true,transform);
                m_PylonEleCtrol.SetChargingVisible(false,transform);
                m_PylonEleCtrol.AddArrowEffect(false, this);
            }
            else if(pylonState == PylonState.CountDown)
            {
                m_PylonEleCtrol.SetChargFullVisible(false,transform);
                m_PylonEleCtrol.SetChargingVisible(true, transform);
                m_PylonEleCtrol.AddArrowEffect(true, this);
                m_CurStep = 0;
            }
            else if(pylonState == PylonState.Frozen)
            {
                m_PylonEleCtrol.SetChargingVisible(false, transform);
                m_PylonEleCtrol.SetChargFullVisible(false,transform);
                m_PylonEleCtrol.AddArrowEffect(false, this);
                m_PylonEleCtrol.DisableStepEffect();
            }
        }
    }
}
