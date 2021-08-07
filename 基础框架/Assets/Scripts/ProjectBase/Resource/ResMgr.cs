using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源
    public T LoadRes<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //
        if ( res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            return res;
        }
        
        
    }
    //异步加载资源
    public void LoadAsync<T>(string name,UnityAction<T> callBack) where T :Object
    {

        MonoMgr.GetInstance().StartCoroutine(reallyLoadAsync(name,callBack));
       
    }
    //协程 UnityAction后面的T 表示注册到这个事件的方法需要带一个这个类型的参数
    private IEnumerator reallyLoadAsync<T>(string name, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest res = Resources.LoadAsync<T>(name);

        yield return res;

        if (res.asset is GameObject)
        {
            callBack(GameObject.Instantiate(res.asset) as T);
        }
        else
        {
            callBack(res.asset as T);
        }

    }
}
