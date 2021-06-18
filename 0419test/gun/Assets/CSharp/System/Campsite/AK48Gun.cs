
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class AK48Gun : Gun
    {

        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            //InitDamage();
        }

        protected override void InitDamage()
        {
            SetDamage(2);
        }

        protected override void Fire()
        {
            Global.gApp.gShakeCompt.StartShake(0, 0.1f, 0.03f);
            //base.Fire();
            InstanceNormalBullet();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
    }
}

