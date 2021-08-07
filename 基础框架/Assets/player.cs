using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("enemyDead", getCoin);
    }

    public void getCoin(object info)
    {
        Debug.Log("获得了10金币"+(info as enemy).name);
    }
}
