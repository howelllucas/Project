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

        public void setTarget(Transform ta)
        {
            target = ta;
        }

        private void Update()
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<enemy>().takeDemage(demage);
            GameObject.Instantiate(deadEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }

}

