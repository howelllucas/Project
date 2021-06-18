using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class BlinkTools : MonoBehaviour
    {
        private float m_StartTime = 1;
        private float m_CurTime = 0;
        Renderer[] m_AllRenders;
        private bool m_StartBlink = false;
        private void Awake()
        {
            m_AllRenders = transform.GetComponentsInChildren<Renderer>();
            Blink(0);
        }
        private void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime > m_StartTime)
            {
                AddBinkEffectImp(20);
            }
        }
        public void SetStartTime(float startTime)
        {
            m_StartTime = startTime;
            m_StartBlink = false;
            m_CurTime = 0;
            Blink(0);
        }
        private void AddBinkEffectImp(float speed)
        {
            if (!m_StartBlink)
            {
                Blink(speed);
                m_StartBlink = true;
            }
        }
        private void Blink(float speed)
        {
            foreach (Renderer mesh in m_AllRenders)
            {
                if (mesh != null && mesh.sharedMaterial != null)
                {
                    if (mesh.sharedMaterial.HasProperty(GameConstVal.Shader_BlinkSpeed))
                    {
                        mesh.material.SetFloat(GameConstVal.Shader_BlinkSpeed, speed);
                    }
                }
            }
        }
    }
}
