using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public ItemManager[] itenManader;
        public Transform startPoint ;
        public float waveDate;
        private GameObject[] enemysArrays;
        public int index=1;

        public GameObject waveUI;
        public int waveNumber;

        public GameObject bossUI;

        private void Awake()
        {
            enemysArrays = this.transform.GetComponent<enemys>().enemysArray;
            waveNumber = Convert.ToInt32(waveUI.GetComponent<Text>().text);
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
            if( index >= 0&& index < itenManader.Length)//总关卡数
            {
                for (int i = 0; i < itenManader[index].dataArray.Length; i++)//关卡里面的波数
                {
                    for (int j = 0; j < Convert.ToInt32(itenManader[index].dataArray[i].enemyCount); j++)//每一波里面的敌人数量
                    {

                        GameObject.Instantiate(enemysArrays[Convert.ToInt32(itenManader[index].dataArray[i].enemyID)], startPoint.position, Quaternion.identity);
                        aliveEnemyCount++;


                        if (j != Convert.ToInt32(itenManader[index].dataArray[i].enemyCount) - 1)
                        {
                            yield return new WaitForSeconds(Convert.ToInt32(itenManader[index].dataArray[i].timeDate));
                        }
                        
                    }
                    //ui显示波数
                    waveUI.GetComponent<Text>().text = waveNumber++.ToString();
                    while (aliveEnemyCount > 0)
                    {

                        yield return 0;
                    }
                    if (i == itenManader[index].dataArray.Length - 2)
                    {
                        //显示boss来袭U
                        bossUI.SetActive(true);
                        yield return new WaitForSeconds(2.0f);
                        bossUI.SetActive(false);
                        
                    }
                    yield return new WaitForSeconds(waveDate);
                }//关卡for-end
                while (aliveEnemyCount > 0)
                {
                    yield return 0;
                }
                
                //显示通关，开始下一关UI
                gameManager.instance.victoryUI();

            }//游戏关卡forend
            else
            {
                while (aliveEnemyCount > 0)
                {
                    yield return 0;
                }
                gameManager.instance.WinUI();
            }
            
            
        }


    }

}

