using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBulletBounce : porpBase
{

    GameObject game01;


    void Start()
    {
        game01 = GameObject.FindGameObjectWithTag("Player");

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            //道具效果
            moveSpeed();
            Porp.resetPorpCreat();

            GameObject.Destroy(this.gameObject);

        }
    }
    //加移动速度方法
    public void addmoveSpeed()
    {
        game01.transform.GetComponent<PlayerTank>().moveSpeed -= 12f;

    }
    public void moveSpeed()
    {
        game01.transform.GetComponent<PlayerTank>().moveSpeed += 12f;
        Invoke("addmoveSpeed", 5);
    }

}
