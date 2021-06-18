using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class DelayDestroy : MonoBehaviour
    {

        [SerializeField] float LiveTime = 1;
        private float m_CurTime = 0;
        private bool m_IgnoreSceneTimeScale = false;
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
                Destroy(gameObject);
            }
        }
        public float GetLiveTime()
        {
            return LiveTime;
        }
        public void SetIgnoreSceneTimeScale(bool arg)
        {
            m_IgnoreSceneTimeScale = arg;
        }
        public void SetLiveTime(float time)
        {
            LiveTime = time;
        }
        public void AddLiveTime(float time)
        {
            LiveTime = time + LiveTime;
        }
        public void ResetTime()
        {
            m_CurTime = 0;
        }
    }
}
