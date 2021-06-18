using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class RugbyBullet : BaseBullet
    {
        public float HitWallLiveTime = 0.2f;
        private bool m_HittedWall = false;
        [SerializeField]private bool m_ForceGround = false;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (!m_IsInDelayDestroy && ! m_HittedWall)
            {

                if (m_CurTime < m_LiveTime)
                {
                    transform.Translate(Vector3.right * m_Speed * BaseScene.GetDtTime(), Space.Self);
                }
                else
                {
                    OnHitted(true);
                }
            }
            else
            {
                if (m_CurTime >= HitWallLiveTime)
                {
                    gameObject.GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        public void Init(float damage, Transform firePoint,Vector3 right)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform,false);
            m_Damage = damage;
            transform.position = new Vector3(firePoint.position.x, firePoint.position.y, 0);
            right.z = 0;
            transform.right = right;
            InitBulletEffect(firePoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            if (collision.gameObject.CompareTag( GameConstVal.DamageRangeTag))
            {
                m_IsInDelayDestroy = true;
                if (!m_HittedWall)
                {
                    collision.gameObject.GetComponentInParent<Player>().StartBackActOnPos(transform);
                    OnHitted(false);
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                }
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (collision.gameObject.CompareTag(GameConstVal.MapTag) && !m_HittedWall)
            {
                OnHitted(true);
            }
        }
        private void OnHitted(bool hittedWall)
        {
            if (hittedWall)
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
            }
            m_HittedWall = hittedWall;
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.SetParent(transform, false);
            if (!m_ForceGround)
            {
                effect.transform.position = m_BulletEffect.transform.position;
            }
            else
            {
                Vector3 newPos = m_BulletEffect.transform.position;
                newPos.z = 0;
                effect.transform.position = newPos;
            }
            float duration = effect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().main.duration;
            Destroy(m_BulletEffect);
            Destroy(gameObject, duration);
            m_CurTime = 0f;
        }
        private void OnDestroy()
        {

        }
    }
}
