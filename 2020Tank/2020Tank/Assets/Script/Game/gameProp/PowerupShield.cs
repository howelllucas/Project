using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShield : porpBase
{
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            //道具效果
            other.GetComponent<PlayerTank>().currHP += 10;
            Porp.resetPorpCreat();
            GameObject.Destroy(this.gameObject);
            Debug.Log("加攻速");
        }
    }
}
