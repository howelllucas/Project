using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Weapon
{
    public abstract class SecondSupportGun : Gun
    {
        protected override float GetDtTime()
        {
            return m_DtTime;
        }
        protected override double GetDamage()
        {
            return m_Damage;
        }

        protected override void InitDamage()
        {
            Gun gun = Global.gApp.CurScene.GetMainPlayerComp().GetWeaponMgr().GetFirstGun();
            if (gun != null)
            {
                m_Damage = gun.GetBaseDamage() * 0.1f;

                Debug.Log("SecondSupportGun m_Damage " + m_Damage);
            }
        }
    }
}
