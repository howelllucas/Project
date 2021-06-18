using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class BounceBullet : BaseBullet
    {
        [SerializeField] private GameObject BouncePopBullet;
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
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                AddHittedWallEffect();
                Destroy(gameObject);
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster);
                OnHitted(obj.transform);
                Destroy(gameObject);
            }
            else if (obj.CompareTag(GameConstVal.ShieldTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
                Destroy(gameObject);
            }
        }

        private void OnHitted(Transform monster)
        {
            if (Global.gApp.CurScene.GetWaveMgr() == null)
                return;
            GameObject bouncePopBullet = Instantiate(BouncePopBullet);
            bouncePopBullet.GetComponent<BouncePopBullet>().Init(monster.gameObject, m_Damage);
        }
        private void OnDestroy()
        {

        }
    }
}
