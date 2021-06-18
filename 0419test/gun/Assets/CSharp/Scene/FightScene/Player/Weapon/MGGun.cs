
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class MGGun : Gun
    {

        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WMG);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WMG);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_mg[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_mg_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Fire()
        {
            base.Fire();
            InstanceNormalBullet();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
    }
}

