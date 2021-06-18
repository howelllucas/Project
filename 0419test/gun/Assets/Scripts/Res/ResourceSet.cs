using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //资源管理模块 可以放在其他功能模块中使用
    public class ResourceSet
    {
        public delegate void OnGameObjectLoadEnd(GameObject obj);
        public delegate void OnObjectLoadEnd(UnityEngine.Object obj);
        public delegate void OnLoadResEnd(ResourceInfo resInfo);

        public struct InstranceInfo
        {
            public UnityEngine.Object obj;
            public BaseLoadInfo resInfo;
            static public InstranceInfo make(UnityEngine.Object _obj, BaseLoadInfo loadInfo)
            {
                InstranceInfo info;
                info.obj = _obj;
                info.resInfo = loadInfo;
                return info;
            }
        }
        protected List<InstranceInfo> instanceList = new List<InstranceInfo>();
        protected Dictionary<string, ResourceInfo> resources = new Dictionary<string, ResourceInfo>();

        bool hasDestroy = false;
        protected bool hasClear = false;
        public class ResourceInfo
        {

            BaseLoadInfo resInfo;
            int count;
            public static ResourceInfo make(BaseLoadInfo _loadInfo)
            {
                return new ResourceInfo() { resInfo = _loadInfo, count = 1 };
            }
            public UnityEngine.Object GetResObject() { return resInfo.GetResObject(); }
            public string Path { get { return resInfo.Path; } }

            public UnityEngine.Object ResObject { get { return resInfo.GetResObject(); } }


            public BaseLoadInfo InternalLoadInfo { get { return resInfo; } }

            public int RefCount { get { return count; } }
            public void AddRef()
            {
                count++;
            }
            public void SubRef()
            {
                count--;
            }
        }

        public bool HasDestroy { get { return hasDestroy; } }
        public void Destroy()
        {
            hasDestroy = true;
            ClearAllInstanceAndResource();
        }
        public void ClearAllInstanceAndResource()
        {
            hasClear = true;
            if (ResLoadMgr.singleton != null)
            {
                foreach (var instObj in instanceList)
                    DeleteInstance(instObj);
                foreach (var resObj in resources.Keys)
                    ResLoadMgr.singleton.UnLoadResource(resObj);
            }
            instanceList.Clear();
            resources.Clear();
        }

        ResourceInfo FindResource(string _path)
        {
            ResourceInfo info;
            resources.TryGetValue(_path, out info);
            return info;
        }



        ResourceInfo RegResource(string _path, BaseLoadInfo resObj)
        {
            ResourceInfo info;
            if (!resources.TryGetValue(_path, out info))
            {
                info = ResourceInfo.make(resObj);
                resources.Add(_path, info);
            }
            else
            {
                info.AddRef();
            }
            return info;
        }

        public void DeleteResource(ResourceInfo loadInfo)
        {
            loadInfo.SubRef();
            if (loadInfo.RefCount == 0)
            {
                resources.Remove(loadInfo.Path);
                ResLoadMgr.singleton.UnLoadResource(loadInfo.InternalLoadInfo);
            }
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="instObj">资源路径</param>
        public void DeleteResource(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                return;
            ResourceInfo info;
            if (resources.TryGetValue(_path, out info))
            {
                DeleteResource(info);
            }
        }

        private ResourceInfo AddResourceEx(string _path)
        {
            if (hasDestroy) return null;
            var find_obj = FindResource(_path);
            if (find_obj != null)
            {
                find_obj.AddRef();
                return find_obj;
            }
            var loadInfo = ResLoadMgr.singleton.ResourceLoad(_path);
            if (loadInfo == null || !loadInfo.GetResObject())
            {
                Debug.LogWarning("not find Resource [" + _path + "]");
                return null;
            }
            //UnityEngine.Object obj = loadInfo.GetResObject();

            find_obj = RegResource(_path, loadInfo);

            return find_obj;
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="widgetName">资源路径</param>
        /// <returns>资源的实例</returns>
        public UnityEngine.Object AddResource(string _path)
        {
            if (string.IsNullOrEmpty(_path))
                return null;
            var info = AddResourceEx(_path);
            if (info == null) return null;
            return info.ResObject;
        }

        public Sprite GetSprite(string atlasName, string spriteName)
        {
            UnityEngine.U2D.SpriteAtlas atlas = AddResource(string.Format("Atlas/{0}", atlasName)) as UnityEngine.U2D.SpriteAtlas;
            if (atlas == null)
                return null;
            return atlas.GetSprite(spriteName);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="widgetName">资源路径</param>
        /// <param name="onLoadEnd">回调函数</param>
        public ResourceInfo AddResourceAsync(string _path, ResourceSet.OnLoadResEnd onLoadEnd)
        {
            if (hasDestroy) return null;
            var find_obj = FindResource(_path);
            if (find_obj != null)
            {
                //_RegResource(_path, find_obj);
                find_obj.AddRef();
            }
            else
            {
                var rLoadInfo = ResLoadMgr.singleton.ResourceLoadAsync(_path, null);
                find_obj = RegResource(_path, rLoadInfo);
            }
            if (onLoadEnd != null)
            {
                find_obj.InternalLoadInfo.WaitLoadAsync((loadInfo) => onLoadEnd(find_obj));
            }
            return find_obj;
        }
        //////////////////////////////////////////////////

        protected void DeleteInstance(InstranceInfo info, bool immedia = false)
        {
            if (info.obj)
            {
                if (immedia)
                {
                    UnityEngine.Object.DestroyImmediate(info.obj);
                }
                else
                {
                    UnityEngine.Object.Destroy(info.obj);
                }
            }
            ResLoadMgr.singleton.UnLoadResource(info.resInfo);
        }

        /// <summary>
        /// 删除预制体
        /// </summary>
        /// <param name="instObj">控件的实例</param>
        /// <param name="immedia">是否立即删除</param>
        /// <returns>是否成功</returns>
        public bool DeleteInstance(UnityEngine.Object instObj, bool immedia = false)
        {
            bool _Deleted = false;
            for (int i = instanceList.Count - 1; i >= 0; i--)
            {
                if (instanceList[i].obj == instObj)
                {
                    DeleteInstance(instanceList[i], immedia);
                    instanceList.RemoveAt(i);
                    _Deleted = true;
                    return true;
                }
            }
            if (!_Deleted)
            {
                if (instObj)
                {
                    if (!hasClear)
                        Debug.LogWarningFormat("destroy object error! not find obj [{0}] in {1}!", instObj, this);
                    if (immedia)
                    {
                        UnityEngine.Object.DestroyImmediate(instObj);
                    }
                    else
                    {
                        UnityEngine.Object.Destroy(instObj);
                    }
                }
            }
            return _Deleted;
        }
        private UnityEngine.Object AddInstance(string path)//, bool _DontDestroyOnLoad = false)
        {
            if (hasDestroy) return null;
            var info = ResLoadMgr.singleton.ResourceLoad(path);
            if (info == null || !info.GetResObject())
            {
                Debug.LogWarning("not find instance [" + path + "]");
                return null;
            }
            var resourceObject = info.GetResObject();
            UnityEngine.Object instObj = GameObject.Instantiate(resourceObject);

            instanceList.Add(InstranceInfo.make(instObj, info));

            return instObj;
        }


        private UnityEngine.Object AddInstance(string path, Vector3 position, Quaternion rotation)//, bool _DontDestroyOnLoad = false)
        {
            if (hasDestroy) return null;
            var info = ResLoadMgr.singleton.ResourceLoad(path);
            if (info == null || !info.GetResObject())
            {
                Debug.LogWarning("not find instance [" + path + "]");
                return null;
            }
            var resourceObject = info.GetResObject();
            UnityEngine.Object instObj = GameObject.Instantiate(resourceObject, position, rotation);

            instanceList.Add(InstranceInfo.make(instObj, info));

            return instObj;
        }

        private BaseLoadInfo AddInstanceAsync(string widgetName, OnObjectLoadEnd onLoadEnd)
        {
            if (hasDestroy) return null;
            return ResLoadMgr.singleton.ResourceLoadAsync(widgetName, (ResLoadMgr.OnLoadEnd)(loadInfo =>
            {
                if (loadInfo == null || !loadInfo.GetResObject())
                {
                    Debug.LogWarning("not find instance Async[" + widgetName + "]");
                    return;
                }
                if (this.hasDestroy)
                {
                    ResLoadMgr.singleton.UnLoadResource(loadInfo);
                    return;
                }
                UnityEngine.Object instObj = GameObject.Instantiate(loadInfo.GetResObject());
                if (instObj == null)
                    return;
                instanceList.Add(InstranceInfo.make(instObj, loadInfo));
                if (onLoadEnd != null)
                    onLoadEnd(instObj);
            }));
        }
        private BaseLoadInfo AddInstanceAsync(string widgetName, Vector3 position, Quaternion rotation, OnObjectLoadEnd onLoadEnd)
        {
            if (hasDestroy) return null;
            return ResLoadMgr.singleton.ResourceLoadAsync(widgetName, (ResLoadMgr.OnLoadEnd)(loadInfo =>
            {
                if (loadInfo == null || !loadInfo.GetResObject())
                {
                    Debug.LogWarning("not find instance Async[" + widgetName + "]");
                    return;
                }
                if (this.hasDestroy)
                {
                    ResLoadMgr.singleton.UnLoadResource(loadInfo);
                    return;
                }
                UnityEngine.Object instObj = GameObject.Instantiate(loadInfo.GetResObject(), position, rotation);
                if (instObj == null)
                    return;
                instanceList.Add(InstranceInfo.make(instObj, loadInfo));
                if (onLoadEnd != null)
                    onLoadEnd(instObj);
            }));
        }

        /// <summary>
        /// 添加预制体
        /// </summary>
        /// <param name="widgetName">预制体路径</param>
        /// <returns>预制体的实例</returns>
        public GameObject AddGameInstance(string widgetName)//, bool _DontDestroyOnLoad = false)
        {
            if (hasDestroy) return null;
            GameObject widget = AddInstance(widgetName) as GameObject;
            return widget;
        }

        /// <summary>
        /// 添加预制体
        /// </summary>
        /// <param name="widgetName">预制体路径</param>
        /// <param name="position">位置</param>
        /// <param name="rotation">旋转</param>
        /// <returns>子预制体的实例</returns>
        public GameObject AddGameInstance(string widgetName, Vector3 position, Quaternion rotation)// , bool _DontDestroyOnLoad = false)
        {
            if (hasDestroy) return null;
            GameObject widget = AddInstance(widgetName, position, rotation) as GameObject;
            return widget;
        }
        /// <summary>
        /// 向父控件，添加子预制体
        /// </summary>
        /// <param name="widgetName">控件路径</param>
        /// <param name="parent">子控件的父控件</param>
        /// <returns>子控件的实例</returns>
        public GameObject AddGameInstanceAsSubObject(string widgetName, Transform parent)
        {
            if (hasDestroy) return null;
            GameObject widget = AddInstance(widgetName) as GameObject;
            if (widget == null)
            {
                //Debug.Log("not find [" + widgetName + "]");
                return null;
            }

            if (parent != null)
            {
                var temp = TransformLocal.Save(widget.transform);
                widget.transform.SetParent(parent, false);
                temp.Load(widget.transform);
            }
            //_RegInstance(widget);
            return widget;
        }

        /// <summary>
        /// 异步加载，向父控件，添加子预制体
        /// </summary>
        /// <param name="widgetName">控件路径</param>
        /// <param name="parent">子控件的父控件</param>
        /// <returns>子控件的实例</returns>
        private BaseLoadInfo AddGameInstanceAsSubObjectAsync(string modelName, Transform parent, OnGameObjectLoadEnd onLoadEnd)
        {
            if (hasDestroy) return null;
            return AddInstanceAsync(modelName, obj =>
            {
                var model = obj as GameObject;
                if (model == null)
                {
                    Debug.LogWarningFormat("load async Instance error!!!msg:{0}", modelName);
                    return;
                }
                if (parent != null)
                {
                    model.transform.SetParent(parent, true);

                }

                if (onLoadEnd != null)
                    onLoadEnd(model);
            });
        }
    };

    public struct TransformLocal
    {
        Vector3 pos;
        Quaternion quat;
        Vector3 scale;
        public static TransformLocal Save(Transform trans)
        {
            TransformLocal localSave;
            localSave.pos = trans.localPosition;
            localSave.quat = trans.localRotation;
            localSave.scale = trans.localScale;
            return localSave;
        }

        public void Load(Transform trans)
        {
            trans.localPosition = pos;
            trans.localRotation = quat;
            trans.localScale = scale;
        }
    }

}
