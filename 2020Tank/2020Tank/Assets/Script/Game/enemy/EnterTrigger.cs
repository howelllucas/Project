using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //ui显示波次
    public GameObject UILevel;
    void Start()
    {
        creatEnemyTank();
        Level = intollLevel;
        UILevel.GetComponent<Text>().text = intollLevel.ToString();
    }

    
    void Update()
    {
        //Debug.Log(enemyArr.Count);
        //关卡判断
        if (enemyArr.Count<=0)
        {
            //游戏结束，玩家胜利
            
            creatEnemyTank();
            Level--;
            UILevel.GetComponent<Text>().text = Level.ToString();
            if (Level==0)
            {
                gameOverWin();
                Debug.Log("玩家胜利");
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
    void gameOverWin()
    {
        uiMain1.Instance.gameOverWin();
    }
}
