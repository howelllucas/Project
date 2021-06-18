using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class LoopRoll : MonoBehaviour
    {

        [SerializeField] private float SpeedTime = 1.6f;
        private float SpeedDis = 20;
        private Vector3 StartPos;
        private Vector3 MinPos;
        private Vector3 MaxPos;
        private float m_CurTime = 0;
        private float m_MaxTime = 1;
        private void Awake()
        {
            StartPos = transform.localPosition;
            MaxPos = new Vector3(0, 0, 50);
            MinPos = new Vector3(0, 0, -30);
            m_MaxTime = (StartPos.z - MinPos.z) / SpeedDis * SpeedTime;
        }

        public void SetSpeedScale(float scale)
        {
            SpeedTime = SpeedTime / scale;
            m_MaxTime = (StartPos.z - MinPos.z) / SpeedDis * SpeedTime;
        }
        void CalcNewTime()
        {
            m_CurTime -= m_MaxTime;
            m_MaxTime = SpeedTime * 4;
            transform.localPosition = MaxPos;
            StartPos = MaxPos;
        }
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                dtTime = 0.0333333f;
                m_CurTime += dtTime;
                float radio = m_CurTime / m_MaxTime;
                if (radio < 1)
                {
                    transform.localPosition = MinPos * radio + StartPos * (1 - radio);
                }
                else
                {
                    CalcNewTime();
                }
            }
        }
    }
}
