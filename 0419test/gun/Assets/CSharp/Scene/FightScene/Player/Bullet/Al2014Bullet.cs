using EZ.Data;
using EZ.DataMgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class Al2014Bullet : BaseBullet
    {
        public double Damage = 5;
        private double HpPram = 5;

        private EffectNode m_EffectNode;
        private void Awake()
        {
            m_EffectNode = Global.gApp.gGameCtrl.EffectCache.GetEffect(EffectConfig.Skill01_2014);
            m_EffectNode.transform.localPosition = Vector3.zero;
            m_EffectNode.transform.SetParent(transform, false);
        }

        private void Start()
        {
            PassItem passData = Global.gApp.CurScene.GetPassData();
            if (passData != null)
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                if ((PassType)passData.passType == PassType.MainPass)
                {
                    HpPram = passData.hpParam;
                }
                else
                {
                    PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItem();
                    if (passItem != null)
                    {
                        HpPram = passItem.hpParam * passData.hpParam;
                    }
                    else
                    {
                        HpPram = passData.hpParam;
                    }
                }
            }
        }
        private void DestroyBullet()
        {
            if (m_EffectNode != null)
            {
                m_EffectNode.Stop();
                m_EffectNode = null;
            }
            Destroy(gameObject);
        }
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();

                if (m_CurTime >= m_LiveTime)
                {
                    DestroyBullet();
                    m_IsInDelayDestroy = true;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                collision.gameObject.GetComponentInParent<Player>().StartBackActOnPos(transform);
            }
            else if(collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                collision.gameObject.GetComponent<Monster>().OnHit_Pos(Damage * HpPram, transform, true);
            }
        }
    }
}
