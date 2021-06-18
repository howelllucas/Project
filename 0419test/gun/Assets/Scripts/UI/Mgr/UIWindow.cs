using EZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIWindow
    {
        string name;
        string resource;
        Transform transParent = null;
        UIMgr mgr;
        UIObject uiObject;
        object param;
        bool hasOpen = false;

        public string Name { get { return name; } }
        public UIObject Obj { get { return uiObject; } }
        public bool HasOpen { get { return hasOpen; } }
        public object Param { get { return param; } }

        public virtual bool Create(UIMgr _mgr, string _name, object val, Transform root)
        {

            mgr = _mgr;
            name = _name;
            resource = UIMgr.UIRootPath + name;
            param = val;
            transParent = root;

            return true;
        }

        public void OnEvent(UIEventType sevent, object obj)
        {
            if (uiObject!=null)
            {
                uiObject.OnEvent(sevent, obj);
            }
        }

        public virtual UIObject Open(object par)
        {
            param = par;
            return Open();
        }

        public virtual UIObject Open( )
        {
            if (uiObject != null)
                return uiObject;

            Transform _parent = transParent;
            if (_parent == null)
            {
                _parent = Global.gApp.gKeepNode.transform;
            }

            var obj = ResourceMgr.singleton.AddGameInstanceAsSubObject(resource, _parent);
            if (obj == null)
                return uiObject;
            uiObject = obj.GetComponent<UIObject>();
            if (uiObject == null)
            {
                Debug.LogErrorFormat("Open [{0}] error! param:{1} ", name, param);
                return uiObject;
            }
            uiObject.Create(this, param);
            hasOpen = true;

            return uiObject;
        }

        public void Update()
        {
            if( Obj )
            {
                Obj.UpdateUIObject();
            }

        }

        public virtual void Close()
        {
            Destroy();
        }
        void Destroy()
        {
            hasOpen = false;

            if (uiObject != null)
            {
                uiObject.Destroy();
                //Debug.Log("release mUIObject {0}", mUIObject);
                if (ResourceMgr.singleton != null)
                    ResourceMgr.singleton.DeleteInstance(uiObject.gameObject);
                uiObject = null;
            }
        }
    }
}
