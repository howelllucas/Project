using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class BoomCrossBowBullet : BaseBullet
    {
        [Tooltip(" 爆炸 效果伤害")]
        public GameObject m_ExplodeBullet;
        private float m_ExplodeDtTime = 0.1f;
        private float m_ExplodeTime = 0.11f;

        [SerializeField] float FireBuffTime = 0;
        [SerializeField] double FireBuffDamageParam = 0;
        [Range(1, 99999)]
        [SerializeField] int FireDamageTimes = 1;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                float dtTime = BaseScene.GetDtTime();

                m_CurTime = m_CurTime + dtTime;
                m_ExplodeTime += dtTime;
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
        private bool AddExplodeBullet(Vector3 pos,bool forceAdd = false)
        {
            if(m_ExplodeTime > m_ExplodeDtTime || forceAdd)
            {
                m_ExplodeTime = 0;
                GameObject explodeBullet = Instantiate(m_ExplodeBullet);
                explodeBullet.transform.position = pos;
                RocketExplode rocketExplode = explodeBullet.GetComponentInChildren<RocketExplode>();
                if (rocketExplode != null)
                {
                    rocketExplode.AddExplodeEffect(m_Damage);
                }
                else{
                    explodeBullet.GetComponentInChildren<RocketExplodeFireBuff>().AddExplodeEffect(m_Damage);
                }
                return true;
            }
            else
            {
                return false;
            }

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
                gameObject.GetComponent<Collider2D>().enabled = false;
                AddExplodeBullet(transform.position,true);
                Destroy(gameObject);
            }
            else if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                if (!AddExplodeBullet(monster.transform.position))
                {
                    monster.OnHit_Vec(m_Damage, transform);
                    if (FireBuffDamageParam > 0 && FireBuffTime > 0)
                    {
                        monster.AddBuff(AiBuffType.FireBuff, FireBuffTime, FireBuffDamageParam * m_Damage, FireBuffTime / FireDamageTimes);
                    }
                }
            }
            else if (obj.CompareTag(GameConstVal.ShieldTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                collision.gameObject.GetComponent<AIShieldEvent>().AddHittedEffect(transform.position);
                m_IsInDelayDestroy = true;
                gameObject.GetComponent<Collider2D>().enabled = false;
                AddExplodeBullet(transform.position, true);
                Destroy(gameObject);
            }
        }
    }
}
