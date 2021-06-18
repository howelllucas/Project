using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    [System.Serializable]
    public class turretDate 
    {
        public GameObject[] turretPrefab;
        //public GameObject turretPrefab;
        //public int cost;
        //public GameObject turretUpPrefab;
        //public int costUp;
        //public GameObject turretUpPrefab3;
        //public int costUp3;
        public turretType type;

    }
    public enum turretType
    {
        LaserTurret,
        MissileTurret,
        StandardTurret
    }

}

