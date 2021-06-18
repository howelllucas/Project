using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{
    public class TankCarrierWeapon : BaseCarrierWeapon
    {
        private float m_DtTime = 1;
        public override void init(float damageCoefficient)
        {
            base.init(damageCoefficient);

            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WTank_carrier);
            m_DtTime = weaponItem.dtime;


            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            if (weaponLevelData != null)
            {
                double atk = weaponLevelData.Tank_carrier[(int)MWeapon.Atk];
                m_Damage = atk * damageCoefficient;
            }
        }
    }
}
