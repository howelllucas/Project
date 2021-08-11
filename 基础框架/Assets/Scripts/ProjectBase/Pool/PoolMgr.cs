using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class PoolMgr : 
{

    public void GetObj()
    {

    }
    public void PushObj()
    {

=======
using UnityEngine.Accessibility;
using UnityEngine.Events;
/// <summary>
/// 抽屉类，也就是一列
/// </summary>
public class PoolData
{
    public GameObject fatherObj;

    public List<GameObject> poolList;

    public PoolData ( GameObject obj,GameObject poolObj)
    {
        poolList = new List<GameObject>() {};
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(poolObj.transform);

        pushObj(obj);

    }
    public GameObject GetObj()
    {
        GameObject obj = null;

        obj = poolList[0];
        poolList.RemoveAt(0);

        obj.SetActive(true);

        obj.transform.parent = null;
        return obj;

    }
    public void pushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.SetParent(fatherObj.transform);
    }

}
/// <summary>
/// 衣柜类，整体缓存池类
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    public Dictionary<string, PoolData> PoolDir = new Dictionary<string, PoolData>();

    private GameObject poolObj=null;

    public void GetObj(string name,UnityAction<GameObject> callBack)
    {

        

        
        if (PoolDir.ContainsKey(name)&&PoolDir[name].poolList.Count>0)//有抽屉有东西
        {
            callBack(PoolDir[name].GetObj());
            //obj =PoolDir[name].GetObj(name);
        }
        else//没有抽屉没有东西
        {
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o)=> {
                o.name = name;
                //因为是异步加载，所以不能把新加载的资源同步的传出去使用（下面的方式），得等资源加载完才能使用，
                //所以使用事件委托函数，等资源加载完调用这个委托方法来使用资源
                //这个是用我们定义的事件委托，同时把创建的o传出去，当外部执行调用的时候，给外部使用
                callBack(o);
            });
            //这个同步加载资源的方法，是资源加载完传给obj，再retrue出去直接使用
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //obj.name = name;
            //retrue obj
        }
        
    }

    public void PushObj(string name,GameObject obj)
    {
        if (poolObj==null)
        {
            poolObj = new GameObject("Pool");
        }
        if (PoolDir.ContainsKey(name))//有
        {
            PoolDir[name].pushObj(obj);
        }
        else//没有
        {
            PoolDir.Add(name, new PoolData(obj, poolObj));
        }
    }

    public void Clear()
    {
        PoolDir.Clear();
        poolObj = null;
>>>>>>> 10d19dcc9c5b5fe46b7fa522c786afe39b0dae5f
    }
}
