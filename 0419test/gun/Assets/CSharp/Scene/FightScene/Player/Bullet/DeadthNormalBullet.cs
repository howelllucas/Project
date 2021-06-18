using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class DeadthNormalBullet : BaseBullet
    {
        public GameObject CtrolEffect;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime > m_LiveTime)
            {
                transform.gameObject.SetActive(false);
                CtrolEffect.gameObject.SetActive(false);
            }
        }

        public void End()
        {
            gameObject.SetActive(false);
            CtrolEffect.gameObject.SetActive(false);
        }
        public void Run(double damage)
        {
            m_Damage = damage;
            m_CurTime = 0;
            gameObject.SetActive(true);
            CtrolEffect.gameObject.SetActive(true);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;
            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster);
            }
        }
    }
}
