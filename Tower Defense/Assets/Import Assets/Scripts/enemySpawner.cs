using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    ///<summary>
    ///敌人管理器
    ///</summary>
    public class enemySpawner : MonoBehaviour
    {
        //敌人存活数量
        public static int aliveEnemyCount=0;
        public Wave[] wa;
        public ItemManager itenManader;
        public Transform startPoint ;
        public float waveDate;
        private GameObject[] enemysArrays;

        private void Awake()
        {
            enemysArrays = this.transform.GetComponent<enemys>().enemysArray;
        }
        private void Start()
        {
            StartCoroutine(creatEnemy());
           
        }
        IEnumerator creatEnemy()
        {
            //foreach (var item in wa)
            //{
            //    for (int i = 0; i < item.enemyCount; i++)
            //    {
            //        GameObject.Instantiate(item.enemy, startPoint.position, Quaternion.identity);
            //        aliveEnemyCount++;
            //        if (i!=item.enemyCount-1)
            //        {
            //            yield return new WaitForSeconds(item.timeDate);
            //        }

            //    }
            //    while (aliveEnemyCount>0)
            //    {
            //        yield return 0;
            //    }
            //    yield return new WaitForSeconds(waveDate);
            //}
            //外部导入
            
            for (int i = 0; i < itenManader.dataArray.Length; i++)//波数
            {
                for (int j = 0; j < Convert.ToInt32(itenManader.dataArray[i].enemyCount); j++)//每一波里面的敌人数量
                {
                    
                    GameObject.Instantiate(enemysArrays[Convert.ToInt32(itenManader.dataArray[i].enemyID)], startPoint.position, Quaternion.identity);
                    aliveEnemyCount++;
                    
                    if (j != Convert.ToInt32(itenManader.dataArray[i].enemyCount) - 1)
                    {
                        yield return new WaitForSeconds(Convert.ToInt32(itenManader.dataArray[i].timeDate));
                    }
                }
                while (aliveEnemyCount > 0)
                {
                    
                    yield return 0;
                }
                yield return new WaitForSeconds(waveDate);
            }
            while (aliveEnemyCount>0)
            {
                yield return 0;
            }
            gameManager.instance.victoryUI();
        }


    }

}

