using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    public event UnityAction updateEvent;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (updateEvent!=null)
        {
            updateEvent();
        }
    }

    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }
    public void removeUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }
}
