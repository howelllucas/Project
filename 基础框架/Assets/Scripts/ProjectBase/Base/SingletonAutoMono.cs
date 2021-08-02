using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 继承这个自动单例模式基类 不需要手动添加脚本
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance==null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).ToString();
            //切场景不想被删除
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }
        
        return instance;
    } 
}
