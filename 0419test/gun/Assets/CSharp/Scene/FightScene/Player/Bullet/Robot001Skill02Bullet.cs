using UnityEngine;
namespace EZ
{
    public class Robot001Skill02Bullet : BaseBullet
    {
        private void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime > m_LiveTime)
            {
                Recycle();
            }
        }
        public override void Init(double damage,Transform firePoint,float dtAngleZ, float offset,float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ,offset,atkRange);
            InitBulletPose(damage,firePoint, dtAngleZ,offset);
            transform.position = firePoint.position;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
                AddHittedEffect(monster, true);
            }
        }
    }
}
