using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{

    public partial class OpeningUI
    {
        private Action m_Action;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            //string idStr = arg as string;
            //if (!UiTools.IsNumeric(idStr))
            //{
            //    Debug.LogErrorFormat("对话id非法 = {0}", idStr);
            //    TouchClose();
            //    return;
            //}
            //campID = int.Parse(idStr);

            //var campRes = TableMgr.singleton.CampSetTable.GetItemByID(campID);
            //if (campRes == null)
            //    return;

            //TitleText.text.text = LanguageMgr.GetText("CampChapter", campID);
            //NameText.text.text = LanguageMgr.GetText(campRes.tid_name);
        }

        public void SetAciton(Action action)
        {
            m_Action = action;
        }

        public void End()
        {
            if (m_Action != null)
                m_Action();
            TouchClose();
        }
    }
}
