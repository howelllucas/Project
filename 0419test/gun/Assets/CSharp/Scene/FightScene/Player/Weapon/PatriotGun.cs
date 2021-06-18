using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class PatriotGun : Gun
    {
        [Tooltip("导弹单个发射间隔")]
        [SerializeField] private float m_FireDtTime = 0.1f;
        private int m_FireCount = 0;
        private int m_CurFireCount = 0;
        private float m_FireDtAngle = 0;
        private float m_FireCurTime = 0;
        private int m_CreateCount = 1;
        private int m_FireTimes = 0;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
            m_CurTime = GetDtTime() * 0.8f;
        }
        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WPartriotGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WPartriotGun);

            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_patriot[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_patriot_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }
        protected override void Update()
        {
            RealFire();
            base.Update();
        }

        protected override void Fire()
        {
            int bulletCount = GetBulletCount();
            int bulletCurves = GetBulletCurves();
            m_FireCount = bulletCount * bulletCurves;
            m_CurFireCount = 0;
            m_FireDtAngle = -180.0f / m_FireCount;
            m_FireCurTime = 0;
            m_FireTimes = 0;
            m_CreateCount = m_FireCount / (m_BaseCount * m_BaseCurve);
            MustCallFire();
            if (m_Player != null)
                m_Player.GetFight().PlayUAnim(GameConstVal.Idle);
            InstanceBullet();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
        private void RealFire()
        {
            m_FireCurTime += BaseScene.GetDtTime();
            if (m_FireCurTime > m_FireDtTime)
            {
                m_FireCurTime -= m_FireDtTime;
                InstanceNewBullet();
            }
        }
        void InstanceBullet()
        {
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
            InstanceNewBullet();
        }
        void InstanceNewBullet()
        {
            if (m_CurFireCount < m_FireCount)
            {
                if (m_FireTimes % 2 == 0)
                {
                    if (m_FireEffect != null)
                    {
                        m_FireEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
                    }
                }
            }
            for (int i = 0; i < m_CreateCount; i++)
            {
                if (m_CurFireCount < m_FireCount)
                {
                    float newAngle = m_FireDtAngle * m_CurFireCount;
                    BaseBullet bullet = GetBullet();
                    bullet.SetLockEnemyObj(m_LockEnemyObj);
                    bullet.Init(m_Damage, FirePoint, m_Offset, newAngle);
                    m_CurFireCount++;
                }
            }
            m_FireTimes++;
        }
    }
}


