using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class PortalEle : MonoBehaviour
    {
        [Tooltip("激活需要的时间")]
        public float m_ActiveTime = 10;
        PortalState m_PortalState = PortalState.None;
        PortalEleCtrol m_PortalEleCtrol;
        ParticleSystem m_CurParticleSystem;
        private float m_CurTime = 0;
        public void Init(PortalEleCtrol portalEleCtrol)
        {
            m_PortalEleCtrol = portalEleCtrol;
            gameObject.SetActive(false);
        }
        public void DelayInit()
        {
            gameObject.SetActive(true);
            SetProtalState(PortalState.Active);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_PortalState == PortalState.Active)
            {
                if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
                {
                    m_PortalEleCtrol.StartPortal(this);
                }
            }
        }
        private void Update()
        {
            if (m_PortalState == PortalState.CountDown)
            {
                m_CurTime += BaseScene.GetDtTime();
                if (m_CurTime > m_ActiveTime)
                {
                    m_PortalEleCtrol.ActiveProtal(this);
                }
            }
        }
        public void SetProtalState(PortalState portalState)
        {
            m_PortalState = portalState;
            if(portalState == PortalState.CountDown)
            {
                m_CurTime = 0;
            }
            FreshEffectState();
        }
        public PortalState GetProtalState()
        {
            return m_PortalState;
        }
        public void FreshEffectState()
        {
            if (m_PortalState == PortalState.Active)
            {
                SetPartlcleEnable(true);
            }
        }
        public void AddAppearEffect()
        {
            GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Chuansong_1);
            effect.transform.position = transform.position;
        }
        public void SetPartlcleEnable(bool partlcleEnable)
        {
            if(m_CurParticleSystem == null)
            {
                GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Cuansongmen_1);
                effect.transform.SetParent(transform,false);
                m_CurParticleSystem = effect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
            }
            if (partlcleEnable)
            {
                m_CurParticleSystem.Play();
            }
            else
            {
                m_CurParticleSystem.Stop();
            }
        }
    }
}
