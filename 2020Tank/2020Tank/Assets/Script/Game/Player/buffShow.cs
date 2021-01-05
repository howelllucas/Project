using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffShow : MonoBehaviour
{
    public GameObject attack;
    public GameObject defence;
    public GameObject movespeed;
    public GameObject hp;
    public GameObject FX;
    //摇杆
    public ETCJoystick joystick;
    private void Start()
    {
        
    }
    //显示图标方法
    public void showAttack()
    {
        attack.SetActive(true);
        GameObject.Instantiate(FX, this.transform.position, FX.transform.rotation);
        movespeed.transform.GetComponentInParent<PlayerTank>().bulletTime -= 0.2f;
        Invoke("hideAttack", 5);
    }
    public void showDefence()
    {
        defence.SetActive(true);
        GameObject.Instantiate(FX, this.transform.position, FX.transform.rotation);
        Invoke("hideDefence", 5);
    }
    public void showMovespeed()
    {
        movespeed.SetActive(true);
        joystick.tmSpeed += 10f;
        GameObject.Instantiate(FX, this.transform.position, FX.transform.rotation);
        //movespeed.transform.GetComponentInParent<PlayerTank>().moveSpeed += 12f;
        Invoke("hideMovespeed", 5);
    }
    public void showHp()
    {
        hp.SetActive(true);
        GameObject.Instantiate(FX, this.transform.position, FX.transform.rotation);
        Invoke("hideHp", 5);
    }
    //隐藏图标方法
    public void hideAttack()
    {
        
        movespeed.transform.GetComponentInParent<PlayerTank>().bulletTime += 0.2f;
        attack.SetActive(false);
    }
    public void hideDefence()
    {
        
        defence.SetActive(false);
    }
    public void hideMovespeed()
    {
        
        joystick.tmSpeed -= 10f;
        //movespeed.transform.GetComponentInParent<PlayerTank>().moveSpeed -= 12f;
        movespeed.SetActive(false);
    }
    public void hideHp()
    {
        
        hp.SetActive(false);
    }
}