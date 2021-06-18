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
        //是否为激光炮台
        public bool isLaser;
        //获取激光
        public LineRenderer lineLaser;
        public float Laserdemage = 50;
        public GameObject liserLight;
        //花费
        public int turretCost;
        //是否受到减速
        private bool isSlowDown = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag=="enemy")
            {
                enemyList.Add(other.gameObject);
            }
         
        }

        private void OnTriggerExit(Collider other)
        {
            if (isLaser == true)
            {
                enemyList[0].GetComponent<enemy>().Speed = enemyList[0].GetComponent<enemy>().interSpeed;
            }
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
            if (enemyList.Count > 0 && enemyList[0] != null)
            {
                Vector3 Yposition = enemyList[0].transform.position;
                Yposition.y = turretTai.transform.position.y;
                
                turretTai.transform.LookAt(enemyList[0].transform);

            }
            if (isLaser == false)
            {
                if (enemyList.Count > 0 && timer >= attackRateTime)
                {
                    timer = 0;

                    attack();
                }
            }
            else if(enemyList.Count > 0)
            {
                //生成激光
                if (lineLaser.enabled == false)
                {
                    lineLaser.enabled = true;
                }
                liserLight.SetActive(true);
                if (enemyList[0] == null)
                {
                        //调用重新整理集合的方法
                    updateEnemyList();
                    
                }
                if (enemyList.Count > 0)
                {
                    lineLaser.SetPositions(new Vector3[] { bulletPoint.transform.position, enemyList[0].transform.position });
                    enemyList[0].GetComponent<enemy>().takeDemage(Laserdemage * Time.deltaTime);
                    enemyList[0].GetComponent<enemy>().slowDown();
                    isSlowDown = true;
                    liserLight.transform.position = enemyList[0].transform.position;
                    
                   

                    

                }

            }
            else
            {

                isSlowDown = false;
                liserLight.SetActive(false);
                lineLaser.enabled = false;
            }
            
            
        }

        private void attack()
        {
            //if (enemyList.Count > 0 && enemyList[0] != null)
            //{
            //    Vector3 Yposition = enemyList[0].transform.position;
            //    Yposition.y = turretTai.transform.position.y;
            //    //Quaternion dir = Quaternion.LookRotation(Yposition);
            //    //turretTai.transform.rotation = Quaternion.Slerp(turretTai.transform.rotation, dir, 29 * Time.deltaTime);
            //    turretTai.transform.LookAt(enemyList[0].transform);

            //}
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

