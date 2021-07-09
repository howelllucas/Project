using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例模式基类
/// </summary>
/// <typeparam name="T">只需继承就可使用</typeparam>
public class BaseManager <T>  where T : new()
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance==null)
        {
            instance = new T();
        }
        return instance;
    }
}
//使用时只需继承这个单例模式基类 就可以使用单例模式
public class Mana :BaseManager<Mana>
{

}
//测试
public class Test
{
    void Main()
    {
        Mana.GetInstance();
    }
}
