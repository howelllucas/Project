using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class IntervalGun : Gun
    {
        [Tooltip("间隔开火 次数")]
        [SerializeField] int IntervalFireCount = 3;
        [Tooltip("间隔子弹Go")]
        [SerializeField] protected GameObject IntervalBullet;
        [Tooltip("间隔开火 子弹数量")]
        [SerializeField] int IntervalBulletCount = 1;

        int m_CurFireCount = 0;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);

            m_CurTime = GetDtTime() * 0.8f;
            m_CurFireCount = IntervalFireCount;
        }

        protected override void Fire()
        {
            m_CurFireCount--;
            FireImp();            
            if (m_CurFireCount <= 0)
            {
                m_CurFireCount = IntervalFireCount;
            }
        }

        private void FireImp()
        {            
            if (m_Player != null)
                m_Player.GetFight().PlayUAnim(GameConstVal.Idle);

            InstanceBullet();
            MustCallFire();
        }
        void InstanceBullet()
        {
            InstanceNormalBullet();
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
            if (m_FireEffect != null)
            {
                m_FireEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
            }
        }

        protected override BaseBullet GetBullet()
        {
            if (m_CurFireCount <= 0)
            {
                GameObject bullet = Instantiate(IntervalBullet);
                return bullet.GetComponentInChildren<BaseBullet>();
            }
            else
            {
                GameObject bullet = Instantiate(Bullet);
                return bullet.GetComponentInChildren<BaseBullet>();
            }
        }

        protected override int GetBulletCount()
        {
            if (m_CurFireCount <= 0)
                return IntervalBulletCount;
            else
                return m_BaseCount;
        }
    }
}


