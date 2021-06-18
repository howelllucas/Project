using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using XLua;

namespace Game.UI
{
    //界面实体类 所有界面预制体挂载的脚本需要继承此类
    public class UIObject : MonoBehaviour
    {
        public event Action<UIObject> OnUIClose;
        bool hasClose = false;
        UIWindow window;
        Canvas canvas;

        public bool HasClose { get { return hasClose; } }
        public UIWindow Window { get { return window; } }
        public Canvas Canvas { get { return canvas; } }
        private HashSet<string> subUIs = new HashSet<string>();

        void _Close()
        {
            if (!hasClose)
            {
                hasClose = true;
                if (OnUIClose != null)
                {
                    OnUIClose(this);
                }
            }
        }
        public virtual void Close()
        {

            _Close();
            if (UIMgr.singleton != null)
            {
                UIMgr.singleton.Close(window);
            }

        }

        public bool Create(UIWindow win, object val)
        {
            window = win;
            OnOpen(val);
            canvas = GetComponentInChildren<Canvas>();
            if (canvas != null /*&&  canvas.renderMode == RenderMode.ScreenSpaceCamera*/)
                canvas.worldCamera = Camera.main;

            //LanguageMgr.singleton.SetUILanguage(gameObject);
            return true;
        }

        public void Show(bool bShow)
        {
            OnShow(bShow);
        }

        public void Refresh()
        {
            OnRefresh();
        }

        public void UpdateUIObject()
        {
            OnUpdate();
        }

        public virtual void OnEvent(UIEventType sevent, object obj)
        {
            if (sevent == UIEventType.CLOSE_UI)
            {
                string uiName = obj as string;
                if (!string.IsNullOrEmpty(uiName) && subUIs.Contains(uiName))
                {
                    subUIs.Remove(uiName);
                    OnSubUIClose(uiName);
                }
            }
        }

        public void Destroy()
        {
            try
            {
                OnClose();
                _Close();
                return;
            }
            catch (Exception e)
            {
                if (window != null)
                    Debug.LogWarningFormat("[{0}] Destroy error!msg:{1}", window.Name, e);
                return;
            }
        }
        protected virtual void OnOpen(object val)
        {

        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnRefresh()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnShow(bool bShow)
        {
        }
        protected void OnDestroy()
        {
            Close();
        }

        internal UIObject OpenSubUI(string name, object val = null, Transform root = null)
        {
            subUIs.Add(name);
            OnSubUIOpen(name);
            return UIMgr.singleton.Open(name, val, root);
        }

        internal void CloseAllSubUI()
        {
            if (subUIs.Count <= 0 || UIMgr.singleton == null)
            {
                return;
            }
            var closeList = subUIs.ToArray();
            for (int i = 0; i < closeList.Length; i++)
            {
                UIMgr.singleton.Close(closeList[i]);
            }
        }

        internal void NetOpenSubUI(string name, object val = null, Transform root = null)
        {
            subUIs.Add(name);
            OnSubUIOpen(name);
            UIMgr.singleton.NetOpen(name, val, root);
        }

        public bool IsThisInTheTop()
        {
            return subUIs.Count <= 0;
        }

        protected virtual void OnSubUIOpen(string name)
        {

        }

        protected virtual void OnSubUIClose(string name)
        {

        }
    }

}
