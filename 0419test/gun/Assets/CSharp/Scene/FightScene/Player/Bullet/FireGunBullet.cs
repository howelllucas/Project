using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FireGunBullet : BaseBullet
    {
        private PolygonCollider2D m_Collider;
        private void Awake()
        {
            m_Collider = GetComponent<PolygonCollider2D>();
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            Destroy(gameObject, m_LiveTime);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Simple(m_Damage,transform);
                if (monster.CheckCanAddHittedEffect())
                {
                    GameObject effect = GetHittedEnemyEffect();
                    effect.transform.SetParent(monster.transform, false);
                    effect.transform.position = monster.transform.position;
                }
            }
        }
    }
}
