using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class AK48Bullet : BaseBullet
    {
        public GameObject HitEffect; 
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_LiveTime)
                {
                    transform.Translate(Vector3.right * m_Speed * BaseScene.GetDtTime(), Space.Self);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MapTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                m_IsInDelayDestroy = true;
                GameObject effect = Instantiate(HitEffect);
                effect.transform.position = transform.position;
                Destroy(effect, 0.5f);
                Destroy(gameObject);
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                GameObject effect = Instantiate(HitEffect);
                effect.transform.position = monster.transform.position;
                Destroy(effect, 0.5f);
                Destroy(gameObject);
            }
        }
    }
}
