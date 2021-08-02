using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    void Start()
    {
        GetControl<Button>("Button").onClick.AddListener(click);
    }

    private void click()
    {
        Debug.Log("0000");
    }
}
