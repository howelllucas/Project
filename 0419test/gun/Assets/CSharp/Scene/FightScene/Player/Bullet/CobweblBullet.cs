using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class CobweblBullet : BaseBullet
    {
        [SerializeField] private float m_LockTime; 
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
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Destroy(gameObject);
            }
            else if (obj.CompareTag(GameConstVal.DamageRangeTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Player player = obj.GetComponentInParent<Player>();
                player.LockMove(m_LockTime);
                Transform elecEffect = player.transform.Find(GameConstVal.LockEffect);
                if (elecEffect == null)
                {
                    GameObject mEffect = Instantiate(HittedEnemyEffect);
                    mEffect.name = GameConstVal.LockEffect;
                    mEffect.transform.SetParent(player.transform, false);
                    mEffect.GetComponent<DelayDestroy>().SetLiveTime(m_LockTime + 0.2f);
                }
                else
                {
                    elecEffect.gameObject.GetComponent<DelayDestroy>().SetLiveTime(m_LockTime + 0.2f);
                    elecEffect.gameObject.GetComponent<DelayDestroy>().ResetTime();
                    if (elecEffect.Find(GameConstVal.ParticleName) != null)
                    {
                        elecEffect.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
