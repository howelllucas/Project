using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBulletPower : porpBase
{
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            //道具效果
            other.GetComponent<PlayerTank>().bulletTime += 0.5f;
            Porp.resetPorpCreat();
            GameObject.Destroy(this.gameObject);
            Debug.Log("加攻速");
        }
    }
}
