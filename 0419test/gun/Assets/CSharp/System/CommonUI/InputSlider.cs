using EZ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSlider : MonoBehaviour
{
    public Slider slider;
    public InputField input;

    void Start()
    {
        input.text = slider.value.ToString();    //初始化
        slider.onValueChanged.AddListener(this.SliderChange);
        input.onValueChanged.AddListener(this.InputChange);
    }

    void SliderChange(float value)
    {
        input.text = slider.value.ToString();
    }
    void InputChange(string va)
    {
        float f = float.Parse(va);
        if (f >= slider.minValue && f <= slider.maxValue)
        {
            slider.value = f;
        }
        else
        {
            Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, "超过范围");
        }
    }
}