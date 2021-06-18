using System;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// provide some native function API, it need sdkds sdk init first. 
/// </summary>
public partial class SdkdsNativeUtil : MonoBehaviour
{


    private static bool _applicationIsQuitting = false;

    private static volatile SdkdsNativeUtil _Instance;
    private static string GameObjName = "SdkdsNativeUtil";

    public static Action<int> updatePercentProcess;
    public static Action ApkDownloadFailedAction;
    public static Action ApkInstallFailAction;
    public static SdkdsNativeUtil Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                return null;
            }

            if (_Instance == null)
            {
                //_Instance = new SdkdsNativeUtil();
                GameObject obj = new GameObject();
                obj.name = GameObjName;
                DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<SdkdsNativeUtil>();
            }
            return _Instance;
        }
    }

    private void Awake()
    {

        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        _Instance = null;
        _applicationIsQuitting = true;
    }

    private SdkdsNativeUtil()
    {
        Init();
    }

    public void Init()
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)
#elif UNITY_ANDROID
        if (m_AndroidObject == null) 
        {
            m_AndroidObject = LoadJavaClassHelper.getBaseNativeUtilClass();
        }
        if (null != m_AndroidObject)
        {
            m_AndroidObject.CallStatic("init", SingletonHolder.instance_context);
        }
        bitmap_util = LoadJavaClassHelper.getBitmapUtilClass();
#elif UNITY_IPHONE
#endif
    }

    #region android、ios统一的对外接口   Android, ios unified external interface

    /// <summary>
    /// 根据淘宝(天猫)商品url打开淘宝(天猫)的商品详情页面
    /// 例如 https://detail.m.tmall.com/item.htm?id=613882152648
    /// </summary>
    /// <param name="url"></param>
    public void OpenTaobao(string url)
    {
#if UNITY_EDITOR
#elif UNITY_IOS
        //TODO
        SDKDSOpenTMAppUrl(url);
#elif UNITY_ANDROID
        m_AndroidObject.CallStatic("openTaobao", SingletonHolder.instance_context, url);
#else
#endif
    }

    /**
    *  是否开启通知栏
    *
    *  @return 0、未开启推送（默认值） 1、已开启推送
    */
    public bool isAllowedNotification()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_IOS
        return SDKDSIsAllowedNotification();
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<bool>("isNotificationsEnabled", SingletonHolder.instance_context);
#else
        return false;
#endif
    }

    /**
     * 获取设备唯一标识符
     * @return
     */
    public string GetDeviceId()
    {
        string szDeviceId = "";
#if UNITY_EDITOR
        #if UNITY_IOS
        szDeviceId = "dfa78df48adf7d6rdfa78df48adwwIos";
        #elif UNITY_ANDROID
        szDeviceId = "dfa78df48adf7d6rdfa78df48adwwAndroid";
        #endif
#elif UNITY_IPHONE
        szDeviceId = SDKDSGetDeviceIdentifier();
#elif UNITY_ANDROID
        szDeviceId = getAndroidId();
#else
        szDeviceId = "";
#endif
        return szDeviceId;
    }

    /**
    *  获取手机具体型号
    *  @return 手机型号
    */
    public string GetDevicePlatform()
    {
#if UNITY_EDITOR
        return "windows";
#elif UNITY_IPHONE
		return SDKDSGetDevicePlatform();
#elif UNITY_ANDROID
        return getModel();
#else
        return "";
#endif
    }

    /**
     * 获取设备信息OS(SDK) version
     * @return
     */
    public string GetOSVersion()
    {
#if UNITY_EDITOR
        return "0";
#elif UNITY_IPHONE
        return SDKDSGetOSVersion();
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<string>("getOSVersion");
#else
        return "";
#endif
    }

    /// <summary>
    /// 获取当前的网络类型
    /// </summary>
    /// <returns></returns>
    public int GetCurrentNetStatue()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<int>("getCurrentNetStatue");
#elif UNITY_IPHONE
        return SDKDSGetCurrentNetStatue();
