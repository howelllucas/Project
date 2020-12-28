using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBulletBounce : porpBase
{
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            other.GetComponent<PlayerTank>().moveSpeed += 2;
            Porp.resetPorpCreat();
            GameObject.Destroy(this.gameObject);
            Debug.Log("加速");
        }
    }
}
