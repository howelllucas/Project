using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtest
{
    public void update1()
    {
        Debug.Log("1111112");
    }
}
public class Test1 : MonoBehaviour
{
    private void Start()
    {
        //测试ui
        UIMgr.GetInstance().ShowPanel("LoginPanel", Ui_Type.mid);
        testtest t = new testtest();
        MonoMgr.GetInstance().AddUpdateListener(t.update1);

        //AudioMgr.GetInstance().PlayBKMusic("0204");
        AudioMgr.GetInstance().PlaySound("SwordSwing", false, (o) =>
        {
            Debug.Log("1256789");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //缓存池
            //PoolMgr.GetInstance().GetObj("Cube");
            //资源同步加载
            ResMgr.GetInstance().LoadRes<GameObject>("Cube");
        }
        if (Input.GetMouseButtonDown(1))
        {
            //缓存池
            //PoolMgr.GetInstance().GetObj("Sphere");
            //资源异步加载
            ResMgr.GetInstance().LoadAsync<GameObject>("Cube",(obj)=> {
                obj.transform.localScale = Vector3.one * 2;
            });
        }
    }
}