#else
        return 0;
#endif
    }

    /// <summary>
    /// 获取sdcard的最大空间(单位Byte)
    /// </summary>
    /// <returns></returns>
    public long GetTotalDiskSpace()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getTotalDiskSpace");
#elif UNITY_IPHONE
        return SDKDSGetTotalDiskSpace();
#else
        return 0;
#endif
    }

    /// <summary>
    /// 获取sdcard的可用空间(单位Byte)
    /// </summary>
    /// <returns></returns>
    public long GetFreeDiskSpace()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getFreeDiskSpace");
#elif UNITY_IPHONE
        return SDKDSGetFreeDiskSpace();
#else
        return 0;
#endif
    }

    /// <summary>
    /// 获取mcc
    /// </summary>
    /// <returns></returns>
    public string GetMcc()
    {
#if UNITY_EDITOR
        return "";
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<string>("getMcc");
#elif UNITY_IPHONE
        return SDKDSGetCountryCode();  //TODO
#else
        return "";
#endif
    }

    /// <summary>
    /// 获取系统语言设置的语言代码,例如zh
    /// </summary>
    /// <returns></returns>
    public string GetLanguage()
    {
#if UNITY_EDITOR
        return "";
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<string>("getLanguage");
#elif UNITY_IPHONE
        return SDKDSGetSystemLanguage();
#else
        return "";
#endif
    }
    
    /// <summary>
    /// 获取系统语言设置的国家代码， 例如CN
    /// </summary>
    /// <returns></returns>
    public string GetCountry()
    {
#if UNITY_EDITOR
        return "";
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<string>("getCountry");
#elif UNITY_IPHONE
        return SDKDSGetLocaleCountryCode();
#else
        return "";
#endif
    }
    
    /// <summary>
    /// 获取电话号码的国家代码，例如：86
    /// </summary>
    /// <returns></returns>
    public string GetSimCountry()
    {
#if UNITY_EDITOR
        return "";
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<string>("getSimCountry");
#elif UNITY_IPHONE
        return SDKDSGetCountryCode();
#else
        return "";
#endif
    }
    
    public void OpenUrl(string url)
    {
#if UNITY_EDITOR
        
#elif UNITY_ANDROID
        Application.OpenURL(url);
#elif UNITY_IPHONE
        SDKDSOpenUrl(url);
#else
		
#endif
    }

    /// <summary>
    /// 在Android上根据包名调起打开应用， 在IOS上根据URL打开应用.
    /// </summary>
    /// <param name="packageNameOrUrl"></param>
    /// <returns></returns>
    public bool OpenApp(string packageNameOrUrl)
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<bool>("openApp", packageNameOrUrl);
#elif UNITY_IPHONE
        return SDKDSOpenAppUrl(packageNameOrUrl);
#else
		return false;
#endif
    }

    /// <summary>
    /// 判断应用是否安装
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    public bool isAppExist(string packageName)
    {
        bool ret = false;
#if UNITY_EDITOR
        ret = false;
#elif UNITY_ANDROID
        ret = m_AndroidObject.CallStatic<bool>("isAppExist", packageName);
#elif UNITY_IPHONE
        ret = SDKDSCanOpenAppUrl(packageName);
#endif
        return ret;
    }

    /// <summary>
    /// 获取CPU最大频率（单位MHZ）
    /// </summary>
    /// <returns></returns>
    public long getMaxCpuFreq()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getMaxCpuFreq");
#elif UNITY_IPHONE
        return SDKDSGetCPUFrequency();
#else
        return 0;
#endif
    }

    /// <summary>
    /// 获取CPU内核数
    /// </summary>
    /// <returns></returns>
    public long getNumberOfCPUCores()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getNumberOfCPUCores");
#elif UNITY_IPHONE
        return SDKDSGetActiveProcessorCount();
#else
        return 0;
#endif
    }

	/// <summary>
    /// 已用内存，单位字节
    /// </summary>
    /// <returns></returns>
    public long GetMemoryUsage()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return 0;
