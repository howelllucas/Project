using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseUI<T> : MonoBehaviour where T:class
{
    //单例模式
    //两个静态
    private static T instance;
    public static T Instance  => instance ;
    
    private void Awake()
    {
        instance = this as T;
    }

    public virtual void ShowMe()
    {
        this.gameObject.SetActive(true);
    }
    public virtual void HideMe()
    {
        this.gameObject.SetActive(false);
    }
}
