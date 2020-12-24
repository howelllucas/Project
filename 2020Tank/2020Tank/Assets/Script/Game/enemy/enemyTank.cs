using UnityEngine;
using System.Collections;

public class enemyTank: MonoBehaviour
{
    //实时距离
    private float distance;
    //追击距离
    private float followDistance;
    //攻击距离
    private float attackDistance;
    //目标玩家坦克
    private GameObject playerTank;
    //旋转速度
    private float rotationSpeed = 5.0f;
    //移动速度
    private float moveSpeed = 5.0f;
    void Start()
    {
        playerTank = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        distance = Vector3.Distance(this.transform.position, playerTank.transform.position);
        if (distance<followDistance)
        {
            //追击
            followMove();
        }
        else if (distance<attackDistance)
        {
            //攻击
        }
        else
        {

        }
    }
    //追击方法
    void followMove()
    {
        //旋转
        Vector3 rot = playerTank.transform.position-this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,Quaternion.LookRotation(rot),Time.deltaTime*rotationSpeed)；
        //移动
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
