using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Swordman,
    Magician
}

public class playerStatus : MonoBehaviour
{
    public PlayerType playerType;
    public string playerName="默认名字";
    public int level = 1;
    public int hp = 100;
    public float nowHp = 100f;
    public int mp = 100;
    public float nowMp = 100f;
    public int coin = 200;

    public int attack;
    public int attack_plus;
    public int def;
    public int def_plus;
    public int speed;
    public int speed_plus;

    public int point_remain;
    public int total_Exp;
    public float exp;

    private void Start()
    {
        getExp(exp);
    }
    public void getCoin(int num)
    {
        coin += num;
    }
    public bool getPoint(int point = 1)
    {
        if (point_remain>=point)
        {
            point_remain-=point;
            return true;
        }
        return false;
    }
    public void useDrug(int hp, int mp)
    {
        nowHp += hp;
        nowMp += mp;
        if (nowHp > this.hp)
        {
            nowHp = this.hp;
        }
        if (nowMp > this.mp)
        {
            nowMp = this.mp;
        }
    }
    public void getExp(float exp)
    {
        
        total_Exp = 100 + level * 30;
        this.exp += exp;
        while (this.exp>=total_Exp)
        {
            level++;
            total_Exp = 100 + level * 30;
            this.exp -= total_Exp;
        }
        float value = this.exp / total_Exp;

        EXPBar.instance.showValue(value);

        
    }
}
