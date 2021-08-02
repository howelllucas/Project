using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABLoad : MonoBehaviour
{
    public Image image;
    void Start()
    {
        //同步加载AB包
        AssetBundle ab= AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/"+"head");
        //加载包里的资源
        GameObject obj= ab.LoadAsset<GameObject>("Cube");
        //赋值测试
        
        //AB包卸载自己,false为保留已经加载的资源，true为连同资源一起删掉，推荐false
         //ab.Unload(false);
        //开启协程
        StartCoroutine(AB("material", "New Material"));
        Instantiate(obj);
    }
    //用协程异步加载
    IEnumerator AB(string ABbao,string ResBao)
    {
        //加载AB包
        AssetBundleCreateRequest ab= AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABbao);
        yield return ab;
        //加载AB包里的资源
        AssetBundleRequest obj = ab.assetBundle.LoadAssetAsync(ResBao,typeof(Material));
        yield return obj;
        //给图片赋值测试
        //image.sprite = obj.asset as Sprite;
        
    }
   
}
