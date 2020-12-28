using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameProp : MonoBehaviour
{
    
    public GameObject gamePorp;

    void Start()
    {
        creatPorp01();


    }
    void creatPorp01()
    {

        GameObject go = GameObject.Instantiate(gamePorp, this.transform.position, Quaternion.identity);
        go.GetComponent<porpBase>().Porp = this;
    }
    //协程控制道具间隔10秒生成
    public void resetPorpCreat()
    {
        StartCoroutine(resetPorp());
    }
    IEnumerator resetPorp()
    {
        yield return new WaitForSeconds(10);
        creatPorp01();
    }
}
