using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class NoDiamondUI
    {
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            CloseBtn.button.onClick.AddListener(OnCancelBtnClick);
            BuyBtn.button.onClick.AddListener(OnBuyBtnClick);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
        }

        private void OnCancelBtnClick()
        {
            TouchClose();
        }

        private void OnBuyBtnClick()
        {
            if (Global.gApp.CurScene is MainScene)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CommonUIIndexChange4Param, 3, "diamond");
                TouchClose();
            }
            else
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.DiamondBuyUI);
                TouchClose();
            }
        }
    }
}