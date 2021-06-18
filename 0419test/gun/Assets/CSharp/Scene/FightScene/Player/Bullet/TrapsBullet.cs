using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class TrapsBullet : BaseBullet
    {
        private bool m_InDeadStep = false;
        public Animator m_Animator;

        void Update()
        {
            if (!m_IsInDelayDestroy && !m_InDeadStep)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime > m_LiveTime)
                {
                    PlayDeadthAnim();
                }
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                PlayCloseAnim();
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster);
            }
        }
        private void PlayCloseAnim()
        {
            if (!m_InDeadStep)
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                m_Animator.Play(GameConstVal.Close, -1, 0);
                gameObject.AddComponent<DelayCallBack>().SetAction(
                    () => {
                        PlayDeadthAnim();
                    },2f
                );
            }
            m_InDeadStep = true; 
        }
        private void PlayDeadthAnim()
        {
            m_Animator.Play(GameConstVal.Death, -1, 0);
            m_IsInDelayDestroy = true;
            Destroy(gameObject, 1.3f);
        }
    }
}
