
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class ShotGun : Gun
    {
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
        }
        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WShotGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WShotGun);
            //WeaponData weaponData = Global.gApp.gGameData.GetWeaponData().Find(l => l.level == gunLevel);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_shotgun[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_shotgun_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Fire()
        {
            MustCallFire();
            Global.gApp.gShakeCompt.StartShake(0, 0.1f, 0.03f);
            //base.Fire();
            Global.gApp.gAudioSource.PlayOneShot(FireClip);
            InstanceNormalBullet();
            if (m_FireEffect != null)
            {
                m_FireEffect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>().Play();
            }
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
    }
}

