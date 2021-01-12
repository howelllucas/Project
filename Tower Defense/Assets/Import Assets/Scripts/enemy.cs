using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class enemy : MonoBehaviour
    {
        public int Speed = 10;
        public float HP = 150;
        Transform[] wayTrans;
        int index = 0;
        Rigidbody rid;
        public GameObject deadEffect;
        private void Awake()
        {
            wayTrans = wayPoint.tans;
            rid = this.transform.GetComponent<Rigidbody>();
        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (index>= wayTrans.Length-1)
            {
                return;
            }
            this.transform.LookAt(wayTrans[index + 1]);
            this.transform.position += this.transform.forward * Speed * Time.deltaTime;
            
            if (Vector3.Distance(this.transform.position, wayTrans[index+1].position)<0.2f)
            {
                index++;
            }
            if (index>= wayTrans.Length-1)
            {
                finishEnd();
            }
            
        }
        void finishEnd()
        {
            
            GameObject.Destroy(this.gameObject);
        }
        void OnDestroy()
        {
            enemySpawner.aliveEnemyCount--;
            
        }

        public void takeDemage(float demage)
        {
            if (HP<=0)
            {
                return;
            }
            HP -= demage;
            if (HP <= 0)
            {
                dead();
            }
        }
        private void dead()
        {
            GameObject eff= GameObject.Instantiate(deadEffect, transform.position, Quaternion.identity);
            Destroy(eff, 1.5f);
            Destroy(this.gameObject);
        }
    }

}

