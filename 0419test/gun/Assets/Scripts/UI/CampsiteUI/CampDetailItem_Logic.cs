using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampDetailItem
    {
        public void SetIcon(Sprite sprite)
        {
            Icon.image.sprite = sprite;
        }

        public void SetTitle(string title)
        {
            Title.text.text = title;
        }

        public void SetValue(string value)
        {
            ValTxt.text.text = value;
        }
    }
}