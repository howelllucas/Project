using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupShield : porpBase
{
    Slider defenceSlider;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag=="Player")
        {
            other.GetComponent<buffShow>().showDefence();
            defenceSlider = other.transform.Find("Shield/Slider").GetComponent<Slider>();
            //道具效果
            if (other.GetComponent<PlayerTank>().defence<60)
            {
                other.GetComponent<PlayerTank>().defence += 20;
                defenceSlider.value = other.GetComponent<PlayerTank>().defence / 20;
                Debug.Log("123214324325");
            }
            
            Porp.resetPorpCreat();
            GameObject.Destroy(this.gameObject);
            Debug.Log("防御");
        }
    }
}
