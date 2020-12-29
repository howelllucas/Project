using System;
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
            other.GetComponent<buffShow>().showAttack();
            Porp.resetPorpCreat();
            
            GameObject.Destroy(this.gameObject);
            
        }
    }
    
    

}
