using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class Tankenemy : MonoBehaviour
{

    public UnityAction TankDead;
    float attackC;
    Vector2 imageA;
    public int HP = 100;
    private int nowHP= 100;
    public GameObject TankExplusion;
    public Slider slider;

    public AudioClip audioA;

    private Image imageAttach;
    private void Awake()
    {
        attackC = GetComponent<TankAI>().attackRange;
        imageA = new Vector2(attackC, attackC);
    }
    private void Start()
    {
       
        imageAttach = this.transform.GetComponentInChildren<Image>();
        imageAttach.color = new Color(255, 0, 0, 0.5f);
        imageAttach.rectTransform.sizeDelta = imageA;
    }



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
            AudioSource.PlayClipAtPoint(audioA, this.transform.position);
            GameObject.Instantiate(TankExplusion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            TankDead();

        }

    }
    
}
