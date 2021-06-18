using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RectSizeFollow : MonoBehaviour
{
    public RectTransform target;
    public RectTransform followRect;

    private void Awake()
    {
        followRect.anchorMin = target.anchorMin;
        followRect.anchorMax = target.anchorMax;
        followRect.pivot = target.pivot;
        followRect.anchoredPosition = target.anchoredPosition;
        followRect.sizeDelta = target.sizeDelta;
    }

    private void Update()
    {
        if (followRect.sizeDelta != target.sizeDelta)
            followRect.sizeDelta = target.sizeDelta;
    }
}
