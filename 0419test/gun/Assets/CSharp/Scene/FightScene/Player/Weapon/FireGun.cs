
using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.Weapon
{
    public class FireGun : Gun
    {
        public GameObject m_StopEffect;
        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WFireGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WFireGun);
            Guns_dataItem weaponLevelData  = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_firegun[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_firegun_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
        }

        protected override void Fire()
        {
            base.Fire();
            InstanceFireBullet();
        }
        void InstanceFireBullet()
        {
            base.Fire();
            InstanceFireBulletEffect();
            InstanceNormalBullet();
        }

        protected override void OnEnable()
        {
            FreshOffsetTime(true);
        }

        protected override void OnDisable()
        {
           
            foreach (GameObject go in m_FireBullets)
            {
                GameObject stopEffect = Instantiate(m_StopEffect);
                stopEffect.transform.SetParent(FirePoint, false);
                stopEffect.transform.position = go.transform.position;
                stopEffect.transform.localRotation = go.transform.localRotation;
            }
            base.OnDisable();
        }
    }
}

