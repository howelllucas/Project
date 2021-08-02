using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable ()
    {
        Invoke("push", 1);
    }

    public void push()
    {
        PoolMgr.GetInstance().PushObj(this.gameObject.name, this.gameObject);
    }
}
