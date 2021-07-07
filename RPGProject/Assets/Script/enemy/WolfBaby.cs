using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WolfState
{
    Idle,
    Walk,
    Attack,
    Death
}
public class WolfBaby : MonoBehaviour
{
    public WolfState state = WolfState.Idle;

    private string animationNOW= "WolfBaby-Idle";
    public int time = 1;
    public float timer;
    private CharacterController cc;

    public int speed = 1;
    public int hp = 100;

    private Color color;
    private void Start()
    {
        color=this.GetComponentInChildren<Renderer>().material.color ;
        cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (state==WolfState.Death)//死亡
        {
            this.GetComponent<Animation>().Play("WolfBaby-Death");
        }
        else if (state == WolfState.Attack)//攻击
        {

        }
        else//巡逻
        {

            this.GetComponent<Animation>().CrossFade(animationNOW);
            if (animationNOW == "WolfBaby-Walk")
            {
                cc.SimpleMove(transform.forward * speed);
            }
            timer += Time.deltaTime;
            if (timer>time)
            {
                timer = 0;
                RandomState();
                

                
               
            }
            
        }
    }
    private void RandomState()
    {
        int num= Random.Range(0, 2);
        if (num==0)
        {
            if (animationNOW != "WolfBaby-Walk")
            {
                transform.Rotate(transform.up, Random.Range(0, 360));
            }
            animationNOW = "WolfBaby-Walk";
        }
        else
        {

            animationNOW = "WolfBaby-Idle";
        }
    }
    public void takeDamage(int attack)
    {
        hp -= attack;
        StartCoroutine(takeRedEffect());
        if (hp<0)
        {
            state = WolfState.Death;
            Destroy(this.gameObject, 2f);
        }
    }
    IEnumerator takeRedEffect()
    {
        this.GetComponentInChildren<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        this.GetComponentInChildren<Renderer>().material.color = color;
    }
}
