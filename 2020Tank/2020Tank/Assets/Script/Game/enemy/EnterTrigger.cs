using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public GameObject[] enemyPosArr;
    public GameObject[] enemyTankArr;
    
    void Start()
    {
        creatEnemyTank();
    }

    
    void Update()
    {
        
    }
    void creatEnemyTank()
    {
        for (int i = 0; i < enemyPosArr.Length; i++)
        {
            GameObject tank = enemyTankArr[Random.Range(0, enemyTankArr.Length)];
            GameObject.Instantiate(tank, enemyPosArr[i].transform.position, Quaternion.identity);
            
        }
    }
}
