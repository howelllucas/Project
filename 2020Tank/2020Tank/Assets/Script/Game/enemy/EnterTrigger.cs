using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public GameObject[] enemyPosArr;
    public GameObject[] enemyTankArr;
    //敌人坦克对象组，用来记录坦克数量，方便判断波次
    public static List<GameObject> enemyArr = new List<GameObject>();
    //波次
    private int Level;
    //总波次
    public int intollLevel;
    
    void Start()
    {
        creatEnemyTank();
        Level = 1;
    }

    
    void Update()
    {
        //Debug.Log(enemyArr.Count);
        //关卡判断
        if (enemyArr.Count<=0)
        {
            //游戏结束，玩家胜利
            Debug.Log("玩家胜利");
            creatEnemyTank();
            Level++;
            if (Level>=intollLevel)
            {
                gameOver();
            }
        }
    }
    void creatEnemyTank()
    {
        for (int i = 0; i < enemyPosArr.Length; i++)
        {
            GameObject tank = enemyTankArr[Random.Range(0, enemyTankArr.Length)];
            GameObject.Instantiate(tank, enemyPosArr[i].transform.position, Quaternion.identity);
            
        }
    }
    void gameOver()
    {
        uiMain1.Instance.gameOver();
    }
}
