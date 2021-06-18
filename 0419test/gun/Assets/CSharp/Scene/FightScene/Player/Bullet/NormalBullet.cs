using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class NormalBullet : BaseBullet
    {
        private enum NormalBulletTriggerType
        {
            None = 0,
            Normal = 1,
            Penetrate = 2,  //穿透
        }
        [SerializeField]private NormalBulletTriggerType BulletType = NormalBulletTriggerType.Normal;
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
                AddHittedWallEffect();
                Recycle();
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster);
                if (BulletType == NormalBulletTriggerType.Normal)
                {
                    m_IsInDelayDestroy = true;
                    Recycle();
                }
            }
            else if (obj.CompareTag(GameConstVal.ShieldTag))
            {
                m_IsInDelayDestroy = true;
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
                Recycle();
            }
        }
    }
}
