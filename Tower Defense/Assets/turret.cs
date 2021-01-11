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
                timer -= attackRateTime;
                attack();
            }
            
        }

        private void attack()
        {
            GameObject bulletg= GameObject.Instantiate(bullet, bulletPoint.transform.position, bulletPoint.transform.rotation);
            bulletg.GetComponent<bullet>().setTarget(enemyList[0].transform);
        }

    }

}

