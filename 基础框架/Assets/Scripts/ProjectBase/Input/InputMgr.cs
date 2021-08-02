using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseManager<InputMgr>
{
    private bool isOpen=false;

    public  void openOrEndCheck(bool b)
    {
        isOpen = b;
    }
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(myUpdate);
    }

    private void getKey(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger("按下", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger("抬起", key);
        }
    }
    private void myUpdate()
    {
        if (!isOpen)
        {
            return;
        }
        getKey(KeyCode.W);
        getKey(KeyCode.S);
        getKey(KeyCode.A);
        getKey(KeyCode.D);
    }
}
