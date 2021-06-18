using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class SwingHandBullet : BaseBullet
    {
        private Action m_TriggerMapAct;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime < m_LiveTime)
                {
                    transform.Rotate(Vector3.forward * (-m_Speed) * BaseScene.GetDtTime(), Space.Self);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ , float offset,float atkRange = 0)
        {
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.DamageRangeTag))
            {
                obj.GetComponentInParent<Player>().OnHit_RealVec(transform,-transform.up);
                OnHitted(obj.transform);
            }
        }
        public void SetCollisionMapAct(Action action)
        {
            m_TriggerMapAct = action;
        }
        private void OnHitted(Transform tsf)
        {
            m_IsInDelayDestroy = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.position = tsf.position;
            float duration = effect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().main.duration;
            Destroy(gameObject, duration);
        }
        private void OnDestroy()
        {

        }
    }
}
