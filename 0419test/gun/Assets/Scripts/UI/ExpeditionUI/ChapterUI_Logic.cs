using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using UnityEngine.UI;

namespace EZ
{

    public partial class ChapterUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            RegisterListeners();

            OkBtn.button.onClick.AddListener(OnOkClick);
            CancelBtn.button.onClick.AddListener(TouchClose);
        }

        private void RegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.AddListener(MsgIds.StarReward, ShowRewardInfo);
        }

        private void UnRegisterListeners()
        {
            //Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.StarReward, ShowRewardInfo);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }

        private void OnOkClick()
        {
            PlayerDataMgr.singleton.NextChapter();
            TouchClose();
        }

    }
}