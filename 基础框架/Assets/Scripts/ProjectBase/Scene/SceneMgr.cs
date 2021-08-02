using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{
    //fun 是切换场景后需要执行的函数，是写在调用这个方法的类里面（比如副本管理类，调
    //用了切场景的方法，那么他那里就有切完场景需要调用的函数）
    //同步加载
    public void LoadScenes(string name, UnityAction fun)
    {
        SceneManager.LoadScene(name);

        fun();
    }
    //异步加载
    public void LoadScenesAsyn(string name,UnityAction fun)
    {
        MonoMgr.GetInstance().StartCoroutine(reallyLoadScenesAsyn(name,fun));
    }
    //通过协程异步加载
    private IEnumerator reallyLoadScenesAsyn(string name,UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while (!ao.isDone)
        {
            //用事件中心模块 触发一个事件 并且传出参数 外部想用就用
            EventCenter.GetInstance().EventTrigger("进度条更新", ao.progress );
            yield return ao.progress;
        }
        fun();
    } 
}
