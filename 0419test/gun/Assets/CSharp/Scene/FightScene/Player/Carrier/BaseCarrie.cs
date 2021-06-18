using EZ.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class BaseCarrie : MonoBehaviour
    {
        [SerializeField] private string m_AppearEffectName ;
        [SerializeField] private GameObject m_Smoke01 ;
        [SerializeField] private GameObject m_Smoke02 ;
        [SerializeField] private float m_StartClipLength = 1;
        [SerializeField] private float m_LoopStartTime = 0.5f;
        [SerializeField] private float m_LoopClipDt = 0.3f;
        [SerializeField] private float m_LoopClipRange = 0.3f;
        //[SerializeField] private AudioClip m_StartClip;
        [SerializeField] private AudioClip m_LoopClip;
        public void Init(float damageCoefficient = 1)
        {
            if (m_AppearEffectName != null && EffectConfig.EffectPath.ContainsKey(m_AppearEffectName))
            {
                GameObject appearEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[m_AppearEffectName]);
                //appearEffect.transform.SetParent(transform, false);
                appearEffect.transform.position = transform.position;
            }
            //Global.gApp.gAudioSource.PlayOneShot(m_StartClip);
            Global.gApp.gAudioSource.PlayLoop(m_LoopClip, m_StartClipLength,m_LoopStartTime, m_LoopClipDt, m_LoopClipRange);
            if (!Global.gApp.CurScene.IsNormalPass())
            {
                if(m_Smoke01 != null)
                {
                    m_Smoke01.SetActive(false);
                }
                if(m_Smoke02 != null)
                {
                    m_Smoke02.SetActive(true);

                }
            }
            BaseCarrierWeapon[] weapons = GetComponentsInChildren<BaseCarrierWeapon>(); 
            foreach(BaseCarrierWeapon weapon in weapons)
            {
                weapon.init(damageCoefficient);
            }
            Gun[] weaponGuns = GetComponentsInChildren<Gun>();
            foreach(Gun weaponGun in weaponGuns)
            {
                weaponGun.Init(null);
                weaponGun.SetDamageCoefficient(damageCoefficient);
            }
        }
    }
}
