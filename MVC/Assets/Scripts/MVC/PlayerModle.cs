using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModle 
{
    private string playerName;
    public string PlayerName
    {
        get
        {
            return playerName;
        }
    }
    private int lev;
    public int Lev
    {
        get
        {
            return lev;
        }
    }
    private int money;
    public int Menoy
    {
        get
        {
            return money;
        }
    }
    private int gem;
    public int Gem
    {
        get
        {
            return gem;
        }
    }
    private int power;
    public int Power
    {
        get
        {
            return power;
        }
    }

    private int hp;
    public int Hp
    {
        get
        {
            return hp;

        }
    }
    private int atk;
    public int Atk
    {
        get
        {
            return atk;

        }
    }
    private int def;
    public int Def
    {
        get
        {
            return def;

        }
    }
    private int crit;
    public int Crit
    {
        get
        {
            return crit;
        }
    }
    //单例模式
    private static PlayerModle instance;

    public static PlayerModle Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new PlayerModle();
                instance.Info();
            }
            return instance;
        }
    }
    

    private event UnityAction<PlayerModle> ModleEvent;

    public void AddModleEventListener(UnityAction<PlayerModle> fun)
    {
        ModleEvent += fun;
    }

    public void RemoveModleEventListener(UnityAction<PlayerModle> fun)
    {
        ModleEvent -= fun;
    }

    //初始化
    public void Info()
    {
        playerName = PlayerPrefs.GetString("playerName", "默认值");
        lev = PlayerPrefs.GetInt("lev", 1);
        money = PlayerPrefs.GetInt("money", 2);
        gem = PlayerPrefs.GetInt("gem", 10);
        power = PlayerPrefs.GetInt("power", 100);

        hp = PlayerPrefs.GetInt("lev", 100);
        atk = PlayerPrefs.GetInt("atk", 200);
        def = PlayerPrefs.GetInt("def", 20);
        crit = PlayerPrefs.GetInt("crit", 10);
        

    }
    //更新数值
    public void updateInfo()
    {
        lev += 1;
        money += lev;
        gem += lev;
        power += 100;

        hp += 100;
        atk += 1;
        def += 10;
        crit += 20;

        saveInfo();
    }
    //存储数值
    public void saveInfo()
    {
        PlayerPrefs.GetInt("lev", lev);
        PlayerPrefs.GetInt("money", money);
        PlayerPrefs.GetInt("gem", gem);
        PlayerPrefs.GetInt("power", power);

        PlayerPrefs.GetInt("hp", hp);
        PlayerPrefs.GetInt("atk", atk);
        PlayerPrefs.GetInt("def", def);
        PlayerPrefs.GetInt("crit", crit);

        update();
    }

    //通知外部数据更新,也就是执行委托函数
    public void update()
    {
        if (ModleEvent!=null)
        {
            ModleEvent(this);
        }
    }

}
