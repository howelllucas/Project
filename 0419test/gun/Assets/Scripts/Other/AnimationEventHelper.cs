using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    public event System.Action action;

    public void Event()
    {
        action?.Invoke();
    }
}
