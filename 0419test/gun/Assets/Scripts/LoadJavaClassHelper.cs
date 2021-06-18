using UnityEngine;


/// <summary>
/// 提供 android 加载 Java class 的管理类 
/// </summary>
public class LoadJavaClassHelper 
{
    public static string UnityPlayerClass = "com.unity3d.player.UnityPlayer";
    public static string BitmapUtilClass = "com.sdkds.base.util.BitmapUtil";
    public static string CommonsClass = "com.sdkds.base.util.Commons";
    public static string ReportUtilsClass = "com.sdkds.internalpush.SdkdsReportUtils";
    public static string CJavaBridgeClass = "com.sdkds.CJavaBridge";
    public static string AdUtilsClass = "com.sdkds.ad.AdUtils";
    public static string BaseNativeUtilClass = "com.sdkds.base.util.SdkdsNativeUtil";
    public static string NativeUtilClass = "com.sdkds.internalpush.SdkdsNativeUtil";
    public static string PromotionUtilsClass = "com.sdkds.internalpush.SdkdsPromotionUtils";
    public static string UnityDataUtilClass = "com.sdkds.data.report.UnityDataUtil";
    public static string CloudHeplerProxyClass = "com.sdkds.internalpush.CloudHeplerProxy";
    public static string SystemPropertyClass = "com.sdkds.base.util.SystemProperty";
    public static string AdTestViewClass = "com.sdkds.SdkdsAdTestView";
    public static string ApkDownloadManager = "com.sdkds.base.util.ApkDownload.ApkDownloadManager";
    
    public static string AdxAdsClass = "com.sdkds.ad.Interstitial.AdxAds";
    public static string AdxAdsNewClass = "com.sdkds.ad.Interstitial.AdxAdsNew";
    public static string MintegralCustomEventInterstitialVideoNativeClass = "com.sdkds.dancingline.ad.admob.Mobvista.interstitialvideoanativedapter.MintegralCustomEventInterstitialVideoNative";
    public static string MTGToAdmobRewardVideoAdapterClass = "com.sdkds.dancingline.ad.admob.Mobvista.rewardadapter.MTGToAdmobRewardVideoAdapter";
    
    

    #region android平台的基础原生接口相关  android native API

#if UNITY_ANDROID
    public static class SingletonHolder
    {
        public static AndroidJavaObject instance_context;
        static SingletonHolder()
        {
            using (AndroidJavaClass cls_UnityPlayer = LoadJavaClass(UnityPlayerClass))
            {
                instance_context = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
    }

    public static AndroidJavaClass LoadJavaClass(string name)
    {
        try
        {
            new AndroidJavaClass("java.lang.Class").CallStatic<AndroidJavaObject>("forName", name);
            AndroidJavaClass javaclass = new AndroidJavaClass(name);
            return javaclass;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("class not found: "+name);
            return null;
        }
    }

    public static AndroidJavaClass getReportUtilsClass()
    {
        return LoadJavaClass(ReportUtilsClass); 
    }

    public static AndroidJavaClass getBitmapUtilClass()
    {
        return LoadJavaClass(BitmapUtilClass);
    }

    public static AndroidJavaClass getCommonsClass()
    {
        return LoadJavaClass(CommonsClass);
    }

    public static AndroidJavaClass getCJavaBridgeClass()
    {
        return LoadJavaClass(CJavaBridgeClass);
    }

    public static AndroidJavaClass getSystemPropertyClass()
    {
        return LoadJavaClass(SystemPropertyClass);
    }

    public static AndroidJavaClass getAdUtilsClass()
    {
        return LoadJavaClass(AdUtilsClass);
    }

    public static AndroidJavaClass getBaseNativeUtilClass()
    {
        return LoadJavaClass(BaseNativeUtilClass);
    }

    public static AndroidJavaClass getNativeUtilClass()
    {
        return LoadJavaClass(NativeUtilClass);
    }

    public static AndroidJavaClass getAdTestViewClass()
    {
        return LoadJavaClass(AdTestViewClass);
    }

    public static AndroidJavaClass getApkDownloadManagerClass()
    {
        return LoadJavaClass(ApkDownloadManager);
    }

    public static AndroidJavaClass getPromotionUtilsClass()
    {
        return LoadJavaClass(PromotionUtilsClass);
    }

    public static AndroidJavaClass getUnityDataUtilClass()
    {
        return LoadJavaClass(UnityDataUtilClass);
    }

    public static AndroidJavaClass getCloudHeplerProxyClass()
    {
        return LoadJavaClass(CloudHeplerProxyClass);
    }

#endif
    #endregion

}
