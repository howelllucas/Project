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
        //存当前建造的炮台
        public turretDate turretDate1;
        public GameObject buildEffect;
        public int turretLevel=0;
        public bool isfullLevel=false;
       
        public void creatTurret(turretDate gam)
        {
            turretDate1 = gam;
            mapCubeGo = GameObject.Instantiate(gam.turretPrefab[turretLevel], this.transform.position, Quaternion.identity);
            
            GameObject effect = GameObject.Instantiate(buildEffect, this.transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            turretLevel++;
            
        }
        public void destoryTurret()
        {
            
            
            Destroy(mapCubeGo);
        }
        public void alldestoryTurret()
        {
            isfullLevel = false;
            turretLevel = 0;
            Destroy(mapCubeGo);
        }
        //升级方法
        public void upgradeTurret()
        {
            
            destoryTurret();
            
            mapCubeGo = GameObject.Instantiate(turretDate1.turretPrefab[turretLevel], this.transform.position, Quaternion.identity);
            
            GameObject effect = GameObject.Instantiate(buildEffect, this.transform.position, Quaternion.identity);
            
            if (turretLevel<4)
            {
                turretLevel++;
            }
            else
            {
                isfullLevel = true;
            }
            
            

        }
    
    }

}