#elif UNITY_IPHONE
        return SDKDSGetMemoryUsage();
#else
        return 0;
#endif
    }

    /// <summary>
    /// 可用内存，单位字节
    /// </summary>
    /// <returns></returns>
    public long GetAvailableMemory()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getFreeRamSpace");
#elif UNITY_IPHONE
        return SDKDSGetAvailableMemory();
#else
        return 0;
#endif
    }

	/// <summary>
    /// 获取设备总共内存，单位字节
    /// </summary>
    /// <returns></returns>
	public long getPhysicalMemory()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getTotalRamSpace");
#elif UNITY_IPHONE
        return SDKDSGetPhysicalMemory()*1024*1024;
#else
        return 0;
#endif
    }

    /// <summary>
    /// A/B测试方案类型  A = 0  B = 1
    /// </summary>
    /// <returns></returns>
    public int getABTestType()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<int>("getABTestType");
#elif UNITY_IPHONE
        return SDKDSGetABTestType();
#else
        return 0;
#endif
    }

    /// <summary>
    /// A/B测试方案类型  A = 0  B = 1(增加crc32校验)
    /// </summary>
    /// <returns></returns>
    public int getABTestCRC32Type()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<int>("getABTestType");
#elif UNITY_IPHONE
        return SDKDSGetABTestCRC32Type();
#else
        return 0;
#endif
    }
    
    /// <summary>
    /// 获取从开机到现在的时间
    /// 
    /// 单位：秒
    /// 默认值：0
    /// </summary>
    /// <returns></returns>
    public long getBootUpTime()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        return m_AndroidObject.CallStatic<long>("getBootUpTime");
#elif UNITY_IPHONE
        return SDKDSGetBootUpTime();
#else
        return 0;
#endif
    }
    /// <summary>
    /// Shakes the device with time.
    /// </summary>
    /// <param name="time">Time.</param>
    public void shakeDeviceWithTime(long time)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        m_AndroidObject.CallStatic("startVibrator", SingletonHolder.instance_context, time);
#elif UNITY_IPHONE
        SDKDSShakeiPhoneWithTime(time);
#else

#endif
    }

    /// <summary>
    /// Haptics feedback shake（触觉反馈震动）.
    /// </summary>
    /// <param name="type">Type(仅iOS需要):iOS震动类型，分为3种: 0.代表选择反馈 1.代表碰撞反馈（用的比较多）2.代表通知反馈.</param>
    /// <param name="strength">Strength(仅iOS需要):iOS震动力度，分为3种：0代表轻度 1代表中度 2.代表重度。注意：选择反馈只有一种力度.</param>
    /// <param name="time">Time(仅安卓需要):震动持续时间.</param>
    public void HapticFeedbackShake(int type, int strength, long time)
    {
#if UNITY_EDITOR
        return;
#endif

#if UNITY_ANDROID
        m_AndroidObject.CallStatic("startVibrator", SingletonHolder.instance_context, time);
#elif UNITY_IPHONE
        SDKDSHapticFeedbackShake(type, strength);
#else

#endif
    }

    /// <summary>
    /// 打开应用商店评分
    /// </summary>
    public void pullupAppReview()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        doRateUsAtGP();
#elif UNITY_IPHONE
        SDKDSPresentAPPReview();
#else

#endif
    }
    public bool IsFullScreenDevice()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        // TODO
        return  m_AndroidObject.CallStatic<bool>("hasNotch", SingletonHolder.instance_context);
#elif UNITY_IPHONE
        return SDKDSIsFullScreenDevice();
#else
        return false;
