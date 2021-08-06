using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerType
{
    idle,
    attack,
    run,
    hurt,
    dead
}

public class PlayerState : MonoBehaviour
{
    private string name;
    private float hp;
    public float tinalHp = 100;
    private int attack;
    private int speed;

    private void Start()
    {
        speed = GetComponent<CharacterMotor>().speed;
        hp = tinalHp;
        
    }

    public void Attack()
    {

    }
    public void Hurt(float num)
    {
        if (hp>0)
        {
            hp -= num;
            if (hp<=0)
            {
                hp = 0;
                dead();
            }
        }
        
    }
    public void dead()
    {
        Destroy(this.gameObject, 5);
    }
}
