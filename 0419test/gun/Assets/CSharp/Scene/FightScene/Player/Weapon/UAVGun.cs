
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ.Weapon
{
    public class UAVGun : Gun
    {

        public override void Init(FightWeaponMgr mgr)
        {
            base.Init(mgr);
            InitDamage();
            FirePoint = transform.parent;
        }

        protected override void InitDamage()
        {
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WUAV);

            m_DtTime = weaponItem.dtime;

            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.UAV[(int)MWeapon.Atk];

            int curLv = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(GameConstVal.WUAV);
            GunsSub_dataItem levelItem = Global.gApp.gGameData.GunSubDataConfig.Get(curLv);
            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + weaponItem.qualevel, levelItem);

            float campAtkInc = Global.gApp.CurScene.GetMainPlayerComp().GetBuffMgr().CampPetIncAtk;
            m_Damage = atk * weaponParams[0] * (1 + campAtkInc);
        }

        protected override void Fire()
        {
            Global.gApp.gShakeCompt.StartShake(0, 0.1f, 0.03f);
            base.Fire();
            InstanceNormalBullet();
            //Shining.VibrationSystem.Vibrations.instance.Vibrate1ms();
        }
    }
}

