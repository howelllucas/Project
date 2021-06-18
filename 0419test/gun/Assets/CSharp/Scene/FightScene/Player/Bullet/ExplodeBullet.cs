using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ExplodeBullet : BaseBullet
    {
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime >= m_LiveTime)
            {
                Destroy(gameObject);
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster);
            }
        }
    }
}
