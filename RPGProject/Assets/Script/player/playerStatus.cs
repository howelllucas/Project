using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public enum PlayerType
{
    Swordman,
    Magician
}

public class playerStatus : MonoBehaviour
{
    public PlayerType playerType;

    public int grade = 1;
    public int hp = 100;
    public int mp = 100;
    public int coin = 200;

    public int attack;
    public int attack_plus;
    public int def;
    public int def_plus;
    public int speed;
    public int speed_plus;

    public int point_remain;

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
}
