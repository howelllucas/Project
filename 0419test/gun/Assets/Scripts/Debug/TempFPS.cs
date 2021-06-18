using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempFPS : MonoBehaviour
{
    Text m_Text;

    public float _UpdateInterval = 0.1f;
    private float _LastInterval;
    private int _Frames = 0;

    private float _FPS;

    void Start()
    {
        m_Text = this.gameObject.GetComponent<Text>();
        _LastInterval = Time.realtimeSinceStartup;
        _Frames = 0;
    }

    void Update()
    {
        _Frames++;
        if (Time.realtimeSinceStartup > _LastInterval + _UpdateInterval)
        {
            _FPS = _Frames / (Time.realtimeSinceStartup - _LastInterval);
            _Frames = 0;
            _LastInterval = Time.realtimeSinceStartup;
            m_Text.text = _FPS.ToString();
        }

    }
}
