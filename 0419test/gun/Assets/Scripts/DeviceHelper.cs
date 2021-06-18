using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class DeviceHelper : MonoBehaviour
{
#if UNITY_IOS
    [DllImport ("__Internal")]
    private static extern bool x_deviceIsiPhoneX(); // 获取ios设备是不是x系列(刘海屏)
#endif

#if UNITY_EDITOR
    public float testNotchWidth;
#endif

#if UNITY_ANDROID
    private AndroidJavaClass androidNotchCheck;
#endif

    private static DeviceHelper s_singleton;
    public static DeviceHelper singleton { get { return s_singleton; } }

    float notchWidth = 0;
    public float NotchWidth
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (androidNotchCheck != null)
            {
                int offsetInPixel = androidNotchCheck.CallStatic<int>("GetNotchHeight");
                Debug.Log("NotchOffsetInPixel:" + offsetInPixel);
                notchWidth = 720f * offsetInPixel / Screen.width;

                androidNotchCheck = null;
            }
#endif
            return notchWidth;
        }
    }

    void Awake()
    {
        s_singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
    }

    public bool IsNotchScreen()
    {
        return NotchWidth > 0;
    }

    private void Init()
    {
        float offset = 0;
#if UNITY_EDITOR
        offset = testNotchWidth;
#elif UNITY_IOS
        if (x_deviceIsiPhoneX())
            offset = 55;
#elif UNITY_ANDROID
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        androidNotchCheck = new AndroidJavaClass("com.smarx.notchlib.NotchBridge");
        androidNotchCheck.CallStatic("Init", currentActivity);
#endif
        Debug.Log("NotchOffset:" + offset);
        notchWidth = offset;
    }
}