#endif
    }

    public float GetSafeAreaInsetsTop()
    {
#if UNITY_EDITOR
        return 0.0f;
#elif UNITY_ANDROID
        // TODO
        return 0.0f;
#elif UNITY_IPHONE
        return SDKDSGetSafeAreaInsetsTop();
#else
        return 0.0f;
#endif
    }

    public float GetSafeAreaInsetsLeft()
    {
#if UNITY_EDITOR
        return 0.0f;
#elif UNITY_ANDROID
        // TODO
        return 0.0f;
#elif UNITY_IPHONE
        return SDKDSGetSafeAreaInsetsLeft();
#else
        return 0.0f;
#endif
    }

    public float GetSafeAreaInsetsBottom()
    {
#if UNITY_EDITOR
        return 0.0f;
#elif UNITY_ANDROID
        // TODO
        return 0.0f;
#elif UNITY_IPHONE
        return SDKDSGetSafeAreaInsetsBottom();
#else
        return 0.0f;
#endif
    }

    public float GetSafeAreaInsetsRight()
    {
#if UNITY_EDITOR
        return 0.0f;
#elif UNITY_ANDROID
        // TODO
        return 0.0f;
#elif UNITY_IPHONE
        return SDKDSGetSafeAreaInsetsRight();
#else
        return 0.0f;
#endif
    }

    /// <summary>
    /// 保存文件到相册.
    /// </summary>
    /// <param name="jpgbyte">图片 byte 数组.</param>
    public bool SaveToGallery(byte[] jpgbyte)
    {
#if UNITY_ANDROID
        bool res = bitmap_util.CallStatic<bool>("saveImageToGalleryByFilePath", SingletonHolder.instance_context, jpgbyte);
        return res;
#endif
        Debug.Log("End Save");
        return false;
    }

    /// <summary>
    /// 保存文件到相册
    /// </summary>
    /// <param name="path">文件路径</param>
    public bool SaveToGalleryByPathOnAndroid(string path)
    {
#if UNITY_ANDROID
        bool res = bitmap_util.CallStatic<bool>("saveImageToGalleryByFilePath", SingletonHolder.instance_context, path);
		return res;
#endif
        return false;
    }

    /// <summary>
    /// 保存文件到相册
    /// </summary>
    /// <param name="base64Data">图片的base64Data数据</param>
    public bool SaveToGalleryByBase64DataOnAndroid(string base64Data)
    {
#if UNITY_ANDROID
        bool res = bitmap_util.CallStatic<bool>("saveImageToGalleryByBase64Data", SingletonHolder.instance_context, base64Data);
        return res;
#endif
        return false;
    }

    /// <summary>
    /// copy string to Clipboard
    /// </summary>
    /// <param name="content">content that you want to copy</param>
    public void CopyToClipboard(string content)
    {
#if (UNITY_EDITOR || DISBLE_PLATFORM)
        Debug.Log("CopyToClipboard");
#elif UNITY_IOS || UNITY_IPHONE
        SDKDSCopyToClipboard(content);
#elif UNITY_ANDROID
        copyToClipboarAos(content);
#endif
    }


    /// <summary>
    /// 得到可用系统内存
    /// </summary>
    /// <returns></returns>
    public long GetCanMemory()
    {
        Debug.Log("Application.dataPath = " + Application.dataPath);
        long memory = 1073741824;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        memory = m_AndroidObject.CallStatic<long>("getPathMemory",  "/data/data/");
        Debug.Log("手机内存为 = " + memory);
#elif UNITY_IOS || UNITY_IPHONE
        return SDKDSGetAvailableMemory();
#endif
        return memory;
    }

    /// <summary>
    /// 系统内存是否足够
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughMemory(float mb)
    {
        Debug.Log("Application.dataPath = " + Application.dataPath);
        bool isEnough = true;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        long memory = m_AndroidObject.CallStatic<long>("getPathMemory",  "/data/data/");
        if (memory < 0)
        {
            isEnough = true;
        }
        else
        {
            memory = (long)Mathf.Floor(memory / 1024 / 1024);
            if (memory < mb)
            {
                isEnough = false;
            }
        }
        Debug.Log("设备内存为 = " + memory + "    需要内存为 = " + mb + "   是否足够 = " + isEnough);
#elif UNITY_IOS || UNITY_IPHONE
        float avilableMem = SDKDSGetAvailableMemory();
        if (avilableMem < mb)
        {
            isEnough = false;
        }
#endif
        return isEnough;
    }

    /// <summary>
    /// 硬盘空间是够足够
    /// </summary>
    /// <param name="mb"></param>
    /// <returns></returns>
    public bool IsEnoughDisk(float mb)
    {
        Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath);
        bool isEnough = true;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        long disk = m_AndroidObject.CallStatic<long>("getPathMemory", Application.persistentDataPath);

        if (disk < 0)
        {
            isEnough = true;
        }
        else
        {
            disk = (long)Mathf.Floor(disk / 1024 / 1024);
            if (disk < mb)
            {
                isEnough = false;
            }
        }
        Debug.Log("设备磁盘空间为 = " + disk + "    需要磁盘空间为 = " + mb + "   是否足够 = " + isEnough);
