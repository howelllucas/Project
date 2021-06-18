using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Game
{
    //全局资源管理器 
    public class ResourceMgr : ResourceSet, ISingleton
    {

        public ResourceMgr()
        {
            s_singleton = this;
        }
        public void ClearSingleton()
        {
            s_singleton = null;
            ClearAllInstanceAndResource();
        }

        static ResourceMgr s_singleton;
        public static ResourceMgr singleton { get { return s_singleton; } }

        public static Sprite Texture2DToSprite(Texture2D tex)
        {
            if (tex == null)
                return null;

            Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            return sp;
        }
    }

    //底层加载资源的管理器 不要直接使用
    public class ResLoadMgr : Singleton<ResLoadMgr>
    {
      
        public delegate void OnLoadEnd(BaseLoadInfo obj);

        private Dictionary<string, BaseLoadInfo> resourceObjects = new Dictionary<string, BaseLoadInfo>();


        public override void ClearSingleton()
        {
            base.ClearSingleton();
            ClearAllResource();
        }

        public ResLoadMgr()
        {

        }

        public bool Init()
        {
            
            return true;
        }

		public BaseLoadInfo ResourceLoad(string path)
        {
            BaseLoadInfo loadinfo = FindLoadedImp(path);
            if (loadinfo != null)
            {
                if (loadinfo.LoadedState == BaseLoadInfo.LoadState.Loding)
                {
                    loadinfo.Load(path);
                }
                loadinfo.AddRef();

                return loadinfo;
            }

            return ResourceLoadImp(path);
        }

        public BaseLoadInfo ResourceLoadAsync(string path, OnLoadEnd onLoadEnd)
        {
			BaseLoadInfo loadinfo = FindLoadedImp(path);
            if (loadinfo != null)
            {
                loadinfo.AddRef();
                if (loadinfo.LoadedState == BaseLoadInfo.LoadState.Loding)
                {
                    if (onLoadEnd != null)
                    {
                        loadinfo.WaitLoadAsync(onLoadEnd);
                    }
                }
                else
                {
					if (onLoadEnd != null)
					{
						onLoadEnd(loadinfo);
					}
                }
                return loadinfo;
            }
            return ResourceLoadAsyncImp(path, onLoadEnd);
        }

        public bool UnLoadResource(BaseLoadInfo info)
        {
            if (info == null)
            {
                return false;
            }
            info.SubRef();
            if (info.NoUse)
            {
                info.UnLoad();
                resourceObjects.Remove(info.Path);

                return true;
            }

            return false;
        }
        public bool UnLoadResource(string path)
        {
            BaseLoadInfo info = null;
            if (!resourceObjects.TryGetValue(path, out info))
                return false;

            return UnLoadResource(info);
        }
        public void ClearAllResource()
        {
            foreach (var pair in resourceObjects)
            {
                if (pair.Value == null)
                    continue;

                pair.Value.UnLoad();
            }
            resourceObjects.Clear();
        }
        public BaseLoadInfo FindLoadInfo(string _path)
        {
            return FindLoadedImp(_path);
        }

        private BaseLoadInfo FindLoadedImp(string path)
        {
            BaseLoadInfo info = null;
            if (resourceObjects.TryGetValue(path, out info))
            {
                return info;
            }
            return null;
        }

        private BaseLoadInfo ResourceLoadImp(string path)
        {
            BaseLoadInfo info = new ResourceLoadInfo();
            if (!info.Load(path))
                return null;

            resourceObjects.Add(path, info);
            return info;
        }


        private BaseLoadInfo ResourceLoadAsyncImp(string path, OnLoadEnd onLoadEnd)
        {
            BaseLoadInfo info = new ResourceLoadInfo();

            resourceObjects.Add(path, info);
			info.LoadAsync(path, loadInfo =>
            {
                if (loadInfo == null) return;
                if (onLoadEnd != null)
                    onLoadEnd(loadInfo);               
            });
            return info;
        }
    }
}
