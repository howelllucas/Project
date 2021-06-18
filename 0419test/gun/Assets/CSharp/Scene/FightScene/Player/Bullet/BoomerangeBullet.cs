using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class BoomerangeBullet : BaseBullet
    {
        private bool m_InBackState = false;
        private Transform m_PlayerTsf;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (!m_InBackState)
            {
                if (m_CurTime < m_EffectTime)
                {
                    transform.Translate(Vector3.right * m_Speed * BaseScene.GetDtTime(), Space.Self);
                }
                else
                {
                    m_InBackState = true;
                    m_CurTime = 0;
                }
            }
            else
            {
                if (m_CurTime < m_EffectTime)
                {
                    Vector3 playerPos = m_PlayerTsf.position;
                    Vector3 curPos = transform.position;
                    if((playerPos - curPos).sqrMagnitude > 0.25f)
                    {
                        float rate = m_CurTime / m_EffectTime;
                        Vector3 newPos = curPos * (1 - rate) + playerPos * rate;
                        transform.position = newPos;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ , float offset,float atkRange = 0)
        {
            m_InBackState = false;
            m_PlayerTsf = Global.gApp.CurScene.GetMainPlayer().transform;
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MapTag))
            {

                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                OnHitted();
                if(!m_InBackState)
                {
                    m_CurTime = 0;
                    m_InBackState = true;
                }
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<Monster>().OnHit_Pos(m_Damage, transform);
                OnHitted();
            }
            else if (obj.CompareTag(GameConstVal.ShieldTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
                if(!m_InBackState)
                {
                    m_InBackState = true;
                    m_CurTime = 0;
                }
            }
        }
        private void OnHitted()
        {
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.position = transform.position;
            Destroy(effect,1);
        }
    }
}
