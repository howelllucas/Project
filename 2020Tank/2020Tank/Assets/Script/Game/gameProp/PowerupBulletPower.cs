using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBulletPower : porpBase
{
    GameObject game01;


     void Start()
    {
        game01 = GameObject.FindGameObjectWithTag("Player");
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag=="Player")
        {
            //道具效果
            attackSpeed();
            Porp.resetPorpCreat();

            GameObject.Destroy(this.gameObject);

        }
    }
    //加攻速方法
    public void addAttackSpeed()
    {
        game01.transform.GetComponent<PlayerTank>().bulletTime += 0.2f;

    }
    public void attackSpeed()
    {
        game01.transform.GetComponent<PlayerTank>().bulletTime -= 0.2f;
        Invoke("addAttackSpeed", 5);
    }

}
