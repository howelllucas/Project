using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class BounceGun : Gun
    {
        [Tooltip("连续开火 次数")]
        [SerializeField] int ContinuousFireCount = 1;
        [Tooltip("连续开火 间隔系数")]
        [SerializeField] float ContinuousFireDtTimeParam = 0.2f;
        int m_CurFireCount = 0;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
            m_CurTime = GetDtTime() * 0.8f;
            m_CurFireCount = 0;
        }
        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WBounceGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WBounceGun);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_bouncegun[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_bouncegun_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Update()
        {
            base.Update();
            if (m_CurFireCount > 0 && m_CurTime > GetDtTime() * ContinuousFireDtTimeParam)
            {
                FireImp();
            }
        }
        protected override void Fire()
        {
            if (m_CurFireCount == 0)
            {
                m_CurFireCount = ContinuousFireCount;
            }
            FireImp();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
        private void FireImp()
        {
            m_CurFireCount--;
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
    }
}


