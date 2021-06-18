
using EZ.Data;
using EZ.DataMgr;
using Game;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Weapon
{
    public class ElecgunGun : Gun
    {
        private double m_CacheDamage = 0;
        private bool m_CanFire = false;
        private bool m_HassEffect = true;
        private float m_EffectTime = 0;

        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            m_CurTime = 0;
            //InitDamage();
            m_CacheDamage = m_Damage;
            m_Damage = 0;
        }

        public override void InitByCardData(GunCard_TableItem cardData)
        {
            base.InitByCardData(cardData);
            m_CacheDamage = m_Damage;
            m_Damage = 0;
        }
        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WElecGun);
            m_DtTime = weaponItem.dtime / Global.gApp.gSystemMgr.GetSkillMgr().GetHitTimeSkillParam();
            WeaponMgr weaponMgr = Global.gApp.gSystemMgr.GetWeaponMgr();
            int gunLevel = weaponMgr.GetWeaponLevel(GameConstVal.WElecGun);
            //WeaponData weaponData = Global.gApp.gGameData.GetWeaponData().Find(l => l.level == gunLevel);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);
            double atk = weaponLevelData.weapon_elecgun[(int)MWeapon.Atk];
            if (GetRealQuality(weaponItem) > 0)
            {
                atk = weaponLevelData.weapon_elecgun_super[(int)MWeapon.Atk];
            }
            SetDamage(atk);
            m_CacheDamage = m_Damage;
            m_Damage = 0;
        }
        public override double GetBaseDamage()
        {
            return m_CacheDamage;
        }
        public override void SetDamageCoefficient(float damageCoefficient)
        {
            m_CacheDamage *= damageCoefficient;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            m_HassEffect = true;
        }
        protected override void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                m_CurTime = m_CurTime + dtTime;
                m_EffectTime += dtTime;
                float newDtTime = GetDtTime();
                if (m_CurTime >= newDtTime)
                {
                    m_CurTime = m_CurTime - newDtTime;
                    m_Damage = m_CacheDamage;
                    m_CanFire = true;
                    m_EffectTime = 0;
                    m_HassEffect = false;
                    Fire();
                }
                else
                {
                    if (m_EffectTime > newDtTime / 2)
                    {
                        m_EffectTime = 0;
                        m_HassEffect = true;
                    }
                    if (m_HassEffect)
                    {
                        m_Damage = 0;
                        m_HassEffect = false;
                    }
                    else
                    {
                        m_Damage = 0;
                    }
                    m_CanFire = false;
                    Fire();
                }
            }
        }
        protected override void Fire()
        {
            if (m_CanFire)
            {
                InstanceFireBulletEffect();
                InstanceNormalBullet(true, true);
                foreach (GameObject go in m_FireBullets)
                {
                    if (go.transform.localScale.z > 0)
                    {
                        base.Fire();
                        break;
                    }
                }
            }
            else
            {
                MustCallFire();
                if (m_Player != null)
                    m_Player.GetFight().PlayUAnim(GameConstVal.Shoot);
                InstanceFireBulletEffect();
                InstanceNormalBullet(true, true);
            }
        }
    }
}

