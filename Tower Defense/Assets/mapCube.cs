using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void creatTurret(GameObject gam)
        {
            mapCubeGo = GameObject.Instantiate(gam, this.transform.position, Quaternion.identity);
            GameObject effect = GameObject.Instantiate(buildEffect, this.transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    
    }

}

