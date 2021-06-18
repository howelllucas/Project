using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{

    public partial class CampSwitchUI
    {
        private Action m_Action;
        private int campID;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            string idStr = arg as string;
            if (!UiTools.IsNumeric(idStr))
            {
                Debug.LogErrorFormat("对话id非法 = {0}", idStr);
                TouchClose();
                return;
            }
            campID = int.Parse(idStr);

            var campRes = TableMgr.singleton.CampSetTable.GetItemByID(campID);
            if (campRes == null)
                return;

            TitleText.text.text = LanguageMgr.GetText("CampChapter", campID);
            NameText.text.text = LanguageMgr.GetText(campRes.tid_name);
        }

        public void SetAciton(Action action)
        {
            m_Action = action;
        }

        public void ChangeSceneStart()
        {
            //Global.gApp.gGameCtrl.ChangeToMainScene(3);
        }

        public void ChangeSceneEnd()
        {
            if (m_Action != null)
                m_Action();
            TouchClose();
        }
    }
}
