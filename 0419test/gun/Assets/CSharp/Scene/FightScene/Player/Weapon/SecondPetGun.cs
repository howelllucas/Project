using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ.Weapon
{
    public abstract class SecondPetGun : Gun
    {
        protected override float GetDtTime()
        {
            return m_DtTime;
        }
        protected override double GetDamage()
        {
            return m_Damage;
        }
    }
}
