using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    
    public class mapCube : MonoBehaviour
    {
        [HideInInspector]
        public GameObject mapCubeGo;
        public GameObject buildEffect;
        public int turretLevel=0;

        public void creatTurret(GameObject gam)
        {
            mapCubeGo = GameObject.Instantiate(gam, this.transform.position, Quaternion.identity);
            
            GameObject effect = GameObject.Instantiate(buildEffect, this.transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            turretLevel++;
        }
        public void destoryTurret()
        {
            Destroy(mapCubeGo);
        }
        //升级方法
        public void upgradeTurret(GameObject gam)
        {
            destoryTurret();
            creatTurret(gam);
        }
    
    }

}

