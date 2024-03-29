﻿
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class S_FireTurret003Gun : SecondSupportGun
    {

        BaseTurret m_FireTurret;
        protected override void Awake()
        {
            base.Awake();
            FirePoint = transform.Find(GameConstVal.FirePoint);
            if (FirePoint != null && m_FireEffect != null)
            {
                m_FireEffect.transform.SetParent(FirePoint, false);
                m_FireEffect.SetActive(true);
            }

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret003);
            m_DtTime = weaponItem.dtime;

            InitDamage();
            m_FireTurret = GetComponentInParent<BaseTurret>();
            m_FireTurret.SetAtkRange(AtkRange);
        }
        //protected override void InitDamage()
        //{
        //    ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WSFireTurret003);
        //    m_DtTime = weaponItem.dtime;


        //    GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
        //    double atk = weaponLevelData.S_Turret003[(int)MWeapon.Atk];
        //    m_Damage = atk;
        //}

        protected override void Fire()
        {
            base.Fire();
            InstanceFireBulletEffect();
            InstanceNormalBullet(true);
        }
    }
}

