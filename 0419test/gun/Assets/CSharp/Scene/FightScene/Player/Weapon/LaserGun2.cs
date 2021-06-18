
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Weapon
{
    public class LaserGun2 : Gun
    {
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            m_CurTime = 0;
            //InitDamage();
        }
        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WLaserX);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WLaserX);
            //WeaponData weaponData = Global.gApp.gGameData.GetWeaponData().Find(l => l.level == gunLevel);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_laserX[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_laserX_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Fire()
        {
            base.Fire();
            InstanceFireBulletEffect();
            InstanceNormalBullet(true);
        }
    }
}

