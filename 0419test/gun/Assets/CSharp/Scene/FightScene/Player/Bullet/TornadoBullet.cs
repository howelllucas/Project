using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class TornadoBullet : BaseBullet
    {
        private ParticleSystem m_ParticleSystem;
        [SerializeField] private GameObject LinkBullet;
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
                    Recycle();
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
            if(m_ParticleSystem == null)
            {
                m_ParticleSystem = m_BulletEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
            }
            m_ParticleSystem.Play();
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
                //m_IsInDelayDestroy = true;
                AddHittedWallEffect();
                //Recycle();
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                //m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Up(m_Damage, transform);
                if (LinkBullet != null)
                {
                    GameObject linkBullet = Instantiate(LinkBullet);
                    linkBullet.GetComponent<ElecPopBullet>().Init(monster.gameObject, m_Damage);
                }
                if (monster.CheckCanAddHittedEffect())
                {
                    GameObject effect = GetHittedEnemyEffect();
                    if (effect != null)
                    {
                        effect.transform.SetParent(monster.GetBodyNode(), false);
                        effect.transform.position = monster.GetBodyNode().position;
                    }
                }
                //Recycle();
            }
            //else if (obj.CompareTag(GameConstVal.ShieldTag))
            //{
            //    m_IsInDelayDestroy = true;
            //    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
            //    collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
            //    //Recycle();
            //}
        }
    }
}
