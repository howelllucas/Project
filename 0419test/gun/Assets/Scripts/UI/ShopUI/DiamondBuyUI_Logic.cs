using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class DiamondBuyUI
    {
        private ShopGroupUI diamondGroup;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            diamondGroup = GetComponentInChildren<ShopGroupUI>();
            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            diamondGroup?.Init();
        }


    }
}