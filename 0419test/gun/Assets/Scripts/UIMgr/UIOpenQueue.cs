using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIOpenQueue : UIObject
    {
        public struct OpenUIParam
        {
            public string uiName;
            public object uiArg;
            public Transform root;
            public UIObject parent;
            public bool netOpen;

            public OpenUIParam(string name, bool netOpen = false, object arg = null, UIObject parent = null, Transform root = null)
            {
                uiName = name;
                uiArg = arg;
                this.root = root;
                this.parent = parent;
                this.netOpen = netOpen;
            }
        }

        Queue<OpenUIParam> openQueue;
        string curUI;
        protected override void OnOpen(object val)
        {
            base.OnOpen(val);

            openQueue = new Queue<OpenUIParam>();
            var openArr = val as OpenUIParam[];
            if (openArr != null)
            {
                for (int i = 0; i < openArr.Length; i++)
                {
                    openQueue.Enqueue(openArr[i]);
                }
            }

            OpenNext();
        }

        void OpenNext()
        {
            if (openQueue.Count <= 0)
            {
                Close();
                return;
            }

            var ui = openQueue.Dequeue();
            curUI = ui.uiName;
            Debug.Log("QueueOpen:" + curUI);
            if (ui.parent == null)
            {
                if (ui.netOpen)
                    UIMgr.singleton.NetOpen(curUI, ui.uiArg, ui.root);
                else
                    UIMgr.singleton.Open(curUI, ui.uiArg, ui.root);
            }
            else
            {
                if (ui.netOpen)
                    ui.parent.NetOpenSubUI(curUI, ui.uiArg, ui.root);
                else
                    ui.parent.OpenSubUI(curUI, ui.uiArg, ui.root);
            }
        }

        public override void OnEvent(UIEventType sevent, object obj)
        {
            base.OnEvent(sevent, obj);
            if (sevent == UIEventType.CLOSE_UI)
            {
                var closeUIName = obj as string;
                if (!string.IsNullOrEmpty(closeUIName) && closeUIName == curUI)
                {
                    OpenNext();
                }
            }
        }
    }

}