#elif UNITY_IOS
        float freeDisk = SDKDSGetFreeDiskSpace();
        if (freeDisk < mb)
        {
            isEnough = false;
        }
#endif
        return isEnough;
    }


#endregion



#region android平台的基础原生接口相关  android native API

#if UNITY_ANDROID
    private AndroidJavaObject m_AndroidObject;
    private AndroidJavaObject bitmap_util;

    public static class SingletonHolder
    {
        public static AndroidJavaObject instance_context;
        static SingletonHolder()
        {
            using (AndroidJavaClass cls_UnityPlayer = LoadJavaClass("com.unity3d.player.UnityPlayer"))
            {
                instance_context = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
    }

    /**
     * 判断GooglePlayServices是否可用
     * @param displayDialog     是否弹窗提示
     * @return
     */
    public bool isHasGooglePlayServices(bool displayDialog)
    {
        return m_AndroidObject.CallStatic<bool>("isHasGooglePlayServices", SingletonHolder.instance_context, displayDialog);
    }

    /// <summary>
    /// 判断GP商店是否存在
    /// </summary>
    /// <returns></returns>
    public bool isGPAvailable()
    {
        return m_AndroidObject.CallStatic<bool>("isGPAvailable", SingletonHolder.instance_context);
    }

    /**
     * 获取当前APP的版本号
     * @return
     */
    public string getVersionCode()
    {
        return m_AndroidObject.CallStatic<string>("getVersionCode");
    }
    
    /**
     * 当前APP的版本名
     * @return
     */
    public string getVersionName()
    {
        return m_AndroidObject.CallStatic<string>("getVersionName");
    }
    
    /**
     * 获取android id
     * @return
     */
    public string getAndroidId()
    {
        return m_AndroidObject.CallStatic<string>("getAndroidId");
    }

    /**
     * 获取 IMEI
     * 注意：
     * 1.应用需要申请到所需的敏感（危险）权限android.permission.READ_PHONE_STATE，才会返回真实的imei值，否则是一个随机生成的字符串
     * 2.另外，android 10或以上，系统API已经无法获取imei
     * @return
     */
    public string getIMEI()
    {
        return m_AndroidObject.CallStatic<string>("getIMEI");
    }

    /**
     * 获取设备信息Model
     * @return
     */
    public string getModel()
    {
        return m_AndroidObject.CallStatic<string>("getModel");
    }

    /**
     * 获取设备信息Board
     * @return
     */
    public string getBoard()
    {
        return m_AndroidObject.CallStatic<string>("getBoard");
    }

    /**
     * 获取设备信息Branch
     * @return
     */
    public string getBranch() 
    {
        return m_AndroidObject.CallStatic<string>("getBranch");
    }
    
    /**
     * 获取设备信息Device
     * @return
     */
    public string getDevice()
    {
        return m_AndroidObject.CallStatic<string>("getDevice");
    }

    /**
     * 获取设备信息Display
     * @return
     */
    public string getDisplay()
    {
        return m_AndroidObject.CallStatic<string>("getDisplay");
    }

    /**
     * 获取设备信息Product
     * @return
     */
    public string getProduct()
    {
        return m_AndroidObject.CallStatic<string>("getProduct");
    }

    /**
     * 获取设备信息CodeName
     * @return
     */
    public string getCodeName()
    {
        return m_AndroidObject.CallStatic<string>("getCodeName");
    }
    
    /**
     * 获取设备信息BuildID
     * @return
     */
    public string getBuildID()
    {
        return m_AndroidObject.CallStatic<string>("getBuildID");
    }

    /**
     * 获取channel id
     * @return
     */
    public string getChannel()
    {
        return m_AndroidObject.CallStatic<string>("getChanel");
    }
    
    /**
     * 获取CPU最小频率（单位MHZ）
     * @return
     */
    public long getMinCpuFreq()
    {
        return m_AndroidObject.CallStatic<long>("getMinCpuFreq");
    }

    /**
     * 实时获取CPU当前频率（单位MHZ）
     * @return
     */
    public long getCurCpuFreq()
    {
        return m_AndroidObject.CallStatic<long>("getCurCpuFreq");
    }

    /**
     * GP 评分
     */
    public void doRateUsAtGP()
    {
        m_AndroidObject.CallStatic("doRateUsAtGP");
    }

    /**
     * 亚马逊评分
     */
    public void doRateUsAtAmazon()
    {
        m_AndroidObject.CallStatic("doRateUsAtAmazon");
    }

    /**
     * print key hash at logcat, search keyword KeyHash will find it
     */
    public void printKeyHash()
    {
        AndroidJavaObject commonsObject = LoadJavaClassHelper.getCommonsClass();
        if(commonsObject != null)
        {
            commonsObject.CallStatic("printKeyHash", SingletonHolder.instance_context);
        }
    }

    /**
     * Toast 提示
     */
    public void showToast(string msg)
    {
        AndroidJavaObject commonsObject = LoadJavaClassHelper.getCommonsClass();
        if (commonsObject != null)
        {
            commonsObject.CallStatic("showToast", SingletonHolder.instance_context, msg);
        }
    }

    /**
     * 触发复制字符串到系统剪贴板
     */
    public void copyToClipboarAos(string content)
    {
        AndroidJavaObject commonsObject = LoadJavaClassHelper.getCommonsClass();
        if (commonsObject != null)
        {
            commonsObject.CallStatic("copyToClipboar", SingletonHolder.instance_context, content);
        }
    }

    /**
     * 下载 apk
     * @param apkUrl        下载的url
     * @param appName       应用名称
     */
    public void DownloadApk(string apkDownloadUrl, string appName)
    {
        AndroidJavaObject apkDownloadManagerObject = LoadJavaClassHelper.getApkDownloadManagerClass();
        if (apkDownloadManagerObject != null)
        {
            apkDownloadManagerObject.CallStatic("downloadApk", SingletonHolder.instance_context, apkDownloadUrl, appName);
        }
    }

    /**
     * 下载 apk，静态方法，unity使用
     * @param context                           Activity 上下文
     * @param apkUrl                            apk 下载链接
     * @param appName                           下载的应用名称
     * @param unityMsgReceiver                  接收unity msg 的 game object name
     * @param startupApkInstallInitiative       下载完成是否主动调起安装向导————有些系统机型在下载完成时会自动调起安装向导
     */
    public void DownloadApk(string apkDownloadUrl, string appName, bool startupApkInstallInitiative)
    {
        AndroidJavaObject apkDownloadManagerObject = LoadJavaClassHelper.getApkDownloadManagerClass();
        if (apkDownloadManagerObject != null)
        {
            apkDownloadManagerObject.CallStatic("downloadApk", SingletonHolder.instance_context, apkDownloadUrl, appName, GameObjName, startupApkInstallInitiative);
        }
    }

    /// <summary>
    /// apk 下载进度回调，百分比
    /// </summary>
    /// <param name="progress"></param>
    public void ApkDownloadProgress(string progress)
    {
        //TODO add your code here
		if (updatePercentProcess != null)
        {
            int iprocess = 0;
            int.TryParse(progress, out iprocess);
            updatePercentProcess(iprocess);
        }
    }

    /// <summary>
    /// apk 下载进度回调,当前已经下载的大小，单位是bytes
    /// </summary>
    /// <param name="downloadedSize"></param>
    public void ApkDownloadProgressDownloadedSize(string downloadedSize)
    {
        Debug.Log("APK_DOWNLOAD ApkDownloadProgressDownloadedSize  downloadedSize:" + downloadedSize);
        //TODO add your code here
    }

    /// <summary>
    /// apk 下载成功回调
    /// </summary>
    /// <param name="msg"></param>
    public void ApkDownloadSuccess(string msg)
    {
        Debug.Log("APK_DOWNLOAD ApkDownloadSuccess  msg:" + msg);
        //TODO add your code here
    }

    /// <summary>
    /// apk 下载失败回调
    /// </summary>
    /// <param name="msg"></param>
    public void ApkDownloadFail(string msg)
    {
        Debug.Log("APK_DOWNLOAD ApkDownloadFail  msg:" + msg);
        //TODO add your code here
		if (ApkDownloadFailedAction != null)
        {
            ApkDownloadFailedAction();
        }
    }

    /// <summary>
    /// apk 安装失败回调
    /// </summary>
    /// <param name="msg"></param>
    public void ApkInstallFail(string msg)
    {
        Debug.Log("APK_DOWNLOAD ApkInstallFail  msg:" + msg);
        //TODO add your code here
		if (ApkInstallFailAction != null)
        {
            ApkInstallFailAction();
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
            Debug.LogError("sdkds not found " + name);
            return null;
        }
    }

#endif
#endregion android平台的基础原生接口相关  android native API

#region ios平台的基础原生接口相关  ios native API

#if UNITY_IPHONE
    [DllImport("__Internal")]
    static extern string SDKDSGetIdfa();

    [DllImport("__Internal")]
    static extern string SDKDSGetReallyIdfa();

    [DllImport("__Internal")]
    static extern string SDKDSGetIdfv();

    [DllImport("__Internal")]
    static extern int SDKDSGetTimeZone();

    [DllImport("__Internal")]
    static extern string SDKDSGetSystemLanguage();

    [DllImport("__Internal")]
    static extern string SDKDSGetCountryCode();

    [DllImport("__Internal")]
    static extern string SDKDSGetLocaleCountryCode();

    [DllImport("__Internal")]
    static extern string SDKDSGetResolution(string separator);

    [DllImport("__Internal")]
    static extern string SDKDSGetCountry();

    [DllImport("__Internal")]
    static extern string SDKDSGetAppVersion();

    [DllImport("__Internal")]
    static extern string SDKDSGetAppVersionName();

    [DllImport("__Internal")]
    static extern string SDKDSGetDeviceModel();

    [DllImport("__Internal")]
    static extern string SDKDSGetDeviceName();
    
    [DllImport("__Internal")]
    static extern void SDKDSOpenUrl(string url);

    [DllImport("__Internal")]
    static extern bool SDKDSCanOpenAppUrl(string url);

    [DllImport("__Internal")]
    static extern bool SDKDSOpenAppUrl(string url);
    
    [DllImport("__Internal")]
    static extern bool SDKDSOpenTMAppUrl(string url);

    [DllImport("__Internal")]
    static extern string SDKDSGetOSVersion();

    [DllImport("__Internal")]
    static extern bool SDKDSIsAllowedNotification();

    [DllImport("__Internal")]
    static extern string SDKDSGetDevicePlatform();

    [DllImport("__Internal")]
    static extern long SDKDSGetPhysicalMemory();


    [DllImport("__Internal")]
    static extern long SDKDSGetTotalDiskSpace();

    [DllImport("__Internal")]
    static extern long SDKDSGetFreeDiskSpace();

    [DllImport("__Internal")]
    static extern string SDKDSGetDeviceIdentifier();

    [DllImport("__Internal")]
    static extern int SDKDSGetCurrentNetStatue();

    [DllImport("__Internal")]
    static extern long SDKDSGetActiveProcessorCount();

    [DllImport("__Internal")]
    static extern long SDKDSGetMemoryUsage();

    [DllImport("__Internal")]
    static extern long SDKDSGetAvailableMemory();

    [DllImport("__Internal")]
    static extern long SDKDSGetCPUFrequency();

    [DllImport("__Internal")]
    static extern bool SDKDSPresentAPPReview();

    [DllImport("__Internal")]
    static extern int SDKDSGetABTestType();
    
    [DllImport("__Internal")]
    static extern int SDKDSGetABTestCRC32Type();

    [DllImport("__Internal")]
    static extern long SDKDSGetBootUpTime();

    [DllImport("__Internal")]
    static extern void SDKDSShakeiPhoneWithTime(long time);

    [DllImport("__Internal")]
    static extern void SDKDSHapticFeedbackShake(int type, int strength);

    [DllImport("__Internal")]
    static extern bool SDKDSIsFullScreenDevice();

    [DllImport("__Internal")]
    static extern float SDKDSGetSafeAreaInsetsTop();

    [DllImport("__Internal")]
    static extern float SDKDSGetSafeAreaInsetsLeft();

    [DllImport("__Internal")]
    static extern float SDKDSGetSafeAreaInsetsBottom();

    [DllImport("__Internal")]
    static extern float SDKDSGetSafeAreaInsetsRight();

    [DllImport("__Internal")]
    static extern void SDKDSCopyToClipboard(string content);

    /**
    *  获取广告标识符，如果用户限制广告追踪则传递第三方生成的openudid
    *
    *  @return 广告用标识
    */
    public string getIdfa()
    {
        return SDKDSGetIdfa();
    }

    /**
    *  获取真正的广告标识符，如果用户限制广告追踪则传递00000000-0000-0000-0000-000000000000
    *
    *  @return 广告用标识
    */
    public string getReallyIdfa()
    {
        return SDKDSGetReallyIdfa();
    }

    /**
    *  获取应用开发商标识符
    *
    *  @return 应用开发商标识
    */
    public string getIdfv()
    {
        return SDKDSGetIdfv();
    }

    /**
    *  获取时区
    *
    *  @return 时区 Number
    */
    public int getTimeZone()
    {
        return SDKDSGetTimeZone();
    }
    
    /**
    * 获取分辨率
    * @param separator 分辨率分割符
    */
    public string getResolution(string separator)
    {
        return SDKDSGetResolution(separator);
    }

    /**
    *  获取国家
    *
    *  @return 国家
    */
    public string getCountry()
    {
        return SDKDSGetCountry();
    }

    /**
    *  获取版本号（构建版本）
    *
    *  @return 版本号
    */
    public string getAppVersion()
    {
        return SDKDSGetAppVersion();
    }

    /**
    *  获取版本名
    *
    *  @return 版本名
    */
    public string getAppVersionName()
    {
        return SDKDSGetAppVersionName();
    }

    /**
    *  获取手机型号
    *
    *  @return 手机型号
    */
    public string getDeviceModel()
    {
        return SDKDSGetDeviceModel();
    }

    /**
    *  获取手机名称
    *
    *  @return 手机名称
    */
    public string getDeviceName()
    {
        return SDKDSGetDeviceName();
    }
    
    

    /**
    * 是否可打开App间通信的URL（一般用于判断是否已安装应用）
    * @param url 目标App内定义的URL，此参值需要字义的Info.plist的白名单里才能正常使用此接口
    */
    public bool canOpenAppUrl(string url)
    {
        return SDKDSCanOpenAppUrl(url);
    }

    public bool PresentAPPReview(){
#if UNITY_EDITOR
        return false;
#elif UNITY_IOS
        return SDKDSPresentAPPReview();
#elif UNITY_ANDROID
        return false;
#endif
    }
    
#endif
#endregion


}
