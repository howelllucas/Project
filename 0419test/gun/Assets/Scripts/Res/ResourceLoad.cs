using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public enum ResourceType
    {
        ResourceType_Resource,
        ResourceType_AssetBundle,

    }
    public abstract class BaseLoadInfo
    {
        public enum LoadState
        {
            Init,
            Loding,
            Loded,
            Error,
        }
        protected string resPath;
        protected UnityEngine.Object assetObj;
        protected int refCount;
        protected LoadState loadedState = LoadState.Init;
        protected bool loadAsync = false;
        protected event ResLoadMgr.OnLoadEnd OnLoadedEnd;
        public BaseLoadInfo()
        {
            assetObj = null;
            refCount = 1;
        }

        public bool IsLoadAsync() { return loadAsync; }
        public LoadState LoadedState
        {
            get { return loadedState; }
        }

        public string Path { get { return resPath; } }

        public bool NoUse { get { return refCount <= 0; } }

        public int RefCount { get { return refCount; } }

        public void AddRef() { refCount++; }
        public void SubRef() { refCount--; }

        public abstract bool Load(string path);
        public abstract bool LoadAsync(string path, ResLoadMgr.OnLoadEnd onLoadEnd);
        public bool WaitLoadAsync(ResLoadMgr.OnLoadEnd onLoadEnd)
        {
            if (onLoadEnd != null)
            {
				//AddRef();
				if (loadedState == LoadState.Loding)
                    OnLoadedEnd += onLoadEnd;
				else
				{
					onLoadEnd(this);
				}
            }
            return true;
        }

        protected void OnLoadEnd()
        {
            if (OnLoadedEnd != null)
            {
                OnLoadedEnd(this);
                OnLoadedEnd = null;
            }
        }

        public abstract void UnLoad();

        public UnityEngine.Object GetResObject() { return assetObj; }
        //public string getAssetPath() { return mResItem.path; }
    }

    public class ResourceLoadInfo : BaseLoadInfo
    {
        ResourceRequest request;
        public ResourceType GetResourceType() { return ResourceType.ResourceType_Resource; }
        public override bool Load(string path)
        {
			loadedState = LoadState.Loding;
            //Debug.Log(@"	start load	{0}", item.path);
            assetObj = Resources.Load(path);
            resPath = path;
            if (assetObj == null)
            {
                loadedState = LoadState.Error;
                Debug.LogError("Not Exist Resource File :[" + path + "]");
                return false;
            }

            loadedState = LoadState.Loded;

            return true;
        }
        public override bool LoadAsync(string path, ResLoadMgr.OnLoadEnd onLoadEnd)
        {
            //Debug.Log(@"	start load Async	{0}", item.path);
            loadAsync = true;
            loadedState = LoadState.Loding;
            resPath = path;

            request = Resources.LoadAsync(path);
          
            if (request==null)
            {
                if (onLoadEnd != null)
                {
                onLoadEnd(null);
                 }
                loadedState = LoadState.Error;
                Debug.LogError("Not Exist Resource File :[" + path + "]");                          
                return false;
            }
            if(onLoadEnd!=null)
                OnLoadedEnd += onLoadEnd;
            IEnumeratorHelper.Start(_loadAsync());
            return true;
        }

        IEnumerator _loadAsync()
        {
            yield return request;
            if (assetObj == null)
            {
                if(request!=null)
                    assetObj = request.asset;
            }
            if (assetObj != null)
            {
                loadedState = LoadState.Loded;
            }
            else
            {
                loadedState = LoadState.Error;
            }
            request = null;
            OnLoadEnd();
            //Debug.Log(@"	end load Async	{0}", item.path);
        }
        public override void UnLoad()
        {
            if (assetObj != null)
            {
                assetObj = null;
            }
            request = null;
        }
    }
    public class AssetBundleLoadInfo : BaseLoadInfo
    {
        public ResourceType GetResourceType() { return ResourceType.ResourceType_AssetBundle; }

        AssetBundleRequest m_Request;

        byte[] getZipFileAllData()
        {
            throw new System.Exception();
        }


        public override bool Load(string path)
        {
            return true;
        }


        public override bool LoadAsync(string path, ResLoadMgr.OnLoadEnd onLoadEnd)
        {
            return true;
        }

        public override void UnLoad()
        {
            throw new NotImplementedException();
        }
    }
}
