using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupHealth : porpBase
{
    Slider HPSlider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            HPSlider = other.transform.Find("HUD/Canvas/Slider").GetComponent<Slider>();
            //道具效果
            other.GetComponent<PlayerTank>().currHP += 10;
            if (other.GetComponent<PlayerTank>().currHP <= other.GetComponent<PlayerTank>().HP)
            {
                HPSlider.value = other.GetComponent<PlayerTank>().currHP / other.GetComponent<PlayerTank>().HP;
            }
            else HPSlider.value = 1;


            Porp.resetPorpCreat();
            GameObject.Destroy(this.gameObject);
            Debug.Log("加血");
        }
    }
}
