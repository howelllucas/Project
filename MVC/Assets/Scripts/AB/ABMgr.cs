using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ABMgr
{
    private static ABMgr instance;

    public static ABMgr Instance
    {
        get
        {
            return instance;
        }
    }

    private Dictionary<string, AssetBundle> dicAB = new Dictionary<string, AssetBundle>();

    private AssetBundle abMain = null;
    private AssetBundle ab = null;
    private AssetBundleManifest abFest;
    private  string  ABPath
    {
        get
        {
            return Application.streamingAssetsPath+"/";
        }
    }

    private string mainAB
    {
        get
        {
#if UNITY_IOS
        return "IOS";
#elif UINTY_ANDROID
        return "android";
#else
            return "pc";
#endif
        }
    }
   //加载资源包
    public void LoadAb(string ABName)
    {
        //加载依赖资源
        if (abMain == null)
        {
            abMain = AssetBundle.LoadFromFile(ABPath + mainAB);
            abFest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        string[] abNames = abFest.GetAllDependencies(ABName);

        for (int i = 0; i < abNames.Length; i++)
        {
            if (!dicAB.ContainsKey(abNames[i]))
            {
                ab = AssetBundle.LoadFromFile(ABPath + abNames[i]);
                dicAB.Add(abNames[i], ab);
            }
        }
        //加载目标资源
        if (!dicAB.ContainsKey(ABName))
        {
            ab = AssetBundle.LoadFromFile(ABPath + ABName);
            dicAB.Add(ABName, ab);
        }
        
    }
    //提供给外部的方法
    public Object ABload(string ABname,string ResName)
    {
        LoadAb(ResName);

        Object obj = dicAB[ABname].LoadAsset(ResName);
        return obj;

    }
    //卸载单个资源包
    public void unload(string ABName)
    {
        if (dicAB.ContainsKey(ABName))
        {
            dicAB[ABName].Unload(false);
            dicAB.Remove(ABName);
        }
    }
    //卸载所有资源包
    public void clearAll(string ABName)
    {
        AssetBundle.UnloadAllAssetBundles(false);
        dicAB.Clear();
        abFest = null;
        abMain = null;
        ab = null;
    }
}
