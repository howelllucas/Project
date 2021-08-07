using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class task : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("enemyDead", pushList);
    }

    public void pushList(object info)
    {
        Debug.Log("任务完成");
    }
}
