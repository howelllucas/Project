
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginUI : UIObject
    {
        protected override void OnOpen(object val)
        {

        }

        protected override void OnClose()
        {
        }

        protected override void OnUpdate()
        {
        }

        public void OnEnterBtnClick()
        {
            Close();
            //Scenes.SceneMgr.singleton.CreateCity();
        }

        public void OnClearBtnClick()
        {
            PlayerDataMgr.singleton.DeleteSaveData();
            PlayerDataMgr.singleton.Init();
        }

    }
}