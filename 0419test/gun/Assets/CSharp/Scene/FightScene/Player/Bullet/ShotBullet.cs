using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ShotBullet : BaseBullet
    {
        [SerializeField] float FireBuffTime = 0;
        [SerializeField] double FireBuffDamageParam = 0;
        [Range(1,99999)]
        [SerializeField] int FireDamageTimes  = 1;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime > m_LiveTime)
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
            m_BulletEffect.transform.SetParent(Global.gApp.gBulletNode.transform, true);
            m_BulletEffect.AddComponent<DelayDestroy>().SetLiveTime(m_ShowTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster,true);
                if(FireBuffDamageParam > 0 && FireBuffTime > 0)
                {
                    monster.AddBuff(AiBuffType.FireBuff, FireBuffTime, FireBuffDamageParam * m_Damage,FireBuffTime / FireDamageTimes);
                }
            }
        }
    }
}
