using System;
using UnityEngine;
namespace EZ
{
    public class Robot001Skill01Bullet : BaseBullet
    {
        private Transform m_LockTsf;
        private Action m_DelayCallAct;
        private bool m_CanAtk = false;
        private float m_DamageTime = 0;
        private float m_DamageEndTime = 0;
        private Vector3 m_OriPos;
        private void Awake()
        {
            m_OriPos = transform.localPosition;
            transform.localPosition = new Vector3(-500, 0, 0);
            m_CanAtk = false;
        }
        private void Update()
        {
            if (m_CanAtk)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                
                if (m_CurTime >= m_DamageEndTime)
                {
                    StopAtkAbs();
                }
                else if(m_CurTime >= m_DamageTime)
                {
                    transform.localPosition = m_OriPos;
                    m_DamageEndTime = 0;
                }
                else
                {
                    transform.localPosition = new Vector3(-500,0, 0);
                }
            }
        }
        public void StopAtkAbs()
        {
            m_CanAtk = false;
            transform.localPosition = new Vector3(-500, 0, 0);
        }
        public void addDamage(double damage, float delayTime, Transform lockTsf)
        {
            transform.localPosition = new Vector3(-500, 0, 0);
            m_Damage = damage;
            m_DamageTime = delayTime;
            m_DamageEndTime = 1000;
            m_CanAtk = true;
            m_CurTime = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Pos(m_Damage, transform);
                AddHittedEffect(monster, true);
            }
        }
    }
}
