
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class CrossbowGun : Gun
    {

        private Animator m_Animator;
        protected override void Awake()
        {
            base.Awake();
            m_Animator = GetComponent<Animator>();
        }
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
        }

        protected override void InitDamage()
        {

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WCrossbowGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WCrossbowGun);
            //WeaponData weaponData = Global.gApp.gGameData.GetWeaponData().Find(l => l.level == gunLevel);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_crossbow[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_crossbow_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Fire()
        {
            m_Animator.Play(GameConstVal.GunAtk, -1, 0);
            Global.gApp.gShakeCompt.StartShake(0, 0.1f, 0.03f);
            base.Fire();
            InstanceNormalBullet();
        }
    }
}

