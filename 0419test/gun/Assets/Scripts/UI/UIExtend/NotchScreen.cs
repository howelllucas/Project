using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotchScreen : MonoBehaviour
{
    public bool isMoveUp = false;
    public float moveDis = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (DeviceHelper.singleton.IsNotchScreen())
        {
            if (isMoveUp)
                transform.localPosition += new Vector3(0.0f, moveDis, 0.0f);
            else
                transform.localPosition -= new Vector3(0.0f, moveDis, 0.0f);
        }
    }

}
