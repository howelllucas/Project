using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public string name = "123";
    // Start is called before the first frame update
    void Start()
    {
        dead();
    }

    public void dead()
    {
        Debug.Log("我死了");
        EventCenter.GetInstance().EventTrigger("enemyDead",this);
    }
}
