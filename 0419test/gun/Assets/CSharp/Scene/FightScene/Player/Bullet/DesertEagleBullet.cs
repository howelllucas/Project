using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class DesertEagleBullet : BaseBullet
    {
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

        public override void Init(double damage, Transform firePoint, float dtAngleZ , float offset,float atkRange = 0)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
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
                OnHitted();
            }
            else if (obj.CompareTag(GameConstVal.DamageRangeTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                OnHitted();
            }
        }
        private void OnHitted()
        {
            m_IsInDelayDestroy = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.SetParent(transform, false);
            effect.transform.position = m_BulletEffect.transform.position;
            float duration = effect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().main.duration;
            Destroy(m_BulletEffect);
            Destroy(gameObject, duration);
        }
    }
}
