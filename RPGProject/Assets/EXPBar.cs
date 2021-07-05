using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public static EXPBar instance;

    private void Awake()
    {
        instance = this;
    }

    public void showValue(float value)
    {
        this.transform.Find("Bar").GetComponent<Image>().fillAmount = value;
    }
}
