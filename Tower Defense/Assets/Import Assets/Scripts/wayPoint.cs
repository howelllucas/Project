using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class wayPoint : MonoBehaviour
    {
        public static Transform[] tans;
        
        void Awake()
        {
            tans = new Transform[transform.childCount];
            
            for (int i = 0; i < tans.Length; i++)
            {
                tans[i] = this.transform.GetChild(i);
            }
            
        }
    }

}

