using EZ.Data;
using EZ.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class ThanksUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_Btn2.button.onClick.AddListener(TouchClose);
        }
    }
}

