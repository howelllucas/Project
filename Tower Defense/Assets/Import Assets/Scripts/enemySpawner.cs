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
        public Transform startPoint ;
        public float waveDate;

        private void Start()
        {
            StartCoroutine(creatEnemy());
        }
        IEnumerator creatEnemy()
        {
            foreach (var item in wa)
            {
                for (int i = 0; i < item.enemyCount; i++)
                {
                    GameObject.Instantiate(item.enemy, startPoint.position, Quaternion.identity);
                    aliveEnemyCount++;
                    if (i!=item.enemyCount-1)
                    {
                        yield return new WaitForSeconds(item.timeDate);
                    }
                    
                }
                while (aliveEnemyCount>0)
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

