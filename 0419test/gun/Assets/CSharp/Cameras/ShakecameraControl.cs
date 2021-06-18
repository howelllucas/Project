using UnityEngine;
namespace EZ
{
    public class ShakecameraControl : MonoBehaviour
    {

        public float m_DelayTime = 0;
        private float m_CurTime = 0f;
        public float m_ShakeTime = 2f;
        public bool m_Ended = false;
        public ShakeCamera m_ShakeCamera;
        public void StartShake(float delayTime, float shakeTime, float ShakeAmp)
        {
            return;
            m_Ended = false;
            m_CurTime = 0f;
            m_DelayTime = delayTime;
            m_ShakeTime = shakeTime + delayTime;
            m_ShakeCamera.Shakeamp = ShakeAmp;
        }
        public void StartShake()
        {
            return;
            m_Ended = false;
            m_CurTime = 0f;
        }
        //void Update()
        //{
        //    if (!m_Ended)
        //    {
        //        m_CurTime += BaseScene.GetDtTime();
        //        if (m_CurTime >= m_DelayTime)
        //        {
        //            m_ShakeCamera.StartShakeCamera();
        //            if (m_CurTime >= m_ShakeTime)
        //            {
        //                m_Ended = true;
        //            }
        //        }
        //    }
        //}
    }
}
