using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ManiacSwingArmBullet : BaseBullet
    {
        [Tooltip("延迟伤害时间")]
        [SerializeField] private float DelayDamageTime;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime > m_LiveTime)
                {
                    Destroy(gameObject);
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            m_Damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_CurTime > DelayDamageTime)
            {
                GameObject obj = collision.gameObject;
                if (obj.CompareTag(GameConstVal.MonsterTag))
                {
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                    Monster monster = collision.gameObject.GetComponent<Monster>();
                    monster.OnHit_Vec(m_Damage, transform);
                    AddHittedEffect(monster);
                }
            }
        }
    }
}
