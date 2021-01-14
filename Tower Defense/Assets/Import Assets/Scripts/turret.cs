using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///
    ///</summary>
    public class turret : MonoBehaviour
    {
        public List<GameObject> enemyList = new List<GameObject>();

        //攻击时间间隔
        public float attackRateTime = 1;
        //攻击时间
        public float timer;
        //获取子弹出生点
        public GameObject bulletPoint;
        //子弹
        public GameObject bullet;
        //获取炮台
        public Transform turretTai;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag=="enemy")
            {
                enemyList.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "enemy")
            {
                enemyList.Remove(other.gameObject);
            }
        }
        private void Start()
        {
            timer = attackRateTime;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (enemyList.Count>0&&timer >= attackRateTime)
            {
                timer = 0;
                attack();
            }
            
        }

        private void attack()
        {
            if (enemyList.Count>0&&enemyList[0]!=null)
            {
                Vector3 Yposition = enemyList[0].transform.position;
                Yposition.y= turretTai.transform.position.y ;

                
                turretTai.transform.LookAt(enemyList[0].transform);

            }
            if (enemyList[0]==null)
            {
                //调用重新整理集合的方法
                updateEnemyList();
            }
            if (enemyList.Count>0)
            {
                GameObject bulletg = GameObject.Instantiate(bullet, bulletPoint.transform.position, bulletPoint.transform.rotation);
                bulletg.GetComponent<bullet>().setTarget(enemyList[0].transform);
            }
            else
            {
                timer = attackRateTime;

            }
        }
            

        private void updateEnemyList()
        {
            List<int> emptyList = new List<int>();

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i]==null)
                {
                    emptyList.Add(i);
                }
            }
            for (int j = 0; j < emptyList.Count; j++)
            {
                enemyList.RemoveAt(emptyList[j] - j);
            }
        }
        
        

    }

}

