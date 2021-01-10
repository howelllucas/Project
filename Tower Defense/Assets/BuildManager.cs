using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class BuildManager : MonoBehaviour
    {
        public turretDate LaserTurret;
        public turretDate MissileTurret;
        public turretDate StandardTurret;
        //当前选择的是哪个（需要创建的）
        public turretDate selectedTurretDate;

        public void onLaserTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = LaserTurret;
            }
        }
        public void onMissileTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = MissileTurret;
            }
        }
        public void onStandardTurret(bool isOn)
        {
            if (isOn)
            {
                selectedTurretDate = StandardTurret;
            }
        }
    }
    

}

