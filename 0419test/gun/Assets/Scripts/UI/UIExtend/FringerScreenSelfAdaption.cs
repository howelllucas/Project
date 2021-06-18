using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tool
{
    public class FringerScreenSelfAdaption : MonoBehaviour
    {
        public RectTransform[] fringeAdeptArr_Pos;
        public RectTransform[] fringeAdeptArr_Size;
        public float offset = 0;

        private void Awake()
        {
            if (DeviceHelper.singleton.IsNotchScreen())
            {
                for (int i = 0; i < fringeAdeptArr_Pos.Length; i++)
                {
                    var pos = fringeAdeptArr_Pos[i].anchoredPosition;
                    pos.y += -DeviceHelper.singleton.NotchWidth + offset;
                    fringeAdeptArr_Pos[i].anchoredPosition = pos;
                }

                for (int i = 0; i < fringeAdeptArr_Size.Length; i++)
                {
                    var item = fringeAdeptArr_Size[i];
                    var size = item.sizeDelta;
                    var delta = DeviceHelper.singleton.NotchWidth - offset;
                    size.y = size.y - delta;
                    item.sizeDelta = size;

                    var pos = item.anchoredPosition;
                    pos.y -= item.pivot.y * delta;
                    item.anchoredPosition = pos;
                }
            }
        }
    }
}

