using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{

    public UnityAction TankDead;

    public int HP = 100;
    private int nowHP= 100;
    public GameObject TankExplusion;
    public Slider slider;

    public AudioClip audio;

    


    public void sendMessage()
    {
        if (nowHP <= 0) return;
        if (nowHP >= 0)
        {
            nowHP -= Random.Range(10, 20);
            slider.value = (float)nowHP / HP;
        }
        if (nowHP <= 0)
        {
            AudioSource.PlayClipAtPoint(audio, this.transform.position);
            GameObject.Instantiate(TankExplusion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            TankDead();

        }

    }
    
}
