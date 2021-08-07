using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputMgr.GetInstance().openOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener("按下", anxia);
        EventCenter.GetInstance().AddEventListener("抬起", taiqi);
    }

    private void taiqi(object key)
    {
        KeyCode code = (KeyCode)key;

        switch (code)
        {
            case KeyCode.W:
                Debug.Log("W");
                break;
            case KeyCode.S:
                Debug.Log("S");
                break;
            case KeyCode.A:
                Debug.Log("A");
                break;
            case KeyCode.D:
                Debug.Log("D");
                break;
            default:
                break;
        }
    }

    private void anxia(object key)
    {
        KeyCode code = (KeyCode)key;

        switch (code)
        {
            case KeyCode.W:
                Debug.Log("W");
                break;
            case KeyCode.S:
                Debug.Log("S");
                break;
            case KeyCode.A:
                Debug.Log("A");
                break;
            case KeyCode.D:
                Debug.Log("D");
                break;
            default:
                break;
        }
    }

    
   
}
