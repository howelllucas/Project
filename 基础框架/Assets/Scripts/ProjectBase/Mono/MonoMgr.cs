using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : BaseManager<MonoMgr>
{
    private MonoController controller = new MonoController();

    public MonoMgr()
    {
        GameObject obj = new GameObject("Mono");
        controller=obj.AddComponent<MonoController>();
    }
   //封装添加事件的方法，把Controller里面的添加时间方法封装一次，因为使用时是在这个单例模式上使用的
    public void AddUpdateListener(UnityAction action)
    {
        controller.updateEvent += action;
        
    }
    public void removeUpdateListener(UnityAction action)
    {
        controller.updateEvent -= action;
    }
    //封装一次开启协程的方法，因为在Controller是继承mono的，他有这个方法
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
}
