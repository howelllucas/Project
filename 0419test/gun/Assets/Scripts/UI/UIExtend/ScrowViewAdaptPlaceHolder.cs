using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class ScrowViewAdaptPlaceHolder : MonoBehaviour
    {
        public float offset = 0;
        private void Awake()
        {
            if (DeviceHelper.singleton.IsNotchScreen())
            {
                RectTransform rect = transform as RectTransform;
                if (rect != null)
                {
                    var size = rect.sizeDelta;
                    var delta = DeviceHelper.singleton.NotchWidth - offset;
                    size.y += delta;
                    rect.sizeDelta = size;

                    var pos = rect.anchoredPosition;
                    pos.y -= (1 - rect.pivot.y) * delta;
                    rect.anchoredPosition = pos;
                }
            }
        }

    }
}