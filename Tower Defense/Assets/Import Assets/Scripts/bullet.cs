using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class bullet : MonoBehaviour
    {
        public float demage=50;

        public float speed=40;

        private Transform target;

        public GameObject deadEffect;

        //通过距离判断是否碰撞
        public float distanceArriveTarget = 1.2f;
        
        public void setTarget(Transform ta)
        {
            target = ta;
        }

        private void Update()
        {
            if (target==null)
            {
                Des();
                return;  
            }
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            Vector3 vec = this.transform.position - target.position;

            if (vec.magnitude< distanceArriveTarget)
            {
                target.GetComponent<enemy>().takeDemage(demage);
                Des();

            }
        }

        private void Des()
        {
            GameObject ga = GameObject.Instantiate(deadEffect, this.transform.position, Quaternion.identity);
            Destroy(ga, 1);
            Destroy(this.gameObject);
        }

    }

}

