using EZ.Data;
using UnityEngine;

namespace EZ
{
    public class CarCarrierWeapon : BaseCarrierWeapon
    {
        private float m_DtTime = 1;

        public override void init(float damageCoefficient)
        {
            base.init(damageCoefficient);
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WCarrier);
            m_DtTime = weaponItem.dtime;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            if (weaponLevelData != null)
            {
                double atk = weaponLevelData.Carrier[(int)MWeapon.Atk];
                m_Damage = atk;
            }
        }
    }
}
