using System;
using UnityEngine;
namespace EZ
{
    public class EffectNode : MonoBehaviour
    {
        [SerializeField] float LiveTime = 1;
        private float m_CurTime = 0;
        private bool m_IgnoreSceneTimeScale = false;
        ParticleSystem m_ParticleSystem;
        Animator m_Animator;
        private string m_Name;
        private Vector3 m_OriScale;
        public void SetName(string name)
        {
            m_Name = name;
        }
        public void Awake()
        {
            m_ParticleSystem = transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
            m_Animator = GetComponentInChildren<Animator>();
            enabled = false;
            m_OriScale = transform.localScale;
        }

        private void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime > LiveTime)
            {
                Stop();
            }
        }
        public virtual void Stop()
        {
            StopImp();
        }
        private void StopImp()
        {
            enabled = false;
            m_ParticleSystem.Stop();
            Global.gApp.gGameCtrl.EffectCache.Recycle(m_Name, this);
            transform.localScale = m_OriScale;
        }
        public void ForceStop()
        {
            StopImp();
        }
        public float GetLiveTime()
        {
            return LiveTime;
        }
        public void ResetEffect()
        {
            Init();
        }
        public void Init()
        {
            enabled = true;
            m_CurTime = 0;
            m_ParticleSystem.Play();
            if(m_Animator != null)
            {
                m_Animator.Play("Effect");
            }
        }
    }
}
