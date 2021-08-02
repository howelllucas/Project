using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABLoad2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //加载head资源包
        AssetBundle ab= AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "head");

        GameObject obj= ab.LoadAsset<GameObject>("Cube");
        //加载head包关联的其他包
        AssetBundle ab_ios= AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "iOS");//先加载主包

        AssetBundleManifest abfest = ab_ios.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] a = abfest.GetAllDependencies("head");

        for (int i = 0; i < a.Length; i++)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + a[i]);
        }
        //测试
        Instantiate(obj);
    }

   
}
